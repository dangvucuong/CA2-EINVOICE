using System.Data.SqlClient;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Xsl;
using Common;
using Contracts.Service.HoaDon;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using Model.Base;
using Model.Enum;
using Model.FuncResult;
using Model.Request.HoaDon;
using Model.Request.ToKhai;
using Model.Request.Xml;
using Model.Respone.Account;
using Model.Respone.HoaDon;
using Model.Static;
using Model.Table;
using Service.Base;
using Service.Caching;
using Service.Helper;
using Service.HoaDon.XuLyThongDiep;
using Service.Hub;
using StackExchange.Redis;
using WebApp;
using System.Xml.Linq;
using WSInterTRCA2;

namespace Service.HoaDon
{
    public class HoaDonService : CRUDService<hoa_don>, IHoaDonService
    {
        private static readonly Dictionary<string, SemaphoreSlim> donViHoaDonLock =
            new Dictionary<string, SemaphoreSlim>();

     

        /// <summary>
        /// giới hạn số request đồng thời đến tvan
        /// </summary>
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(10, 10);


        IHoaDonLogService _hoaDonLogService;
        IHoaDonDangKyPhatHanhService _hoaDonDangKyPhatHanhService;
        HoaDonPhatHanhHub _hoaDonPhatHanhHub;

        public HoaDonService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.HoaDon.HoaDon;
            this._hoaDonLogService = _serviceWrapper.HoaDon.HoaDonLog;
            this._hoaDonDangKyPhatHanhService = _serviceWrapper.HoaDon.HoaDonDangKyPhatHanh;
            this._hoaDonPhatHanhHub = _serviceProvider.GetRequiredService<HoaDonPhatHanhHub>();
        }

        private SemaphoreSlim GetLockForDonVi(string donvi_ma_dv, string hoa_don_dang_ky_phat_hanh_mau_so,
            string hoa_don_dang_ky_phat_hanh_ky_hieu, string taskName)
        {
            var key =
                $"{donvi_ma_dv}_{hoa_don_dang_ky_phat_hanh_mau_so}_{hoa_don_dang_ky_phat_hanh_ky_hieu}_${taskName}";
            lock (donViHoaDonLock)
            {
                if (!donViHoaDonLock.ContainsKey(key))
                {
                    donViHoaDonLock[key] = new SemaphoreSlim(1, 1);
                }

                return donViHoaDonLock[key];
            }
        }

        public async Task<FunctionResult<int>> SaveHoaDonAsync(HoaDonAddOrEditModel model)
        {
            model.CheckAndSetThongTinKhacJson();
            model.ma_so_hoa_don = null;
            model.ma_so_hoa_don_mtt = null;

            var nguoiMuaError = NguoiMuaValidationHelper.ValidateAndNormalize(model);
            if (nguoiMuaError != null)
                return new ErrorResult<int>(nguoiMuaError);

            var user = this.GetCurrentUser();
            var insert = model.id <= 0;
            List<hoa_don_hang_hoa> hangHoaGocs = null;
            if (model.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_DIEU_CHINH
                && model.hoa_don_id_goc > 0
                && model.hoa_don_ly_do_dieu_chinh_id != 20)
            {
                hangHoaGocs = (await _repositoryWrapper.HoaDon.HoaDonHangHoa.SelectByHoaDonIdAsync(model.hoa_don_id_goc)).ToList();
            }
            CalculateThanhTienHoaDon(model, hangHoaGocs);
            var tienbangchu = string.Empty;
            if (model.loai_tien == null) model.loai_tien = "VND";

            if (model.loai_tien == "VND")
            {
                tienbangchu = clsMoneyreader.DocTienBangChu(model.tong_tien_thanh_toan);
            }
            else if (model.loai_tien == "VNĐ")
            {
                tienbangchu = clsMoneyreader.DocTienBangChu(model.tong_tien_thanh_toan);
            }
            else
            {
                tienbangchu = clsMoneyreader.DocTienTheoDonVi(model.tong_tien_thanh_toan, model.loai_tien);
            }


            model.SetInsertInfo(user.id);
            model.tong_tien_chu = tienbangchu;
            if (model.id <= 0)
            {
                if (model.invoice_id.ConvertToString().Trim() != "")
                {
                    model.ma_tra_cuu = model.invoice_id.ConvertToString().Trim();
                    model.invoice_id = model.donvi_ma_dv + "-" + model.invoice_id.ConvertToString().Trim();
                    var checkExisted =
                        await _repositoryWrapper.HoaDon.HoaDon.SelectHoaDonIdByInvoiceIdAsync(model.invoice_id
                            .ConvertToString().Trim());
                    if (checkExisted > 0) return new ErrorResult<int>("Hóa đơn đã tồn tại");
                }

                var donVi = await _serviceWrapper.Category.DonVi.SelectByMaDonViAsync(model.donvi_ma_dv);
                if (donVi == null) return new ErrorResult<int>("Không tìm thấy đơn vị");
                if (donVi.to_khai_success_id.ConvertToInt() == 0)
                    return new ErrorResult<int>("Đơn vị chưa tạo tờ khai");

                // if (donVi.ngay_hoa_don_max != null && model.ngay_hoa_don < donVi.ngay_hoa_don_max)
                //     return new ErrorResult<int>(
                //         $"Ngày hóa đơn phải từ ngày {donVi.ngay_hoa_don_max.Value.ToString("dd/MM/yyyy")}");

                if (model.hoa_don_hinh_thuc_code != "M")
                {
                    var ngayHoaDonError = await ValidateNgayHoaDonKhiLuuAsync(
                        model.donvi_ma_dv,
                        model.hoa_don_dang_ky_phat_hanh_mau_so,
                        model.hoa_don_dang_ky_phat_hanh_ky_hieu,
                        model.ngay_hoa_don,
                        0);
                    if (ngayHoaDonError != null)
                        return ngayHoaDonError;
                }

                //hóa đơn đã thay thế -> không đc điều chỉnh hoặc thay thế
                //hóa đơn đã điều chỉnh -> chỉ được điều chỉnh tiếp
                if (model.IsHoaDonDieuChinhThayThe())
                {
                    var isHoaDonDieuChinh = model.hoa_don_hinh_thuc_id == 3;
                    var isHoaDonThayThe = model.hoa_don_hinh_thuc_id == 2;

                    var hoaDonGoc = await _repositoryWrapper.HoaDon.HoaDon.SelectHoaDonGocAsync(
                        model.donvi_ma_dv,
                        model.hoa_don_dang_ky_phat_hanh_mau_so_goc,
                        model.hoa_don_dang_ky_phat_hanh_ky_hieu_goc,
                        model.ma_so_hoa_don_goc.ConvertToInt()
                    );

                    if (isHoaDonDieuChinh && hoaDonGoc != null)
                    {
                        //chỉ được phép lập điều chỉnh cho hóa đơn gốc, không cho phép điều chỉnh cho hóa đơn điều chỉnh hoặc hóa đơn thay thế.
                        if (hoaDonGoc.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_DIEU_CHINH
                           )
                        {
                            return new ErrorResult<int>(
                                "Không được điều chỉnh cho hóa đơn điều chỉnh.");
                        }
                        if (hoaDonGoc.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_THAY_THE

                         )
                        {
                            return new ErrorResult<int>(
                                "Không được điều chỉnh cho hóa đơn thay thế.");
                        }

                        //-	Kiểm tra trạng thái hóa đơn gốc chưa bị hủy
                        if (hoaDonGoc.hoa_don_trang_thai_id == (int)e_hoa_don_trang_thai.DA_HUY)
                        {
                            return new ErrorResult<int>("Hóa đơn gốc đã hủy.");
                        }

                        //-	Kiểm tra trạng thái hóa đơn gốc chưa bị thay thế
                        if (hoaDonGoc.hoa_don_trang_thai_id == (int)e_hoa_don_hinh_thuc.HOA_DON_BI_THAY_THE)
                        {
                            return new ErrorResult<int>("Hóa đơn gốc đã bị thay thế.");
                        }
                        // //- Kiểm tra hóa đơn này đang có hóa đơn nháp điều chỉnh/thay thế nào chưa
                        // var objCheck = await _repositoryWrapper.HoaDon.HoaDon.SelectHoaDonDieuChinhThayTheChoHoaDonAsync(
                        // model.donvi_ma_dv,
                        // model.hoa_don_dang_ky_phat_hanh_mau_so_goc,
                        // model.hoa_don_dang_ky_phat_hanh_ky_hieu_goc,
                        // model.ma_so_hoa_don_goc.ConvertToInt()
                        // );
                        // if (objCheck != null)
                        // {
                        //     return new ErrorResult<int>($"Đã tồn tại hóa đơn thay thế/ điều chỉnh (id={objCheck.id}) cho hóa đơn gốc.");
                        // }
                    }

                    if (isHoaDonThayThe && hoaDonGoc != null)
                    {
                        //-	Kiểm tra hóa đơn gốc phải là hóa đơn mới hoặc hóa đơn thay thế, không phải là hóa đơn điểu chỉnh
                        if (hoaDonGoc.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_DIEU_CHINH)
                        {
                            return new ErrorResult<int>("Không được thay thế cho hóa đơn điều chỉnh.");
                        }

                        //-	Kiểm tra trạng thái hóa đơn gốc chưa bị hủy
                        if (hoaDonGoc.hoa_don_trang_thai_id == (int)e_hoa_don_trang_thai.DA_HUY)
                        {
                            return new ErrorResult<int>("Hóa đơn gốc đã hủy.");
                        }

                        //-	Kiểm tra hóa đơn gốc chưa bị điều chỉnh hoặc chưa bị thay thế 
                        if (hoaDonGoc.hoa_don_trang_thai_id == (int)e_hoa_don_hinh_thuc.HOA_DON_BI_THAY_THE

                           )
                        {
                            return new ErrorResult<int>("Hóa đơn gốc đã bị thay thế.");
                        }
                        //-	Kiểm tra hóa đơn gốc chưa bị điều chỉnh hoặc chưa bị thay thế 
                        if (hoaDonGoc.hoa_don_trang_thai_id == (int)e_hoa_don_hinh_thuc.HOA_DON_BI_DIEU_CHINH
                           )
                        {
                            return new ErrorResult<int>("Hóa đơn gốc đã bị điều chỉnh.");
                        }
                        //- Kiểm tra hóa đơn này đang có hóa đơn nháp điều chỉnh/thay thế nào chưa
                        var objCheck = await _repositoryWrapper.HoaDon.HoaDon.SelectHoaDonDieuChinhThayTheChoHoaDonAsync(
                        model.donvi_ma_dv,
                        model.hoa_don_dang_ky_phat_hanh_mau_so_goc,
                        model.hoa_don_dang_ky_phat_hanh_ky_hieu_goc,
                        model.ma_so_hoa_don_goc.ConvertToInt()
                        );
                        if (objCheck != null)
                        {
                            return new ErrorResult<int>($"Đã tồn tại hóa đơn thay thế/ điều chỉnh (id={objCheck.id}) cho hóa đơn gốc.");
                        }
                    }
                }

                var CKM = this.GetHoaDonType(model.hoa_don_dang_ky_phat_hanh_ky_hieu);
                model.hoa_don_hinh_thuc_code = CKM;
                //tạo khóa cho đơn vị, mẫu số , ký hiệu -> tránh sinh cùng mã
                var donViHoaDonLock = GetLockForDonVi(model.donvi_ma_dv, model.hoa_don_dang_ky_phat_hanh_mau_so,
                    model.hoa_don_dang_ky_phat_hanh_ky_hieu, "SaveHoaDonAsync");
                var isHoldKey = false;

                try
                {
                    // if (CKM == "M")
                    // {

                    //     await donViHoaDonLock.WaitAsync();
                    //     isHoldKey = true;
                    //     var soHoaDonResult = await this.GetSoHoaDonAsyn(model.donvi_ma_dv, model.hoa_don_dang_ky_phat_hanh_mau_so, model.hoa_don_dang_ky_phat_hanh_ky_hieu);
                    //     if (!soHoaDonResult.is_success)
                    //     {
                    //         return new ErrorResult<int>(soHoaDonResult.message);
                    //     }
                    //     var soHoaDonMTTResult = await this.GetSoHoaDonMTTAsync(model.donvi_ma_dv, model.hoa_don_dang_ky_phat_hanh_mau_so, model.hoa_don_dang_ky_phat_hanh_ky_hieu);
                    //     if (!soHoaDonMTTResult.is_success)
                    //     {
                    //         return new ErrorResult<int>(soHoaDonMTTResult.message);
                    //     }
                    //     model.ma_so_hoa_don = soHoaDonResult.data;
                    //     model.ma_so_hoa_don_mtt = soHoaDonMTTResult.data;
                    // }
                    model.so_hoa_don = model.hoa_don_dang_ky_phat_hanh_mau_so +
                                       model.hoa_don_dang_ky_phat_hanh_ky_hieu + model.ma_so_hoa_don;
                    model.hoa_don_trang_thai_id = (int)e_hoa_don_trang_thai.NHAP;
                    model.hoa_don_hinh_thuc_id = model.hoa_don_hinh_thuc_id;
                    model.hoa_don_ly_do_dieu_chinh_id = model.hoa_don_ly_do_dieu_chinh_id;
                    model.ngay_tao = DateTime.Now;
                    model.nguoi_tao = user.full_name;
                    // model.ngay_hoa_don = DateTime.Now;
                    model.ket_qua_phat_hanh = "";
                    model.nguoi_ban_dia_chi = donVi.dia_chi;
                    model.nguoi_ban_dien_thoai = donVi.dien_thoai;
                    model.nguoi_ban_email = donVi.email;
                    model.nguoi_ban_fax = donVi.fax;
                    model.nguoi_ban_mst = donVi.mst;
                    model.nguoi_ban_ngan_hang = donVi.ngan_hang;
                    model.nguoi_ban_ten_donvi = donVi.ten_dv;
                    model.nguoi_ban_website = donVi.website;
                    model.nguoi_ban_stk = donVi.stk;
                    model.SetInsertInfo(user.id);

                    model.id = await this.InsertAsync(model.Map<hoa_don>());
                }
                finally
                {
                    if (isHoldKey)
                        donViHoaDonLock.Release(); // Giải phóng khóa
                }

                if (model.invoice_id.ConvertToString().Trim() == "")
                {
                    model.ma_tra_cuu = myExtension.CreateMaTraCuu(8);
                    await _repositoryWrapper.HoaDon.HoaDon.UpdateMaTraCuuAsync(model.id, model.ma_tra_cuu);
                }

                var log = new hoa_don_log()
                {
                    file_thong_diep_url = string.Empty,
                    ngay_thuc_hien = DateTime.Now,
                    nguoi_thuc_hien = user.full_name,
                    noi_dung_thuc_hien = "Tạo mới hóa đơn",
                    hoa_don_id = model.id,
                    hoa_don_log_type_id = (int)e_hoa_don_log_type.TAO_MOI
                };
                log.SetInsertInfo(user.id);
                _serviceWrapper.Core.TaskQueue.EnqueueTask(async _ =>
                {
                    await _serviceWrapper.HoaDon.HoaDonLog.InsertAsync(log);
                });
            }
            else
            {
                var obj = await this.SelectByIdAsync(model.id);
                if (obj != null)
                {
                    var donVi = await _serviceWrapper.Category.DonVi.SelectByMaDonViAsync(model.donvi_ma_dv);
                    if (donVi == null) return new ErrorResult<int>("Không tìm thấy đơn vị");
                    if (donVi.to_khai_success_id.ConvertToInt() == 0)
                        return new ErrorResult<int>("Đơn vị chưa tạo tờ khai");
                    // if (donVi.ngay_hoa_don_max != null && model.ngay_hoa_don < donVi.ngay_hoa_don_max)
                    //     return new ErrorResult<int>(
                    //         $"Ngày hóa đơn phải từ ngày {donVi.ngay_hoa_don_max.Value.ToString("dd/MM/yyyy")}");

                    if (model.hoa_don_hinh_thuc_code != "M")
                    {
                        var ngayHoaDonError = await ValidateNgayHoaDonKhiLuuAsync(
                            model.donvi_ma_dv,
                            model.hoa_don_dang_ky_phat_hanh_mau_so,
                            model.hoa_don_dang_ky_phat_hanh_ky_hieu,
                            model.ngay_hoa_don,
                            model.id);
                        if (ngayHoaDonError != null)
                            return ngayHoaDonError;
                    }

                    obj.ngay_hoa_don = model.ngay_hoa_don;
                    obj.nguoi_mua_mst = model.nguoi_mua_mst;
                    obj.nguoi_mua_ten_donvi = model.nguoi_mua_ten_donvi;
                    obj.nguoi_mua_ten = model.nguoi_mua_ten;
                    obj.nguoi_mua_dia_chi = model.nguoi_mua_dia_chi;
                    obj.nguoi_mua_email = model.nguoi_mua_email;
                    obj.nguoi_mua_dien_thoai = model.nguoi_mua_dien_thoai;
                    obj.nguoi_mua_stk = model.nguoi_mua_stk;
                    obj.nguoi_mua_ngan_hang = model.nguoi_mua_ngan_hang;
                    obj.loai_tien = model.loai_tien;
                    obj.hinh_thuc_tt = model.hinh_thuc_tt;
                    obj.ty_gia = model.ty_gia;
                    obj.hoa_don_ly_do_dieu_chinh_id = model.hoa_don_ly_do_dieu_chinh_id;

                    obj.ma_so_hoa_don_goc = model.ma_so_hoa_don_goc;
                    obj.hoa_don_dang_ky_phat_hanh_ky_hieu_goc = model.hoa_don_dang_ky_phat_hanh_ky_hieu_goc;
                    obj.hoa_don_dang_ky_phat_hanh_mau_so_goc = model.hoa_don_dang_ky_phat_hanh_mau_so_goc;
                    obj.ngay_hoa_don_goc = model.ngay_hoa_don_goc;

                    obj.ma_dai_ly = model.ma_dai_ly;
                    obj.ten_dai_ly = model.ten_dai_ly;

                    obj.tong_tien_truong_thue = model.tong_tien_truong_thue;
                    obj.tong_tien_thue = model.tong_tien_thue;
                    obj.tong_tien_phi = model.tong_tien_phi;
                    obj.tong_tien_thanh_toan = model.tong_tien_thanh_toan;
                    obj.tong_tien_chiet_khau = model.tong_tien_chiet_khau;
                    obj.so_tien_tang_giam = model.so_tien_tang_giam;
                    obj.so_tien_tang_giam_tien_hang = model.so_tien_tang_giam_tien_hang;
                    obj.so_tien_tang_giam_tien_thue = model.so_tien_tang_giam_tien_thue;

                    obj.ma_dv_ngan_sach = model.ma_dv_ngan_sach;
                    obj.nguoi_mua_cccd = model.nguoi_mua_cccd;
                    obj.so_ho_chieu = model.so_ho_chieu;



                    obj.giam_thue_phan_tram = model.giam_thue_phan_tram;
                    obj.giam_thue_ty_le = model.giam_thue_ty_le;
                    obj.giam_thue_thanh_tien = model.giam_thue_thanh_tien;
                    obj.ly_do_dieu_chinh = model.ly_do_dieu_chinh;
                   // obj.tong_tien_chu = model.tong_tien_chu;
                    obj.tong_tien_chu = tienbangchu;

                    obj.xuat_kho_dia_chi = model.xuat_kho_dia_chi;
                    obj.xuat_kho_dl_hop_dong_kinh_te_so = model.xuat_kho_dl_hop_dong_kinh_te_so;
                    obj.xuat_kho_dl_hop_dong_ngay = model.xuat_kho_dl_hop_dong_ngay;
                    obj.xuat_kho_hop_dong_so = model.xuat_kho_hop_dong_so;
                    obj.xuat_kho_nguoi_van_chuyen = model.xuat_kho_nguoi_van_chuyen;
                    obj.xuat_kho_nguoi_xuat_hang = model.xuat_kho_nguoi_xuat_hang;
                    obj.xuat_kho_phuong_tien_van_chuyen = model.xuat_kho_phuong_tien_van_chuyen;
                    obj.xuat_kho_vc_lenh_dieu_dong_noi_bo = model.xuat_kho_vc_lenh_dieu_dong_noi_bo;


                    obj.nguoi_ban_dia_chi = donVi.dia_chi;
                    obj.nguoi_ban_dien_thoai = donVi.dien_thoai;
                    obj.nguoi_ban_email = donVi.email;
                    obj.nguoi_ban_fax = donVi.fax;
                    obj.nguoi_ban_mst = donVi.mst;
                    obj.nguoi_ban_ngan_hang = donVi.ngan_hang;
                    obj.nguoi_ban_ten_donvi = donVi.ten_dv;
                    obj.nguoi_ban_website = donVi.website;
                    obj.nguoi_ban_stk = donVi.stk;

                    //          tong_tien_truong_thue: (tongTienData?.tong_thanh_tien ?? 0) - (tongTienData?.vats_total ?? 0),
                    // tong_tien_thue: (tongTienData?.vats_total ?? 0),
                    // tong_tien_phi: loaiPhis.map(x => x.so_tien).reduce((a, b) => a + b, 0),
                    // tong_tien_thanh_toan: (tongTienData?.tong_thanh_tien ?? 0),
                    obj.SetUpdateInfo(user.id);

                    //
                    //

                    await this.UpdateAsync(obj);
                    var log = new hoa_don_log()
                    {
                        file_thong_diep_url = string.Empty,
                        ngay_thuc_hien = DateTime.Now,
                        nguoi_thuc_hien = user.full_name,
                        noi_dung_thuc_hien = "Cập nhật hóa đơn",
                        hoa_don_id = model.id,
                        hoa_don_log_type_id = (int)e_hoa_don_log_type.CAP_NHAT
                    };
                    log.SetInsertInfo(user.id);

                    _serviceWrapper.Core.TaskQueue.EnqueueTask(async _ =>
                    {
                        await _serviceWrapper.HoaDon.HoaDonLog.InsertAsync(log);
                    });
                }
            }

            await SaveHoaDonThongTinBoSung(model, user.id);

            await SaveThueSuatHoaDon(model, user.id);
            await this.SaveHangHoas(model, user.id, !insert);
            await this.SaveLoaiPhis(model, user.id, !insert);

            _serviceWrapper.Core.TaskQueue.EnqueueTask(async _ => { await this.SaveKhachHangToDanhMucAsync(model); });
            if (model.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_THAY_THE ||
            model.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_DIEU_CHINH
            )
            {
                await this.CreateXmlBienBanAsync(model.id);
            }
            return new SuccessResult<int>("", model.id);
        }
        private async Task<FunctionResult<string>> CreateXmlBienBanAsync(int hoaDonId)
        {
            var user = this.GetCurrentUser();
            var hoaDon = await _serviceWrapper.HoaDon.HoaDon.SelectByIdAsync(hoaDonId);
            if (hoaDon == null) return new ErrorResult<string>("Dữ liệu không hợp lệ");
            var donVi = await _serviceWrapper.Category.DonVi.SelectByMaDonViAsync(hoaDon.donvi_ma_dv);
            if (donVi == null) return new ErrorResult<string>("Dữ liệu không hợp lệ");
            string kq = "";
            //Tao thong tin XML chung
            string linkelement = "";
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", "");
            doc.AppendChild(docNode);


            var BBan = doc.CreateElement("", "BBan", linkelement);
            doc.AppendChild(BBan);

            var NDBBan = doc.CreateElement("", "NDBBan", linkelement);
            XmlAttribute productAttribute = doc.CreateAttribute("Id");
            productAttribute.Value = "_" + hoaDonId.ToString();
            NDBBan.Attributes.Append(productAttribute);
            BBan.AppendChild(NDBBan);

            var DSCKS = doc.CreateElement("", "DSCKS", linkelement);


            var NBAN = doc.CreateElement("", "NBan", linkelement);
            DSCKS.AppendChild(NBAN);

            var NMUA = doc.CreateElement("", "NMua", linkelement);
            DSCKS.AppendChild(NMUA);
            BBan.AppendChild(DSCKS);


            var TTChung = doc.CreateElement("", "TTChung", linkelement);
            NDBBan.AppendChild(TTChung);

            var PBan = doc.CreateElement("", "PBan", linkelement);
            PBan.AppendChild(doc.CreateTextNode("2.1.0"));
            TTChung.AppendChild(PBan);

            var TBBan = doc.CreateElement("", "TBBan", linkelement);
            TBBan.AppendChild(doc.CreateTextNode("BIÊN BẢN ĐIỀU CHỈNH HÓA ĐƠN"));
            TTChung.AppendChild(TBBan);

            var SBBan = doc.CreateElement("", "SBBan", linkelement);
            SBBan.AppendChild(doc.CreateTextNode(hoaDon.so_hoa_don.ToString()));
            TTChung.AppendChild(SBBan);

            var NBBan = doc.CreateElement("", "NBBan", linkelement);
            NBBan.AppendChild(doc.CreateTextNode(hoaDon.ngay_hoa_don.ToString("yyyy-MM-dd")));
            TTChung.AppendChild(NBBan);

            var TCHDon = doc.CreateElement("", "TCHDon", linkelement);
            //Số (1: Thay thế, 2: Điều chỉnh)
            if (hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_THAY_THE)
                TCHDon.AppendChild(doc.CreateTextNode("1"));
            if (hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_DIEU_CHINH)
                TCHDon.AppendChild(doc.CreateTextNode("2"));
            TTChung.AppendChild(TCHDon);

            var NBan = doc.CreateElement("", "NBan", linkelement);
            NBan.AppendChild(doc.CreateTextNode(donVi.ten_dv));
            TTChung.AppendChild(NBan);

            var MSTNBan = doc.CreateElement("", "MSTNBan", linkelement);
            MSTNBan.AppendChild(doc.CreateTextNode(donVi.ma_dv));
            TTChung.AppendChild(MSTNBan);

            var DCNban = doc.CreateElement("", "DCNBan", linkelement);
            DCNban.AppendChild(doc.CreateTextNode(donVi.dia_chi));
            TTChung.AppendChild(DCNban);

            // Xác định giá trị tên người mua
            string tenNguoiMua = !string.IsNullOrWhiteSpace(hoaDon.nguoi_mua_mst)
        ? (string.IsNullOrWhiteSpace(hoaDon.nguoi_mua_ten_donvi) ? "" : hoaDon.nguoi_mua_ten_donvi)
        : (string.IsNullOrWhiteSpace(hoaDon.nguoi_mua_ten) ? "" : hoaDon.nguoi_mua_ten);

            var NMua = doc.CreateElement("", "NMua", linkelement);
            NMua.AppendChild(doc.CreateTextNode(tenNguoiMua));
            TTChung.AppendChild(NMua);

            var MSTNMua = doc.CreateElement("", "MSTNMua", linkelement);
            MSTNMua.AppendChild(doc.CreateTextNode(hoaDon.nguoi_mua_mst));
            TTChung.AppendChild(MSTNMua);

            var DCNMua = doc.CreateElement("", "DCNMua", linkelement);
            DCNMua.AppendChild(doc.CreateTextNode(hoaDon.nguoi_mua_dia_chi));
            TTChung.AppendChild(DCNMua);

            var KHMSHDon = doc.CreateElement("", "KHMSHDon", linkelement);
            KHMSHDon.AppendChild(doc.CreateTextNode(hoaDon.hoa_don_dang_ky_phat_hanh_mau_so_goc));
            TTChung.AppendChild(KHMSHDon);

            var KHHDon = doc.CreateElement("", "KHHDon", linkelement);
            KHHDon.AppendChild(doc.CreateTextNode(hoaDon.hoa_don_dang_ky_phat_hanh_ky_hieu_goc));
            TTChung.AppendChild(KHHDon);

            var SHDon = doc.CreateElement("", "SHDon", linkelement);
            SHDon.AppendChild(doc.CreateTextNode(hoaDon.ma_so_hoa_don_goc.ToString()));
            TTChung.AppendChild(SHDon);

            var NLHDGoc = doc.CreateElement("", "NLHDGoc", linkelement);
            NLHDGoc.AppendChild(doc.CreateTextNode(hoaDon.ngay_hoa_don_goc?.ToString("yyyy-MM-dd")));
            TTChung.AppendChild(NLHDGoc);

            var DSLDTDoi = doc.CreateElement("", "DSLDTDoi", linkelement);
            TTChung.AppendChild(DSLDTDoi);

            var LDo = doc.CreateElement("", "LDo", linkelement);
            var lyDoDieuChinh = hoaDon.ly_do_dieu_chinh.ConvertToString();
            // if (hoaDon.hoa_don_ly_do_dieu_chinh_id == (int)e_hoa_don_ly_do_dieu_chinh.DIEU_CHINH_GIAM)
            //     lyDoDieuChinh = "Điều chỉnh giảm";
            // if (hoaDon.hoa_don_ly_do_dieu_chinh_id == (int)e_hoa_don_ly_do_dieu_chinh.DIEU_CHINH_TANG)
            //     lyDoDieuChinh = "Điều chỉnh tăng";
            // if (hoaDon.hoa_don_ly_do_dieu_chinh_id == (int)e_hoa_don_ly_do_dieu_chinh.DIEU_CHINH_THONG_TIN)
            //     lyDoDieuChinh = "Điều chỉnh thông tin";
            // if (hoaDon.hoa_don_ly_do_dieu_chinh_id == (int)e_hoa_don_ly_do_dieu_chinh.DIEU_CHINH_THUE)
            //     lyDoDieuChinh = "Điều chỉnh thuế";
            LDo.AppendChild(doc.CreateTextNode(lyDoDieuChinh));
            DSLDTDoi.AppendChild(LDo);
            kq = doc.InnerXml;
            var fileName = Guid.NewGuid().ToString() + ".xml";
            var filePath = $"Xml/{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{fileName}";
            var directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            await File.WriteAllTextAsync(filePath, kq);
            var log = new hoa_don_log()
            {
                file_thong_diep_url = filePath,
                ngay_thuc_hien = DateTime.Now,
                nguoi_thuc_hien = user.full_name,
                noi_dung_thuc_hien = "Tạo XML Biên bản",
                hoa_don_id = hoaDon.id,
                hoa_don_log_type_id = (int)e_hoa_don_log_type.TAO_XML_BIEN_BAN
            };
            log.SetInsertInfo(user.id);
            _serviceWrapper.Core.TaskQueue.EnqueueTask(async _ =>
            {
                await _serviceWrapper.HoaDon.HoaDonLog.InsertAsync(log);
            });
            log.SetInsertInfo(user.id);
            await _serviceWrapper.HoaDon.HoaDonLog.InsertAsync(log);

            return new SuccessResult<string>(string.Empty, kq);
        }
        private async Task<bool> SaveHangHoas(HoaDonAddOrEditModel model, int user_id, bool isCheckUpdateDelete)
        {
            var hoaDonHangHoas = isCheckUpdateDelete
                ? await _serviceWrapper.HoaDon.HoaDonHangHoa.SelectByHoaDonIdAsync(model.id)
                : new List<hoa_don_hang_hoa>();
            var hoaDonHangHoaDbIds = hoaDonHangHoas.Select(x => x.id).ToList();
            var hoaDonHangHoaNewIds = model.hoang_hoas.Select(x => x.id).ToList();

            var hangHoaInserts = model.hoang_hoas.Where(x => x.id == 0).ToList();
            var hangHoaDeletes = hoaDonHangHoas.Where(x => !hoaDonHangHoaNewIds.Contains(x.id));
            var hangHoaUpdates = model.hoang_hoas.Where(x => x.id > 0).ToList();

            foreach (var item in hangHoaInserts)
            {
                item.hoa_don_id = model.id;
                item.SetInsertInfo(model.last_modified_user_id);
                item.id = await _serviceWrapper.HoaDon.HoaDonHangHoa.InsertAsync(item);
            }

            foreach (var item in hangHoaDeletes)
            {
                await _serviceWrapper.HoaDon.HoaDonHangHoa.DeleteAsync(item.id);
            }

            foreach (var item in hangHoaUpdates)
            {
                var hangHoa = hoaDonHangHoas.Where(x => x.id == item.id).FirstOrDefault();
                if (hangHoa != null)
                {
                    hangHoa.hang_hoa_tinh_chat_id = item.hang_hoa_tinh_chat_id;
                    hangHoa.stt = item.stt;
                    hangHoa.ma_hang = item.ma_hang;
                    hangHoa.ten_hang = item.ten_hang;
                    hangHoa.dvt = item.dvt;
                    hangHoa.so_luong = item.so_luong;
                    hangHoa.don_gia = item.don_gia;
                    hangHoa.ty_le_chiet_khau = item.ty_le_chiet_khau;
                    hangHoa.tien_chiet_khau = item.tien_chiet_khau;
                    hangHoa.thanh_tien = item.thanh_tien;
                    hangHoa.thue_vat = item.thue_vat;
                    hangHoa.hang_hoa_dac_trung_json = item.hang_hoa_dac_trung_json;
                    hangHoa.SetUpdateInfo(user_id);
                    await _serviceWrapper.HoaDon.HoaDonHangHoa.UpdateAsync(hangHoa);
                }
            }

            // _ = Task.Run(() => this.SaveHangHoaToDanhMucAsync(model));
            _serviceWrapper.Core.TaskQueue.EnqueueTask(async _ => { await this.SaveHangHoaToDanhMucAsync(model); });
            return true;
        }

