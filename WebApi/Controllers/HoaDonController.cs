using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Contract.Service;
using Contracts.Service.HoaDon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Base;
using Model.Enum;
using Model.Request.Base;
using Model.Request.HoaDon;
using Model.Request.ToKhai;
using Model.Respone.HoaDon;
using Model.Respone.Upload;
using Model.Static;
using WebApi.Filters;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System;
using Model.Cache;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/hoa-don")]
    //[MustLogged]

    public class HoaDonController : BaseController
    {
        private IHoaDonService _hoaDonService;
        public HoaDonController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
            this._hoaDonService = _serviceWrapper.HoaDon.HoaDon;
        }
        /// <summary>
        /// Truy vấn danh sách hóa đơn của đơn vị đang đăng nhập
        /// </summary>
        /// <remarks>
        ///payload ví dụ: {"hoa_don_trang_thai_ids":[1],"loai_hoa_don_ct_id":10,"hoa_don_dang_ky_phat_hanh_mau_so":"6","hoa_don_dang_ky_phat_hanh_ky_hieu":"C24BDL","page_index":0,"page_size":20,"sort_by":"","sort_mode":"desc"}
        /// </remarks>
        [HttpPost]
        [Route("select")]
        [MustAuthorized("[GET]api/hoa-don")]
        public async Task<ContentResult> SelectByDonViAsync([FromBody] HoaDonSelectPagingRequest pagingRequest)
        {
            // await _hoaDonService.SaoChepHoaDonNghichDaoAsync();
            var userInfo = this.GetUserInfo();
            var list = await _hoaDonService.SelectByDonViAsync(userInfo.donvi_ma_dv, pagingRequest);
            return this.OK(list);
        }
        /// <summary>
        /// Xem chi tiết 1 hóa đơn
        /// </summary>
        /// <remarks>
        ///ví dụ: api/hoa-don/1
        /// </remarks>
        [HttpGet("{id}")]
        [MustAuthorized("[GET]api/hoa-don")]
        public async Task<ContentResult> SelectByIdAsync(int id)
        {
            var userInfo = this.GetUserInfo();
            var model = await _hoaDonService.SelectViewModelAsync(id);
            return this.OK(model);
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("select-by-ids")]
        [MustAuthorized("[GET]api/hoa-don")]
        public async Task<ContentResult> SelectByIdsAsync([FromBody] HoaDonDeletesRequest request)
        {
            var hoaDons = await _hoaDonService.SelectByIdsAsync(request.ids);
            return this.OK(hoaDons);
        }
        /// <summary>
        /// Thêm mới hóa đơn
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [HttpPost]
        [MustAuthorized]

        public async Task<ContentResult> InsertAsync([FromBody] HoaDonAddOrEditModel model)
        {
            var user = this.GetUserInfo();
            var user_id = user.id;

            model.SetInsertInfo(user_id);

            if (user.vender_id.ConvertToString().Trim() != string.Empty)
            {
                model.vender_id = user.vender_id;
            }
            else
            {
                model.donvi_ma_dv = user.donvi_ma_dv;
            }

            var maDonVi = !string.IsNullOrWhiteSpace(model.nguoi_ban_mst)
                ? model.nguoi_ban_mst
                : model.donvi_ma_dv;

            // 1. Lấy danh sách hóa đơn của đơn vị về trước
            var dSHoaDonDangKyPhatHang = await _serviceWrapper.HoaDon.HoaDonDangKyPhatHanh.SelectByDonViAsync(maDonVi);

            // 2. Lọc ra bản ghi khớp ký hiệu và mẫu số
            var hoaDonDangKyPhatHanh = dSHoaDonDangKyPhatHang
                .FirstOrDefault(x => x.ky_hieu == model.hoa_don_dang_ky_phat_hanh_ky_hieu && x.mau_so == model.hoa_don_dang_ky_phat_hanh_mau_so && x.loai_hoa_don_ct_id == model.loai_hoa_don_ct_id);

            model.hoa_don_nghi_dinh_id = 123;
            model.ma_so_hoa_don = null;
            model.ten_hoa_don = hoaDonDangKyPhatHanh.ten_hoa_don;
            var result = await _hoaDonService.SaveHoaDonAsync(model);
            if (result.is_success)
            {
                await this.SaveLogAsync($"Thêm hóa đơn: {model.id}", model, user);
                return this.OK(model);
            }
            return this.BadRequest(result.message);
        }
        [HttpPost]
        [MustAuthorized("[POST]api/hoa-don")]
        [Route("inserts")]

        public async Task<ContentResult> InsertsAsync([FromBody] List<HoaDonAddOrEditModel> list)
        {
            var user = this.GetUserInfo();
            var user_id = user.id;
            var tasks = list.Select(async model =>
            {
                if (user.vender_id.ConvertToString().Trim() != string.Empty)
                {
                    model.vender_id = user.vender_id;
                }
                else
                {
                    model.donvi_ma_dv = user.donvi_ma_dv;
                }
                model.hoa_don_nghi_dinh_id = 123;
                model.ma_so_hoa_don = null;
                var result = await _hoaDonService.SaveHoaDonAsync(model);
                if (result.is_success)
                {
                    await this.SaveLogAsync($"Thêm hóa đơn: {model.id}", model, user);
                }

                return model;
            }).ToList();
            await this.ExcuteDbTasks(tasks, 10);
            var hoaDons = tasks.Select(x => x.Result).ToList();
            return this.OK(hoaDons);
        }
        /// <summary>
        /// Sửa hóa đơn
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [HttpPut]
        [MustAuthorized]
        public async Task<ContentResult> UpdateAsync([FromBody] HoaDonAddOrEditModel model)
        {
            var user = this.GetUserInfo();
            var user_id = user.id;

            var obj = await _hoaDonService.SelectByIdAsync(model.id);
            if (obj == null || obj.hoa_don_trang_thai_id != (int)e_hoa_don_trang_thai.NHAP || obj.donvi_ma_dv != user.donvi_ma_dv) return this.BadRequest();
            obj.SetUpdateInfo(user_id);
            var result = await _hoaDonService.SaveHoaDonAsync(model);
            if (result.is_success)
            {
                // await this.SaveLogAsync($"Sửa hóa đơn: {model.id}", model);
                await this.SaveLogAsync($"Sửa hóa đơn: {model.id}", model, user);

                return this.OK(model);
            }
            return this.BadRequest(result.message);

        }
        /// <summary>
        /// Xóa hóa đơn
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [HttpDelete("{id}")]
        [MustAuthorized]
        public async Task<ContentResult> DeleteAsync([FromRoute] int id)
        {
            var hoaDon = await _hoaDonService.SelectByIdAsync(id);
            var hoaDonTrangThaiDeleteIds = new List<int>() { (int)e_hoa_don_trang_thai.NHAP, (int)e_hoa_don_trang_thai.CHUA_GUI_CQT, (int)e_hoa_don_trang_thai.KHONG_HOP_LE };
            if (hoaDon == null || !hoaDonTrangThaiDeleteIds.Contains(hoaDon.hoa_don_trang_thai_id)) return this.BadRequest();
            var user_id = this.GetUserId();
            // var isDeleted = await _hoaDonService.DeleteAsync(hoaDon.id);
            // if (isDeleted) await this.SaveLogAsync($"Xóa hóa đơn id: {hoaDon.id}", null);

            if (hoaDon.hoa_don_trang_thai_id == (int)e_hoa_don_trang_thai.NHAP)
            {
                var isDeleted = await _hoaDonService.DeleteAsync(hoaDon.id);
                if (isDeleted)
                {
                    hoaDon.is_deleted = true;
                    await this.SaveLogAsync($"Xóa hóa đơn id: {hoaDon.id}", null);
                    await _serviceWrapper.HoaDon.PushMessageToVender.CheckAndPushMessageAsync(hoaDon);
                }
            }
            if (hoaDon.hoa_don_trang_thai_id == (int)e_hoa_don_trang_thai.CHUA_GUI_CQT ||
            hoaDon.hoa_don_trang_thai_id == (int)e_hoa_don_trang_thai.KHONG_HOP_LE

            )
            {
                hoaDon.hoa_don_trang_thai_id = (int)e_hoa_don_trang_thai.DA_HUY;
                hoaDon.hoa_don_hinh_thuc_id = (int)e_hoa_don_hinh_thuc.HOA_DON_DA_HUY_NOI_BO;
                hoaDon.SetUpdateInfo(user_id);
                var isUpdated = await _hoaDonService.UpdateAsync(hoaDon);
                if (isUpdated)
                {
                    await _serviceWrapper.HoaDon.PushMessageToVender.CheckAndPushMessageAsync(hoaDon);
                    await this.SaveLogAsync($"Hủy nộ bộ id: {hoaDon.id}", null);
                }
            }
            return this.OK();
            // return isDeleted ? this.OK(obj) : this.BadRequest();
        }
        /// <summary>
        /// Xóa nhiều hóa đơn
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [HttpPost]
        [Route("deletes")]
        [MustAuthorized("[DELETE]api/hoa-don/{id}")]
        public async Task<ContentResult> DeletesAsync([FromBody] HoaDonDeletesRequest request)
        {
            var hoaDons = await _hoaDonService.SelectByIdsAsync(request.ids);
            var user_id = this.GetUserId();
            var checkNotHoaDonNhaps = hoaDons.Where(x => !(x.hoa_don_trang_thai_id == (int)e_hoa_don_trang_thai.NHAP
            || x.hoa_don_trang_thai_id == (int)e_hoa_don_trang_thai.CHUA_GUI_CQT
            || x.hoa_don_trang_thai_id == (int)e_hoa_don_trang_thai.LOI_THONG_DIEP
            || x.hoa_don_trang_thai_id == (int)e_hoa_don_trang_thai.KHONG_HOP_LE)).ToList();
            if (checkNotHoaDonNhaps.Count > 0) return this.BadRequest($"Có {checkNotHoaDonNhaps.Count} hóa đơn không phải hóa đơn nháp hoặc chưa phát hành");
            foreach (var hoaDon in hoaDons)
            {
                if (hoaDon.hoa_don_trang_thai_id == (int)e_hoa_don_trang_thai.NHAP)
                {
                    var isDeleted = await _hoaDonService.DeleteAsync(hoaDon.id);
                    if (isDeleted)
                    {
                        hoaDon.is_deleted = true;
                        await this.SaveLogAsync($"Xóa hóa đơn id: {hoaDon.id}", null);
                        await _serviceWrapper.HoaDon.PushMessageToVender.CheckAndPushMessageAsync(hoaDon);
                    }
                }
                if (hoaDon.hoa_don_trang_thai_id == (int)e_hoa_don_trang_thai.CHUA_GUI_CQT ||
                hoaDon.hoa_don_trang_thai_id == (int)e_hoa_don_trang_thai.KHONG_HOP_LE

                )
                {
                    hoaDon.hoa_don_trang_thai_id = (int)e_hoa_don_trang_thai.DA_HUY;
                    hoaDon.hoa_don_hinh_thuc_id = (int)e_hoa_don_hinh_thuc.HOA_DON_DA_HUY_NOI_BO;
                    hoaDon.SetUpdateInfo(user_id);
                    var isUpdated = await _hoaDonService.UpdateAsync(hoaDon);
                    if (isUpdated)
                    {
                        await _serviceWrapper.HoaDon.PushMessageToVender.CheckAndPushMessageAsync(hoaDon);
                        await this.SaveLogAsync($"Hủy nộ bộ id: {hoaDon.id}", null);
                    }
                }
            }
            return this.OK();
        }
        /// <summary>
        /// Lấy xml để ký số của 1 hóa đơn
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [HttpGet("{id}/ky-so")]
        [MustAuthorized("[POST]api/hoa-don/phat-hanh")]
        public async Task<ContentResult> XmlKySoBase64Async([FromRoute] int id)
        {
            var hoaDon = await _hoaDonService.SelectByIdAsync(id);
            if (hoaDon != null)
            {
                if (hoaDon.hoa_don_hinh_thuc_code != "M")
                {
                    var getNgayPhatHanhMax = await _hoaDonService.GetNgayHoaDonPhatHanhMaxAsynsc(hoaDon.donvi_ma_dv, hoaDon.hoa_don_dang_ky_phat_hanh_mau_so, hoaDon.hoa_don_dang_ky_phat_hanh_ky_hieu);
                    if (getNgayPhatHanhMax.is_success && getNgayPhatHanhMax.data.HasValue)
                    {
                        if (getNgayPhatHanhMax.data.Value.Date > hoaDon.ngay_hoa_don.Date)
                        {
                            return this.BadRequest($"Đã tồn tại hóa đơn phát hành ngày {getNgayPhatHanhMax.data.Value.ToString("dd/MM/yyyy")}");
                        }
                    }
                }

                if (hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_DA_HUY_NOI_BO)
                {
                    return this.BadRequest("Hóa đơn đã hủy nội bộ");
                }
                var donVi = await _serviceWrapper.Category.DonVi.SelectByMaDonViAsync(hoaDon.donvi_ma_dv);
                if (donVi != null && donVi.total_cks_con_lai <= 0) return this.BadRequest("Đã hết số lượng hóa đơn đăng ký. Vui lòng gia hạn thêm gói dịch vụ để tiếp tục");
                if (hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_DIEU_CHINH ||
        hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_THAY_THE)
                {
                    var getBase64BienBanResult = await _hoaDonService.GetBase64BienBanAsync(id);
                    var base64BienBan = getBase64BienBanResult.data;
                    var base64HoaDon = "";
                    if (hoaDon.hoa_don_hinh_thuc_code == "M")
                    {
                        var base64HoaDonResult = await _hoaDonService.CreateBase64MTTAsync(hoaDon);
                        if (!base64HoaDonResult.is_success)
                        {
                            return this.BadRequest(base64HoaDonResult.message);
                        }
                        else
                        {
                            base64HoaDon = base64HoaDonResult.data;
                        }
                    }
                    else
                    {
                        var xmlResutl = await _hoaDonService.CreateXmlKySoAsync(hoaDon);

                        if (xmlResutl.is_success)
                        {
                            base64HoaDon = xmlResutl.data.ConvertToBase64();
                        }
                        else
                        {
                            return this.BadRequest(xmlResutl.message);
                        }

                    }
                    //hóa đơn cũ chưa có biên bản thì k return
                    if (base64BienBan != string.Empty)
                    {
                        return this.OK(new HoaDonBase64BienBan()
                        {
                            hoa_don_base64 = base64HoaDon,
                            bien_ban_base64 = base64BienBan

                        });
                    }
                    return this.OK(base64HoaDon);

                }
                else
                {
                    if (hoaDon.hoa_don_hinh_thuc_code == "M")
                    {
                        var base64Result = await _hoaDonService.CreateBase64MTTAsync(hoaDon);
                        return base64Result.is_success ? this.OK(base64Result.data) : this.BadRequest(base64Result.message);
                    }
                    else
                    {
                        var xmlResult = await _hoaDonService.CreateXmlKySoAsync(hoaDon);
                        if (!xmlResult.is_success) return this.BadRequest(xmlResult.message);
                        var base64 = xmlResult.data.ConvertToBase64();
                        return this.OK(base64);
                    }
                }


            }
            return this.BadRequest();

        }
        [Route("ky-so-multiple")]
        [HttpPost]
        [MustAuthorized("[POST]api/hoa-don/phat-hanh")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ContentResult> XmlKySoBase64MultiplyAsync([FromBody] HoaDonDeletesRequest request)
        {
            var result = new List<HoaDonCreateXmlKySoResponeList>();
            var hoaDons = await _hoaDonService.SelectByIdsAsync(request.ids);
            foreach (var hoaDon in hoaDons)
            {
                var id = hoaDon.id;
                if (hoaDon.hoa_don_hinh_thuc_code != "M")
                {
                    var getNgayPhatHanhMax = await _hoaDonService.GetNgayHoaDonPhatHanhMaxAsynsc(hoaDon.donvi_ma_dv, hoaDon.hoa_don_dang_ky_phat_hanh_mau_so, hoaDon.hoa_don_dang_ky_phat_hanh_ky_hieu);
                    if (getNgayPhatHanhMax.is_success && getNgayPhatHanhMax.data.HasValue)
                    {
                        if (getNgayPhatHanhMax.data.Value.Date > hoaDon.ngay_hoa_don.Date)
                        {
                            return this.BadRequest($"Đã tồn tại hóa đơn phát hành ngày {getNgayPhatHanhMax.data.Value.ToString("dd/MM/yyyy")}");
                        }
                    }
                }
                if (hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_DA_HUY_NOI_BO)
                {
                    return this.BadRequest("Hóa đơn đã hủy nội bộ");
                }
                var donVi = await _serviceWrapper.Category.DonVi.SelectByMaDonViAsync(hoaDon.donvi_ma_dv);
                if (donVi != null && donVi.total_cks_con_lai <= 0) return this.BadRequest("Đã hết số lượng hóa đơn đăng ký. Vui lòng gia hạn thêm gói dịch vụ để tiếp tục");
                if (hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_DIEU_CHINH ||
        hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_THAY_THE)
                {
                    var getBase64BienBanResult = await _hoaDonService.GetBase64BienBanAsync(id);
                    var getBase64HoaDon = hoaDon.hoa_don_hinh_thuc_code == "M" ? await _hoaDonService.CreateBase64MTTAsync(hoaDon) : await _hoaDonService.CreateXmlKySoAsync(hoaDon);
                    var base64BienBan = getBase64BienBanResult.data;
                    var base64HoaDon = hoaDon.hoa_don_hinh_thuc_code == "M" ? getBase64HoaDon.data : getBase64HoaDon.data.ConvertToBase64();// getBase64HoaDon.data.ConvertToBase64();
                    if (getBase64HoaDon.is_success && getBase64BienBanResult.is_success)
                    {
                        result.Add(new HoaDonCreateXmlKySoResponeList()
                        {
                            id = id,
                            xml_base64 = base64HoaDon,
                            bien_ban_base64 = base64BienBan,
                            is_success = true
                        });
                    }
                    else
                    {
                        result.Add(new HoaDonCreateXmlKySoResponeList()
                        {
                            id = id,
                            is_success = false,
                            message = new List<string>() { getBase64BienBanResult.message, getBase64HoaDon.message }.Where(x => x != "").Join(";")
                        });
                    }
                }
                else
                {

                    var base64Result = hoaDon.hoa_don_hinh_thuc_code == "M" ? await _hoaDonService.CreateBase64MTTAsync(hoaDon) : await _hoaDonService.CreateXmlKySoAsync(hoaDon);
                    if (base64Result.is_success)
                        result.Add(new HoaDonCreateXmlKySoResponeList()
                        {
                            id = id,
                            xml_base64 = hoaDon.hoa_don_hinh_thuc_code == "M" ? base64Result.data : base64Result.data.ConvertToBase64(),
                            is_success = true
                        });
                    else
                    {
                        result.Add(new HoaDonCreateXmlKySoResponeList()
                        {
                            id = id,
                            xml_base64 = base64Result.data,
                            is_success = false,
                            message = base64Result.message
                        });
                    }


                }
            }
            return this.OK(result);
        }

        [HttpPut]
        [Route("preview")]
        [MustAuthorized("[GET]api/hoa-don")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ContentResult> PreviewAsync([FromBody] HoaDonAddOrEditModel model)
        {

            var result = await _hoaDonService.GetHtmlPreviewAsync(model);
            return result.is_success ? this.OK(result.data) : this.BadRequest(result.message);
        }
        /// <summary>
        /// Cập nhật hóa đơn đã ký số
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [HttpPost("{id}/ky-so")]
        [MustAuthorized("[POST]api/hoa-don/phat-hanh")]
        public async Task<ContentResult> KySoSuccess([FromBody] HoaDonPhatHanhRequest request)
        {
            await _hoaDonService.UpdteKySoSuccessAsync(request);
            return this.OK();
        }
        /// <summary>
        /// Phát hành hóa đơn
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [HttpPost]
        [Route("phat-hanh")]
        // [Route("/api/phat-hanh-hoa-don")]
        public async Task<ContentResult> PhatHanhAsync([FromBody] HoaDonPhatHanhRequest request)
        {
            var hoaDon = await _hoaDonService.SelectByIdAsync(request.id);
            if (hoaDon.hoa_don_hinh_thuc_code == "M")
            {
                var base64 = await _hoaDonService.PhatHanhMTTAsync(request, hoaDon);
                return this.OK(base64);
            }
            var result = await _hoaDonService.PhatHanhAsync(request);
            if (result.is_success)
            {
                return this.OK(result.data);
            }
            return this.BadRequest(result.message);
        }
        [HttpPost]
        [Route("import/valid")]
        [MustAuthorized("[POST]api/hoa-don")]
        [ApiExplorerSettings(IgnoreApi = true)]

        public async Task<ContentResult> ReadAndValidImportData([FromBody] HoaDonImportRequest upload)
        {
            var userInfo = this.GetUserInfo();
            var result = new FunctionResult<DataTable>();

            switch (upload.template.ConvertToString())
            {
                case "hoc_phi":
                    result = await _serviceWrapper.HoaDon.HoaDonImport.ReadAndValidImportDataHocPhiAsync(upload);
                    break;
                case "nuoc":
                    result = await _serviceWrapper.HoaDon.HoaDonImport.ReadAndValidImportDataNuocAsync(upload);
                    break;
                default:
                    result = await _serviceWrapper.HoaDon.HoaDonImport.ReadAndValidImportDataAsync(upload);
                    break;
            }
            // var result = await _serviceWrapper.HoaDon.HoaDonImport.ReadAndValidImportDataAsync(upload);
            if (result.is_success)
            {
                return this.OK(result.data);
            }
            return this.BadRequest(result.message);
        }
        [HttpPost]
        [Route("import")]
        [MustAuthorized("[POST]api/hoa-don")]
        [ApiExplorerSettings(IgnoreApi = true)]

        public async Task<ContentResult> ImportData([FromBody] HoaDonImportRequest upload)
        {
            var userInfo = this.GetUserInfo();
            var result = new FunctionResult<string>();
            if (upload.ten_hoa_don.ConvertToString() == "")
            {
                var dangKyPhatHanhs = await _serviceWrapper.HoaDon.HoaDonDangKyPhatHanh.SelectByDonViAsync(userInfo.donvi_ma_dv);
                dangKyPhatHanhs = dangKyPhatHanhs.Where(x => x.ky_hieu == upload.hoa_don_dang_ky_phat_hanh_ky_hieu
                && x.mau_so == upload.hoa_don_dang_ky_phat_hanh_mau_so).ToList();
                upload.ten_hoa_don = dangKyPhatHanhs.LastOrDefault()?.ten_hoa_don ?? "";
                if (upload.loai_hoa_don_ct_id == 0) upload.loai_hoa_don_ct_id = dangKyPhatHanhs?.LastOrDefault()?.loai_hoa_don_ct_id ?? 0;
            }
            if (upload.ten_hoa_don.ConvertToString() == "")
            {
                var loaiHoaDonChiTiet = await _serviceWrapper.HoaDon.LoaiHoaDonCT.SelectByIdAsync(upload.loai_hoa_don_ct_id);
                var loaiHoaDon = await _serviceWrapper.HoaDon.LoaiHoaDon.SelectByIdAsync(loaiHoaDonChiTiet?.loai_hoa_don_id ?? 0);
                upload.ten_hoa_don = loaiHoaDon?.name ?? "";
            }
            switch (upload.template.ConvertToString())
            {
                case "hoc_phi":
                    result = await _serviceWrapper.HoaDon.HoaDonImport.ImportDataHocPhiAsync(upload);
                    break;
                case "nuoc":
                    result = await _serviceWrapper.HoaDon.HoaDonImport.ImportDataNuocAsync(upload);
                    break;
                default:
                    result = await _serviceWrapper.HoaDon.HoaDonImport.ImportDataAsync(upload);
                    break;
            }
            if (result.is_success)
            {
                return this.OK(result.data);
            }
            return this.BadRequest(result.message);
        }
        /// <summary>
        /// Gửi email hóa đơn đến email đơn vị mua hàng
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [HttpPost]
        [Route("send-email")]
        [MustAuthorized("[POST]api/hoa-don")]
        public async Task<ContentResult> SendEmailAsync([FromBody] IdsRequest request)
        {
            var result = await _serviceWrapper.HoaDon.HoaDonSendEmail.SendEmailHoaDonAsync(request.ids);
            return result.is_success ? this.OK(result.data) : this.BadRequest(result.message);
        }
        /// <summary>
        /// Gửi email hóa đơn đến email cụ thể
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [HttpPost]
        [Route("send-email-custom")]
        [MustAuthorized("[POST]api/hoa-don")]
        public async Task<ContentResult> SendEmailCustomAsync([FromBody] HoaDonSendEmailCustomRequest request)
        {
            var result = await _serviceWrapper.HoaDon.HoaDonSendEmail.SendEmailHoaDonAsync(request);
            return result.is_success ? this.OK(result.data) : this.BadRequest(result.message);
        }
        /// <summary>
        /// Lấy link public để xem hóa đơn
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [HttpGet]
        [Route("{id}/link")]
        [MustAuthorized("[POST]api/hoa-don")]
        public async Task<ContentResult> CreateLinkAsync(int id)
        {
            var url = AppSettings.FixedValue.FileDomain + "/hoa-don/view/" + id.ToString() + "?hash=" + id.ConvertToString().GenerateBcrypt();
            return this.OK(url);
        }

        //update tach ds

        [HttpPost("select-cho-phan-hoi-cqt")]
        public async Task<IActionResult> SelectChoPhanHoiCQTAsync([FromBody] HoaDonSelectPagingRequest request)
        {
            var userInfo = this.GetUserInfo();
            var result = await _hoaDonService.SelectChoPhanHoiCQTAsync(userInfo.donvi_ma_dv,request);
            return Ok(new SuccessResult<object>(new
            {
                data = result.data,
                total_count = result.total_count,
                page_count = result.page_count,
                page_number = result.page_number,
                page_size = result.page_size
            }));
        }

        [HttpPost("select-chua-gui-cqt")]
        public async Task<IActionResult> SelectChuaGuiCQTAsync([FromBody] HoaDonSelectPagingRequest request)
        {
            var userInfo = this.GetUserInfo();
            var result = await _hoaDonService
                .SelectChuaGuiCQTAsync(
                    userInfo.donvi_ma_dv,
                    request
                );
            return Ok(new SuccessResult<object>(new
            {
                data = result.data,
                total_count = result.total_count,
                page_count = result.page_count,
                page_number = result.page_number,
                page_size = result.page_size
            }));
        }

        [HttpPost("gui-lai-cqt/{id}")]
        public async Task<IActionResult> GuiLaiCQTAsync(int id)
        {
            var result = await _hoaDonService
                .GuiLaiCQTAsync(id);

            return Ok(result);
        }

        // test ky hash

        [AllowAnonymous]
        [HttpGet("test-prepare-hash/{id}")]
        public async Task<IActionResult>TestPrepareHashAsync(int id)
        {
            var result =await _hoaDonService.PrepareHashSignAsync(id);
            return Ok(new SuccessResult<object>(result));
        }

        [AllowAnonymous]
        [HttpPost("finalize-hash-sign")]
        public async Task<IActionResult> FinalizeHashSignAsync([FromBody]HoaDonFinalizeHashSignRequest request)
        {
            string signedXml =await _hoaDonService.FinalizeHashSignAsync(request);
            return Ok(new SuccessResult<object>(signedXml));
        }

        


    }
}