        private async Task<bool> SaveHangHoaToDanhMucAsync(HoaDonAddOrEditModel model)
        {
            try
            {
                var maHangs = model.hoang_hoas.Where(x => x.ma_hang != null && x.ma_hang != "").Select(x => x.ma_hang)
                    .ToList();
                var donViHangHoas =
                    await _serviceWrapper.Category.HangHoa.SelectByDonViAsync(model.donvi_ma_dv, maHangs);
                // var listInsert = 
                foreach (var maHang in maHangs)
                {
                    var donViHangHoaMaHang = donViHangHoas.Where(x => x.ma_hang_hoa == maHang).FirstOrDefault();
                    var maHangInfo = model.hoang_hoas.Where(x => x.ma_hang == maHang).LastOrDefault();
                    if (maHangInfo != null)
                    {
                        if (donViHangHoaMaHang != null)
                        {
                            donViHangHoaMaHang.ma_hang_hoa = maHangInfo.ma_hang;
                            donViHangHoaMaHang.ten_hang_hoa = maHangInfo.ten_hang;
                            donViHangHoaMaHang.dvt = maHangInfo.dvt;
                            donViHangHoaMaHang.don_gia = maHangInfo.don_gia;
                            donViHangHoaMaHang.ma_loai_hoang_hoa = maHangInfo.hang_hoa_tinh_chat_id.ToString();
                            donViHangHoaMaHang.SetUpdateInfo(model.last_modified_user_id);
                            await _serviceWrapper.Category.HangHoa.UpdateAsync(donViHangHoaMaHang);
                        }
                        else
                        {
                            var obj = new dm_hanghoa()
                            {
                                donvi_ma_dv = model.donvi_ma_dv,
                                ma_hang_hoa = maHangInfo.ma_hang,
                                ma_loai_hoang_hoa = maHangInfo.hang_hoa_tinh_chat_id.ToString(),
                                ten_hang_hoa = maHangInfo.ten_hang,
                                don_gia = maHangInfo.don_gia,
                                dvt = maHangInfo.dvt
                            };
                            obj.SetInsertInfo(model.last_modified_user_id);
                            obj.id = await _serviceWrapper.Category.HangHoa.InsertAsync(obj);
                        }
                    }
                }

                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        private async Task<bool> SaveKhachHangToDanhMucAsync(HoaDonAddOrEditModel model)
        {
            try
            {
                // var maHangs = model.hoang_hoas.Where(x => x.ma_hang != null && x.ma_hang != "").Select(x => x.ma_hang).ToList();
                var khachHangService = _serviceWrapper.Category.KhachHang;
                if (model.nguoi_mua_mst.ConvertToString() != "")
                {
                    var obj = await khachHangService.SelectByDonViAsync(model.donvi_ma_dv, model.nguoi_mua_mst);
                    if (obj != null)
                    {
                        obj.ten_khach_hang = model.nguoi_mua_ten;
                        obj.ten_don_vi = model.nguoi_mua_ten_donvi.ConvertToString();
                        obj.dia_chi = model.nguoi_mua_dia_chi;

                        obj.stk = model.nguoi_mua_stk;
                        obj.mst = model.nguoi_mua_mst;
                        obj.email = model.nguoi_mua_email;
                        // obj.p
                        obj.SetUpdateInfo(model.last_modified_user_id);
                        await khachHangService.UpdateAsync(obj);
                    }
                    else
                    {
                        obj = new khachhang();
                        obj.donvi_ma_dv = model.donvi_ma_dv;
                        obj.ten_khach_hang = model.nguoi_mua_ten;
                        obj.ten_don_vi = model.nguoi_mua_ten_donvi.ConvertToString();
                        obj.dia_chi = model.nguoi_mua_dia_chi;
                        obj.stk = model.nguoi_mua_stk;
                        obj.mst = model.nguoi_mua_mst;
                        obj.email = model.nguoi_mua_email;
                        obj.SetInsertInfo(model.last_modified_user_id);
                        await khachHangService.InsertAsync(obj);
                    }
                }

                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        private async Task<bool> SaveLoaiPhis(HoaDonAddOrEditModel model, int user_id, bool isCheckUpdateDelete)
        {
            var hoaDonLoaiPhis = isCheckUpdateDelete
                ? await _serviceWrapper.HoaDon.HoaDonLoaiPhi.SelectByHoaDonAsync(model.id)
                : new List<hoa_don_loai_phi>();
            var hoaDonLoaiPhiDbIds = hoaDonLoaiPhis.Select(x => x.id).ToList();
            var hoaDonLoaiPhiNewIds = model.loai_phis.Select(x => x.id).ToList();

            var loaiPhiInserts = model.loai_phis.Where(x => x.id == 0).ToList();
            var loaiPhiDeletes = hoaDonLoaiPhis.Where(x => !hoaDonLoaiPhiNewIds.Contains(x.id));
            var loaiPhiUpdates = model.loai_phis.Where(x => x.id > 0).ToList();

            foreach (var item in loaiPhiInserts)
            {
                item.hoa_don_id = model.id;
                item.SetInsertInfo(model.last_modified_user_id);
                item.id = await _serviceWrapper.HoaDon.HoaDonLoaiPhi.InsertAsync(item);
            }

            foreach (var item in loaiPhiDeletes)
            {
                await _serviceWrapper.HoaDon.HoaDonLoaiPhi.DeleteAsync(item.id);
            }

            foreach (var item in loaiPhiUpdates)
            {
                var loaiPhi = hoaDonLoaiPhis.Where(x => x.id == item.id).FirstOrDefault();
                if (loaiPhi != null)
                {
                    loaiPhi.ten_le_phi = item.ten_le_phi;
                    loaiPhi.so_tien = item.so_tien;
                    loaiPhi.stt = item.stt;
                    loaiPhi.SetUpdateInfo(user_id);
                    await _serviceWrapper.HoaDon.HoaDonLoaiPhi.UpdateAsync(loaiPhi);
                }
            }

            return true;
        }


        private async Task<bool> SaveThueSuatHoaDon(HoaDonAddOrEditModel model, int user_id)
        {
            if (model.hoang_hoas == null || !model.hoang_hoas.Any()) return true;

            var isAllChietKhau = !model.hoang_hoas.Where(x => x.hang_hoa_tinh_chat_id != 4)
                                                  .Any(x => x.hang_hoa_tinh_chat_id != 3);

            var dsThue = model.hoang_hoas
                .Where(x => x.hang_hoa_tinh_chat_id != 4)
                .Where(x => !string.IsNullOrEmpty(x.thue_vat) &&
                           (x.thue_vat.Contains("%") || x.thue_vat == "KCT" || x.thue_vat == "KKKNT"))
                .GroupBy(x => x.thue_vat)
                .Select(g =>
                {
                    string tenThue = g.Key;
                    double phanTram = tenThue.Replace("KHAC:", "").Replace("%", "").Trim().ConvertToDouble(2);

                    var thanh_tien = g.Where(x => x.hang_hoa_tinh_chat_id == 1 || x.hang_hoa_tinh_chat_id == 5).Sum(x => x.thanh_tien);
                    var thanh_tien_ck = g.Where(x => x.hang_hoa_tinh_chat_id == 3).Sum(x => x.thanh_tien);
                    if (model.loai_tien != null && model.loai_tien.ToUpper() == "VND")
                    {
                        thanh_tien = Math.Round(thanh_tien, 0, MidpointRounding.AwayFromZero);
                        thanh_tien_ck = Math.Round(thanh_tien_ck, 0, MidpointRounding.AwayFromZero);
                    }

                    if (isAllChietKhau) thanh_tien = -1 * thanh_tien;

                    decimal thTienFinal = (phanTram == 0) ? thanh_tien : (thanh_tien - thanh_tien_ck);

                    decimal tThueFinal;
                    if (phanTram == 0)
                    {
                        tThueFinal = (decimal)thanh_tien * (decimal)phanTram / 100;
                    }
                    else
                    {
                        tThueFinal = ((decimal)thanh_tien - (decimal)thanh_tien_ck) * (decimal)phanTram / 100;
                    }

                    // Làm tròn nếu là VND
                    if (model.loai_tien != null && model.loai_tien.ToUpper() == "VND")
                    {
                        thTienFinal = Math.Round(thTienFinal, 0, MidpointRounding.AwayFromZero);
                        tThueFinal = Math.Round(tThueFinal, 0, MidpointRounding.AwayFromZero);
                    }

                    return new ThueSuatModel
                    {
                        TSuat = tenThue,
                        ThTien = thTienFinal,
                        TThue = tThueFinal
                    };
                }).ToList();

            // Logic 5 đồng tăng giảm (Chỉ áp dụng nếu có 1 mức thuế duy nhất)
            if (dsThue.Count == 1 && model.so_tien_tang_giam_tien_thue != 0)
            {
                dsThue[0].TThue += model.so_tien_tang_giam_tien_thue;
                // Nếu là VND thì làm tròn lại lần nữa
                if (model.loai_tien != null && model.loai_tien.ToUpper() == "VND")
                {
                    dsThue[0].TThue = Math.Round(dsThue[0].TThue, 0, MidpointRounding.AwayFromZero);
                }
            }

            return await _serviceWrapper.HoaDon.HoaDon.InsertThueSuatHoaDonAsync(model.id, dsThue);
        }


        public async Task<bool> InsertThueSuatHoaDonAsync(int hoaDonId, List<ThueSuatModel> dsThue)
        {
            return await _repositoryWrapper.HoaDon.HoaDon.InsertThueSuatHoaDonAsync(hoaDonId, dsThue);
        }

        private async Task<bool> SaveHoaDonThongTinBoSung(HoaDonAddOrEditModel model, int user_id)
        {
            // HoaDonThongTinBoSung
            var infor = new HoaDonThongTinBoSung()
            {
                IsHdBanTaiSanCong = model.IsHdBanTaiSanCong ?? 0,
                SoQuyetDinh = model.SoQuyetDinh ?? "",
                NgayQuyetDinh = model.NgayQuyetDinh ?? "",
                CoQuanBanHanhQD = model.CoQuanBanHanhQD ?? "",
                HinhThucBan = model.HinhThucBan ?? "",
                DiaDiemVCHangDen = model.DiaDiemVCHangDen ?? "",
                TgianVCHangDenTu = model.TgianVCHangDenTu ?? "",
                TgianVCHangDenDen = model.TgianVCHangDenDen ?? "",
                IsHdPhiThueQuan = model.IsHdPhiThueQuan ?? 0,
            };


            return await _serviceWrapper.HoaDon.HoaDon.InsertHoaDonThongTinBoSungAsync(model.id, infor);
        }


        public async Task<bool> InsertHoaDonThongTinBoSungAsync(int hoaDonId, HoaDonThongTinBoSung infor)
        {
            return await _repositoryWrapper.HoaDon.HoaDon.InsertHoaDonThongTinBoSungAsync(hoaDonId, infor);
        }



        public Task<PagingResult<IEnumerable<hoa_don_vm>>> SelectByDonViAsync(string donvi_ma_dv,
            HoaDonSelectPagingRequest pagingRequest)
        {
            return _repositoryWrapper.HoaDon.HoaDon.SelectByDonViAsync(donvi_ma_dv, pagingRequest);
        }
        public async Task<hoa_don_vm> SelectViewModelAsync(int id)
        {
            var obj = await this.SelectByIdAsync(id);
            if (obj != null)
            {

                var item = obj.Map<hoa_don_vm>();
                item.hang_hoas = (await _serviceWrapper.HoaDon.HoaDonHangHoa.SelectByHoaDonIdAsync(id)).ToList();
                item.loai_phis = (await _serviceWrapper.HoaDon.HoaDonLoaiPhi.SelectByHoaDonAsync(id)).ToList();

                if (obj.hoa_don_dang_ky_phat_hanh_mau_so == "2" || obj.hoa_don_dang_ky_phat_hanh_mau_so == "3")
                {
                    item.thong_tin_bo_sungs = await _repositoryWrapper.HoaDon.HoaDon.SelectHoaDonThongTinBoSungByHoaDonIdAsync(obj.id);
                }


                return item;
            }

            return null;
        }
        public async Task<FunctionResult<string>> CreateBase64MTTAsync(int id)
        {
            var hoaDon = await this.SelectByIdAsync(id);
            if (hoaDon != null)
            {
                return await this.CreateBase64MTTAsync(hoaDon);
            }

            return new ErrorResult<string>();
        }

        public async Task<FunctionResult<string>> CreateBase64MTTAsync(hoa_don hoaDon)
        {
            if (hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_DA_HUY_NOI_BO)
            {
                return new ErrorResult<string>("Hóa đơn đã hủy nội bộ");
            }
            var modelResult = await this.CreateXmlObjectKySoAsync(hoaDon);
            if (!modelResult.is_success)
            {
                return new ErrorResult<string>(modelResult.message);
            }

            var userId = this.GetCurrentUserId();
            var hoaDonXml = modelResult.data.ConvertToXml();
            var uuid = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
            await _serviceWrapper.Cache.SetDataAsync<string>(uuid, "hoa_don", DateTime.Now.AddDays(30));
            await _repositoryWrapper.HoaDon.PhatHanhUUID.SaveLogUuidAsync(uuid, "hoa_don", userId);

            var thongDiep = new ThongDiep()
            {
                ThongTinChung = new ThongTinChungThongDiep()
                {
                    phien_ban = hoaDon.phien_ban,
                    ma_noi_gui = AppSettings.FixedValue.MNGui,
                    ma_noi_nhan = AppSettings.FixedValue.MNNhan,
                    thong_diep = "206",
                    ma_noi_gui_uuid = $"{AppSettings.FixedValue.MNGui}{uuid}".ToUpper(),
                    ma_thong_diep_tham_chieu = $"",
                    mst = hoaDon.nguoi_ban_mst,
                    so_luong = 1
                },
            };
            hoaDon.phat_hanh_uuid = uuid;
            hoaDon.user_id_phathanh = userId;
            await this.UpdateAsync(hoaDon);
            await _serviceWrapper.Cache.SetDataAsync<hoa_don>(uuid + "_hoa_don", hoaDon, DateTime.Now.AddDays(30));
            var base64thongdiep = thongDiep.ConvertToXmlAndAppendChild("/TDiep", "DLieu", hoaDonXml, false,
                System.Xml.NewLineHandling.None, true, $"_{uuid}").ConvertToBase64();
            // LogWriter.Writer(base64thongdiep, "CreateBase64MTTAsync", "");
            return new SuccessResult<string>(base64thongdiep);
        }

        public async Task<FunctionResult<Model.Request.Xml.HoaDon>> CreateXmlObjectKySoAsync(int id, bool isPreview = false)
        {
            var obj = await this.SelectByIdAsync(id);
            return await this.CreateXmlObjectKySoAsync(obj, isPreview);
        }

        public async Task<FunctionResult<Model.Request.Xml.HoaDon>> CreateXmlObjectKySoAsync(hoa_don hoaDon, bool isPreview = false)
        {
            var obj = hoaDon;
            var user_id = this.GetCurrentUserId();
            if (obj == null) return null;
            var donViHoaDonLock = !isPreview ? GetLockForDonVi(obj.donvi_ma_dv, obj.hoa_don_dang_ky_phat_hanh_mau_so,
                obj.hoa_don_dang_ky_phat_hanh_ky_hieu, "CreateXmlObjectKySoAsync") : null;
            var isHoldKey = false;
            try
            {
                if (donViHoaDonLock != null)
                {
                    await donViHoaDonLock.WaitAsync();
                    isHoldKey = true;
                }
                var isUpdated = true;
                if (!isPreview)
                {
                    var objDb = await this.SelectByIdAsync(hoaDon.id);
                    if (objDb != null && objDb.ma_so_hoa_don.ConvertToInt() > 0)
                    {
                        obj.ma_so_hoa_don = objDb.ma_so_hoa_don;
                        obj.so_hoa_don = objDb.so_hoa_don;
                    }
                    else if (objDb != null && objDb.ma_so_hoa_don.ConvertToString() == "")
                    {
                        if (obj.ma_so_hoa_don.ConvertToString() == "")
                        {
                            var soHoaDonResult = await this.GetSoHoaDonAsyn(obj.donvi_ma_dv,
                                obj.hoa_don_dang_ky_phat_hanh_mau_so, obj.hoa_don_dang_ky_phat_hanh_ky_hieu);
                            if (!soHoaDonResult.is_success)
                            {
                                return new ErrorResult<Model.Request.Xml.HoaDon>("Không sinh được số hóa đơn");
                            }

                            obj.ma_so_hoa_don = soHoaDonResult.data;
                            obj.so_hoa_don = obj.hoa_don_dang_ky_phat_hanh_mau_so + obj.hoa_don_dang_ky_phat_hanh_ky_hieu +
                                             obj.ma_so_hoa_don;
                        }
                    }

                    var CKM = this.GetHoaDonType(obj.hoa_don_dang_ky_phat_hanh_ky_hieu);
                    if (CKM == "M" && obj.ma_so_hoa_don_mtt.ConvertToString() == "")
                    {
                        var soHoaDonMTTResult = await this.GetSoHoaDonMTTAsync(obj.donvi_ma_dv,
                            obj.hoa_don_dang_ky_phat_hanh_mau_so, obj.hoa_don_dang_ky_phat_hanh_ky_hieu, obj.id);
                        if (!soHoaDonMTTResult.is_success)
                        {
                            return new ErrorResult<Model.Request.Xml.HoaDon>(soHoaDonMTTResult.message);
                        }

                        obj.ma_so_hoa_don_mtt = soHoaDonMTTResult.data;
                    }

                    var validateSoKySo = await this.ValidateKhongCoSoHoaDonNhoHonChuaKySoAsync(obj);
                    if (!validateSoKySo.is_success)
                    {
                        return new ErrorResult<Model.Request.Xml.HoaDon>(validateSoKySo.message);
                    }

                    obj.SetUpdateInfo(user_id);
                    isUpdated = await this.UpdateAsync(obj);
                }

                if (isHoldKey)
                {
                    try
                    {
                        if (donViHoaDonLock != null) donViHoaDonLock.Release();
                        isHoldKey = false;
                    }
                    finally
                    {
                    }
                }

                if (!isUpdated)
                {
                    return new ErrorResult<Model.Request.Xml.HoaDon>("Không lưu được số hóa đơn");
                }
            }
            finally
            {
                if (isHoldKey)
                {
                    if (donViHoaDonLock != null) donViHoaDonLock.Release(); // Giải phóng khóa nếu đã giữ khóa
                }
            }


            var model = await this.CreateHoaDonXmlObject(obj);

            return new SuccessResult<Model.Request.Xml.HoaDon>(model);
        }


        private async Task<Model.Request.Xml.HoaDon> CreateHoaDonXmlObject(hoa_don obj)
        {

            var thueSuatTuDb = await _repositoryWrapper.HoaDon.HoaDon.SelectThueSuatHoaDonByHoaDonIdAsync(obj.id);

            var hangHoas = await _serviceWrapper.HoaDon.HoaDonHangHoa.SelectByHoaDonIdAsync(obj.id);

            var thue_suats = thueSuatTuDb.Select(x => new LTSuat()
            {
                ten_thue_suat = x.TSuat, // Bê nguyên từ DB lên (đã là "8%", "KCT"...)
                thanh_tien = x.ThTien.ConvertToStringAndRemoveZeroPart(),
                tien_thue = x.TThue.ConvertToStringAndRemoveZeroPart()
            }).ToList();

            var loaiPhis = await _serviceWrapper.HoaDon.HoaDonLoaiPhi.SelectByHoaDonAsync(obj.id);
            var isApDungDieuChinh5DongVaoThueSuat = thue_suats.Count == 1;
            var isAllChietKhau = !hangHoas.Where(x => x.hang_hoa_tinh_chat_id != 4).Any(x => x.hang_hoa_tinh_chat_id != 3);

            var tong_tien_thanh_toan_bang_so = obj.tong_tien_thanh_toan;
            ;
            if (obj.loai_tien == "VND")
            {
                // tong_tien_thanh_toan_bang_so = (obj.tong_tien_truong_thue + obj.tong_tien_thue).ConvertToDouble(0).ConvertToDecimal();
            }
            // var test = obj.nguoi_mua_mst.ConvertToString() != ""
            //                ? (obj.nguoi_mua_ten_donvi.ConvertToString() != "" ? obj.nguoi_mua_ten_donvi.ConvertToString() : obj.nguoi_mua_ten)
            //                : obj.nguoi_mua_ten_donvi;


            var thongTinThanhToan = new Model.Request.Xml.ThongTinThanhToan()
            {
                tong_tien_thanh_toan_bang_chu = "",
                tong_tien_thanh_toan_bang_so = tong_tien_thanh_toan_bang_so.ConvertToStringAndRemoveZeroPart(),
                tong_tien_chiet_khau = obj.tong_tien_chiet_khau.ConvertToStringAndRemoveZeroPart(),
                thong_tin_phis = loaiPhis.Count() > 0
                              ? new DSLPhi()
                              {
                                  loai_phis = loaiPhis.Select(lp =>
                                  {
                                      return new LPhi()
                                      {
                                          ten_loai_phi = lp.ten_le_phi,
                                          tien_phi = lp.so_tien.ConvertToStringAndRemoveZeroPart()
                                      };
                                  }).ToList()
                              }
                              : null
            };

            // --- NẾU MẪU SỐ KHÁC 2 THÌ THÊM THUẾ SUẤT ---
            if (obj.hoa_don_dang_ky_phat_hanh_mau_so != "2")
            {
                thongTinThanhToan.thong_tin_thue_suat = new Model.Request.Xml.THTTLTSuat()
                {
                    thue_suats = thue_suats
                };
                if (thueSuatTuDb.Any())
                {
                    thongTinThanhToan.tong_tien_chua_thue = thueSuatTuDb.Sum(x => x.ThTien)
                        .ConvertToStringAndRemoveZeroPart();
                    thongTinThanhToan.tong_tien_thue = thueSuatTuDb.Sum(x => x.TThue)
                        .ConvertToStringAndRemoveZeroPart();
                }
                else
                {
                    thongTinThanhToan.tong_tien_chua_thue = obj.tong_tien_truong_thue.ConvertToStringAndRemoveZeroPart();
                    thongTinThanhToan.tong_tien_thue = obj.tong_tien_thue.ConvertToStringAndRemoveZeroPart();
                }
            }

            var model = new Model.Request.Xml.HoaDon()
            {
                du_lieu_hoa_don = new Model.Request.Xml.DuLieuHoaDon()
                {
                    id = "_" + obj.id.ToString(),
                    thong_tin_chung = new Model.Request.Xml.ThongTinChung()
                    {
                        phien_ban = "2.1.0",
                        ten_hoa_don = obj.ten_hoa_don,
                        ky_hieu_mau_so_hoa_don = obj.hoa_don_dang_ky_phat_hanh_mau_so,
                        ky_hieu_hoa_don = obj.hoa_don_dang_ky_phat_hanh_ky_hieu,
                        don_vi_tien_te = obj.loai_tien,
                        ty_gia = (obj.loai_tien.ConvertToString() != "VND" && obj.loai_tien.ConvertToString() != "")
                            ? obj.ty_gia.ConvertToStringAndRemoveZeroPart()
                            : null,
                        hinh_thuc_thanh_toan = obj.hinh_thuc_tt,
                        ma_so_thue_co_quan_quan_ly = AppSettings.FixedValue.MNNhan,
                        ngay_lap = obj.ngay_hoa_don.ToString("yyyy-MM-dd") ?? "",
                        so_hoa_don = obj.ma_so_hoa_don.ToString(),
                        thong_tin_khac = new Model.Request.Xml.ThongTinKhac()
                        {
                            thong_tin_khac_noi_dung = new List<Model.Request.Xml.ThongTinKhacNoiDung>()
                            {
                                new Model.Request.Xml.ThongTinKhacNoiDung()
                                {
                                    thong_tin_truong = "MTCuu",
                                    kieu_du_lieu = "string",
                                    du_lieu = obj.ma_tra_cuu.ToString(),
                                }
                            }
                        }
                    },
                    noi_dung_hoa_don = new Model.Request.Xml.NoiDungHoaDon()
                    {
                        nguoi_ban = new Model.Request.Xml.NguoiBan()
                        {
                            ten_nguoi_ban = obj.nguoi_ban_ten_donvi,
                            dien_thoai = obj.nguoi_ban_dien_thoai.Trim(),
                            mst = obj.nguoi_ban_mst,
                            dia_chi = obj.nguoi_ban_dia_chi,
                            stk = obj.nguoi_ban_stk,
                            ngan_hang = obj.nguoi_ban_ngan_hang,
                            email = obj.nguoi_ban_email,
                            fax = obj.nguoi_ban_fax.ConvertToString() != "" ? obj.nguoi_ban_fax : null,
                            website = obj.nguoi_ban_website.ConvertToString() != "" ? obj.nguoi_ban_website : null
                        },
                        nguoi_mua = new Model.Request.Xml.NguoiMua()
                        {
                        },
                        danh_sach_hang_hoa_dich_vu = new Model.Request.Xml.DanhSachHangHoaDichVu()
                        {
                        },
                        thong_tin_thanh_toan = thongTinThanhToan,
                    },
                },
                // qr_code = AppSettings.FixedValue.QRCode,
                qr_code = obj.CreateQRCode(),
                danh_sach_chu_ky_so = new Model.Request.Xml.DanhSachChuKySo()
                {
                    nguoi_ban = new Model.Request.Xml.CKSNguoiBan() { },
                    nguoi_mua = new Model.Request.Xml.CKSNguoiMua() { }
                }
            };
            if (obj.hoa_don_dang_ky_phat_hanh_mau_so.ConvertToString() == "7")
            {
                model.du_lieu_hoa_don.noi_dung_hoa_don.thong_tin_thanh_toan = new Model.Request.Xml.ThongTinThanhToan()
                {
                    tong_tien_thanh_toan_bang_chu = "",
                    tong_tien_thanh_toan_bang_so = tong_tien_thanh_toan_bang_so.ToString(),

                };
            }
            if (obj.giam_thue_ghi_chu.ConvertToString() != "")
            {
                model.du_lieu_hoa_don.thong_tin_chung.thong_tin_khac.thong_tin_khac_noi_dung.Add(
                    new ThongTinKhacNoiDung()
                    {
                        thong_tin_truong = "GhiChu",
                        kieu_du_lieu = "string",
                        du_lieu = obj.giam_thue_ghi_chu.ConvertToString(),
                    });
            }

            var nguoi_mua = model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_mua;

            if (obj.hoa_don_hinh_thuc_code != "M")
            {
                //-	Hóa đơn thường
                if (obj.nguoi_mua_mst.ConvertToString() != "")
                {
                    // •Đối với người mua là doanh nghiệp/ tổ chức/ hộ kinh doanh có Mã số thuế : bắt buộc phải có thông tin MST, Tên, Đia chỉ 
                    // và gen thẻ xml tương ứng <MST>, <Ten>, <DChi>.
                    //  Nếu có nhập thêm cả tên người mua hàng thì gen thêm thẻ <HVTNMHang>.
                    //bắt buộc
                    nguoi_mua.ten_don_vi = obj.nguoi_mua_ten_donvi;
                    nguoi_mua.dia_chi = obj.nguoi_mua_dia_chi.ConvertToString();
                    nguoi_mua.mst = obj.nguoi_mua_mst.ConvertToString();
                    //option
                    if (obj.nguoi_mua_ten.ConvertToString() != "") nguoi_mua.ho_ten_nguoi_mua_hang = obj.nguoi_mua_ten;
                    if (obj.nguoi_mua_dien_thoai.ConvertToString() != "") nguoi_mua.dien_thoai = obj.nguoi_mua_dien_thoai;
                    if (obj.nguoi_mua_stk.ConvertToString() != "") nguoi_mua.stk = obj.nguoi_mua_stk;
                    if (obj.nguoi_mua_ngan_hang.ConvertToString() != "") nguoi_mua.ngan_hang = obj.nguoi_mua_ngan_hang;
                    if (obj.nguoi_mua_email.ConvertToString() != "") nguoi_mua.email = obj.nguoi_mua_email;
                    if (obj.ma_dv_ngan_sach.ConvertToString() != "") nguoi_mua.ma_dv_ngan_sach = obj.ma_dv_ngan_sach;
                    if (obj.nguoi_mua_cccd.ConvertToString() != "") nguoi_mua.cccd = obj.nguoi_mua_cccd;
                    if (obj.so_ho_chieu.ConvertToString() != "") nguoi_mua.so_ho_chieu = obj.so_ho_chieu;
                }
                else
                {
                    // •Đối với người mua là cá nhân hoặc khách lẻ không lấy hóa đơn:
                    //  yêu cầu nhập thông tin Họ tên người mua hàng  gen vào  thẻ < HVTNMHang >, 
                    // các thông tin khác nếu có nhập vào giá trị thì gen thẻ tương ứng, 
                    // không có giá trị thì không gen thẻ xml.


                    if (obj.nguoi_mua_ten_donvi.ConvertToString() != "") nguoi_mua.ten_don_vi = obj.nguoi_mua_ten_donvi.ConvertToString();
                    if (obj.nguoi_mua_dia_chi.ConvertToString() != "") nguoi_mua.dia_chi = obj.nguoi_mua_dia_chi.ConvertToString();
                    if (obj.nguoi_mua_mst.ConvertToString() != "") nguoi_mua.mst = obj.nguoi_mua_mst.ConvertToString();
                    if (obj.nguoi_mua_ten.ConvertToString() != "") nguoi_mua.ho_ten_nguoi_mua_hang = obj.nguoi_mua_ten;

                    if (obj.nguoi_mua_dien_thoai.ConvertToString() != "") nguoi_mua.dien_thoai = obj.nguoi_mua_dien_thoai;
                    if (obj.nguoi_mua_stk.ConvertToString() != "") nguoi_mua.stk = obj.nguoi_mua_stk;
                    if (obj.nguoi_mua_ngan_hang.ConvertToString() != "") nguoi_mua.ngan_hang = obj.nguoi_mua_ngan_hang;
                    if (obj.nguoi_mua_email.ConvertToString() != "") nguoi_mua.email = obj.nguoi_mua_email;
                    if (obj.nguoi_mua_cccd.ConvertToString() != "") nguoi_mua.cccd = obj.nguoi_mua_cccd;
                    if (obj.so_ho_chieu.ConvertToString() != "") nguoi_mua.so_ho_chieu = obj.so_ho_chieu;
                    if (obj.ma_dv_ngan_sach.ConvertToString() != "") nguoi_mua.ma_dv_ngan_sach = obj.ma_dv_ngan_sach;
                }
            }

            if (obj.hoa_don_hinh_thuc_code == "M")
            {
                //hóa đơn MTT
                if (obj.nguoi_mua_mst.ConvertToString() != "")
                {
                    // •Đối với người mua là doanh nghiệp/ tổ chức/ hộ kinh doanh có Mã số thuế : 
                    // bắt buộc phải có thông tin MST, Tên, Đia chỉ và gen thẻ xml tương ứng <MST>, <Ten>, <DChi>.
                    // Nếu có nhập thêm cả tên người mua hàng thì gen thêm thẻ TTKhac theo cấu trúc HVTNMHang:
                    //bắt buộc
                    nguoi_mua.ten_don_vi = obj.nguoi_mua_ten_donvi;
                    nguoi_mua.dia_chi = obj.nguoi_mua_dia_chi.ConvertToString();
                    nguoi_mua.mst = obj.nguoi_mua_mst.ConvertToString();


                    //option
                    if (obj.nguoi_mua_dien_thoai.ConvertToString() != "") nguoi_mua.dien_thoai = obj.nguoi_mua_dien_thoai;
                    if (obj.ma_dv_ngan_sach.ConvertToString() != "") nguoi_mua.ma_dv_ngan_sach = obj.ma_dv_ngan_sach;
                    if (obj.nguoi_mua_cccd.ConvertToString() != "") nguoi_mua.cccd = obj.nguoi_mua_cccd;
                    if (obj.so_ho_chieu.ConvertToString() != "") nguoi_mua.so_ho_chieu = obj.so_ho_chieu;

                    if (model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_mua.thong_tin_khac == null)
                    {
                        model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_mua.thong_tin_khac = new ThongTinKhac()
                        {
                            thong_tin_khac_noi_dung = new List<ThongTinKhacNoiDung>()
                        };
                    }
                    if (obj.nguoi_mua_email.ConvertToString() != "")
                    {
                        model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_mua.thong_tin_khac.thong_tin_khac_noi_dung.Add(
                            new ThongTinKhacNoiDung()
                            {
                                du_lieu = obj.nguoi_mua_email.ConvertToString(),
                                kieu_du_lieu = "string",
                                thong_tin_truong = "DCTDTu",
                            }
                        );
                    }
                    if (obj.nguoi_mua_stk.ConvertToString() != "")
                    {
                        model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_mua.thong_tin_khac.thong_tin_khac_noi_dung.Add(
                            new ThongTinKhacNoiDung()
                            {
                                du_lieu = obj.nguoi_mua_stk.ConvertToString(),
                                kieu_du_lieu = "string",
                                thong_tin_truong = "STKNHang",
                            }
                        );
                    }
                    if (obj.nguoi_mua_ngan_hang.ConvertToString() != "")
                    {
                        model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_mua.thong_tin_khac.thong_tin_khac_noi_dung.Add(
                            new ThongTinKhacNoiDung()
                            {
                                du_lieu = obj.nguoi_mua_ngan_hang.ConvertToString(),
                                kieu_du_lieu = "string",
                                thong_tin_truong = "TNHang",
                            }
                        );
                    }

                    if (obj.nguoi_mua_ten.ConvertToString() != "")
                    {

                        nguoi_mua.ho_ten_nguoi_mua_hang = obj.nguoi_mua_ten.ConvertToString();
                    }
                }
                else
                {
                    // •Đối với người mua là cá nhân hoặc khách lẻ không lấy hóa đơn:
                    //  yêu cầu nhập thông tin Tên người mua hàng  gen vào  thẻ <Ten>, 
                    // Gen thêm thẻ TTKhac theo cấu trúc bên dưới.
                    // Các thông tin khác nếu có nhập giá trị thì gen thẻ tương ứng và đặt trong cặp thẻ TTKhac.
                    //  Mỗi thông tin nằm trong 1 cặp thẻ <TTin> như hình bên dưới
                    //bắt buộc
                    // nguoi_mua.ten_don_vi = obj.nguoi_mua_ten_donvi.ConvertToString() != ""
                    //     ? obj.nguoi_mua_ten_donvi.ConvertToString() //tổ chức hành chinh
                    //     : obj.nguoi_mua_ten.ConvertToString();

                    //option
                    if (obj.nguoi_mua_ten_donvi.ConvertToString() != "") nguoi_mua.ten_don_vi = obj.nguoi_mua_ten_donvi.ConvertToString();
                    if (obj.nguoi_mua_dia_chi.ConvertToString() != "") nguoi_mua.dia_chi = obj.nguoi_mua_dia_chi.ConvertToString();
                    if (obj.nguoi_mua_cccd.ConvertToString() != "") nguoi_mua.cccd = obj.nguoi_mua_cccd;
                    if (obj.so_ho_chieu.ConvertToString() != "") nguoi_mua.so_ho_chieu = obj.so_ho_chieu;
                    if (obj.nguoi_mua_ten.ConvertToString() != "") nguoi_mua.ho_ten_nguoi_mua_hang = obj.nguoi_mua_ten;
                    if (obj.nguoi_mua_dien_thoai.ConvertToString() != "") nguoi_mua.dien_thoai = obj.nguoi_mua_dien_thoai;
                    if (obj.ma_dv_ngan_sach.ConvertToString() != "") nguoi_mua.ma_dv_ngan_sach = obj.ma_dv_ngan_sach;
                    if (model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_mua.thong_tin_khac == null)
                    {
                        model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_mua.thong_tin_khac = new ThongTinKhac()
                        {
                            thong_tin_khac_noi_dung = new List<ThongTinKhacNoiDung>()
                        };
                    }



                    if (obj.nguoi_mua_ten.ConvertToString() != "" && obj.nguoi_mua_ten_donvi.ConvertToString() == "")
                    {
                        model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_mua.thong_tin_khac.thong_tin_khac_noi_dung.Add(
                            new ThongTinKhacNoiDung()
                            {
                                du_lieu = obj.nguoi_mua_ten.ConvertToString(),
                                kieu_du_lieu = "string",
                                thong_tin_truong = "TenNMHCNHan",
                            }
                        );
                    }

                    if (obj.nguoi_mua_email.ConvertToString() != "")
                    {
                        model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_mua.thong_tin_khac.thong_tin_khac_noi_dung.Add(
                            new ThongTinKhacNoiDung()
                            {
                                du_lieu = obj.nguoi_mua_email.ConvertToString(),
                                kieu_du_lieu = "string",
                                thong_tin_truong = "DCTDTu",
                            }
                        );
                    }

                    if (obj.nguoi_mua_stk.ConvertToString() != "")
                    {
                        model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_mua.thong_tin_khac.thong_tin_khac_noi_dung.Add(
                            new ThongTinKhacNoiDung()
                            {
                                du_lieu = obj.nguoi_mua_stk.ConvertToString(),
                                kieu_du_lieu = "string",
                                thong_tin_truong = "STKNHang",
                            }
                        );
                    }

                    if (obj.nguoi_mua_ngan_hang.ConvertToString() != "")
                    {
                        model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_mua.thong_tin_khac.thong_tin_khac_noi_dung.Add(
                            new ThongTinKhacNoiDung()
                            {
                                du_lieu = obj.nguoi_mua_ngan_hang.ConvertToString(),
                                kieu_du_lieu = "string",
                                thong_tin_truong = "TNHang",
                            }
                        );
                    }
                }
            }

            // if (nguoi_mua.ten_don_vi.ConvertToString() == "" )
            // {
            //     if (obj.nguoi_mua_ten_donvi.ConvertToString() != "")
            //     {
            //         nguoi_mua.ten_don_vi = obj.nguoi_mua_ten_donvi;
            //     }
            //     else
            //     {
            //         if (obj.nguoi_mua_ten.ConvertToString() != "")
            //             nguoi_mua.ten_don_vi = obj.nguoi_mua_ten.ConvertToString();
            //     }
            // }

            // if (nguoi_mua.ho_ten_nguoi_mua_hang.ConvertToString() == "")
            // {
            //     if (obj.nguoi_mua_ten.ConvertToString() != "")
            //         nguoi_mua.ho_ten_nguoi_mua_hang = obj.nguoi_mua_ten.ConvertToString();
            // }

            if (nguoi_mua.dien_thoai.ConvertToString() == "")
            {
                if (obj.nguoi_mua_dien_thoai.ConvertToString() != "")
                    nguoi_mua.dien_thoai = obj.nguoi_mua_dien_thoai.ConvertToString();
            }

            if (nguoi_mua.dia_chi.ConvertToString() == "")
            {
                if (obj.nguoi_mua_dia_chi.ConvertToString() != "")
                    nguoi_mua.dia_chi = obj.nguoi_mua_dia_chi.ConvertToString();
            }

            if (nguoi_mua.stk.ConvertToString() == "")
            {
                if (obj.nguoi_mua_stk.ConvertToString() != "") nguoi_mua.stk = obj.nguoi_mua_stk.ConvertToString();
            }

            if (nguoi_mua.ngan_hang.ConvertToString() == "")
            {
                if (obj.nguoi_mua_ngan_hang.ConvertToString() != "")
                    nguoi_mua.ngan_hang = obj.nguoi_mua_ngan_hang.ConvertToString();
            }

            if (nguoi_mua.email.ConvertToString() == "")
            {
                if (obj.nguoi_mua_email.ConvertToString() != "")
                    nguoi_mua.email = obj.nguoi_mua_email.ConvertToString();
            }

            if (nguoi_mua.cccd.ConvertToString() == "")
            {
                if (obj.nguoi_mua_cccd.ConvertToString() != "") nguoi_mua.cccd = obj.nguoi_mua_cccd.ConvertToString();
            }
            if (nguoi_mua.so_ho_chieu.ConvertToString() == "")
            {
                if (obj.so_ho_chieu.ConvertToString() != "") nguoi_mua.so_ho_chieu = obj.so_ho_chieu.ConvertToString();
            }

            if (nguoi_mua.ma_dv_ngan_sach.ConvertToString() == "")
            {
                if (obj.ma_dv_ngan_sach.ConvertToString() != "")
                    nguoi_mua.ma_dv_ngan_sach = obj.ma_dv_ngan_sach.ConvertToString();
            }

            if (obj.loai_tien == "VND")
            {
                model.du_lieu_hoa_don.noi_dung_hoa_don.thong_tin_thanh_toan.tong_tien_thanh_toan_bang_chu =
                    obj.tong_tien_chu.ConvertToString() != ""
                    ? obj.tong_tien_chu
                    : await tong_tien_thanh_toan_bang_so.ConvertToTextAsync(
                        obj.loai_tien.ConvertToString() != "" ? obj.loai_tien.ConvertToString() : "VND"
                    );
            }
            else
            {
                model.du_lieu_hoa_don.noi_dung_hoa_don.thong_tin_thanh_toan.tong_tien_thanh_toan_bang_chu =
                    obj.tong_tien_chu.ConvertToString() != ""
                    ? obj.tong_tien_chu
                    : await tong_tien_thanh_toan_bang_so.ConvertToTextAsync(
                        obj.loai_tien.ConvertToString() != "" ? obj.loai_tien.ConvertToString() : "VND"
                    );

            }
            // 1	Hóa đơn điện tử theo Nghị định 123/2020/NĐ-CP
            // 2	Hóa đơn điện tử có mã xác thực của cơ quan thuế theo Quyết định số 1209/QĐ-BTC ngày 23 tháng 6 năm 2015 và Quyết định số 2660/QĐ-BTC ngày 14 tháng 12 năm 2016 của Bộ Tài chính (Hóa đơn có mã xác thực của CQT theo Nghị định số 51/2010/NĐ-CP và Nghị định số 04/2014/NĐ-CP)
            // 3	Các loại hóa đơn theo Nghị định số 51/2010/NĐ-CP và Nghị định số 04/2014/NĐ-CP (Trừ hóa đơn điện tử có mã xác thực của cơ quan thuế theo Quyết định số 1209/QĐ-BTC và Quyết định số 2660/QĐ-BTC)
            // 4	Hóa đơn đặt in theo Nghị định 123/2020/NĐ-CP

            if (obj.hoa_don_dang_ky_phat_hanh_ky_hieu_goc.ConvertToString() != "" && obj.ngay_hoa_don_goc.HasValue)
            {
                // var hoaDonGoc = await this.SelectByIdAsync(obj.hoa_don_id_goc);
                model.du_lieu_hoa_don.thong_tin_chung.thong_tin_lien_quan = new ThongTinLienQuan()
                {
                    KHHDCLQuan = obj.hoa_don_dang_ky_phat_hanh_ky_hieu_goc,
                    KHMSHDCLQuan = obj.hoa_don_dang_ky_phat_hanh_mau_so_goc,
                    LHDCLQuan = obj.hoa_don_nghi_dinh_id_goc == 123 ? "1" : "3",
                    NLHDCLQuan = obj.ngay_hoa_don_goc.HasValue
                        ? obj.ngay_hoa_don_goc.Value.ToString("yyyy-MM-dd")
                        : null,
                    SHDCLQuan = obj.ma_so_hoa_don_goc.ToString(),
                    TCHDon = obj.hoa_don_hinh_thuc_id == 3 ? "2" : "1",
                };
            }

            if (obj.hoa_don_dang_ky_phat_hanh_mau_so == "2" || obj.hoa_don_dang_ky_phat_hanh_mau_so == "3")
            {

                hd_thong_tin_bo_sung obj_ttbosung = await _repositoryWrapper.HoaDon.HoaDon.SelectHoaDonThongTinBoSungByHoaDonIdAsync(obj.id);


                if (obj.hoa_don_dang_ky_phat_hanh_mau_so == "2")
                {

                    if(obj_ttbosung==null)
                    {
                        model.du_lieu_hoa_don.thong_tin_chung.HDDCKPTQuan = "0";
                    }
                    else
                    {
                        model.du_lieu_hoa_don.thong_tin_chung.HDDCKPTQuan = obj_ttbosung.is_hd_phi_thue_quan.ToString();
                    }

                }
                if (obj.hoa_don_dang_ky_phat_hanh_mau_so == "3")
                {

                    static string? ToIsoDateOrNull(string? value)
                    {
                        if (string.IsNullOrWhiteSpace(value))
                            return null;

                        if (!DateTime.TryParse(value, out var dt))
                            return null;

                        // DB default "ngày rỗng"
                        if (dt.Date == new DateTime(1900, 1, 1))
                            return null;

                        return dt.ToString("yyyy-MM-dd");
                    }

                    //hoa don ban tai san cong
                    model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.SoQuyetdinh = obj_ttbosung.so_quyet_dinh;
                    model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.NgayQuyetdinh = ToIsoDateOrNull(obj_ttbosung.ngay_quyet_dinh);
                    model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.CoQuanBHQDinh = obj_ttbosung.co_quan_ban_hanh_qd;
                    model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.HThucban = obj_ttbosung.hinh_thuc_ban;

                    model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_mua.DiadiemVCHDen = obj_ttbosung.dia_diem_vc_hang_den;
                    model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_mua.TGianVCTu = ToIsoDateOrNull(obj_ttbosung.tgian_vc_hang_den_tu);
                    model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_mua.TGianVCDen = ToIsoDateOrNull(obj_ttbosung.tgian_vc_hang_den_den);
                }
            }


            // Phiếu xuất kho
            if (obj.hoa_don_dang_ky_phat_hanh_mau_so == "6")
            {
                //Phiếu xuất kho kiêm vận chuyển nội bộ
                if (obj.loai_hoa_don_ct_id == 9)
                {
                    model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.LDDNBo = obj.xuat_kho_vc_lenh_dieu_dong_noi_bo;
                    model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.PTVChuyen = obj.xuat_kho_phuong_tien_van_chuyen;

                    model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.HDSo = obj.xuat_kho_hop_dong_so;
                    model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.HVTNXHang = obj.xuat_kho_nguoi_xuat_hang;
                    model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.TNVChuyen = obj.xuat_kho_nguoi_van_chuyen;
                    model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.dia_chi = obj.xuat_kho_dia_chi;
                }

                //Phiếu xuất kho đại lý
                if (obj.loai_hoa_don_ct_id == 10)
                {
                    model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.HDKTSo = obj.xuat_kho_dl_hop_dong_kinh_te_so;
                    if (obj.xuat_kho_dl_hop_dong_ngay.HasValue)
                        model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.HDKTNgay =
                            obj.xuat_kho_dl_hop_dong_ngay.Value.ToString("yyyy-MM-dd");
                    model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.PTVChuyen = obj.xuat_kho_phuong_tien_van_chuyen;

                    model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.HDSo = obj.xuat_kho_hop_dong_so;
                    model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.HVTNXHang = obj.xuat_kho_nguoi_xuat_hang;
                    model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.TNVChuyen = obj.xuat_kho_nguoi_van_chuyen;
                    model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.dia_chi = obj.xuat_kho_dia_chi;
                    model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_mua.HVTNNHang = obj.nguoi_mua_ten;
                    model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_mua.ho_ten_nguoi_mua_hang = null;
                }
            }

            if (obj.hoa_don_hinh_thuc_code == "M")
            {
                model.ma_co_quan_thue = obj.ma_so_hoa_don_mtt;
            }

            foreach (var item in hangHoas)
            {
                var objItem = new Model.Request.Xml.HangHoaDichVu()
                {
                    tinh_chat = item.hang_hoa_tinh_chat_id,
                    stt = item.stt > 0 ? item.stt.ToString() : string.Empty,
                    don_gia = item.don_gia != 0 ? item.don_gia.ConvertToStringAndRemoveZeroPart() : null,
                    don_vi_tinh = item.dvt,
                    ma_hang_hoa_dich_vu = item.ma_hang,
                    so_luong = item.so_luong != 0 ? item.so_luong.ConvertToStringAndRemoveZeroPart() : null,
                    ten_hang_hoa_dich_vu = item.ten_hang,
                    thanh_tien = obj.loai_tien == "VND" ? ((decimal)item.thanh_tien.ConvertToDouble(0)).ConvertToStringAndRemoveZeroPart() :
                     item.thanh_tien.ConvertToStringAndRemoveZeroPart(),
                    thue_suat = obj.hoa_don_dang_ky_phat_hanh_mau_so != "2"
                    ? item.thue_vat
                    : null,
                };
                if (item.ty_le_chiet_khau.ConvertToString() != "")
                {
                    objItem.ty_le_chiet_khau = item.ty_le_chiet_khau.ConvertToStringAndRemoveZeroPart();
                    objItem.so_tien_chiet_khau = item.tien_chiet_khau.ConvertToStringAndRemoveZeroPart();
                }
                if (item.hang_hoa_tinh_chat_id == 5 && item.hang_hoa_dac_trung_json.ConvertToString() != "")
                {
                    if (objItem.TTHHDTrung == null)
                    {
                        objItem.TTHHDTrung = new TTHHDTrung();
                        objItem.TTHHDTrung.TTHHDTrungTTins = new List<TTHHDTrungTTin>();
                    }
                    //lấy tất cả các field và giá trị từ hoaDon.thong_tin_khac_json (đang kiểu string)
                    var jsonStr = item.hang_hoa_dac_trung_json.ConvertToString();
                    try
                    {
                        var objTTHHDTrung = Newtonsoft.Json.JsonConvert.DeserializeObject<TTHHDTrungInfo>(jsonStr);
                        if (objTTHHDTrung != null)
                        {
                            // Lấy tất cả property của object
                            foreach (var prop in objTTHHDTrung.GetType().GetProperties())
                            {
                                string propName = prop.Name;
                                var propValue = prop.GetValue(objTTHHDTrung, null).ConvertToString();
                                if (propValue != "" && propName != "LHHDTrung")
                                {
                                    objItem.TTHHDTrung.TTHHDTrungTTins.Add(new TTHHDTrungTTin()
                                    {
                                        LHHDTrung = objTTHHDTrung.LHHDTrung,
                                        TTruong = propName,
                                        DLieu = propValue
                                    });
                                }

                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        // Log lỗi nếu JSON sai format
                        Console.WriteLine("Lỗi parse hang_hoa_dac_trung_json: " + ex.Message);
                    }
                }

                model.du_lieu_hoa_don.noi_dung_hoa_don.danh_sach_hang_hoa_dich_vu.hang_hoa_dich_vus.Add(objItem);
            }

            //hóa đơn nước
            if (obj.tt_nuoc_ma_bill.ConvertToString().Trim() != string.Empty)
            {
                if (model.du_lieu_hoa_don.thong_tin_chung.thong_tin_khac.thong_tin_khac_noi_dung == null)
                {
                    model.du_lieu_hoa_don.thong_tin_chung.thong_tin_khac.thong_tin_khac_noi_dung =
                        new List<ThongTinKhacNoiDung>();
                }
            }

            if (obj.tt_nuoc_ma_bill.ConvertToString().Trim() != string.Empty)
                model.du_lieu_hoa_don.thong_tin_chung.thong_tin_khac.thong_tin_khac_noi_dung.Add(
                    new ThongTinKhacNoiDung()
                    {
                        du_lieu = obj.tt_nuoc_ma_bill.ConvertToString().Trim(),
                        kieu_du_lieu = "string",
                        thong_tin_truong = "MaBill"
                    });
            if (obj.tt_nuoc_ngay_doc_thang_nay.ConvertToString().Trim() != string.Empty)
                model.du_lieu_hoa_don.thong_tin_chung.thong_tin_khac.thong_tin_khac_noi_dung.Add(
                    new ThongTinKhacNoiDung()
                    {
                        du_lieu = obj.tt_nuoc_ngay_doc_thang_nay.ConvertToString().Trim(),
                        kieu_du_lieu = "string",
                        thong_tin_truong = "NgayDocThangNay"
                    });
            if (obj.tt_nuoc_ngay_doc_thang_truoc.ConvertToString().Trim() != string.Empty)
                model.du_lieu_hoa_don.thong_tin_chung.thong_tin_khac.thong_tin_khac_noi_dung.Add(
                    new ThongTinKhacNoiDung()
                    {
                        du_lieu = obj.tt_nuoc_ngay_doc_thang_truoc.ConvertToString().Trim(),
                        kieu_du_lieu = "string",
                        thong_tin_truong = "NgayDocThangTruoc"
                    });
            if (obj.tt_nuoc_so_cuong.ConvertToString().Trim() != string.Empty)
                model.du_lieu_hoa_don.thong_tin_chung.thong_tin_khac.thong_tin_khac_noi_dung.Add(
                    new ThongTinKhacNoiDung()
                    {
                        du_lieu = obj.tt_nuoc_so_cuong.ConvertToString().Trim(),
                        kieu_du_lieu = "string",
                        thong_tin_truong = "SoCuong"
                    });
            if (obj.tt_nuoc_ma_nguoi_mua.ConvertToString().Trim() != string.Empty)
                model.du_lieu_hoa_don.thong_tin_chung.thong_tin_khac.thong_tin_khac_noi_dung.Add(
                    new ThongTinKhacNoiDung()
                    {
                        du_lieu = obj.tt_nuoc_ma_nguoi_mua.ConvertToString().Trim(),
                        kieu_du_lieu = "string",
                        thong_tin_truong = "MaNguoiMua"
                    });
            if (obj.tt_nuoc_chi_so_thang_ngay.ConvertToString().Trim() != string.Empty)
                model.du_lieu_hoa_don.thong_tin_chung.thong_tin_khac.thong_tin_khac_noi_dung.Add(
                    new ThongTinKhacNoiDung()
                    {
                        du_lieu = obj.tt_nuoc_chi_so_thang_ngay.ConvertToString().Trim(),
                        kieu_du_lieu = "string",
                        thong_tin_truong = "ChiSoDHThangNay"
                    });
            if (obj.tt_nuoc_chi_so_thang_truoc.ConvertToString().Trim() != string.Empty)
                model.du_lieu_hoa_don.thong_tin_chung.thong_tin_khac.thong_tin_khac_noi_dung.Add(
                    new ThongTinKhacNoiDung()
                    {
                        du_lieu = obj.tt_nuoc_chi_so_thang_truoc.ConvertToString().Trim(),
                        kieu_du_lieu = "string",
                        thong_tin_truong = "ChiSoDHThangTruoc"
                    });
            if (obj.tt_nuoc_ma_nuoc.ConvertToString().Trim() != string.Empty)
                model.du_lieu_hoa_don.thong_tin_chung.thong_tin_khac.thong_tin_khac_noi_dung.Add(
                    new ThongTinKhacNoiDung()
                    {
                        du_lieu = obj.tt_nuoc_ma_nuoc.ConvertToString().Trim(),
                        kieu_du_lieu = "string",
                        thong_tin_truong = "MaNuoc"
                    });
            if (obj.tt_nuoc_tong_so_ngay.ConvertToString().Trim() != string.Empty)
                model.du_lieu_hoa_don.thong_tin_chung.thong_tin_khac.thong_tin_khac_noi_dung.Add(
                    new ThongTinKhacNoiDung()
                    {
                        du_lieu = obj.tt_nuoc_tong_so_ngay.ConvertToString().Trim(),
                        kieu_du_lieu = "string",
                        thong_tin_truong = "TongSoNgay"
                    });
            if (obj.tt_nuoc_tong_tieu_thu.ConvertToString().Trim() != string.Empty)
                model.du_lieu_hoa_don.thong_tin_chung.thong_tin_khac.thong_tin_khac_noi_dung.Add(
                    new ThongTinKhacNoiDung()
                    {
                        du_lieu = obj.tt_nuoc_tong_tieu_thu.ConvertToString().Trim(),
                        kieu_du_lieu = "string",
                        thong_tin_truong = "Tieuthu"
                    });
            if (obj.tt_nuoc_so_ho.ConvertToString().Trim() != string.Empty)
                model.du_lieu_hoa_don.thong_tin_chung.thong_tin_khac.thong_tin_khac_noi_dung.Add(
                    new ThongTinKhacNoiDung()
                    {
                        du_lieu = obj.tt_nuoc_so_ho.ConvertToString().Trim(),
                        kieu_du_lieu = "string",
                        thong_tin_truong = "SoHo"
                    });
            if (obj.tt_nuoc_serial_dong_ho.ConvertToString().Trim() != string.Empty)
                model.du_lieu_hoa_don.thong_tin_chung.thong_tin_khac.thong_tin_khac_noi_dung.Add(
                    new ThongTinKhacNoiDung()
                    {
                        du_lieu = obj.tt_nuoc_serial_dong_ho.ConvertToString().Trim(),
                        kieu_du_lieu = "string",
                        thong_tin_truong = "SeriDongHo"
                    });

            //thong tin khac
            if (obj.thong_tin_khac_json.ConvertToString() != "")
            {
                if (model.du_lieu_hoa_don.thong_tin_chung.thong_tin_khac == null)
                {
                    model.du_lieu_hoa_don.thong_tin_chung.thong_tin_khac = new ThongTinKhac();
                    model.du_lieu_hoa_don.thong_tin_chung.thong_tin_khac.thong_tin_khac_noi_dung = new List<ThongTinKhacNoiDung>();
                }
                //lấy tất cả các field và giá trị từ hoaDon.thong_tin_khac_json (đang kiểu string)
                var jsonStr = obj.thong_tin_khac_json.ConvertToString();
                try
                {
                    // Giả định JSON có cấu trúc dạng: { "field1": "value1", "field2": "value2", ... }
                    var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonStr);

                    foreach (var kv in dict)
                    {
                        if (kv.Value.ConvertToString() != "")
                            model.du_lieu_hoa_don.thong_tin_chung.thong_tin_khac.thong_tin_khac_noi_dung.Add(new ThongTinKhacNoiDung()
                            {
                                thong_tin_truong = kv.Key,
                                kieu_du_lieu = "string",
                                du_lieu = kv.Value
                            });
                    }
                }
                catch (Exception ex)
                {
                    // Log lỗi nếu JSON sai format
                    Console.WriteLine("Lỗi parse thong_tin_khac_json: " + ex.Message);
                }
            }
            if (obj.hoa_don_dang_ky_phat_hanh_mau_so == "6")
            {
                //phiếu xuất kho
                var tong_tien_thanh_toan_bang_chu = model.du_lieu_hoa_don.noi_dung_hoa_don.thong_tin_thanh_toan != null
                ? model.du_lieu_hoa_don.noi_dung_hoa_don.thong_tin_thanh_toan.tong_tien_thanh_toan_bang_chu
                : await tong_tien_thanh_toan_bang_so.ConvertToTextAsync(
                        obj.loai_tien.ConvertToString() != "" ? obj.loai_tien.ConvertToString() : "VND"
                    ); ;
                model.du_lieu_hoa_don.noi_dung_hoa_don.thong_tin_thanh_toan = null;
                if (model.du_lieu_hoa_don.thong_tin_khac == null)
                {
                    model.du_lieu_hoa_don.thong_tin_khac = new ThongTinKhac();
                    model.du_lieu_hoa_don.thong_tin_khac.thong_tin_khac_noi_dung = new List<ThongTinKhacNoiDung>();
                }
                model.du_lieu_hoa_don.thong_tin_khac.thong_tin_khac_noi_dung.Add(
                    new ThongTinKhacNoiDung()
                    {
                        du_lieu = tong_tien_thanh_toan_bang_so.ConvertToStringAndRemoveZeroPart(),
                        kieu_du_lieu = "numeric",
                        thong_tin_truong = "TgTTTBSo"
                    });
                model.du_lieu_hoa_don.thong_tin_khac.thong_tin_khac_noi_dung.Add(
                    new ThongTinKhacNoiDung()
                    {
                        du_lieu = tong_tien_thanh_toan_bang_chu,
                        kieu_du_lieu = "string",
                        thong_tin_truong = "TgTTTBChu"
                    });
            }
            model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.stk = obj.nguoi_ban_stk;
            model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.email = obj.nguoi_ban_email;
            model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.dien_thoai = obj.nguoi_ban_dien_thoai;
            model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_mua.stk = obj.nguoi_mua_stk;
            model.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_mua.ngan_hang = obj.nguoi_mua_ngan_hang;
            return model;
        }

        public async Task<FunctionResult<string>> CreateXmlKySoAsync(int id, bool isPreview = false)
        {
            var xmlData = await this.CreateXmlObjectKySoAsync(id, isPreview);

            if (xmlData.is_success)
            {
                return new SuccessResult<string>(xmlData.data.ConvertToXml());
            }

            return new ErrorResult<string>(xmlData.message);
        }

        public async Task<FunctionResult<string>> CreateXmlKySoAsync(hoa_don hoaDon)
        {
            if (hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_DA_HUY_NOI_BO)
            {
                //return string.Empty;
                return new ErrorResult<string>("Hóa đơn đã hủy nội bộ");
            }
            var xmlData = await this.CreateXmlObjectKySoAsync(hoaDon);

            if (xmlData.is_success)
            {
                //return xmlData.data.ConvertToXml();
                return new SuccessResult<string>(xmlData.data.ConvertToXml());
            }
            return new ErrorResult<string>(xmlData.message);

            //return string.Empty;
        }

        private async Task<bool> AcquireLockHoaDonAsync(int id)
        {
            int retryCount = 1;
            int retryDelayMs = 300;
            var lockKey = $"hoa-don-{id}";
            var db = RedisCacheService.redis.GetDatabase();
            for (int i = 0; i < retryCount; i++)
            {
                if (await db.StringSetAsync(lockKey, id.ToString(), TimeSpan.FromSeconds(60), When.NotExists))
                {
                    return true; // Đã lấy được khóa
                }

                await Task.Delay(retryDelayMs);
            }

            return false;
        }

        private async Task ReleaseLockHoaDonAsync(int id)
        {
            var lockKey = $"hoa-don-{id}";
            var db = RedisCacheService.redis.GetDatabase();
            await db.KeyDeleteAsync(lockKey);
        }

        public async Task<FunctionResult<HoaDonPhatHanhRespone>> PhatHanhAsync(HoaDonPhatHanhRequest request,
            int user_id_phathanh = 0)
        {
            try
            {
                //tránh phát hành chưa xong user gọi phát hành tiếp -> phát hành lần đầu tạo lock -> phát hành xong xóa lock
                var isGetLockKey = await AcquireLockHoaDonAsync(request.id);
                if (!isGetLockKey) return new ErrorResult<HoaDonPhatHanhRespone>("Tạo khóa phát hành thất bại");
                var taskHoaDon = this.SelectByIdAsync(request.id);
                var taskHoaDonLog = _hoaDonLogService.SelectByHoaDonAsync(request.id);
                await Task.WhenAll(taskHoaDon, taskHoaDonLog);
                var hoaDon = taskHoaDon.Result;
                var hoaDonLogs = taskHoaDonLog.Result;
                var logGui = hoaDonLogs.Where(x => x.hoa_don_log_type_id == (int)e_hoa_don_log_type.GUI_THONG_DIEP)
                    .FirstOrDefault();
                if (logGui != null)
                {
                    return new ErrorResult<HoaDonPhatHanhRespone>("Hóa đơn đã gửi thông điệp");
                }

                // var hoaDon = await this.SelectByIdAsync(request.id);
                // var hoaDonLogs = await _hoaDonLogService.SelectByHoaDonAsync(hoaDon.id);
                // _logExecutionTimeHelper.WriteLog("Truy vấn hóa đơn theo id");
                if (hoaDon == null) return new ErrorResult<HoaDonPhatHanhRespone>("Không tìm thấy dữ liệu hợp lệ");
                if (request.signed_text == "")
                {
                    var hoaDonLogKySo = hoaDonLogs
                        .Where(x => x.hoa_don_log_type_id == (int)e_hoa_don_log_type.KY_SO_SUCCESS)
                        .OrderByDescending(x => x.created_time).FirstOrDefault();
                    if (hoaDonLogKySo == null)
                        return new ErrorResult<HoaDonPhatHanhRespone>("Không tìm thấy dữ liệu hợp lệ");
                    var signedTextXmlFile = hoaDonLogKySo.file_thong_diep_url;
                    var xmlContent = File.ReadAllText(signedTextXmlFile);
                    request.signed_text = xmlContent.ConvertToBase64();
                }

                var hoaDonType = GetHoaDonType(hoaDon.hoa_don_dang_ky_phat_hanh_ky_hieu);
                if (hoaDonType == "C")
                {
                    return await this.PhatHanhHoaDonCoMaAsync(hoaDon, request.signed_text, user_id_phathanh);
                }

                if (hoaDonType == "K")
                {
                    return await this.PhatHanhHoaDonKhongMaAsync(hoaDon, request.signed_text, user_id_phathanh);
                }

                // if (hoaDonType == "M")
                // {
                //     return await this.PhatHanhHoaDonMTTAsync(hoaDon, request.signed_text);
                // }
                return new ErrorResult<HoaDonPhatHanhRespone>("Không xác định được loại hóa đơn");
            }
            finally
            {
                await ReleaseLockHoaDonAsync(request.id);
            }
        }

        private string GetHoaDonType(string kyHieu)
        {
            if (kyHieu.ConvertToString().Length >= 4)
            {
                if (kyHieu.ConvertToString().Substring(3, 1).ConvertToString().ToUpper() == "M") return "M";
            }

            if (kyHieu.ConvertToString().FirstOrDefault().ConvertToString().ToUpper() == "K") return "K";
            if (kyHieu.ConvertToString().FirstOrDefault().ConvertToString().ToUpper() == "C") return "C";

            return "";
        }

        public async Task<FunctionResult<HoaDonPhatHanhRespone>> PhatHanhHoaDonCoMaAsync(hoa_don hoaDon,
                string signed_text, int user_id_phathanh = 0)
        {
            // var _logExecutionTimeHelper = new LogExecutionTimeHelper();
            var userId = this.GetCurrentUserId();
            var userInfo = this.GetCurrentUser();
            if (user_id_phathanh > 0)
            {
                userId = user_id_phathanh;
                var objUser = await _serviceWrapper.User.User.SelectAndFormatJwtTokenAsync(userId);
                if (objUser != null) userInfo = objUser;
            }

            var hoaDonXml = signed_text.ConvertToXmlFromBase64();
            var uuid = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
            await _serviceWrapper.Cache.SetDataAsync<string>(uuid, "hoa_don", DateTime.Now.AddDays(30));
            await _repositoryWrapper.HoaDon.PhatHanhUUID.SaveLogUuidAsync(uuid, "hoa_don", userId);
            // _logExecutionTimeHelper.WriteLog("Lưu cache uuid");
            var thongDiep = new ThongDiep()
            {
                ThongTinChung = new ThongTinChungThongDiep()
                {
                    phien_ban = hoaDon.phien_ban,
                    ma_noi_gui = AppSettings.FixedValue.MNGui,
                    ma_noi_nhan = AppSettings.FixedValue.MNNhan,
                    thong_diep = "200",
                    ma_noi_gui_uuid = $"{AppSettings.FixedValue.MNGui}{uuid}".ToUpper(),
                    ma_thong_diep_tham_chieu = $"",
                    mst = hoaDon.nguoi_ban_mst,
                    so_luong = 1
                }
            };
            hoaDon.phat_hanh_uuid = uuid;
            hoaDon.user_id_phathanh = userId;
            await this.UpdateAsync(hoaDon);
            // _logExecutionTimeHelper.WriteLog("Cập nhật uuid phát hành vào db");
            var base64thongdiep = thongDiep.ConvertToXmlAndAppendChild("/TDiep", "DLieu", hoaDonXml).ConvertToBase64();
            // _logExecutionTimeHelper.WriteLog("Convert sang base64");
            using (var client = Helper.WSInterTRCA2Helper.GetClient())
            {
                await client.OpenAsync();
                // _logExecutionTimeHelper.WriteLog("Mở kết nối tới WSInterTRCA2SoapClient");
                var authHeader = Helper.WSInterTRCA2Helper.GetAuthHeader();
                await _serviceWrapper.Cache.SetDataAsync<hoa_don>(uuid + "_hoa_don", hoaDon, DateTime.Now.AddDays(30));
                // _logExecutionTimeHelper.WriteLog("Set cache hóa đơn");
                //
                var fileName = Guid.NewGuid().ToString() + ".xml";
                var filePath = $"Xml/{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{fileName}";
                var directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                await File.WriteAllTextAsync(filePath, base64thongdiep.ConvertToXmlFromBase64());
                // _logExecutionTimeHelper.WriteLog("Lưu log phát hành thành file xml");

                //
                try
                {
                    await _semaphore.WaitAsync();
                    bool needRetry = false;
                    var stopwatch = new System.Diagnostics.Stopwatch();

                    do
                    {
                        needRetry = false;

                        try
                        {
                            var guiThongDiepResult = await client.Guithongdiep2024Async(authHeader, base64thongdiep, 1);

                            if (guiThongDiepResult.Guithongdiep2024Result.ConvertToString().Length > 2)
                            {
                                var log = new hoa_don_log()
                                {
                                    file_thong_diep_url = filePath,
                                    ngay_thuc_hien = DateTime.Now,
                                    nguoi_thuc_hien = userInfo.full_name,
                                    noi_dung_thuc_hien = "Gửi thông điệp lên CQT",
                                    hoa_don_id = hoaDon.id,
                                    hoa_don_log_type_id = (int)e_hoa_don_log_type.GUI_THONG_DIEP
                                };
                                log.SetInsertInfo(userId);
                                _serviceWrapper.Core.TaskQueue.EnqueueTask(async _ =>
                                {
                                    await _serviceWrapper.HoaDon.HoaDonLog.InsertAsync(log);
                                });
                            }
                            else
                            {
                                if (!stopwatch.IsRunning)
                                    stopwatch.Start();

                                if (stopwatch.Elapsed < TimeSpan.FromSeconds(30))
                                {
                                    needRetry = true;
                                }
                                else
                                {
                                    var log = new hoa_don_log()
                                    {
                                        file_thong_diep_url = filePath,
                                        ngay_thuc_hien = DateTime.Now,
                                        nguoi_thuc_hien = userInfo.full_name,
                                        noi_dung_thuc_hien = $"Gửi thông điệp lỗi {guiThongDiepResult.Guithongdiep2024Result.ConvertToString()}",
                                        hoa_don_id = hoaDon.id,
                                        hoa_don_log_type_id = -1 * (int)e_hoa_don_log_type.GUI_THONG_DIEP
                                    };
                                    log.SetInsertInfo(userId);
                                    _serviceWrapper.Core.TaskQueue.EnqueueTask(async _ =>
                                    {
                                        await _serviceWrapper.HoaDon.HoaDonLog.InsertAsync(log);
                                    });
                                }
                            }
                        }
                        catch (System.Exception ex)

                        {
                            if (!stopwatch.IsRunning)
                                stopwatch.Start();

                            if (stopwatch.Elapsed < TimeSpan.FromSeconds(30))
                            {
                                needRetry = true;
                            }
                            else

                            {
                                var log = new hoa_don_log()
                                {
                                    file_thong_diep_url = filePath,
                                    ngay_thuc_hien = DateTime.Now,
                                    nguoi_thuc_hien = userInfo.full_name,
                                    noi_dung_thuc_hien = $"Gửi thông điệp lỗi {ex.Message}",
                                    hoa_don_id = hoaDon.id,
                                    hoa_don_log_type_id = -1 * (int)e_hoa_don_log_type.GUI_THONG_DIEP
                                };
                                log.SetInsertInfo(userId);
                                _serviceWrapper.Core.TaskQueue.EnqueueTask(async _ =>
                                {
                                    await _serviceWrapper.HoaDon.HoaDonLog.InsertAsync(log);
                                });
                            }
                        }

                        if (needRetry)
                        {
                            var remaining = TimeSpan.FromSeconds(30) - stopwatch.Elapsed;

                            if (remaining > TimeSpan.Zero)
                            {
                                var delayTime = remaining > TimeSpan.FromSeconds(10)
                                    ? TimeSpan.FromSeconds(10)
                                    : remaining;

                                await Task.Delay(delayTime);
                            }

                        }

                    }
                    while (needRetry);

                }
                finally
                {
                    _semaphore.Release();
                }

                // _logExecutionTimeHelper.WriteLog("Đóng kết nối");
                return new SuccessResult<HoaDonPhatHanhRespone>();
            }
        }

        public async Task<FunctionResult<HoaDonPhatHanhRespone>> PhatHanhHoaDonKhongMaAsync(hoa_don hoaDon,
            string signed_text, int user_id_phathanh = 0)
        {
            var userId = this.GetCurrentUserId();
            var userInfo = this.GetCurrentUser();
            if (user_id_phathanh > 0)
            {
                userId = user_id_phathanh;
                var objUser = await _serviceWrapper.User.User.SelectAndFormatJwtTokenAsync(userId);
                if (objUser != null) userInfo = objUser;
            }

            var hoaDonXml = signed_text.ConvertToXmlFromBase64();
            // var hoaDonXmlObj = hoaDonXml.ConvertToObject<Model.Request.Xml.HoaDon>(true);
            var uuid = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
            await _serviceWrapper.Cache.SetDataAsync<string>(uuid, "hoa_don", DateTime.Now.AddDays(30));
            await _repositoryWrapper.HoaDon.PhatHanhUUID.SaveLogUuidAsync(uuid, "hoa_don", userId);

            var thongDiep = new ThongDiep()
            {
                ThongTinChung = new ThongTinChungThongDiep()
                {
                    phien_ban = hoaDon.phien_ban,
                    ma_noi_gui = AppSettings.FixedValue.MNGui,
                    ma_noi_nhan = AppSettings.FixedValue.MNNhan,
                    thong_diep = "203",
                    ma_noi_gui_uuid = $"{AppSettings.FixedValue.MNGui}{uuid}".ToUpper(),
                    ma_thong_diep_tham_chieu = $"",
                    mst = hoaDon.nguoi_ban_mst,
                    so_luong = 1
                },
            };
            hoaDon.phat_hanh_uuid = uuid;
            hoaDon.user_id_phathanh = userId;
            await this.UpdateAsync(hoaDon);
            var base64thongdiep = thongDiep.ConvertToXmlAndAppendChild("/TDiep", "DLieu", hoaDonXml).ConvertToBase64();
            using (var client = Helper.WSInterTRCA2Helper.GetClient())
            {
                await client.OpenAsync();
                var authHeader = Helper.WSInterTRCA2Helper.GetAuthHeader();
                await _serviceWrapper.Cache.SetDataAsync<hoa_don>(uuid + "_hoa_don", hoaDon, DateTime.Now.AddDays(30));


                //
                var fileName = Guid.NewGuid().ToString() + ".xml";
                // var filePath = $"Xml/{DateTime.Now.Year}/{DateTime.Now.Month}/{fileName}";
                var filePath = $"Xml/{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{fileName}";
                var directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                await File.WriteAllTextAsync(filePath, base64thongdiep.ConvertToXmlFromBase64());


                try
                {
                    await _semaphore.WaitAsync();

                    try
                    {
                        bool needRetry = false;
                        var stopwatch = new System.Diagnostics.Stopwatch();

                        do
                        {
                            needRetry = false;
                            try
                            {
                                var guiThongDiepResult = await client.Guithongdiep2024Async(authHeader, base64thongdiep, 1);

                                if (guiThongDiepResult.Guithongdiep2024Result.ConvertToString().Length > 2)
                                {
                                    var log = new hoa_don_log()
                                    {
                                        file_thong_diep_url = filePath,
                                        ngay_thuc_hien = DateTime.Now,
                                        nguoi_thuc_hien = userInfo.full_name,
                                        noi_dung_thuc_hien = "Gửi thông điệp lên CQT",
                                        hoa_don_id = hoaDon.id,
                                        hoa_don_log_type_id = (int)e_hoa_don_log_type.GUI_THONG_DIEP
                                    };
                                    log.SetInsertInfo(userId);
                                    _serviceWrapper.Core.TaskQueue.EnqueueTask(async _ =>
                                    {
                                        await _serviceWrapper.HoaDon.HoaDonLog.InsertAsync(log);
                                    });
                                }
                                else
                                {
                                    if (!stopwatch.IsRunning)
                                        stopwatch.Start();

                                    if (stopwatch.Elapsed < TimeSpan.FromSeconds(30))
                                    {
                                        needRetry = true;
                                    }
                                    else
                                    {
                                        var log = new hoa_don_log()
                                        {
                                            file_thong_diep_url = filePath,
                                            ngay_thuc_hien = DateTime.Now,
                                            nguoi_thuc_hien = userInfo.full_name,
                                            noi_dung_thuc_hien = $"Gửi thông điệp thất bại {guiThongDiepResult.Guithongdiep2024Result.ConvertToString()}",
                                            hoa_don_id = hoaDon.id,
                                            hoa_don_log_type_id = -1 * (int)e_hoa_don_log_type.GUI_THONG_DIEP
                                        };
                                        log.SetInsertInfo(userId);
                                        _serviceWrapper.Core.TaskQueue.EnqueueTask(async _ =>
                                        {
                                            await _serviceWrapper.HoaDon.HoaDonLog.InsertAsync(log);
                                        });
                                    }
                                }
                            }
                            catch (System.Exception ex)
                            {
                                if (!stopwatch.IsRunning)
                                    stopwatch.Start();

                                if (stopwatch.Elapsed < TimeSpan.FromSeconds(30))
                                {
                                    needRetry = true;
                                }
                                else
                                {
                                    var log = new hoa_don_log()
                                    {
                                        file_thong_diep_url = filePath,
                                        ngay_thuc_hien = DateTime.Now,
                                        nguoi_thuc_hien = userInfo.full_name,
                                        noi_dung_thuc_hien = $"Gửi thông điệp thất bại {ex.Message}",
                                        hoa_don_id = hoaDon.id,
                                        hoa_don_log_type_id = -1 * (int)e_hoa_don_log_type.GUI_THONG_DIEP
                                    };
                                    log.SetInsertInfo(userId);
                                    _serviceWrapper.Core.TaskQueue.EnqueueTask(async _ =>
                                    {
                                        await _serviceWrapper.HoaDon.HoaDonLog.InsertAsync(log);
                                    });
                                }
                            }

                            if (needRetry)
                            {
                                var remaining = TimeSpan.FromSeconds(30) - stopwatch.Elapsed;

                                if (remaining > TimeSpan.Zero)
                                {
                                    var delayTime = remaining > TimeSpan.FromSeconds(10)
                                        ? TimeSpan.FromSeconds(10)
                                        : remaining;

                                    await Task.Delay(delayTime);
                                }
                            }

                        }
                        while (needRetry);

                    }
                    finally
                    {
                        await client.CloseAsync();   // ✅ đúng scope
                    }

                }
                finally
                {
                    _semaphore.Release();            // ✅ đúng scope
                }

                return new SuccessResult<HoaDonPhatHanhRespone>();
            }
        }

        public async Task<FunctionResult<string>> GetSoHoaDonMTTAsync(string donvi_ma_dv, string mau_so, string ky_hieu, int hoa_don_id)
        {
            var toKhais = await _serviceWrapper.ToKhaiSerivceWrapper.ToKhai.SelectByDonViAsync(donvi_ma_dv);
            var toKhaiKyHieu = toKhais.Where(x => x.to_khai_status_id == (int)e_to_khai_status.CQT_DONG_Y)
                .OrderByDescending(x => x.id).FirstOrDefault();
            if (toKhaiKyHieu != null)
            {
                // var getMaxSoHoaDon =
                //     await _repositoryWrapper.HoaDon.HoaDon.GetMaxMaSoHoaDonMTT(donvi_ma_dv, mau_so, DateTime.Now.Year);
                // var soHoaDonDuKien = 1;
                // if (getMaxSoHoaDon.Length >= 11)
                // {
                //     soHoaDonDuKien = getMaxSoHoaDon.Substring(getMaxSoHoaDon.ConvertToString().Length - 11)
                //         .ConvertToInt() + 1;
                // }
                var soHoaDonDuKien = hoa_don_id;
                var soHoaDonDuKienText = "279" + soHoaDonDuKien.ToString().PadLeft(8, '0');
                var maHoaDon =
                    $"M{mau_so}-{DateTime.Now.Year.ToString().Substring(2)}-{toKhaiKyHieu.ma_dang_ky.ConvertToString()}-{soHoaDonDuKienText}";
                return new SuccessResult<string>(maHoaDon);
            }

            return new ErrorResult<string>("Không tạo được số hóa đơn hợp lệ");
        }

        public async Task<FunctionResult<int>> GetSoHoaDonAsyn(string donvi_ma_dv, string mau_so, string ky_hieu)
        {
            var getMaxSoHoaDon = await _repositoryWrapper.HoaDon.HoaDon.GetMaxMaSoHoaDon(donvi_ma_dv, mau_so, ky_hieu);
            var soHoaDonDuKien = getMaxSoHoaDon.ConvertToInt() + 1;
            var hoaDonPhatHanhDonVis = await _hoaDonDangKyPhatHanhService.SelectByDonViAsync(donvi_ma_dv);
            var hoaDonPhatHanhMauSoKyHieu = hoaDonPhatHanhDonVis.Where(x =>
                x.mau_so == mau_so && x.ky_hieu == ky_hieu && x.ngay_su_dung.Date <= DateTime.Now.Date
                && (x.so_bat_dau.ConvertToInt() <= soHoaDonDuKien || soHoaDonDuKien == 1) &&
                soHoaDonDuKien <= x.so_ket_thuc.ConvertToInt()
            ).OrderByDescending(x => x.ngay_qd).FirstOrDefault();
            if (hoaDonPhatHanhMauSoKyHieu == null)
            {
                return new ErrorResult<int>("Không tạo được số hóa đơn hợp lệ");
            }

            return new SuccessResult<int>(soHoaDonDuKien == 1
                ? hoaDonPhatHanhMauSoKyHieu.so_bat_dau.ConvertToInt()
                : soHoaDonDuKien);
        }

        private static bool LaHoaDonNhapCoSoChuaKySo(hoa_don hd)
        {
            if (hd == null || hd.is_deleted)
            {
                return false;
            }

            if (hd.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_DA_HUY_NOI_BO)
            {
                return false;
            }

            if (hd.ma_so_hoa_don.ConvertToInt() <= 0)
            {
                return false;
            }

            if (hd.hoa_don_trang_thai_id != (int)e_hoa_don_trang_thai.NHAP)
            {
                return false;
            }

            if (hd.is_ky_so_succes == true)
            {
                return false;
            }

            return true;
        }

        private async Task<FunctionResult<string>> ValidateKhongCoSoHoaDonNhoHonChuaKySoAsync(hoa_don hoaDon)
        {
            var maSo = hoaDon.ma_so_hoa_don.ConvertToInt();
            if (maSo <= 0)
            {
                return new SuccessResult<string>();
            }

            var pending = await _repositoryWrapper.HoaDon.HoaDon.SelectSoHoaDonNhoHonChuaKySoAsync(
                hoaDon.donvi_ma_dv,
                hoaDon.hoa_don_dang_ky_phat_hanh_mau_so,
                hoaDon.hoa_don_dang_ky_phat_hanh_ky_hieu,
                hoaDon.id,
                maSo,
                hoaDon.ngay_hoa_don);
            if (pending == null || pending.id <= 0 || pending.ma_so_hoa_don <= 0)
            {
                return new SuccessResult<string>();
            }

            var pendingDb = await this.SelectByIdAsync(pending.id);
            if (!LaHoaDonNhapCoSoChuaKySo(pendingDb))
            {
                return new SuccessResult<string>();
            }

            return new ErrorResult<string>(
                $"Còn hóa đơn số {pendingDb.ma_so_hoa_don} ngày {pendingDb.ngay_hoa_don:dd/MM/yyyy} chưa ký số. Vui lòng ký số hóa đơn đó trước.");
        }

        public async Task<FunctionResult<HoaDonPhatHanhRespone>> XuLyThongDiepAsync(string thongDiep)
        {
            var ketQuaThongDiepRespone = thongDiep.ConvertToObject<Model.Respone.Xml.KetQuaThongDiepRespone>();
            if (ketQuaThongDiepRespone == null)
            {
                return new ErrorResult<HoaDonPhatHanhRespone>("Không xử lý được thông điệp");
            }

            var maThamChieu = ketQuaThongDiepRespone.TTChung.MTDTChieu;
            // CQT trả về MTDTChieu prefix V0103930279 (khác MNGui gửi lên: 0103930279-001)
            var uuid = maThamChieu.Replace("V0103930279", "");
            var cachedDataType = await _serviceWrapper.Cache.GetDataAsync<string>(uuid);
            var cachedTypes = new List<string> { "hoa_don", "tbss", "to_khai", "bang_ke_mtt" };
            if (!cachedTypes.Contains(cachedDataType.ConvertToString()))
            {
                try
                {
                    var cachedTypeDb = await _repositoryWrapper.HoaDon.PhatHanhUUID.SelectByUuIdAsync(uuid);
                    if (cachedTypeDb != null)
                    {
                        cachedDataType = cachedTypeDb.type_name;
                    }
                }
                catch
                {
                    // TODO
                }
            }

            if (cachedDataType == "hoa_don")
            {
                var cachedData = await _serviceWrapper.Cache.GetDataAsync<hoa_don>(uuid + "_hoa_don");
                var hoaDon = cachedData != null ? cachedData : await this.SelectByPhatHanhUuidAsync(uuid);
                var service = await _serviceWrapper.HoaDon.XyLyThongDiepProvider.GetServiceAsync(hoaDon);

                if (service != null)
                {
                    if (ThongDiepHoaDonHelper.ShouldRunFullXuLy(ketQuaThongDiepRespone, thongDiep))
                    {
                        var result = await service.XuLyThongDiepAsync(hoaDon, ketQuaThongDiepRespone, thongDiep);
                        await _serviceWrapper.HoaDon.PushMessageToVender.CheckAndPushMessageAsync(hoaDon);
                        await _hoaDonPhatHanhHub.OnNewNotifyCreated(
                            new Model.Request.Hub.HoaDonPhatHanhPushNotifyModel()
                            {
                                file_thong_diep_url = result.data.file_thong_diep_url,
                                hoa_don_trang_thai_id = result.data.hoa_don_trang_thai_id,
                                id = result.data.id,
                                ket_qua_phat_hanh = result.data.ket_qua_phat_hanh,
                                user_id = hoaDon.user_id_phathanh.ToString()
                            });

                        if (result.data.hoa_don_trang_thai_id == (int)e_hoa_don_trang_thai.DA_PHAT_HANH)
                        {
                            await CapNhatHoaDonGocSauPhatHanhThanhCongAsync(result.data.id, hoaDon.user_id_phathanh);

                            await _serviceWrapper.HoaDon.HoaDonSendEmail.SendEmailHoaDonAsync(new List<int>()
                                { result.data.id }, true);

                        }

                        var donVi = await _serviceWrapper.Category.DonVi.SelectByMaDonViAsync(hoaDon.donvi_ma_dv);
                        if (donVi != null)
                        {
                            if (donVi.ngay_hoa_don_max == null ||
                                donVi.ngay_hoa_don_max.Value.Date <= hoaDon.ngay_hoa_don.Date)
                            {
                                donVi.ngay_hoa_don_max = hoaDon.ngay_hoa_don.Date;
                                await _serviceWrapper.Category.DonVi.UpdateAsync(donVi);
                            }
                        }

                        return result;
                    }
                    else
                    {
                        /// 999 thì chỉ lưu lại log file
                        await _hoaDonLogService.SaveFromPhatHanhAsync(hoaDon.id, string.Empty, thongDiep, false);
                    }
                }
            }

            if (cachedDataType == "tbss")
            {
                var cachedData = await _serviceWrapper.Cache.GetDataAsync<thong_bao_sai_sot>(uuid + "_tbss");
                var thongBaoSaiSot = cachedData != null
                    ? cachedData
                    : await _repositoryWrapper.ThongBaoSaiSot.ThongBaoSaiSot.SelectByPhatHanhUuidAsync(uuid);
                var result =
                    await _serviceWrapper.ThongBaoSaiSot.ThongBaoSaiSot.XuLyThongDiepAsync(thongBaoSaiSot,
                        ketQuaThongDiepRespone, thongDiep);

                return new SuccessResult<HoaDonPhatHanhRespone>();
            }

            if (cachedDataType == "to_khai")
            {
                var cachedData = await _serviceWrapper.Cache.GetDataAsync<to_khai>(uuid + "_to_khai");
                var toKhai = cachedData != null
                    ? cachedData
                    : await _repositoryWrapper.ToKhaiWrapper.ToKhai.SelectByPhatHanhUuidAsync(uuid);
                var result =
                    await _serviceWrapper.ToKhaiSerivceWrapper.ToKhai.XuLyThongDiepAsync(toKhai, ketQuaThongDiepRespone,
                        thongDiep);

                return new SuccessResult<HoaDonPhatHanhRespone>();
            }

            if (cachedDataType == "bang_ke_mtt")
            {
                // var cachedData = await _serviceWrapper.Cache.GetDataAsync<List<int>>(uuid + "_bang_ke_mtt");
                var result =
                    await _serviceWrapper.HoaDon.KyLo.XuLyThongDiepKetQuaPhanHanhAsync(ketQuaThongDiepRespone,
                        thongDiep);
                return new SuccessResult<HoaDonPhatHanhRespone>();
            }

            return new ErrorResult<HoaDonPhatHanhRespone>("Không tìm thấy dữ liệu");
            //truong hop thong diep tra ve lau -> k con cache

            var toKhaiCheck = await _repositoryWrapper.ToKhaiWrapper.ToKhai.SelectByPhatHanhUuidAsync(uuid);
            if (toKhaiCheck != null)
            {
                var result =
                    await _serviceWrapper.ToKhaiSerivceWrapper.ToKhai.XuLyThongDiepAsync(toKhaiCheck,
                        ketQuaThongDiepRespone, thongDiep);
                return new SuccessResult<HoaDonPhatHanhRespone>();
            }

            var hoaDonCheck = await this.SelectByPhatHanhUuidAsync(uuid);
            if (hoaDonCheck != null)
            {
                var serviceCheck = await _serviceWrapper.HoaDon.XyLyThongDiepProvider.GetServiceAsync(hoaDonCheck);
                if (serviceCheck != null)
                {
                    var result = await serviceCheck.XuLyThongDiepAsync(hoaDonCheck, ketQuaThongDiepRespone, thongDiep);
                    await _hoaDonPhatHanhHub.OnNewNotifyCreated(new Model.Request.Hub.HoaDonPhatHanhPushNotifyModel()
                    {
                        file_thong_diep_url = result.data.file_thong_diep_url,
                        hoa_don_trang_thai_id = result.data.hoa_don_trang_thai_id,
                        id = result.data.id,
                        ket_qua_phat_hanh = result.data.ket_qua_phat_hanh,
                        user_id = hoaDonCheck.user_id_phathanh.ToString()
                    });
                    return result;
                }
            }

            var thongBaoSaiSotCheck =
                await _repositoryWrapper.ThongBaoSaiSot.ThongBaoSaiSot.SelectByPhatHanhUuidAsync(uuid);
            if (thongBaoSaiSotCheck != null)
            {
                var result =
                    await _serviceWrapper.ThongBaoSaiSot.ThongBaoSaiSot.XuLyThongDiepAsync(thongBaoSaiSotCheck,
                        ketQuaThongDiepRespone, thongDiep);
                return new SuccessResult<HoaDonPhatHanhRespone>();
            }

            return new ErrorResult<HoaDonPhatHanhRespone>("Không xử lý được thông điệp");
        }

        public async Task<FunctionResult<string>> UpdteKySoSuccessAsync(HoaDonPhatHanhRequest request, int user_id = 0)
        {
            var obj = await this.SelectByIdAsync(request.id);
            var user = this.GetCurrentUser();
            if (user_id > 0)
            {
                var objUser = await _serviceWrapper.User.User.SelectByIdAsync(user_id);
                if (objUser != null)
                {
                    user = objUser.Map<JwtTokenInfo>();
                }
            }

            //tạo khóa cho đơn vị, mẫu số , ký hiệu -> tránh sinh cùng mã
            var donViHoaDonLock = GetLockForDonVi(obj.donvi_ma_dv, obj.hoa_don_dang_ky_phat_hanh_mau_so,
                obj.hoa_don_dang_ky_phat_hanh_ky_hieu, "UpdteKySoSuccessAsync");
            var isHoldKey = false;
            if (obj != null && obj.is_ky_so_succes != true)
            {
                try
                {
                    await donViHoaDonLock.WaitAsync();
                    isHoldKey = true;
                    if (obj.ma_so_hoa_don.ConvertToString() == "")
                    {
                        var soHoaDonResult = await this.GetSoHoaDonAsyn(obj.donvi_ma_dv,
                            obj.hoa_don_dang_ky_phat_hanh_mau_so, obj.hoa_don_dang_ky_phat_hanh_ky_hieu);
                        if (!soHoaDonResult.is_success)
                        {
                            return new ErrorResult<string>("Không sinh được số hóa đơn");
                        }

                        obj.ma_so_hoa_don = soHoaDonResult.data;
                        obj.so_hoa_don = obj.hoa_don_dang_ky_phat_hanh_mau_so + obj.hoa_don_dang_ky_phat_hanh_ky_hieu +
                                         obj.ma_so_hoa_don;
                    }

                    var validateSoKySo = await this.ValidateKhongCoSoHoaDonNhoHonChuaKySoAsync(obj);
                    if (!validateSoKySo.is_success)
                    {
                        return new ErrorResult<string>(validateSoKySo.message);
                    }

                    obj.is_ky_so_succes = true;
                    obj.hoa_don_trang_thai_id = (int)e_hoa_don_trang_thai.CHUA_GUI_CQT;

                    var CKM = this.GetHoaDonType(obj.hoa_don_dang_ky_phat_hanh_ky_hieu);
                    if (CKM == "M" && obj.ma_so_hoa_don_mtt.ConvertToString() == "")
                    {
                        var soHoaDonMTTResult = await this.GetSoHoaDonMTTAsync(obj.donvi_ma_dv,
                            obj.hoa_don_dang_ky_phat_hanh_mau_so, obj.hoa_don_dang_ky_phat_hanh_ky_hieu, obj.id);
                        if (!soHoaDonMTTResult.is_success)
                        {
                            return new ErrorResult<string>(soHoaDonMTTResult.message);
                        }

                        obj.ma_so_hoa_don_mtt = soHoaDonMTTResult.data;
                    }

                    obj.SetUpdateInfo(user.id);
                    var isUpdated = await this.UpdateAsync(obj);
                    if (isUpdated)
                    {
                        if (isHoldKey)
                        {
                            try
                            {
                                donViHoaDonLock.Release();
                                isHoldKey = false;
                            }
                            finally
                            {
                            }
                        }

                        var fileName = Guid.NewGuid().ToString() + ".xml";
                        var filePath = $"Xml/{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{fileName}";
                        var directoryPath = Path.GetDirectoryName(filePath);
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        var hoaDonXml = request.signed_text.ConvertToXmlFromBase64();
                        await File.WriteAllTextAsync(filePath, hoaDonXml);
                        var log = new hoa_don_log()
                        {
                            file_thong_diep_url = filePath,
                            ngay_thuc_hien = DateTime.Now,
                            nguoi_thuc_hien = user.full_name,
                            noi_dung_thuc_hien = "Ký số thành công",
                            hoa_don_id = obj.id,
                            hoa_don_log_type_id = (int)e_hoa_don_log_type.KY_SO_SUCCESS
                        };
                        log.SetInsertInfo(user.id);
                        await _serviceWrapper.HoaDon.HoaDonLog.InsertAsync(log);
                        var donVi = await _serviceWrapper.Category.DonVi.SelectByMaDonViAsync(obj.donvi_ma_dv);
                        if (donVi != null)
                        {
                            donVi.total_cks_con_lai = donVi.total_cks_con_lai.ConvertToInt() - 1;
                            await _serviceWrapper.Category.DonVi.UpdateAsync(donVi);
                        }

                        //
                        if (obj.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_DIEU_CHINH ||
        obj.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_THAY_THE)
                        {
                            if (request.bienBanSignedText != null)
                            {
                                var bienBanXml = request.bienBanSignedText.ConvertToXmlFromBase64();
                                var fileNameBienBan = Guid.NewGuid().ToString() + ".xml";
                                var filePathBienBan = $"Xml/{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{fileNameBienBan}";
                                await File.WriteAllTextAsync(filePathBienBan, bienBanXml);
                                var logBienBan = new hoa_don_log()
                                {
                                    file_thong_diep_url = filePathBienBan,
                                    ngay_thuc_hien = DateTime.Now,
                                    nguoi_thuc_hien = user.full_name,
                                    noi_dung_thuc_hien = "Ký số Biên bản thành công",
                                    hoa_don_id = obj.id,
                                    hoa_don_log_type_id = (int)e_hoa_don_log_type.KY_SO_XML_BIEN_BAN_THANH_CONG
                                };
                                logBienBan.SetInsertInfo(user.id);
                                await _serviceWrapper.HoaDon.HoaDonLog.InsertAsync(logBienBan);
                            }
                        }

                    }
                }
                finally
                {
                    if (isHoldKey)
                    {
                        donViHoaDonLock.Release(); // Giải phóng khóa nếu đã giữ khóa
                    }
                }

                return new SuccessResult<string>();
            }

            return new ErrorResult<string>("Không tìm thấy dữ liệu hoặc hóa đơn đã ký số trước đó");
        }

        public Task<hoa_don> SelectByPhatHanhUuidAsync(string phat_hanh_uuid)
        {
            return _repositoryWrapper.HoaDon.HoaDon.SelectByPhatHanhUuidAsync(phat_hanh_uuid);
        }

        public async Task<FunctionResult<string>> GetHtmlPrintAsync(int id, int page_size = 10,
            MauHoaDonInChuyenDoiParam chuyenDoiParam = null)
        {
            var hoaDon = await this.SelectByIdAsync(id);
            if (hoaDon == null) return new ErrorResult<string>("Dữ liệu không hợp lệ");
            if (hoaDon.hoa_don_trang_thai_id == (int)e_hoa_don_trang_thai.NHAP)
            {
                // var previewModel = hoaDon.Map<HoaDonAddOrEditModel>();
                // previewModel.hoang_hoas =
                //     (await _serviceWrapper.HoaDon.HoaDonHangHoa.SelectByHoaDonIdAsync(id)).ToList();
                // previewModel.loai_phis = (await _serviceWrapper.HoaDon.HoaDonLoaiPhi.SelectByHoaDonAsync(id)).ToList();
                return await this.GetHtmlPreviewAsync(id);
            }

            var getHtmlHoaDon = await _serviceWrapper.HoaDon.MauHoaDon.CreatePrintHtmlAsync(hoaDon, page_size, chuyenDoiParam);
            if (getHtmlHoaDon.is_success)
            {
                var result = getHtmlHoaDon.data;
                if (hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_DIEU_CHINH ||
                   hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_THAY_THE)
                {
                    var getHtmlBienBan = await this.GetHtmlPrintBienBanAsync(id);
                    var htmlBienBan = getHtmlBienBan.is_success ? getHtmlBienBan.data : string.Empty;
                    if (htmlBienBan != string.Empty)
                        result += "<div class=\"page-break\"></div>" + htmlBienBan;
                }
                return new SuccessResult<string>(result);
            }
            return new ErrorResult<string>(getHtmlHoaDon.message);
        }


        public async Task<FunctionResult<string>> GetHtmlForDownloadAsync(int id, int page_size = 10,
           MauHoaDonInChuyenDoiParam chuyenDoiParam = null)
        {
            var hoaDon = await this.SelectByIdAsync(id);
            if (hoaDon == null) return new ErrorResult<string>("Dữ liệu không hợp lệ");
            if (hoaDon.hoa_don_trang_thai_id == (int)e_hoa_don_trang_thai.NHAP)
            {
                // var previewModel = hoaDon.Map<HoaDonAddOrEditModel>();
                // previewModel.hoang_hoas =
                //     (await _serviceWrapper.HoaDon.HoaDonHangHoa.SelectByHoaDonIdAsync(id)).ToList();
                // previewModel.loai_phis = (await _serviceWrapper.HoaDon.HoaDonLoaiPhi.SelectByHoaDonAsync(id)).ToList();
                return await this.GetHtmlPreviewAsync(id);
            }

            var getHtmlHoaDon = await _serviceWrapper.HoaDon.MauHoaDon.CreatePrintHtmlAsync(hoaDon, page_size, chuyenDoiParam);
            if (getHtmlHoaDon.is_success)
            {
                var result = getHtmlHoaDon.data;
                return new SuccessResult<string>(result);
            }
            return new ErrorResult<string>(getHtmlHoaDon.message);
        }


        public async Task<FunctionResult<string>> GetHtmlPreviewAsync(HoaDonAddOrEditModel model)
        {
            LogWriter.Writer($"GetHtmlPreviewAsync Start", "api/hoa-don/{id}/print", "");
            var donVi = await _serviceWrapper.Category.DonVi.SelectByMaDonViAsync(model.donvi_ma_dv);
            if (donVi == null) return new ErrorResult<string>("Không tìm thấy đơn vị");
            model.ket_qua_phat_hanh = "";
            model.nguoi_ban_dia_chi = donVi.dia_chi;
            model.nguoi_ban_dien_thoai = donVi.dien_thoai;
            model.nguoi_ban_email = donVi.email;
            model.nguoi_ban_fax = donVi.fax;
            model.nguoi_ban_mst = donVi.mst;
            model.nguoi_ban_ngan_hang = donVi.ngan_hang;
            model.nguoi_ban_ten_donvi = donVi.ten_dv;
            model.nguoi_ban_website = donVi.website;
            return await _serviceWrapper.HoaDon.MauHoaDon.CreatePreviewHtmlAsync(model, false);
        }
        public async Task<FunctionResult<string>> GetHtmlPreviewAsync(int id)
        {
            // LogWriter.Writer($"GetHtmlPreviewAsync Start", "api/hoa-don/{id}/print", "");
            // var donVi = await _serviceWrapper.Category.DonVi.SelectByMaDonViAsync(model.donvi_ma_dv);
            // if (donVi == null) return new ErrorResult<string>("Không tìm thấy đơn vị");
            // model.ket_qua_phat_hanh = "";
            // model.nguoi_ban_dia_chi = donVi.dia_chi;
            // model.nguoi_ban_dien_thoai = donVi.dien_thoai;
            // model.nguoi_ban_email = donVi.email;
            // model.nguoi_ban_fax = donVi.fax;
            // model.nguoi_ban_mst = donVi.mst;
            // model.nguoi_ban_ngan_hang = donVi.ngan_hang;
            // model.nguoi_ban_ten_donvi = donVi.ten_dv;
            // model.nguoi_ban_website = donVi.website;
            return await _serviceWrapper.HoaDon.MauHoaDon.CreatePreviewHtmlAsync(id, false);
        }

        public Task<IEnumerable<hoa_don>> SelectByIdsAsync(List<int> ids)
        {
            return _repositoryWrapper.HoaDon.HoaDon.SelectByIdsAsync(ids);
        }
        public async Task<FunctionResult<HoaDonPhatHanhRespone>> PhatHanhMTTAsync(HoaDonPhatHanhRequest request,
            hoa_don hoaDon, int user_id_phathanh = 0)
        {
            try
            {
                //tránh phát hành chưa xong user gọi phát hành tiếp -> phát hành lần đầu tạo lock -> phát hành xong xóa lock
                var isGetLockKey = await AcquireLockHoaDonAsync(request.id);
                if (!isGetLockKey) return new ErrorResult<HoaDonPhatHanhRespone>("Tạo khóa phát hành thất bại");
                var hoaDonLogs = await _hoaDonLogService.SelectByHoaDonAsync(request.id);
                var logGui = hoaDonLogs.Where(x => x.hoa_don_log_type_id == (int)e_hoa_don_log_type.GUI_THONG_DIEP)
                    .FirstOrDefault();
                if (logGui != null)
                {
                    return new ErrorResult<HoaDonPhatHanhRespone>("Hóa đơn đã gửi thông điệp");
                }

                if (request.signed_text == "")
                {
                    var hoaDonLogKySo = hoaDonLogs
                        .Where(x => x.hoa_don_log_type_id == (int)e_hoa_don_log_type.KY_SO_SUCCESS)
                        .OrderByDescending(x => x.created_time).FirstOrDefault();
                    if (hoaDonLogKySo == null)
                        return new ErrorResult<HoaDonPhatHanhRespone>("Không tìm thấy dữ liệu hợp lệ");
                    var signedTextXmlFile = hoaDonLogKySo.file_thong_diep_url;
                    var xmlContent = File.ReadAllText(signedTextXmlFile);
                    request.signed_text = xmlContent.ConvertToBase64();
                }

                // var base64thongdiep = request.signed_text;
                var base64thongdiep = request.signed_text;
                var user = this.GetCurrentUser();
                if (user_id_phathanh > 0)
                {
                    var objUser = await _serviceWrapper.User.User.SelectAndFormatJwtTokenAsync(user_id_phathanh);
                    if (objUser != null) user = objUser;
                }

                using (var client = Helper.WSInterTRCA2Helper.GetClient())
                {
                    await client.OpenAsync();
                    var authHeader = Helper.WSInterTRCA2Helper.GetAuthHeader();
                    // await _serviceWrapper.Cache.SetDataAsync<hoa_don>(uuid + "_hoa_don", hoaDon, DateTime.Now.AddDays(3));
                    //
                    var fileName = Guid.NewGuid().ToString() + ".xml";
                    // var filePath = $"Xml/{DateTime.Now.Year}/{DateTime.Now.Month}/{fileName}";
                    var filePath = $"Xml/{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{fileName}";
                    var directoryPath = Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    await File.WriteAllTextAsync(filePath, base64thongdiep.ConvertToXmlFromBase64());


                    try
                    {
                        await _semaphore.WaitAsync();
                        try
                        {
                            var guiThongDiepResult = await client.Guithongdiep2024Async(authHeader, base64thongdiep, 1);
                            if (guiThongDiepResult.Guithongdiep2024Result.ConvertToString().Length > 2)
                            {
                                var log = new hoa_don_log()
                                {
                                    file_thong_diep_url = filePath,
                                    ngay_thuc_hien = DateTime.Now,
                                    nguoi_thuc_hien = user.full_name,
                                    noi_dung_thuc_hien = "Gửi thông điệp lên CQT",
                                    hoa_don_id = hoaDon.id,
                                    hoa_don_log_type_id = (int)e_hoa_don_log_type.GUI_THONG_DIEP
                                };
                                log.SetInsertInfo(user.id);
                                _serviceWrapper.Core.TaskQueue.EnqueueTask(async _ =>
                                {
                                    await _serviceWrapper.HoaDon.HoaDonLog.InsertAsync(log);
                                });

                                //updaate hóa đơn
                            }
                            else
                            {
                                var log = new hoa_don_log()
                                {
                                    file_thong_diep_url = filePath,
                                    ngay_thuc_hien = DateTime.Now,
                                    nguoi_thuc_hien = user.full_name,
                                    noi_dung_thuc_hien = $"Gửi thông điệp thất bại {guiThongDiepResult.Guithongdiep2024Result.ConvertToString()}",
                                    hoa_don_id = hoaDon.id,
                                    hoa_don_log_type_id = -1 * (int)e_hoa_don_log_type.GUI_THONG_DIEP
                                };
                                log.SetInsertInfo(user.id);
                                _serviceWrapper.Core.TaskQueue.EnqueueTask(async _ =>
                                {
                                    await _serviceWrapper.HoaDon.HoaDonLog.InsertAsync(log);
                                });
                            }
                        }
                        catch (System.Exception ex)
                        {
                            var log = new hoa_don_log()
                            {
                                file_thong_diep_url = filePath,
                                ngay_thuc_hien = DateTime.Now,
                                nguoi_thuc_hien = user.full_name,
                                noi_dung_thuc_hien = $"Gửi thông điệp thất bại {ex.Message.ConvertToString()}",
                                hoa_don_id = hoaDon.id,
                                hoa_don_log_type_id = -1 * (int)e_hoa_don_log_type.GUI_THONG_DIEP
                            };
                            log.SetInsertInfo(user.id);
                            _serviceWrapper.Core.TaskQueue.EnqueueTask(async _ =>
                            {
                                await _serviceWrapper.HoaDon.HoaDonLog.InsertAsync(log);
                            });
                        }
                        finally
                        {
                            await client.CloseAsync();
                        }



                    }
                    finally
                    {
                        _semaphore.Release();
                    }

                    // LogWriter.Writer(guiThongDiepResult.Guithongdiep2024Result, base64thongdiep, hoaDon.id.ToString());
                    // LogWriter.Writer(Newtonsoft.Json.JsonConvert.SerializeObject(guiThongDiepResult), base64thongdiep, hoaDon.id.ToString());

                    return new SuccessResult<HoaDonPhatHanhRespone>();
                }
            }
            finally
            {
                await ReleaseLockHoaDonAsync(request.id);
            }
        }

        public async Task<FunctionResult<HoaDonPhatHanhRespone>> PhatHanhHoaDonMTTAsync(hoa_don hoaDon,
            string signed_text)
        {
            return null;
            // var userId = this.GetCurrentUserId();
            // var hoaDonXml = signed_text.ConvertToXmlFromBase64();
            // // var hoaDonXmlObj = hoaDonXml.ConvertToObject<Model.Request.Xml.HoaDon>(true);
            // var uuid = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
            // await _serviceWrapper.Cache.SetDataAsync<string>(uuid, "hoa_don", DateTime.Now.AddDays(3));

            // var thongDiep = new ThongDiep()
            // {

            //     ThongTinChung = new ThongTinChungThongDiep()
            //     {
            //         phien_ban = hoaDon.phien_ban,
            //         ma_noi_gui = AppSettings.FixedValue.MNGui,
            //         ma_noi_nhan = AppSettings.FixedValue.MNNhan,
            //         thong_diep = "206",
            //         ma_noi_gui_uuid = $"{AppSettings.FixedValue.MNGui}{uuid}".ToUpper(),
            //         ma_thong_diep_tham_chieu = $"",
            //         mst = hoaDon.nguoi_ban_mst,
            //         so_luong = 1
            //     },

            // };
            // hoaDon.phat_hanh_uuid = uuid;
            // hoaDon.user_id_phathanh = userId;
            // await this.UpdateAsync(hoaDon);
            // var base64thongdiep = thongDiep.ConvertToXmlAndAppendChild("/TDiep", "DLieu", hoaDonXml).ConvertToBase64();
            // using (var client = Helper.WSInterTRCA2Helper.GetClient())
            // {
            //     await client.OpenAsync();
            //     var authHeader = Helper.WSInterTRCA2Helper.GetAuthHeader();
            //     await _serviceWrapper.Cache.SetDataAsync<hoa_don>(uuid + "_hoa_don", hoaDon, DateTime.Now.AddDays(3));

            //     var guiThongDiepResult = await client.Guithongdiep2024Async(authHeader, base64thongdiep, 1);
            //     await client.CloseAsync();
            //     LogWriter.Writer(guiThongDiepResult.Guithongdiep2024Result, base64thongdiep, hoaDon.id.ToString());

            //     return new SuccessResult<HoaDonPhatHanhRespone>();
            // }
        }

        public Task<int> SelectHoaDonIdByMaTraCuuAsync(string maTraCuu)
        {
            return _repositoryWrapper.HoaDon.HoaDon.SelectHoaDonIdByMaTraCuuAsync(maTraCuu);
        }

        public async Task<PagingResult<IEnumerable<hoa_don_vm>>> SelectByDonViThongKePageAsync(string donvi_ma_dv,
            HoaDonSelectPagingRequest pagingRequest)
        {
            var list = await _repositoryWrapper.HoaDon.HoaDon.SelectByDonViThongKePageAsync(donvi_ma_dv, pagingRequest);
            // list.data = list.data.Select(x =>
            // {
            //     var url = AppSettings.FixedValue.FileDomain + "/hoa-don/view/" + x.id.ToString() + "?hash=" + x.id.ConvertToString().GenerateBcrypt();
            //     x.link = url;
            //     return x;
            // });

            return list;
        }

        private decimal CalcThanhTienDieuChinhLine(
            hoa_don_hang_hoa line,
            List<hoa_don_hang_hoa> allLines,
            List<hoa_don_hang_hoa> gocLines)
        {
            var goc = gocLines.FirstOrDefault(x =>
                x.ma_hang.ConvertToString() == line.ma_hang.ConvertToString()
                && (x.hang_hoa_tinh_chat_id == 1 || x.hang_hoa_tinh_chat_id == 5));
            if (goc == null)
            {
                return line.so_luong * line.don_gia;
            }

            var slInput = line.so_luong;
            var dgInput = line.don_gia;
            var soLuongGoc = goc.so_luong;
            var donGiaGoc = goc.don_gia;
            decimal thanhTienBase = 0;

            if (slInput != 0 && dgInput == 0)
            {
                thanhTienBase = slInput * donGiaGoc;
            }
            else if (slInput == 0 && dgInput != 0)
            {
                var tongSlDieuChinh = allLines
                    .Where(x => x.ma_hang.ConvertToString() == line.ma_hang.ConvertToString())
                    .Sum(x => x.so_luong);
                thanhTienBase = (soLuongGoc + tongSlDieuChinh) * dgInput;
            }
            else if (slInput != 0 && dgInput != 0)
            {
                thanhTienBase = slInput * dgInput;
            }

            return thanhTienBase;
        }

        private void CalculateThanhTienHoaDon(HoaDonAddOrEditModel model, List<hoa_don_hang_hoa> hangHoaGocs = null)
        {

            var thuesuatck = "";

            if (model.hoa_don_ly_do_dieu_chinh_id == 20)
            {
                // Điều chỉnh thuế
                return;
            }
            var mausohd = model.hoa_don_dang_ky_phat_hanh_mau_so;

            decimal tong_tien_goc = 0;

            decimal tong_tien_chiet_khau = 0;
            decimal tong_tien_mat_hang_chieu_khau = 0;
            decimal tong_thanh_tien = 0;
            decimal tong_vat = 0;
            decimal tong_tien_phi = (model.loai_phis ?? new List<hoa_don_loai_phi>()).Select(x => x.so_tien).Sum();
            decimal co_hang_hoa_dv = 0;
            var isAllChietKhau = !model.hoang_hoas
                .Where(x => x.hang_hoa_tinh_chat_id != 4)
                .Any(x => x.hang_hoa_tinh_chat_id != 3);

            if (model.hoang_hoas != null)
            {
                // Tính thành tiền (không làm tròn)
                foreach (var hang_hoa in model.hoang_hoas)
                {
                    if (hang_hoa.hang_hoa_tinh_chat_id == 3)
                    {
                        thuesuatck = hang_hoa.thue_vat;
                    }

                    // ✅ Chỉ tính hàng hóa (tính chất = 1 và = 5)
                    if (hang_hoa.hang_hoa_tinh_chat_id == 1 || hang_hoa.hang_hoa_tinh_chat_id == 5)
                    {
                        var isDieuChinhHangHoa = model.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_DIEU_CHINH
                            && hangHoaGocs != null && hangHoaGocs.Count > 0;

                        if (isDieuChinhHangHoa)
                        {
                            var i_thanh_tien_goc = CalcThanhTienDieuChinhLine(hang_hoa, model.hoang_hoas, hangHoaGocs);
                            var ty_le_chiet_khau = hang_hoa.ty_le_chiet_khau;
                            var i_tien_chiet_khau = (ty_le_chiet_khau / 100) * i_thanh_tien_goc;
                            var i_thanh_tien = i_thanh_tien_goc - i_tien_chiet_khau;
                            tong_tien_goc += i_thanh_tien_goc;
                            tong_tien_chiet_khau += i_tien_chiet_khau;
                            tong_thanh_tien += i_thanh_tien;
                            hang_hoa.thanh_tien = i_thanh_tien;
                        }
                        else if (hang_hoa.so_luong != 0 || hang_hoa.don_gia != 0)
                        {
                            var i_tong_tien_goc = hang_hoa.so_luong * hang_hoa.don_gia;
                            // if (model.loai_tien.ConvertToString() == "VND")
                            // {
                            //     i_tong_tien_goc = Math.Round(i_tong_tien_goc, MidpointRounding.ToEven);
                            // }
                            var ty_le_chiet_khau = hang_hoa.ty_le_chiet_khau;
                            var i_tien_chiet_khau = (ty_le_chiet_khau / 100) * i_tong_tien_goc;
                            var i_thanh_tien = i_tong_tien_goc - i_tien_chiet_khau;


                            tong_tien_goc += i_tong_tien_goc;
                            tong_tien_chiet_khau += i_tien_chiet_khau;
                            tong_thanh_tien += i_thanh_tien;
                            hang_hoa.thanh_tien = i_thanh_tien;
                        }
                        else
                        {
                            var i_tong_tien_goc = hang_hoa.thanh_tien;
                            var ty_le_chiet_khau = hang_hoa.ty_le_chiet_khau;
                            var i_tien_chiet_khau = (ty_le_chiet_khau / 100) * tong_tien_goc;
                            var i_thanh_tien = i_tong_tien_goc - i_tien_chiet_khau;

                            tong_tien_goc += hang_hoa.thanh_tien;
                            tong_thanh_tien += i_thanh_tien;
                            hang_hoa.thanh_tien = i_thanh_tien;
                        }

                        co_hang_hoa_dv = 1;
                    }

                    // Khuyến mại (2) => không cộng
                    if (hang_hoa.hang_hoa_tinh_chat_id == 2)
                    {
                        continue;
                    }

                    // Chiết khấu (3)
                    if (hang_hoa.hang_hoa_tinh_chat_id == 3)
                    {
                        if (hang_hoa.so_luong > 0 || hang_hoa.don_gia > 0)
                        {
                            var i_tong_tien_goc = hang_hoa.so_luong * hang_hoa.don_gia;
                            tong_tien_chiet_khau += i_tong_tien_goc;
                            tong_tien_mat_hang_chieu_khau += i_tong_tien_goc;
                        }
                        else
                        {
                            var i_tong_tien_goc = hang_hoa.thanh_tien;
                            tong_tien_chiet_khau += i_tong_tien_goc;
                            tong_tien_mat_hang_chieu_khau += i_tong_tien_goc;
                        }
                    }

                    // Ghi chú, diễn giải (4) => không cộng
                    if (hang_hoa.hang_hoa_tinh_chat_id == 4)
                    {
                        continue;
                    }
                }

                // Làm tròn từng mặt hàng trước khi tính tổng/thuế (đồng bộ với XML)
                if (model.loai_tien.ConvertToString() == "VND" || model.loai_tien.ConvertToString() == "")
                {
                    foreach (var hang_hoa in model.hoang_hoas)
                    {
                        hang_hoa.thanh_tien = Math.Round(hang_hoa.thanh_tien, 0, MidpointRounding.AwayFromZero);
                    }
                }

                tong_thanh_tien = model.hoang_hoas
                    .Where(x => x.hang_hoa_tinh_chat_id == 1 || x.hang_hoa_tinh_chat_id == 5)
                    .Sum(x => x.thanh_tien);
                tong_tien_mat_hang_chieu_khau = model.hoang_hoas
                    .Where(x => x.hang_hoa_tinh_chat_id == 3)
                    .Sum(x => x.thanh_tien);
                tong_tien_chiet_khau = tong_tien_mat_hang_chieu_khau;

                // Tính thuế suất từ thành tiền đã làm tròn
                tong_vat = 0;
                var thue_suats = model.hoang_hoas
                    .Select(x => x.thue_vat.ConvertToString())
                    .Distinct()
                    .Where(x => x.Contains("%"))
                    .ToList()
                    .Select(x => new LTSuat() { ten_thue_suat = x })
                    .ToList();

                foreach (var thue_suat in thue_suats)
                {
                    var phanTramThue = thue_suat.ten_thue_suat
                        .Replace("KHAC:", "")
                        .Replace("%", "")
                        .Trim()
                        .ConvertToDouble(2);

                    var thanh_tien_hang_hoa = model.hoang_hoas
                     .Where(x => (x.hang_hoa_tinh_chat_id == 1 || x.hang_hoa_tinh_chat_id == 5) &&
                                 x.thue_vat.ConvertToString() == thue_suat.ten_thue_suat)
                     .Select(x => x.thanh_tien)
                     .Sum();

                    var thanh_tien_chiet_khau = model.hoang_hoas
                        .Where(x => x.hang_hoa_tinh_chat_id == 3 &&
                                    x.thue_vat.ConvertToString() == thue_suat.ten_thue_suat)
                        .Select(x => x.thanh_tien)
                        .Sum();

                    var thanh_tien_tinh_thue = thanh_tien_hang_hoa - thanh_tien_chiet_khau;
                    if (model.loai_tien == "VND")
                    {
                        thanh_tien_tinh_thue = Math.Round(thanh_tien_tinh_thue, 0, MidpointRounding.AwayFromZero);
                    }
                    if (mausohd == "2")
                    {
                        phanTramThue = 0;
                    }
                    var tien_thue = model.loai_tien == "VND"
                        ? ((double)thanh_tien_tinh_thue * phanTramThue / 100).ConvertToDouble(0).ConvertToDecimal()
                        : ((double)thanh_tien_tinh_thue * phanTramThue / 100).ConvertToDouble().ConvertToDecimal();

                    tong_vat += tien_thue;
                }
            }

            // Giảm thuế theo nghị quyết
            if (model.giam_thue_ty_le > 0)
            {
                model.giam_thue_phan_tram = 20;
                model.giam_thue_thanh_tien =
                    ((double)tong_thanh_tien *
                     ((double)model.giam_thue_ty_le / 100) *
                     ((double)model.giam_thue_phan_tram / 100)).ConvertToDecimal();
            }
            else
            {
                model.giam_thue_thanh_tien = 0;
            }

            if (model.loai_tien.ConvertToString() == "VND" || model.loai_tien.ConvertToString() == "")
            {
                model.giam_thue_thanh_tien = Math.Round(model.giam_thue_thanh_tien, 0, MidpointRounding.AwayFromZero);
                tong_thanh_tien = Math.Round(tong_thanh_tien, 0, MidpointRounding.AwayFromZero);
                tong_tien_chiet_khau = Math.Round(tong_tien_chiet_khau, 0, MidpointRounding.AwayFromZero);
                tong_tien_mat_hang_chieu_khau = Math.Round(tong_tien_mat_hang_chieu_khau, 0, MidpointRounding.AwayFromZero);
                tong_vat = Math.Round(tong_vat, MidpointRounding.AwayFromZero);
            }

            //   if (isAllChietKhau)
            //   {
            //       tong_vat = tong_vat * -1;
            //       tong_thanh_tien = tong_tien_mat_hang_chieu_khau * -1;
            //   }

            if (thuesuatck == "0%" || thuesuatck == "")
            {
                if (Convert.ToInt16(mausohd) == 2)
                {
                    if (tong_tien_mat_hang_chieu_khau > 0)
                    {
                        //chiet khau la mat hang doc lap, tru vao cong tien hang

                        model.tong_tien_truong_thue = tong_thanh_tien - tong_tien_chiet_khau;
                    }
                    else
                    {
                        //chiet khau di theo mat hang da tru vao tong thanh tien
                        model.tong_tien_truong_thue = tong_thanh_tien;
                    }

                }
                else
                {
                    model.tong_tien_truong_thue = tong_thanh_tien;
                }

                model.tong_tien_thue = tong_vat;
                model.tong_tien_thanh_toan =
               tong_thanh_tien + tong_vat + tong_tien_phi - tong_tien_mat_hang_chieu_khau - model.giam_thue_thanh_tien;

            }
            else
            {
                if (co_hang_hoa_dv == 1)
                {
                    if (tong_tien_mat_hang_chieu_khau > 0)
                    {
                        //chiet khau la mat hang doc lap, tru vao tong cong tien hang 
                        model.tong_tien_truong_thue = tong_thanh_tien - tong_tien_chiet_khau;
                    }
                    else
                    {
                        //chiet khau theo mat hang da tru vao tong thanh tien
                        model.tong_tien_truong_thue = tong_thanh_tien;
                    }

                    model.tong_tien_thue = tong_vat;
                    model.tong_tien_thanh_toan =
                    tong_thanh_tien + tong_vat + tong_tien_phi - tong_tien_mat_hang_chieu_khau - model.giam_thue_thanh_tien;


                }
                else
                {
                    if (isAllChietKhau)
                    {
                        model.tong_tien_truong_thue = tong_tien_chiet_khau * -1;
                        model.tong_tien_thue = tong_vat;
                        model.tong_tien_thanh_toan = tong_vat + (tong_tien_mat_hang_chieu_khau * -1) - model.giam_thue_thanh_tien;
                    }
                }
            }

            model.tong_tien_chiet_khau = tong_tien_chiet_khau;
            //   model.tong_tien_thanh_toan =
            //        tong_thanh_tien + tong_vat + tong_tien_phi - tong_tien_mat_hang_chieu_khau - model.giam_thue_thanh_tien;


            if (model.loai_tien.ConvertToString() == "VND" || model.loai_tien.ConvertToString() == "")
            {
                model.tong_tien_truong_thue = Math.Round(model.tong_tien_truong_thue, MidpointRounding.AwayFromZero)
                                              + model.so_tien_tang_giam_tien_hang;

                model.tong_tien_thue = Math.Round(model.tong_tien_thue, 0, MidpointRounding.AwayFromZero)
                                       + model.so_tien_tang_giam_tien_thue;

                model.tong_tien_chiet_khau = Math.Round(model.tong_tien_chiet_khau, 0, MidpointRounding.AwayFromZero);

                //model.tong_tien_thanh_toan = Math.Round(model.tong_tien_thanh_toan, 0, MidpointRounding.AwayFromZero)
                //                             + model.so_tien_tang_giam
                //                             + model.so_tien_tang_giam_tien_thue
                //                             + model.so_tien_tang_giam_tien_hang;
                model.tong_tien_thanh_toan = model.tong_tien_thanh_toan
                              + model.so_tien_tang_giam
                              + model.so_tien_tang_giam_tien_thue
                              + model.so_tien_tang_giam_tien_hang;

            }

        }

        public async Task<FunctionResult<int>> UpdateHoaDonPhatHanhLoiNhieuLanAsync()
        {
            // return new SuccessResult<int>(0);
            var hoaDons = await _repositoryWrapper.HoaDon.HoaDon.SelectHoaDonLoiPhatHanhNhieuLanAsync();
            foreach (var hoaDon in hoaDons)
            {
                var hoaDonLogs = await _hoaDonLogService.SelectByHoaDonAsync(hoaDon.id);
                var logErr = hoaDonLogs.Where(x =>
                    x.noi_dung_thuc_hien == "Bộ MST, ký hiệu mẫu số, ký hiệu và số hóa đơn không duy nhất"
                    && x.nguoi_thuc_hien == "Cơ quan thuế").FirstOrDefault();
                if (logErr == null)
                {
                    var logKySos = hoaDonLogs.Where(x => x.noi_dung_thuc_hien == "Ký số thành công");
                    if (logKySos.Count() > 1)
                    {
                        logErr = logKySos.LastOrDefault();
                    }
                }

                if (logErr != null)
                {
                    var coQuanThueLogBeforeErrs = hoaDonLogs.Where(x => x.nguoi_thuc_hien == "Cơ quan thuế" &&
                                                                        x.id < logErr.id
                    ).ToList();
                    string log202 = null;
                    foreach (var log in coQuanThueLogBeforeErrs)
                    {
                        var xmlPath = log.file_thong_diep_url;
                        var thongDiep =
                            await this.ReadXmlContentFromUrlAsync($"https://ca2einv.nacencomm.vn/{xmlPath}");
                        var ketQuaThongDiepRespone =
                            thongDiep.ConvertToObject<Model.Respone.Xml.KetQuaThongDiepRespone>();
                        if (ketQuaThongDiepRespone.TTChung.MLTDiep == "202" ||
                            ketQuaThongDiepRespone.TTChung.MLTDiep == "204")
                        {
                            var service = await _serviceWrapper.HoaDon.XyLyThongDiepProvider.GetServiceAsync(hoaDon);
                            var result = await service.XuLyThongDiepAsync(hoaDon, ketQuaThongDiepRespone, thongDiep);
                            if (result.is_success)
                            {
                                //xóa các log đằng sau
                                hoaDonLogs = await _hoaDonLogService.SelectByHoaDonAsync(hoaDon.id);
                                var coQuanThueLogAfters =
                                    hoaDonLogs.Where(x =>
                                        (x.nguoi_thuc_hien == "Cơ quan thuế" ||
                                         x.noi_dung_thuc_hien == "Ký số thành công") &&
                                        x.id > log.id
                                    ).ToList();
                                foreach (var logAfter in coQuanThueLogAfters)
                                {
                                    await _repositoryWrapper.HoaDon.HoaDonLog.DeleteAsync(logAfter.id, 1);
                                }
                            }

                            break;
                        }
                    }
                }
            }

            return new SuccessResult<int>(1);
        }

        private async Task<string> ReadXmlContentFromUrlAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
                return content;
            }
        }

        public async Task<FunctionResult<int>> UpdateHoaDonPhatHanhLoiChuaPhatHanhAsync()
        {
            var hoaDons = await _repositoryWrapper.HoaDon.HoaDon.SelectHoaDonLoiChaPhathanhAsync();
            foreach (var hoaDon in hoaDons)
            {
                var hoaDonLogs = await _hoaDonLogService.SelectByHoaDonAsync(hoaDon.id);
                var hoaDonLogsCQT = hoaDonLogs.Where(x => x.hoa_don_log_type_id == 8).ToList();
                foreach (var log in hoaDonLogsCQT)
                {
                    var xmlPath = log.file_thong_diep_url;
                    var thongDiep = await this.ReadXmlContentFromUrlAsync($"https://ca2einv.nacencomm.vn/{xmlPath}");
                    var ketQuaThongDiepRespone = thongDiep.ConvertToObject<Model.Respone.Xml.KetQuaThongDiepRespone>();
                    if (ketQuaThongDiepRespone?.TTChung?.MLTDiep == "202" ||
                        ketQuaThongDiepRespone?.TTChung?.MLTDiep == "204")
                    {
                        var service = await _serviceWrapper.HoaDon.XyLyThongDiepProvider.GetServiceAsync(hoaDon);
                        var result = await service.XuLyThongDiepAsync(hoaDon, ketQuaThongDiepRespone, thongDiep);
                        if (result.is_success)
                        {
                            //xóa các log đằng sau
                            hoaDonLogs = await _hoaDonLogService.SelectByHoaDonAsync(hoaDon.id);
                            var coQuanThueLogAfters =
                                hoaDonLogs.Where(x =>
                                    (x.nguoi_thuc_hien == "Cơ quan thuế" ||
                                     x.noi_dung_thuc_hien == "Ký số thành công") &&
                                    x.id > log.id
                                ).ToList();
                            foreach (var logAfter in coQuanThueLogAfters)
                            {
                                await _repositoryWrapper.HoaDon.HoaDonLog.DeleteAsync(logAfter.id, 1);
                            }
                        }

                        break;
                    }
                }
            }

            return new SuccessResult<int>(1);
        }

        public async Task<FunctionResult<string>> CreateBase64MTTBangKeAsync(List<hoa_don> hoaDons)
        {
            var tasks = hoaDons.Select(hoaDon => { return this.CreateXmlObjectKySoAsync(hoaDon); }).ToList();
            await this.ExcuteDbTasks<FunctionResult<Model.Request.Xml.HoaDon>>(tasks, 10);
            var listHoaDonXmlModel = tasks.Where(x => x.Result.is_success).Select(x => x.Result.data).ToList();
            var hoaDonXmls = listHoaDonXmlModel.Select(hoaDonXmlModel => { return hoaDonXmlModel.ConvertToXml(); })
                .ToList();
            var userId = this.GetCurrentUserId();
            var uuid = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
            await _serviceWrapper.Cache.SetDataAsync<string>(uuid, "bang_ke_mtt", DateTime.Now.AddDays(30));
            await _repositoryWrapper.HoaDon.PhatHanhUUID.SaveLogUuidAsync(uuid, "bang_ke_mtt", userId);

            var thongDiep = new ThongDiep()
            {
                ThongTinChung = new ThongTinChungThongDiep()
                {
                    phien_ban = "2.1.0",
                    ma_noi_gui = AppSettings.FixedValue.MNGui,
                    ma_noi_nhan = AppSettings.FixedValue.MNNhan,
                    thong_diep = "206",
                    ma_noi_gui_uuid = $"{AppSettings.FixedValue.MNGui}{uuid}".ToUpper(),
                    ma_thong_diep_tham_chieu = $"",
                    mst = hoaDons.FirstOrDefault()?.nguoi_ban_mst ?? "",
                    so_luong = hoaDons.Count
                },
            };
            foreach (var hoaDon in hoaDons)
            {
                hoaDon.phat_hanh_uuid = uuid;
                hoaDon.user_id_phathanh = userId;
            }

            var hoaDonIds = hoaDons.Select(x => x.id).ToList();
            await _repositoryWrapper.HoaDon.HoaDon.UpdatePhatHanhBangKeAsync(hoaDonIds, uuid, userId);
            await _serviceWrapper.Cache.SetDataAsync<List<int>>(uuid + "_bang_ke_mtt", hoaDonIds,
                DateTime.Now.AddDays(30));
            var base64thongdiep = thongDiep.ConvertToXmlAndAppendChilds("/TDiep", "DLieu", hoaDonXmls, false,
                System.Xml.NewLineHandling.None, true, $"_{uuid}").ConvertToBase64();
            return new SuccessResult<string>(base64thongdiep);
        }

        public async Task<List<HoaDonUpdateKySoSuccessItemRespone>> UpdteKySoSuccessBangKeAsync(List<hoa_don> hoaDons,
            string signed_text, int user_id = 0)
        {
            var result = new List<HoaDonUpdateKySoSuccessItemRespone>();
            var tasks = hoaDons.Select(async hoaDon =>
            {
                var result = await this.UpdteKySoSuccessAsync(
                    new HoaDonPhatHanhRequest()
                    {
                        id = hoaDon.id,
                        signed_text = signed_text,
                    },
                    user_id
                );
                return new HoaDonUpdateKySoSuccessItemRespone()
                {
                    data = hoaDon.id,
                    id = hoaDon.id,
                    is_success = result.is_success,
                    message = result.message
                };
            }).ToList();
            await this.ExcuteDbTasks(tasks);
            result = tasks.Select(x => x.Result).ToList();
            return result;
        }

        public async Task<FunctionResult<HoaDonPhatHanhRespone>> PhatHanhMTTBangKeAsync(List<hoa_don> hoaDons,
            string signed_text, int user_id_phathanh = 0)
        {
            var base64thongdiep = signed_text;
            var user = this.GetCurrentUser();
            if (user_id_phathanh > 0)
            {
                var objUser = await _serviceWrapper.User.User.SelectAndFormatJwtTokenAsync(user_id_phathanh);
                if (objUser != null) user = objUser;
            }

            using (var client = Helper.WSInterTRCA2Helper.GetClient())
            {
                await client.OpenAsync();
                var authHeader = Helper.WSInterTRCA2Helper.GetAuthHeader();
                //
                var fileName = Guid.NewGuid().ToString() + ".xml";
                // var filePath = $"Xml/{DateTime.Now.Year}/{DateTime.Now.Month}/{fileName}";
                var filePath = $"Xml/{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{fileName}";
                var directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                await File.WriteAllTextAsync(filePath, base64thongdiep.ConvertToXmlFromBase64());


                try
                {
                    await _semaphore.WaitAsync();
                    try
                    {
                        var guiThongDiepResult = await client.Guithongdiep2024Async(authHeader, base64thongdiep, 1);
                        if (guiThongDiepResult.Guithongdiep2024Result.ConvertToString().Length > 2)
                        {
                            foreach (var hoaDon in hoaDons)
                            {
                                var log = new hoa_don_log()
                                {
                                    file_thong_diep_url = filePath,
                                    ngay_thuc_hien = DateTime.Now,
                                    nguoi_thuc_hien = user.full_name,
                                    noi_dung_thuc_hien = "Gửi thông điệp lên CQT",
                                    hoa_don_id = hoaDon.id,
                                    hoa_don_log_type_id = (int)e_hoa_don_log_type.GUI_THONG_DIEP
                                };
                                log.SetInsertInfo(user.id);
                                _serviceWrapper.Core.TaskQueue.EnqueueTask(async _ =>
                                {
                                    await _serviceWrapper.HoaDon.HoaDonLog.InsertAsync(log);
                                });
                            }
                        }
                        else
                        {
                            foreach (var hoaDon in hoaDons)
                            {
                                var log = new hoa_don_log()
                                {
                                    file_thong_diep_url = filePath,
                                    ngay_thuc_hien = DateTime.Now,
                                    nguoi_thuc_hien = user.full_name,
                                    noi_dung_thuc_hien = $"Gửi thông điệp thất bại {guiThongDiepResult.Guithongdiep2024Result.ConvertToString()}",
                                    hoa_don_id = hoaDon.id,
                                    hoa_don_log_type_id = -1 * (int)e_hoa_don_log_type.GUI_THONG_DIEP
                                };
                                log.SetInsertInfo(user.id);
                                _serviceWrapper.Core.TaskQueue.EnqueueTask(async _ =>
                                {
                                    await _serviceWrapper.HoaDon.HoaDonLog.InsertAsync(log);
                                });
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        foreach (var hoaDon in hoaDons)
                        {
                            var log = new hoa_don_log()
                            {
                                file_thong_diep_url = filePath,
                                ngay_thuc_hien = DateTime.Now,
                                nguoi_thuc_hien = user.full_name,
                                noi_dung_thuc_hien = $"Gửi thông điệp thất bại {ex.Message.ConvertToString()}",
                                hoa_don_id = hoaDon.id,
                                hoa_don_log_type_id = -1 * (int)e_hoa_don_log_type.GUI_THONG_DIEP
                            };
                            log.SetInsertInfo(user.id);
                            _serviceWrapper.Core.TaskQueue.EnqueueTask(async _ =>
                            {
                                await _serviceWrapper.HoaDon.HoaDonLog.InsertAsync(log);
                            });
                        }
                    }
                    finally
                    {
                        await client.CloseAsync();
                    }


                }
                finally
                {
                    _semaphore.Release();
                }


                return new SuccessResult<HoaDonPhatHanhRespone>();
            }
        }

        public async Task<FunctionResult<int>> SaoChepHoaDonNghichDaoAsync()
        {
            // var jsonLogs = File.ReadAllText("103/update-ma-so-hoa-don.json");
            // var listString = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(jsonLogs);
            var hoaDons = await _repositoryWrapper.HoaDon.HoaDon.SelectHoaDonLoiChaPhathanhAsync();
            foreach (var item in hoaDons)
            {
                // var temp = item.Split("-");
                // var hoa_don_id = temp[0].ConvertToInt();
                // var ma_so_hoa_don_cu = temp[1].Trim().ConvertToInt();
                // var ma_so_hoa_don_moi = temp[2].Trim().ConvertToInt();
                var hoa_don_id = item.id;
                var model = await this.SelectViewModelAsync(hoa_don_id);
                var payload = model.Map<HoaDonAddOrEditModel>();
                payload.id = 0;

                payload.ma_so_hoa_don_mtt = string.Empty;
                payload.ngay_hoa_don = DateTime.Now.Date;
                payload.hoa_don_id_goc = hoa_don_id;
                payload.hoa_don_nghi_dinh_id_goc = payload.hoa_don_nghi_dinh_id;
                payload.hoa_don_dang_ky_phat_hanh_mau_so_goc = payload.hoa_don_dang_ky_phat_hanh_mau_so;
                payload.hoa_don_dang_ky_phat_hanh_ky_hieu_goc = payload.hoa_don_dang_ky_phat_hanh_ky_hieu;
                payload.ma_so_hoa_don_goc = item.ma_so_hoa_don.ToString();
                payload.ngay_hoa_don_goc = item.ngay_hoa_don;
                payload.hoa_don_ly_do_dieu_chinh_id = 2;//điều chỉnh giảm
                payload.hoa_don_hinh_thuc_id = 3;//điều chỉnh giảm
                payload.user_id_phathanh = 0;
                payload.phat_hanh_uuid = null;
                payload.ket_qua_phat_hanh = null;
                payload.phat_hanh_ma_ketqua_cqt = null;
                payload.ma_tra_cuu = string.Empty;
                payload.hoa_don_trang_thai_id = 1;
                payload.tong_tien_chu = null;
                payload.is_ky_so_succes = false;

                // var payload = model.Map<HoaDonAddOrEditModel>();
                payload.hoang_hoas = model.hang_hoas.Select(x =>
                {
                    x.id = 0;
                    x.hoa_don_id = 0;
                    return x;
                }).ToList(); ;
                payload.id = 0;
                payload.hoa_don_trang_thai_id = 1;
                var result = await this.SaveHoaDonAsync(payload);
                payload.id = result.data;
            }
            return new SuccessResult<int>();
        }
        public async Task<FunctionResult<int>> XuLyLoiMaKhongLienTiepAsync()
        {
            var hoaDons = await _repositoryWrapper.HoaDon.HoaDon.SelectHoaDonLoiChaPhathanhAsync();
            var result = new List<string>();
            foreach (var hoaDon in hoaDons)
            {
                var hoaDonLogs = await _hoaDonLogService.SelectByHoaDonAsync(hoaDon.id);
                var hoaDonLogsCQT = hoaDonLogs.Where(x => x.hoa_don_log_type_id == 8).ToList();
                foreach (var log in hoaDonLogsCQT)
                {
                    var xmlPath = log.file_thong_diep_url;
                    var thongDiep = await this.ReadXmlContentFromUrlAsync($"https://ca2einv.nacencomm.vn/{xmlPath}");
                    var ketQuaThongDiepRespone = thongDiep.ConvertToObject<Model.Respone.Xml.KetQuaThongDiepRespone>();
                    if (ketQuaThongDiepRespone?.TTChung?.MLTDiep == "202" ||
                        ketQuaThongDiepRespone?.TTChung?.MLTDiep == "204")
                    {
                        string pattern = @"<SHDon>(.*?)</SHDon>";
                        var match = Regex.Match(thongDiep, pattern, RegexOptions.Singleline);
                        if (match.Success)
                        {
                            var SHDOn = match.Groups[1].Value;
                            if (SHDOn != null && SHDOn != hoaDon.ma_so_hoa_don.ToString())
                            {
                                var logContent = $"{hoaDon.id} - {hoaDon.ma_so_hoa_don} -{SHDOn}";
                                result.Add(logContent);
                                LogWriter.Writer($"{hoaDon.id} - ${hoaDon.ma_so_hoa_don} -${SHDOn}", hoaDon.ma_so_hoa_don.ToString(), SHDOn);
                                await _repositoryWrapper.HoaDon.HoaDon.UpdateMaSoHoaDonAsync(hoaDon.id, SHDOn.ConvertToInt());
                            }
                        }

                        break;
                    }
                }
            }
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            return new SuccessResult<int>();
        }

        public async Task<FunctionResult<string>> GetHtmlPrintBienBanAsync(int id)
        {
            var hoaDonLogs = await _hoaDonLogService.SelectByHoaDonAsync(id, (int)e_hoa_don_log_type.KY_SO_XML_BIEN_BAN_THANH_CONG);
            if (hoaDonLogs.Count() <= 0)
            {
                hoaDonLogs = await _hoaDonLogService.SelectByHoaDonAsync(id, (int)e_hoa_don_log_type.TAO_XML_BIEN_BAN);
            }
            var logBienBan = hoaDonLogs.Where(x => x.hoa_don_log_type_id == (int)e_hoa_don_log_type.TAO_XML_BIEN_BAN || x.hoa_don_log_type_id == (int)e_hoa_don_log_type.KY_SO_XML_BIEN_BAN_THANH_CONG).LastOrDefault();
            if (logBienBan == null) return new ErrorResult<string>("Không tìm thấy dữ liệu");
            var xsltPath = "Template/bien-ban/bienbanDCTT.xslt";
            var xsltArgument = new XsltArgumentList();
            // var xmlData = File.ReadAllText("Template/to_khai/test.xml");
            var xmlData = File.ReadAllText(logBienBan.file_thong_diep_url);
            var html = await _serviceWrapper.Xslt.FillDataAsXmlAsync(xsltPath, xmlData, xsltArgument);
            return html.is_success ? new SuccessResult<string>(html.data) : new ErrorResult<string>(html.message);
        }

        public async Task<FunctionResult<string>> GetBase64BienBanAsync(int id)
        {
            var hoaDonLogs = await _hoaDonLogService.SelectByHoaDonAsync(id, (int)e_hoa_don_log_type.TAO_XML_BIEN_BAN);
            var logBienBan = hoaDonLogs.Where(x => x.hoa_don_log_type_id == (int)e_hoa_don_log_type.TAO_XML_BIEN_BAN).LastOrDefault();
            if (logBienBan == null) return new ErrorResult<string>("Không tìm thấy dữ liệu");

            var xmlData = File.ReadAllText(logBienBan.file_thong_diep_url);
            return new SuccessResult<string>(xmlData.ConvertToBase64());
        }

        public async Task<FunctionResult<DateTime?>> GetNgayHoaDonPhatHanhMaxAsynsc(string donvi_ma_dv, string hoa_don_dang_ky_phat_hanh_mau_so, string hoa_don_dang_ky_phat_hanh_ky_hieu)
        {
            var ngayMax = await _repositoryWrapper.HoaDon.HoaDon.GetNgayHoaDonPhatHanhMaxAsynsc(donvi_ma_dv, hoa_don_dang_ky_phat_hanh_mau_so, hoa_don_dang_ky_phat_hanh_ky_hieu);
            return new SuccessResult<DateTime?>(ngayMax);
        }

        public async Task<FunctionResult<bool>> ValidateNgayHoaDonAsync(hoa_don hoaDon)
        {
            if (hoaDon == null)
                return new ErrorResult<bool>("Không tìm thấy hóa đơn");
            if (hoaDon.hoa_don_hinh_thuc_code == "M")
                return new SuccessResult<bool>(true);

            var error = await ValidateNgayHoaDonKhiLuuAsync(
                hoaDon.donvi_ma_dv,
                hoaDon.hoa_don_dang_ky_phat_hanh_mau_so,
                hoaDon.hoa_don_dang_ky_phat_hanh_ky_hieu,
                hoaDon.ngay_hoa_don,
                hoaDon.id);
            if (error != null)
                return new ErrorResult<bool>(error.message);
            return new SuccessResult<bool>(true);
        }

        private static string ValidateNgayHoaDonTheoSo(
            DateTime ngaySua,
            DateTime? ngayToiThieu,
            DateTime? ngayToiDa)
        {
            var ngay = ngaySua.Date;
            if (ngayToiThieu.HasValue && ngay < ngayToiThieu.Value.Date)
                return $"không thể sửa ngày hóa đơn nhỏ hơn \"{ngayToiThieu.Value:dd/MM/yyyy}\"";
            if (ngayToiDa.HasValue && ngay > ngayToiDa.Value.Date)
                return $"không thể sửa ngày hóa đơn lớn hơn \"{ngayToiDa.Value:dd/MM/yyyy}\"";
            if (ngay > DateTime.Today)
                return "Không được lập hóa đơn cho ngày tương lai";
            return null;
        }

        private static string ValidateNgayHoaDonTheoLuong(
            DateTime ngaySua,
            DateTime? ngayLienKeTruoc,
            DateTime? ngayLienKeSau,
            DateTime? ngayHoaDonMax,
            bool laHoaDonCuoiKhongCoNgaySau)
        {
            var ngay = ngaySua.Date;
            var today = DateTime.Today;
            var minChoPhep = today.AddDays(-2);

            if (laHoaDonCuoiKhongCoNgaySau && ngayHoaDonMax.HasValue && ngay < ngayHoaDonMax.Value.Date)
                return $"không thể sửa ngày hóa đơn nhỏ hơn \"{ngayHoaDonMax.Value:dd/MM/yyyy}\"";

            if (!ngayLienKeTruoc.HasValue && !ngayLienKeSau.HasValue)
            {
                if (ngay > today)
                    return "Không được lập hóa đơn cho ngày tương lai";
                if (ngay < minChoPhep)
                    return "không được lập hóa đơn cách ngày hiện tại quá 2 ngày";
                return null;
            }

            if (ngayLienKeTruoc.HasValue && !ngayLienKeSau.HasValue)
            {
                if (ngayHoaDonMax.HasValue && ngay < ngayHoaDonMax.Value.Date)
                    return $"không thể sửa ngày hóa đơn nhỏ hơn \"{ngayHoaDonMax.Value:dd/MM/yyyy}\"";
                if (ngay > today)
                    return "Không được lập hóa đơn cho ngày tương lai";
                if (ngay < minChoPhep)
                    return "không được lập hóa đơn cách ngày hiện tại quá 2 ngày";
                return null;
            }

            if (ngayLienKeTruoc.HasValue && ngayLienKeSau.HasValue)
            {
                if (ngay < ngayLienKeTruoc.Value.Date)
                    return $"không được lập hóa đơn nhỏ hơn \"{ngayLienKeTruoc.Value:dd/MM/yyyy}\"";
                if (ngay > ngayLienKeSau.Value.Date)
                    return "không lập được hóa đơn lớn hơn ngày hóa đơn liền kề sau";
                return null;
            }

            if (!ngayLienKeTruoc.HasValue && ngayLienKeSau.HasValue)
            {
                if (ngay > ngayLienKeSau.Value.Date)
                    return "không lập được hóa đơn lớn hơn ngày hóa đơn liền kề sau";
                if (ngay > today)
                    return "Không được lập hóa đơn cho ngày tương lai";
                if (ngay < minChoPhep)
                    return "không được lập hóa đơn cách ngày hiện tại quá 2 ngày";
                return null;
            }

            if (ngay > today)
                return "Không được lập hóa đơn cho ngày tương lai";
            if (ngay < minChoPhep)
                return "không được lập hóa đơn cách ngày hiện tại quá 2 ngày";
            return null;
        }

        private async Task<ErrorResult<int>?> ValidateNgayHoaDonKhiLuuAsync(
            string donvi_ma_dv,
            string mau_so,
            string ky_hieu,
            DateTime ngay_hoa_don,
            int hoa_don_id)
        {
            var maSoHoaDon = 0;
            if (hoa_don_id > 0)
            {
                var hoaDonHienTai = await _repositoryWrapper.HoaDon.HoaDon.SelectByIdAsync(hoa_don_id);
                maSoHoaDon = hoaDonHienTai?.ma_so_hoa_don.ConvertToInt() ?? 0;
            }

            if (maSoHoaDon > 0)
            {
                var choPhepTheoSo = await _repositoryWrapper.HoaDon.HoaDon.SelectNgayHoaDonChoPhepTheoSoAsync(
                    donvi_ma_dv, mau_so, ky_hieu, hoa_don_id, maSoHoaDon);
                var messageTheoSo = ValidateNgayHoaDonTheoSo(
                    ngay_hoa_don,
                    choPhepTheoSo?.ngay_toi_thieu,
                    choPhepTheoSo?.ngay_toi_da);
                if (messageTheoSo != null)
                    return new ErrorResult<int>(messageTheoSo);
                return null;
            }

            var lienKe = await _repositoryWrapper.HoaDon.HoaDon.SelectNgayHoaDonLienKeAsync(
                donvi_ma_dv, mau_so, ky_hieu, hoa_don_id, ngay_hoa_don);
            var ngayMax = await _repositoryWrapper.HoaDon.HoaDon.GetNgayHoaDonPhatHanhMaxAsynsc(
                donvi_ma_dv, mau_so, ky_hieu);

            var laHoaDonCuoiKhongCoNgaySau = false;
            if (hoa_don_id > 0)
            {
                var hoaDonHienTai = await _repositoryWrapper.HoaDon.HoaDon.SelectByIdAsync(hoa_don_id);
                if (hoaDonHienTai != null)
                {
                    var lienKeTheoNgayHienTai = await _repositoryWrapper.HoaDon.HoaDon.SelectNgayHoaDonLienKeAsync(
                        donvi_ma_dv,
                        mau_so,
                        ky_hieu,
                        hoa_don_id,
                        hoaDonHienTai.ngay_hoa_don);
                    laHoaDonCuoiKhongCoNgaySau = lienKeTheoNgayHienTai == null
                        || !lienKeTheoNgayHienTai.ngay_sau.HasValue;
                }
            }

            var message = ValidateNgayHoaDonTheoLuong(
                ngay_hoa_don,
                lienKe?.ngay_truoc,
                lienKe?.ngay_sau,
                ngayMax,
                laHoaDonCuoiKhongCoNgaySau);
            if (message != null)
                return new ErrorResult<int>(message);
            return null;
        }

        // update tach ds

        public Task<PagingResult<IEnumerable<hoa_don_vm>>> SelectChoPhanHoiCQTAsync(string donvi_ma_dv,HoaDonSelectPagingRequest pagingRequest)
        {
            return _repositoryWrapper.HoaDon.HoaDon.SelectChoPhanHoiCQTAsync(donvi_ma_dv,pagingRequest);
        }

        public Task<PagingResult<IEnumerable<hoa_don_vm>>> SelectChuaGuiCQTAsync(string donvi_ma_dv, HoaDonSelectPagingRequest pagingRequest)
        {
            return _repositoryWrapper.HoaDon.HoaDon.SelectChuaGuiCQTAsync(donvi_ma_dv, pagingRequest);
        }

        public async Task<FunctionResult<object>> GuiLaiCQTAsync(int id)
        {
            try
            {
                var hoaDon = await SelectByIdAsync(id);
                if (hoaDon == null)
                {
                    return new ErrorResult<object>("Không tìm thấy hóa đơn");
                }
                string signedXml = await GetXMLString(id, hoaDon);


                if (string.IsNullOrEmpty(signedXml))
                {
                    return new ErrorResult<object>("Không tìm thấy signed xml");
                }

                // GetXMLString có thể đã gán phat_hanh_uuid mới — cần reload và đăng ký cache để Rabbit map được
                hoaDon = await SelectByIdAsync(id);
                if (hoaDon == null)
                {
                    return new ErrorResult<object>("Không tìm thấy hóa đơn");
                }
                await RegisterPhatHanhUuidCacheAsync(hoaDon);

                string mngui = GetMNGui(signedXml);

                var client =Helper.WSInterTRCA2Helper.GetClient();

                await client.OpenAsync();

                var base64 = Convert.ToBase64String(
                    Encoding.UTF8.GetBytes(signedXml)
                );

                var authHeader = Helper.WSInterTRCA2Helper.GetAuthHeader();
                if (mngui == "0103930279")
                {
                    authHeader.Username = "ntvan";
                    authHeader.Password = "123456";
                }

                var guiThongDiepResult =
                    await client.Guithongdiep2024Async(
                        authHeader,
                        base64,
                        1
                    );

                if (guiThongDiepResult.Guithongdiep2024Result.ConvertToString().Length <= 2)
                {
                    return new ErrorResult<object>(
                        $"Gửi thông điệp lên CQT thất bại: {guiThongDiepResult.Guithongdiep2024Result}"
                    );
                }

                return new SuccessResult<object>(guiThongDiepResult.Guithongdiep2024Result);
            }
            catch (Exception ex)
            {
                return new ErrorResult<object>(ex.Message);
            }
        }

        public async Task<FunctionResult<object>> CapNhatKetQuaCQTAsync(int id)
        {
            try
            {
                var hoaDon = await SelectByIdAsync(id);
                if (hoaDon == null)
                {
                    return new ErrorResult<object>("Không tìm thấy hóa đơn");
                }

                var hinhThucCode = hoaDon.hoa_don_hinh_thuc_code.ConvertToString().ToUpper();
                var logs = (await _hoaDonLogService.SelectByHoaDonAsync(id)).ToList();

                if (hinhThucCode == "C")
                {
                    var log202 = logs.FirstOrDefault(x => x.mltdiep == "202");
                    if (log202 != null)
                    {
                        var xml = await ReadXmlFromLogPathAsync(log202.file_thong_diep_url);
                        if (string.IsNullOrWhiteSpace(xml))
                        {
                            return new ErrorResult<object>("Không đọc được nội dung XML thông điệp 202");
                        }

                        var maCqt = ExtractMccqtFromXml(xml);
                        if (string.IsNullOrWhiteSpace(maCqt))
                        {
                            return new ErrorResult<object>("Không tìm thấy mã CQT trong thông điệp 202");
                        }

                        hoaDon.phat_hanh_ma_ketqua_cqt = maCqt;
                        hoaDon.hoa_don_trang_thai_id = (int)e_hoa_don_trang_thai.DA_PHAT_HANH;
                        hoaDon.ket_qua_phat_hanh = "";
                        await UpdateAsync(hoaDon);
                        return new SuccessResult<object>("Cập nhật kết quả CQT thành công");
                    }

                    var xmlTvan = await LayKetQuaThongDiepTuTvanAsync(hoaDon);
                    if (string.IsNullOrWhiteSpace(xmlTvan))
                    {
                        return new ErrorResult<object>("Không lấy được kết quả thông điệp từ TVAN");
                    }

                    var mltdiep = ExtractXmlTagValue(xmlTvan, "MLTDiep");
                    if (mltdiep != "202")
                    {
                        return new ErrorResult<object>($"TVAN chưa trả về thông điệp 202 (MLTDiep={mltdiep})");
                    }

                    var maCqtTvan = ExtractMccqtFromXml(xmlTvan);
                    if (string.IsNullOrWhiteSpace(maCqtTvan))
                    {
                        return new ErrorResult<object>("Không tìm thấy mã CQT trong thông điệp TVAN");
                    }

                    hoaDon.phat_hanh_ma_ketqua_cqt = maCqtTvan;
                    hoaDon.hoa_don_trang_thai_id = (int)e_hoa_don_trang_thai.DA_PHAT_HANH;
                    hoaDon.ket_qua_phat_hanh = "";
                    await UpdateAsync(hoaDon);
                    await _hoaDonLogService.SaveFromPhatHanhAsync(hoaDon.id, hoaDon.ket_qua_phat_hanh, xmlTvan, true);
                    return new SuccessResult<object>("Cập nhật kết quả CQT thành công");
                }

                if (hinhThucCode == "M" || hinhThucCode == "K")
                {
                    string xml;
                    var saveLog = false;

                    var log204 = logs.FirstOrDefault(x => x.mltdiep == "204");
                    if (log204 != null)
                    {
                        xml = await ReadXmlFromLogPathAsync(log204.file_thong_diep_url);
                        if (string.IsNullOrWhiteSpace(xml))
                        {
                            return new ErrorResult<object>("Không đọc được nội dung XML thông điệp 204");
                        }
                    }
                    else
                    {
                        xml = await LayKetQuaThongDiepTuTvanAsync(hoaDon);
                        if (string.IsNullOrWhiteSpace(xml))
                        {
                            return new ErrorResult<object>("Không lấy được kết quả thông điệp từ TVAN");
                        }

                        saveLog = true;
                    }

                    var ltBao = ExtractXmlTagValue(xml, "LTBao");
                    if (ltBao == "2")
                    {
                        hoaDon.hoa_don_trang_thai_id = (int)e_hoa_don_trang_thai.DA_PHAT_HANH;
                        hoaDon.ket_qua_phat_hanh = "";
                    }
                    else
                    {
                        var mtLoi = ExtractMtLoiFromXml(xml);
                        hoaDon.hoa_don_trang_thai_id = (int)e_hoa_don_trang_thai.KHONG_HOP_LE;
                        hoaDon.ket_qua_phat_hanh = mtLoi;
                    }

                    await UpdateAsync(hoaDon);
                    if (saveLog)
                    {
                        await _hoaDonLogService.SaveFromPhatHanhAsync(
                            hoaDon.id,
                            hoaDon.ket_qua_phat_hanh,
                            xml,
                            ltBao == "2"
                        );
                    }

                    return new SuccessResult<object>("Cập nhật kết quả CQT thành công");
                }

                return new ErrorResult<object>($"Loại hóa đơn '{hinhThucCode}' không hỗ trợ cập nhật kết quả CQT");
            }
            catch (Exception ex)
            {
                return new ErrorResult<object>(ex.Message);
            }
        }

        public async Task CapNhatHoaDonGocSauPhatHanhThanhCongAsync(int hoaDonId, int userIdPhatHanh = 0)
        {
            var hoaDon = await SelectByIdAsync(hoaDonId);
            if (hoaDon == null || !hoaDon.IsHoaDonDieuChinhHoacThayTheDangPhatHanh())
            {
                return;
            }

            var soHoaDonGoc = hoaDon.ma_so_hoa_don_goc.ConvertToInt();
            if (soHoaDonGoc <= 0)
            {
                return;
            }

            var hoaDonGoc = await _repositoryWrapper.HoaDon.HoaDon.SelectHoaDonGocAsync(
                hoaDon.donvi_ma_dv,
                hoaDon.hoa_don_dang_ky_phat_hanh_mau_so_goc,
                hoaDon.hoa_don_dang_ky_phat_hanh_ky_hieu_goc,
                soHoaDonGoc);

            if (hoaDonGoc == null || hoaDonGoc.id == hoaDon.id)
            {
                return;
            }

            if (hoaDonGoc.ma_so_hoa_don != soHoaDonGoc)
            {
                return;
            }

            if (hoaDonGoc.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_DIEU_CHINH
                || hoaDonGoc.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_THAY_THE)
            {
                return;
            }

            var hoa_don_ids_thaythe_dieuchinh = hoaDonGoc.hoa_don_ids_thaythe_dieuchinh
                .ConvertToString()
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => x != "")
                .ToList();

            var hoaDonIdStr = hoaDon.id.ToString();
            if (!hoa_don_ids_thaythe_dieuchinh.Contains(hoaDonIdStr))
            {
                hoa_don_ids_thaythe_dieuchinh.Add(hoaDonIdStr);
            }

            hoaDonGoc.hoa_don_ids_thaythe_dieuchinh = string.Join(",", hoa_don_ids_thaythe_dieuchinh);

            if (hoaDon.IsHoaDonDieuChinhDangPhatHanh())
            {
                hoaDonGoc.hoa_don_hinh_thuc_id = (int)e_hoa_don_hinh_thuc.HOA_DON_BI_DIEU_CHINH;
            }
            else if (hoaDon.IsHoaDonThayTheDangPhatHanh())
            {
                hoaDonGoc.hoa_don_hinh_thuc_id = (int)e_hoa_don_hinh_thuc.HOA_DON_BI_THAY_THE;
            }

            var userId = userIdPhatHanh > 0 ? userIdPhatHanh : hoaDon.user_id_phathanh;
            hoaDonGoc.SetUpdateInfo(userId);
            await UpdateAsync(hoaDonGoc);
        }

        public async Task<FunctionResult<object>> KhoiPhucTrangThaiAsync(int id)
        {
            try
            {
                var hoaDon = await SelectByIdAsync(id);
                if (hoaDon == null)
                {
                    return new ErrorResult<object>("Không tìm thấy hóa đơn");
                }

                if (hoaDon.hoa_don_trang_thai_id != (int)e_hoa_don_trang_thai.DA_HUY)
                {
                    return new ErrorResult<object>("Chỉ khôi phục được hóa đơn đã hủy");
                }

                var logs = (await _hoaDonLogService.SelectByHoaDonAsync(id)).ToList();
                var hasLog7 = logs.Any(x => x.hoa_don_log_type_id == (int)e_hoa_don_log_type.GUI_THONG_DIEP);
                var hasLog8 = logs.Any(x => x.hoa_don_log_type_id == (int)e_hoa_don_log_type.CO_QUAN_THUE_CHAP_NHAN);
                var hasLog3 = logs.Any(x => x.hoa_don_log_type_id == (int)e_hoa_don_log_type.KY_SO_SUCCESS);
                var hasLog202 = logs.Any(x => x.mltdiep == "202");
                var hasLog204 = logs.Any(x => x.mltdiep == "204");
                var hasLogLoiThongDiep = logs.Any(x => x.mltdiep?.Trim() == "-1");

                hoaDon.hoa_don_hinh_thuc_id = (int)e_hoa_don_hinh_thuc.HOA_DON_GOC;

                if (hasLogLoiThongDiep)
                {
                    hoaDon.hoa_don_trang_thai_id = (int)e_hoa_don_trang_thai.LOI_THONG_DIEP;
                }
                else if (hasLog7 && hasLog8 && hasLog202)
                {
                    hoaDon.hoa_don_trang_thai_id = (int)e_hoa_don_trang_thai.DA_PHAT_HANH;
                }
                else if (hasLog7 && hasLog8 && hasLog204)
                {
                    var log204 = logs.FirstOrDefault(x => x.mltdiep == "204");
                    var xml = await ReadXmlFromLogPathAsync(log204?.file_thong_diep_url);
                    if (xml.Contains("<LTBao>2</LTBao>"))
                    {
                        hoaDon.hoa_don_trang_thai_id = (int)e_hoa_don_trang_thai.DA_PHAT_HANH;
                    }
                    else
                    {
                        hoaDon.hoa_don_trang_thai_id = (int)e_hoa_don_trang_thai.KHONG_HOP_LE;
                    }
                }
                else if (hasLog7 && !hasLog8)
                {
                    hoaDon.hoa_don_trang_thai_id = (int)e_hoa_don_trang_thai.DA_GUI_LEN_CQT_CHUA_PHAN_HOI_KIEM_TRA_DU_LIEU;
                }
                else if (!hasLog7 && hasLog3)
                {
                    hoaDon.hoa_don_trang_thai_id = (int)e_hoa_don_trang_thai.CHUA_GUI_CQT;
                }
                else if (!hasLog3)
                {
                    hoaDon.hoa_don_trang_thai_id = (int)e_hoa_don_trang_thai.NHAP;
                }
                else
                {
                    return new ErrorResult<object>("Không xác định được trạng thái khôi phục từ lịch sử hóa đơn");
                }

                await UpdateAsync(hoaDon);
                return new SuccessResult<object>("Khôi phục trạng thái hóa đơn thành công");
            }
            catch (Exception ex)
            {
                return new ErrorResult<object>(ex.Message);
            }
        }

        private async Task<string> ReadXmlFromLogPathAsync(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return string.Empty;
            }

            if (File.Exists(filePath))
            {
                return await File.ReadAllTextAsync(filePath);
            }

            return await ReadXmlContentFromUrlAsync($"https://ca2einv.nacencomm.vn/{filePath}");
        }

        private async Task<string> LayKetQuaThongDiepTuTvanAsync(hoa_don hoaDon)
        {
            var khoaPhienPrefix =
                $"{hoaDon.donvi_ma_dv}_{hoaDon.hoa_don_dang_ky_phat_hanh_mau_so}{hoaDon.hoa_don_dang_ky_phat_hanh_ky_hieu}_{hoaDon.ma_so_hoa_don}_000_";
            var mstTcgp = $"{AppSettings.FixedValue.MNGui}-001";

            using var client = Helper.WSInterTRCA2Helper.GetClient();
            await client.OpenAsync();
            try
            {
                var authHeader = Helper.WSInterTRCA2Helper.GetAuthHeader();
                if (hoaDon.created_user_id == 28057)
                {
                    authHeader.Username = "ntvan";
                    authHeader.Password = "123456";
                }

                var response = await client.LayketquathongdiepTQ_Khoaphien_prefixAsync(
                    authHeader,
                    khoaPhienPrefix,
                    mstTcgp
                );

                return ExtractXmlFromTvanKhoaphienPrefixResult(
                    response?.LayketquathongdiepTQ_Khoaphien_prefixResult
                );
            }
            finally
            {
                await client.CloseAsync();
            }
        }

        private static string ExtractXmlFromTvanKhoaphienPrefixResult(
            WSInterTRCA2.LayketquathongdiepTQ_Khoaphien_prefixResponseLayketquathongdiepTQ_Khoaphien_prefixResult result)
        {
            if (result?.Any == null || result.Any.Length == 0)
            {
                return string.Empty;
            }

            var combined = string.Concat(result.Any.Select(x => x.OuterXml));
            if (combined.Contains("<MLTDiep>") || combined.Contains("<TDiep"))
            {
                return combined;
            }

            var ds = new DataSet();
            using (var reader = new StringReader($"<root>{combined}</root>"))
            {
                ds.ReadXml(reader);
            }

            foreach (DataTable table in ds.Tables)
            {
                foreach (DataRow row in table.Rows)
                {
                    foreach (DataColumn col in table.Columns)
                    {
                        var value = row[col]?.ToString() ?? "";
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            continue;
                        }

                        if (value.Contains("<MLTDiep>") || value.Contains("<TDiep"))
                        {
                            return value;
                        }

                        if (value.Length > 100 && TryDecodeBase64Xml(value, out var decoded))
                        {
                            return decoded;
                        }
                    }
                }
            }

            return combined;
        }

        private static bool TryDecodeBase64Xml(string value, out string xml)
        {
            xml = string.Empty;
            try
            {
                var bytes = Convert.FromBase64String(value.Trim());
                var decoded = Encoding.UTF8.GetString(bytes);
                if (decoded.Contains("<MLTDiep>") || decoded.Contains("<TDiep"))
                {
                    xml = decoded;
                    return true;
                }
            }
            catch
            {
            }

            return false;
        }

        private static string ExtractXmlTagValue(string xml, string tagName)
        {
            var match = Regex.Match(xml, $@"<{tagName}>(.*?)</{tagName}>", RegexOptions.Singleline);
            return match.Success ? match.Groups[1].Value.Trim() : "";
        }

        private static string ExtractMccqtFromXml(string xml)
        {
            var ketQua = xml.ConvertToObject<Model.Respone.Xml.KetQuaThongDiepRespone>();
            var maCqt = ketQua?.DLieu?.HDon?.MCCQT?.Text.ConvertToString() ?? "";
            if (!string.IsNullOrWhiteSpace(maCqt))
            {
                return maCqt;
            }

            return ExtractXmlTagValue(xml, "MCCQT");
        }

        private static string ExtractMtLoiFromXml(string xml)
        {
            var ketQua = xml.ConvertToObject<Model.Respone.Xml.KetQuaThongDiepRespone>();
            var mtLoi = ketQua?.DLieu?.TBao?.DLTBao?.LHDKMa?.DSHDon?.HDon?.DSLDo?.LDo?.MTLoi ?? "";
            if (!string.IsNullOrWhiteSpace(mtLoi))
            {
                return mtLoi;
            }

            return ExtractXmlTagValue(xml, "MTLoi");
        }

        private async Task RegisterPhatHanhUuidCacheAsync(hoa_don hoaDon)
        {
            var uuid = hoaDon.phat_hanh_uuid.ConvertToString().Trim();
            if (uuid == "")
            {
                return;
            }

            var userId = hoaDon.user_id_phathanh > 0
                ? hoaDon.user_id_phathanh
                : this.GetCurrentUserId();

            await _serviceWrapper.Cache.SetDataAsync<string>(uuid, "hoa_don", DateTime.Now.AddDays(30));
            await _repositoryWrapper.HoaDon.PhatHanhUUID.SaveLogUuidAsync(uuid, "hoa_don", userId);
            await _serviceWrapper.Cache.SetDataAsync<hoa_don>(uuid + "_hoa_don", hoaDon, DateTime.Now.AddDays(30));
        }

        public async Task<string> GetXMLString(int inhd,hoa_don hoaDon)
        {
            try
            {
                string sql = @"select  hoa_don_id,ma_so_hoa_don,hoa_don_hinh_thuc_code,hoa_don_log_type_id, donvi_ma_dv, a.created_user_id,concat ('https://ca2einv.nacencomm.vn/',file_thong_diep_url) as linkxml from hoa_don a left join evoice_user_log.dbo.hoa_don_log b on a.id = b.hoa_don_id where hoa_don_id = @hoa_don_id and hoa_don_log_type_id in(3, -7)";

                DataTable dt = new DataTable();

                var connStr = AppSettings.DbConnections[
                    eConectionStringKey.DefaultConnection.ToString()
                ];

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@hoa_don_id", inhd);

                        conn.Open();

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows.Count == 1) // chi co 1 ban ghi
                    {
                        string urlXML = dt.Rows[0]["linkxml"]?.ToString() ?? "";
                        string plainXML =await GetXmlAsync(urlXML);
                        if ((int)dt.Rows[0]["hoa_don_log_type_id"] == -7)
                        {                            
                            if (!string.IsNullOrEmpty(plainXML))
                            {
                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(plainXML);
                                string thongdiep = string.Empty;
                                string key = "0103930279-001" + Guid.NewGuid().ToString().Replace("-", "").ToUpper();

                                XmlNode mtDiepNode = doc.SelectSingleNode("/TDiep/TTChung/MTDiep");
                                if (mtDiepNode != null)
                                {
                                    mtDiepNode.InnerText = key;
                                    thongdiep = doc.InnerXml;
                                }
                                string uid = key.Replace("0103930279-001", "").ToUpper();
                                hoaDon.phat_hanh_uuid = uid;
                                hoaDon.id = (int)dt.Rows[0]["hoa_don_id"];
                                hoaDon.user_id_phathanh = (int)dt.Rows[0]["created_user_id"];
                                await this.UpdateAsync(hoaDon);
                                await RegisterPhatHanhUuidCacheAsync(hoaDon);

                                var fileName = Guid.NewGuid().ToString() + ".xml";
                                var filePath = $"Xml/{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{fileName}";
                                var directoryPath = Path.GetDirectoryName(filePath);
                                if (!Directory.Exists(directoryPath))
                                {
                                    Directory.CreateDirectory(directoryPath);
                                }
                                await File.WriteAllTextAsync(filePath, thongdiep);
                                var log = new hoa_don_log()
                                {
                                    file_thong_diep_url = filePath,
                                    ngay_thuc_hien = DateTime.Now,
                                    nguoi_thuc_hien = "adminNCM",
                                    noi_dung_thuc_hien = "Gửi lai thông điệp lên CQT",
                                    hoa_don_id = hoaDon.id,
                                    hoa_don_log_type_id = (int)e_hoa_don_log_type.GUI_THONG_DIEP
                                };
                                log.SetInsertInfo(28057);
                                _serviceWrapper.Core.TaskQueue.EnqueueTask(async _ =>
                                {
                                    await _serviceWrapper.HoaDon.HoaDonLog.InsertAsync(log);
                                });

                                return thongdiep;
                            }
                            else
                            {
                                return string.Empty;
                            }
                        }
                        else if ((int)dt.Rows[0]["hoa_don_log_type_id"] == 3)
                        {
                            var hd_hinhthuc_code = dt.Rows[0]["hoa_don_hinh_thuc_code"].ToString();
                            string nguoi_ban_mst = string.Empty;
                             nguoi_ban_mst= dt.Rows[0]["donvi_ma_dv"].ToString();                                                     
                            var uuid = Guid.NewGuid().ToString().Replace("-", "").ToUpper();

                            if (hd_hinhthuc_code == "C")
                            {
                                var thongDiep = new ThongDiep()
                                {
                                    ThongTinChung = new ThongTinChungThongDiep()
                                    {
                                        phien_ban = "2.1.0",
                                        ma_noi_gui = AppSettings.FixedValue.MNGui,
                                        ma_noi_nhan = AppSettings.FixedValue.MNNhan,
                                        thong_diep = "200",
                                        ma_noi_gui_uuid = $"{AppSettings.FixedValue.MNGui}{uuid}".ToUpper(),
                                        ma_thong_diep_tham_chieu = $"",
                                        mst = nguoi_ban_mst,
                                        so_luong = 1
                                    }
                                };
                                hoaDon.phat_hanh_uuid = uuid;
                                hoaDon.id = (int)dt.Rows[0]["hoa_don_id"];                                
                                hoaDon.user_id_phathanh =(int) dt.Rows[0]["created_user_id"];

                                await this.UpdateAsync(hoaDon);
                                await RegisterPhatHanhUuidCacheAsync(hoaDon);
                                var base64thongdiep = thongDiep.ConvertToXmlAndAppendChild("/TDiep", "DLieu", plainXML).ConvertToBase64();
                                
                                var authHeader = Helper.WSInterTRCA2Helper.GetAuthHeader();
                              
                                var fileName = Guid.NewGuid().ToString() + ".xml";
                                var filePath = $"Xml/{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{fileName}";
                                var directoryPath = Path.GetDirectoryName(filePath);
                                if (!Directory.Exists(directoryPath))
                                {
                                    Directory.CreateDirectory(directoryPath);
                                }
                                await File.WriteAllTextAsync(filePath, base64thongdiep.ConvertToXmlFromBase64());
                                var log = new hoa_don_log()
                                {
                                    file_thong_diep_url = filePath,
                                    ngay_thuc_hien = DateTime.Now,
                                    nguoi_thuc_hien = "adminNCM",
                                    noi_dung_thuc_hien = "Gửi lai thông điệp lên CQT",
                                    hoa_don_id = hoaDon.id,
                                    hoa_don_log_type_id = (int)e_hoa_don_log_type.GUI_THONG_DIEP
                                };
                                log.SetInsertInfo(28057);
                                _serviceWrapper.Core.TaskQueue.EnqueueTask(async _ =>
                                {
                                    await _serviceWrapper.HoaDon.HoaDonLog.InsertAsync(log);
                                });
                                return base64thongdiep.ConvertToXmlFromBase64();
                            }

                            else if (hd_hinhthuc_code == "M")
                            {
                                if (!string.IsNullOrEmpty(plainXML))
                                {
                                    XmlDocument doc = new XmlDocument();
                                    doc.LoadXml(plainXML);
                                    string thongdiep = string.Empty;
                                    string key = "0103930279-001" + Guid.NewGuid().ToString().Replace("-", "").ToUpper();
                                   
                                    XmlNode mtDiepNode = doc.SelectSingleNode("/TDiep/TTChung/MTDiep");
                                    if (mtDiepNode != null)
                                    {
                                        mtDiepNode.InnerText = key;
                                        thongdiep = doc.InnerXml;
                                    }
                                    string uid = key.Replace("0103930279-001", "").ToUpper();
                                    hoaDon.phat_hanh_uuid = uid;
                                    hoaDon.id = (int)dt.Rows[0]["hoa_don_id"];
                                    hoaDon.user_id_phathanh = (int)dt.Rows[0]["created_user_id"];
                                    await this.UpdateAsync(hoaDon);
                                    await RegisterPhatHanhUuidCacheAsync(hoaDon);

                                    var fileName = Guid.NewGuid().ToString() + ".xml";
                                    var filePath = $"Xml/{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{fileName}";
                                    var directoryPath = Path.GetDirectoryName(filePath);
                                    if (!Directory.Exists(directoryPath))
                                    {
                                        Directory.CreateDirectory(directoryPath);
                                    }
                                    await File.WriteAllTextAsync(filePath, thongdiep);
                                    var log = new hoa_don_log()
                                    {
                                        file_thong_diep_url = filePath,
                                        ngay_thuc_hien = DateTime.Now,
                                        nguoi_thuc_hien = "adminNCM",
                                        noi_dung_thuc_hien = "Gửi lai thông điệp lên CQT",
                                        hoa_don_id = hoaDon.id,
                                        hoa_don_log_type_id = (int)e_hoa_don_log_type.GUI_THONG_DIEP
                                    };
                                    log.SetInsertInfo(28057);
                                    _serviceWrapper.Core.TaskQueue.EnqueueTask(async _ =>
                                    {
                                        await _serviceWrapper.HoaDon.HoaDonLog.InsertAsync(log);
                                    });

                                    return thongdiep;
                                }
                                else
                                {
                                    return string.Empty;
                                }
                            }
                            else
                            {
                                return plainXML ?? string.Empty;
                            }
                        }
                        else
                        {
                            return string.Empty;
                        }
                           
                    }
                    else
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            if ((int)row["hoa_don_log_type_id"] == -7)
                            {
                                string urlXML = row["linkxml"]?.ToString() ?? "";
                                string plainXML = await GetXmlAsync(urlXML);
                                if (!string.IsNullOrEmpty(plainXML))
                                {
                                    XmlDocument doc = new XmlDocument();
                                    doc.LoadXml(plainXML);
                                    string thongdiep = string.Empty;
                                    string key = "0103930279-001" + Guid.NewGuid().ToString().Replace("-", "").ToUpper();

                                    XmlNode mtDiepNode = doc.SelectSingleNode("/TDiep/TTChung/MTDiep");
                                    if (mtDiepNode != null)
                                    {
                                        mtDiepNode.InnerText = key;
                                        thongdiep = doc.InnerXml;
                                    }
                                    string uid = key.Replace("0103930279-001", "").ToUpper();
                                    hoaDon.phat_hanh_uuid = uid;
                                    hoaDon.id = (int)dt.Rows[0]["hoa_don_id"];
                                    hoaDon.user_id_phathanh = (int)dt.Rows[0]["created_user_id"];
                                    await this.UpdateAsync(hoaDon);
                                    await RegisterPhatHanhUuidCacheAsync(hoaDon);

                                    var fileName = Guid.NewGuid().ToString() + ".xml";
                                    var filePath = $"Xml/{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{fileName}";
                                    var directoryPath = Path.GetDirectoryName(filePath);
                                    if (!Directory.Exists(directoryPath))
                                    {
                                        Directory.CreateDirectory(directoryPath);
                                    }
                                    await File.WriteAllTextAsync(filePath, thongdiep);
                                    var log = new hoa_don_log()
                                    {
                                        file_thong_diep_url = filePath,
                                        ngay_thuc_hien = DateTime.Now,
                                        nguoi_thuc_hien = "adminNCM",
                                        noi_dung_thuc_hien = "Gửi lai thông điệp lên CQT",
                                        hoa_don_id = hoaDon.id,
                                        hoa_don_log_type_id = (int)e_hoa_don_log_type.GUI_THONG_DIEP
                                    };
                                    log.SetInsertInfo(28057);
                                    _serviceWrapper.Core.TaskQueue.EnqueueTask(async _ =>
                                    {
                                        await _serviceWrapper.HoaDon.HoaDonLog.InsertAsync(log);
                                    });
                                    return thongdiep;
                                }
                                return string.Empty;
                            }
                        }
                       

                        return string.Empty;
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        private string GetMNGui(string xmlContent)
        {
            try
            {
                var doc = XDocument.Parse(xmlContent);

                var mnGui = doc
                    .Descendants("MNGui")
                    .FirstOrDefault()?.Value;

                return mnGui ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public async Task<string> GetXmlAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                return await client.GetStringAsync(url);
            }
        }

    }
}