using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;
using Amazon.Runtime;
using Common;
using Contracts.Service.HoaDon;
using Model.Base;
using Model.Enum;
using Model.Request.HoaDon;
using Model.Request.ToKhai;
using Model.Request.Xml;
using Model.Respone.HoaDon;
using Model.Respone.MauHoaDon;
using Model.Table;
using Service.Base;
using WebApp;

namespace Service.HoaDon
{
    public class MauHoaDonService : CRUDService<mau_hoa_don>, IMauHoaDonService
    {
        public MauHoaDonService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.HoaDon.MauHoaDon;
        }
        private string GetCompactXmlString(XDocument doc)
        {
            // Sử dụng XmlWriterSettings để tạo XML string dạng compact
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = false,         // Không xuống dòng
                OmitXmlDeclaration = false, // bao gồm khai báo XML
                NewLineChars = "",      // Không thêm dòng mới
                Encoding = Encoding.UTF8    // Sử dụng UTF-8
            };

            var sb = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                doc.Save(writer);
            }

            return sb.ToString();
        }
        private async Task<FunctionResult<string>> CreatePreviewHtmlV1Async(mau_hoa_don mauHoaDon, MauHoaDonCreateHtmlInput hoaDonData, hoa_don hoaDon, XsltArgumentList xsltArgument)
        {
            var xsltContent = "";
            if (File.Exists(mauHoaDon.xslt_path))
            {
                xsltContent = await File.ReadAllTextAsync(mauHoaDon.xslt_path);
            }
            if (hoaDon.is_ky_so_succes == true)
            {
                xsltContent = xsltContent.Replace("paramSign", "display:normal");
            }
            else
            {
                xsltContent = xsltContent.Replace("paramSign", "display:none");
            }
            xsltContent = xsltContent.Replace("{paramtiengiam}", hoaDon.giam_thue_ghi_chu.ConvertToString());
            xsltContent = xsltContent.Replace("paramLogo", mauHoaDon.logo_path.ConvertToString());
            xsltContent = xsltContent.Replace("paramMau", "display:none");
            xsltContent = xsltContent.Replace("paramNguoiCD", "display:none");
            xsltContent = xsltContent.Replace("paramChuyendoi", "display:none");
            xsltContent = xsltContent.Replace("hoadon78.nacencomm.vn", "ca2einvoice.nacencomm.vn");
            xsltContent = xsltContent.Replace("paramqrcode", $"https://api.qrserver.com/v1/create-qr-code/?size=100x100&amp;data={hoaDon.CreateQRCode()}");
            if (mauHoaDon.is_show_wattermark_inner_table == true)
            {

                xsltContent = xsltContent.Replace("viewstyle", "position:relative;width:900px;margin:auto; border:2px solid black; padding-top:20px;z-index:1;background-image: url(''); background-size:80%; background-position: center;width:900px;background-color: hsla(0,0%,100%,0.60);background-blend-mode: overlay;background-repeat:no-repeatwidth:900px;margin:auto; border:2px solid black; padding-top:20px;z-index:1;  background-image: url('" + mauHoaDon.watermark_path.ConvertToString() + "'); background-size:80%; background-position: center;width:900px;background-color: hsla(0,0%,100%,0.60);background-blend-mode: overlay;background-repeat:no-repeat");
                xsltContent = xsltContent.Replace("paramTableBG", "background-image: url('" + mauHoaDon.watermark_path.ConvertToString() + "'); background-size:cover; background-position: center;background-color: hsla(0,0%,100%,0.60);background-blend-mode: overlay;background-repeat:no-repeat");
            }
            else
            {
                xsltContent = xsltContent.Replace("viewstyle", "position:relative;width:900px;margin:auto; border:2px solid black; padding-top:20px;z-index:1;  background-image: url('" + mauHoaDon.watermark_path + "'); background-size:80%; background-position: center;width:900px;background-color: hsla(0,0%,100%,0.60);background-blend-mode: overlay;background-repeat:no-repeat");
                xsltContent = xsltContent.Replace("paramTableBG", "");

            }
            if (hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_DIEU_CHINH)
            {
                xsltContent = xsltContent.Replace("param1", "(Hóa đơn điều chỉnh)");
                xsltContent = xsltContent.Replace("param1_1", "normal");
                xsltContent = xsltContent.Replace("param2_2", "normal");
                xsltContent = xsltContent.Replace("param2", $"Hóa đơn điều chỉnh cho hóa đơn số {hoaDon.ma_so_hoa_don_goc}, mẫu số  {hoaDon.hoa_don_dang_ky_phat_hanh_mau_so_goc}, ký hiệu {hoaDon.hoa_don_dang_ky_phat_hanh_ky_hieu_goc}, ngày hóa đơn {(hoaDon.ngay_hoa_don_goc?.ToString("dd/MM/yyyy") ?? "")}");
            }
            if (hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_THAY_THE)
            {
                xsltContent = xsltContent.Replace("param1", "(Hóa đơn thay thế)");
                xsltContent = xsltContent.Replace("param1_1", "normal");
                xsltContent = xsltContent.Replace("param2_2", "normal");
                xsltContent = xsltContent.Replace("param2", $"Hóa đơn thay thế cho hóa đơn số {hoaDon.ma_so_hoa_don_goc}, mẫu số  {hoaDon.hoa_don_dang_ky_phat_hanh_mau_so_goc}, ký hiệu {hoaDon.hoa_don_dang_ky_phat_hanh_ky_hieu_goc}, ngày hóa đơn {(hoaDon.ngay_hoa_don_goc?.ToString("dd/MM/yyyy") ?? "")}");
            }
            if (hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_BI_DIEU_CHINH || hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_BI_THAY_THE)
            {
                xsltContent = xsltContent.Replace("param1", "");
                xsltContent = xsltContent.Replace("param1_1", "none");
                xsltContent = xsltContent.Replace("param2_2", "normal");
                xsltContent = xsltContent.Replace("param2", $"");
                xsltContent = xsltContent.Replace("contentDisable", $"{(hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_BI_DIEU_CHINH ? "HÓA ĐƠN BỊ ĐIỀU CHỈNH" : "HÓA ĐƠN BỊ THAY THẾ")}");
                xsltContent = xsltContent.Replace("paramdisable", $"position:absolute;z-index:0; width:auto; height:70px; border:4px solid red;  background:transparent; display:block;top:45%;left:50%;transform: translate(-50%, -50%);color:red;font-size:25pt;font-weight:bold;text-align:center;padding-top:10px;opacity:0.5");
            }
            if (hoaDon.hoa_don_hinh_thuc_id != (int)e_hoa_don_hinh_thuc.HOA_DON_BI_DIEU_CHINH && hoaDon.hoa_don_hinh_thuc_id != (int)e_hoa_don_hinh_thuc.HOA_DON_BI_THAY_THE && hoaDon.hoa_don_hinh_thuc_id != (int)e_hoa_don_hinh_thuc.DA_GUI_TBSS_THAY_THE)
            {
                xsltContent = xsltContent.Replace("param1", "");
                xsltContent = xsltContent.Replace("param1_1", "none");
                xsltContent = xsltContent.Replace("param2_2", "normal");
                xsltContent = xsltContent.Replace("param2", $"");
                xsltContent = xsltContent.Replace("contentDisable", $"&#160;");
                xsltContent = xsltContent.Replace("paramdisable", $"position:absolute;z-index:0 ; width:300px; height:100px; border:3px solid red; background:transparent; display:none;  top:45%; left:40%; color:red;font-size:70pt;text-align:center;padding-top:10px;");
            }
            if (hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.DA_GUI_TBSS_THAY_THE)
            {
                xsltContent = xsltContent.Replace("param1", "");
                xsltContent = xsltContent.Replace("param1_1", "none");
                xsltContent = xsltContent.Replace("param2_2", "normal");
                xsltContent = xsltContent.Replace("param2", $"");
                xsltContent = xsltContent.Replace("contentDisable", "HÓA ĐƠN BỊ THAY THẾ");
                xsltContent = xsltContent.Replace("paramdisable", $"position:absolute;z-index:0; width:auto; height:70px; border:4px solid red;  background:transparent; display:block;top:45%;left:50%;transform: translate(-50%, -50%);color:red;font-size:25pt;font-weight:bold;text-align:center;padding-top:10px;opacity: 0.5");
            }
            xsltContent = xsltContent.Replace("paramlien", "0");
            string xmlInput = hoaDonData.SerializeToXml();
            var getXmlKySoPreview = await _serviceWrapper.HoaDon.HoaDon.CreateXmlKySoAsync(hoaDon.id, true);
            if (!getXmlKySoPreview.is_success) return new ErrorResult<string>(getXmlKySoPreview.message, null);
            var html = await _serviceWrapper.HoaDon.LoaiHoaDonCTTemplate.GeneratePrintHtmlFromXsltContentAsync(xsltContent, getXmlKySoPreview.data, xsltArgument);
            // var html = await _serviceWrapper.HoaDon.LoaiHoaDonCTTemplate.GeneratePrintHtmlFromXsltContentAsync(xsltContent, xmlInput, xsltArgument);
            var css = @"
<style>
@media print {
    .page-break {
        page-break-before: always;
    }
}
</style>";
            html = css + html;
            html = html.Replace("NaN", "");
            return new SuccessResult<string>(html);
        }
        public async Task<FunctionResult<string>> CreatePreviewHtmlAsync(HoaDonAddOrEditModel hoaDon, bool isShowMau = true)
        {
            // LogWriter.Writer("CreatePreviewHtmlAsync Start", $"{hoaDon.id}", "");
            var mauHoaDon = await this.SelectMauActiveByDonVAsync(hoaDon.donvi_ma_dv, hoaDon.loai_hoa_don_ct_id);
            if (mauHoaDon == null) return new ErrorResult<string>("Không có mẫu hóa đơn");
            // var loaiHoaDonCTTemplate = await _serviceWrapper.HoaDon.LoaiHoaDonCTTemplate.SelectByIdAsync(mauHoaDon.loai_hoa_don_ct_template_id);
            // if (loaiHoaDonCTTemplate == null) return new ErrorResult<string>("Không có template");
            // foreach (var item in hoaDon.hoang_hoas)
            // {
            //     item.thanh_tien = hoaDon.loai_tien == "VND" ? (decimal)item.thanh_tien.ConvertToDouble(0) : item.thanh_tien;
            // }
            // LogWriter.Writer("hoang_hoas done", $"{hoaDon.id}", "");
            var hangHoas = hoaDon.hoang_hoas;
            var donVi = await this.GetCurrentDonViAsync();
            var hoaDonData = new MauHoaDonCreateHtmlInput();
            hoaDonData.hoa_don = new Model.Request.Xml.HoaDon();
            hoaDonData.hoa_don.qr_code = hoaDon.CreateQRCode();
            var isAllChietKhau = !hangHoas.Where(x => x.hang_hoa_tinh_chat_id != 4).Any(x => x.hang_hoa_tinh_chat_id != 3);

            hoaDonData.hoa_don.du_lieu_hoa_don.thong_tin_chung = new ThongTinChung()
            {
                ngay_lap = hoaDon.ngay_hoa_don.ToString("yyyy-MM-dd"),
                ky_hieu_mau_so_hoa_don = hoaDon.hoa_don_dang_ky_phat_hanh_mau_so,
                don_vi_tien_te = hoaDon.loai_tien,
                hinh_thuc_thanh_toan = hoaDon.hinh_thuc_tt,
                ky_hieu_hoa_don = hoaDon.hoa_don_dang_ky_phat_hanh_ky_hieu,
                so_hoa_don = hoaDon.ma_so_hoa_don.ConvertToString(),
                ten_hoa_don = hoaDon.ten_hoa_don,
                ty_gia = (hoaDon.loai_tien.ConvertToString() != "VND" && hoaDon.loai_tien.ConvertToString() != "") ? hoaDon.ty_gia.ConvertToStringAndRemoveZeroPart() : null,
            };
            if (hoaDon.giam_thue_ghi_chu.ConvertToString() != "")
            {
                if (hoaDonData.hoa_don.du_lieu_hoa_don.thong_tin_chung.thong_tin_khac == null)
                {
                    hoaDonData.hoa_don.du_lieu_hoa_don.thong_tin_chung.thong_tin_khac = new ThongTinKhac();
                    hoaDonData.hoa_don.du_lieu_hoa_don.thong_tin_chung.thong_tin_khac.thong_tin_khac_noi_dung = new List<ThongTinKhacNoiDung>();
                }
                if (hoaDonData.hoa_don.du_lieu_hoa_don.thong_tin_chung.thong_tin_khac.thong_tin_khac_noi_dung == null)
                {
                    hoaDonData.hoa_don.du_lieu_hoa_don.thong_tin_chung.thong_tin_khac.thong_tin_khac_noi_dung = new List<ThongTinKhacNoiDung>();
                }
                hoaDonData.hoa_don.du_lieu_hoa_don.thong_tin_chung.thong_tin_khac.thong_tin_khac_noi_dung.Add(new ThongTinKhacNoiDung()
                {
                    thong_tin_truong = "GhiChu",
                    kieu_du_lieu = "string",
                    du_lieu = hoaDon.giam_thue_ghi_chu.ConvertToString(),
                });

            }
            ;
            if (hoaDon.thong_tin_khac_json.ConvertToString() != "")
            {
                if (hoaDonData.hoa_don.du_lieu_hoa_don.thong_tin_chung.thong_tin_khac == null)
                {
                    hoaDonData.hoa_don.du_lieu_hoa_don.thong_tin_chung.thong_tin_khac = new ThongTinKhac();
                    hoaDonData.hoa_don.du_lieu_hoa_don.thong_tin_chung.thong_tin_khac.thong_tin_khac_noi_dung = new List<ThongTinKhacNoiDung>();
                }
                //lấy tất cả các field và giá trị từ hoaDon.thong_tin_khac_json (đang kiểu string)
                var jsonStr = hoaDon.thong_tin_khac_json.ConvertToString();
                try
                {
                    // Giả định JSON có cấu trúc dạng: { "field1": "value1", "field2": "value2", ... }
                    var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonStr);

                    foreach (var kv in dict)
                    {
                        hoaDonData.hoa_don.du_lieu_hoa_don.thong_tin_chung.thong_tin_khac.thong_tin_khac_noi_dung.Add(new ThongTinKhacNoiDung()
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
            // LogWriter.Writer("ThongTinChung done", $"{hoaDon.id}", "");
            hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban = new NguoiBan()
            {
                dia_chi = hoaDon.nguoi_ban_dia_chi,
                mst = hoaDon.nguoi_ban_mst,
                ten_nguoi_ban = hoaDon.nguoi_ban_ten_donvi,
                dien_thoai = hoaDon.nguoi_ban_dien_thoai,
                fax = hoaDon.nguoi_ban_fax,
                ngan_hang = hoaDon.nguoi_ban_ngan_hang,
                stk = hoaDon.nguoi_ban_stk,
                website = hoaDon.nguoi_ban_website,
                email = hoaDon.nguoi_ban_email

            };
            // LogWriter.Writer("NguoiBan done", $"{hoaDon.id}", "");
            hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_mua = new NguoiMua()
            {
                dia_chi = hoaDon.nguoi_mua_dia_chi,
                mst = hoaDon.nguoi_mua_mst,
                ten_don_vi = hoaDon.nguoi_mua_ten_donvi,
                ho_ten_nguoi_mua_hang = hoaDon.nguoi_mua_ten,
                dien_thoai = hoaDon.nguoi_mua_dien_thoai,
                fax = hoaDon.nguoi_mua_fax,
                ngan_hang = hoaDon.nguoi_mua_ngan_hang,
                stk = hoaDon.nguoi_mua_stk,
                website = hoaDon.nguoi_mua_website,
                email = hoaDon.nguoi_mua_email,
                cccd = hoaDon.nguoi_mua_cccd.ConvertToString() != "" ? hoaDon.nguoi_mua_cccd : null,
                ma_dv_ngan_sach = hoaDon.ma_dv_ngan_sach.ConvertToString() != "" ? hoaDon.ma_dv_ngan_sach : null


            };

            // LogWriter.Writer("NguoiMua done", $"{hoaDon.id}", "");
            hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.danh_sach_hang_hoa_dich_vu = new DanhSachHangHoaDichVu();
            hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.danh_sach_hang_hoa_dich_vu.hang_hoa_dich_vus =
            hangHoas.Select(x =>
            {
                var objItem = new HangHoaDichVu()
                {
                    don_gia = x.don_gia != 0 ? x.don_gia.ConvertToStringAndRemoveZeroPart() : string.Empty,
                    don_vi_tinh = x.dvt,
                    ma_hang_hoa_dich_vu = x.ma_hang,
                    so_luong = x.so_luong != 0 ? x.so_luong.ConvertToStringAndRemoveZeroPart() : string.Empty,
                    stt = x.stt > 0 ? x.stt.ToString() : string.Empty,
                    ten_hang_hoa_dich_vu = x.ten_hang,
                    thanh_tien = hoaDon.loai_tien == "VND" ? ((decimal)x.thanh_tien.ConvertToDouble(0)).ConvertToStringAndRemoveZeroPart() : x.thanh_tien.ConvertToStringAndRemoveZeroPart(),
                    thue_suat = x.thue_vat,
                    tinh_chat = x.hang_hoa_tinh_chat_id,
                    ty_le_chiet_khau = x.ty_le_chiet_khau.ConvertToStringAndRemoveZeroPart(),
                    so_tien_chiet_khau = x.tien_chiet_khau.ConvertToStringAndRemoveZeroPart()
                };
                if (x.ty_le_chiet_khau.ConvertToString() != "")
                {
                    objItem.ty_le_chiet_khau = x.ty_le_chiet_khau.ConvertToStringAndRemoveZeroPart();
                    objItem.so_tien_chiet_khau = x.tien_chiet_khau.ConvertToStringAndRemoveZeroPart();
                }
                if (x.hang_hoa_tinh_chat_id == 5 && x.hang_hoa_dac_trung_json.ConvertToString() != "")
                {
                    if (objItem.TTHHDTrung == null)
                    {
                        objItem.TTHHDTrung = new TTHHDTrung();
                        objItem.TTHHDTrung.TTHHDTrungTTins = new List<TTHHDTrungTTin>();
                    }
                    //lấy tất cả các field và giá trị từ hoaDon.thong_tin_khac_json (đang kiểu string)
                    var jsonStr = x.hang_hoa_dac_trung_json.ConvertToString();
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
                return objItem;
            }).ToList();

            // LogWriter.Writer($"CreatePreviewHtmlAsync", "hang_hoa", "");
            var loaiPhis = hoaDon.loai_phis;
            // LogWriter.Writer("hang_hoa_dich_vus done", $"{hoaDon.id}", "");
            var thue_suats = hangHoas.Select(x => x.thue_vat).Distinct().Where(x => x.Contains("%")).ToList().Select(x => new LTSuat()
            {
                ten_thue_suat = x
            }).ToList();
            var isApDungDieuChinh5DongVaoThueSuat = thue_suats.Count == 1;
            foreach (var thue_suat in thue_suats)
            {
                var phanTramThue = thue_suat.ten_thue_suat.Replace("%", "").ConvertToInt();
                var thanh_tien = hangHoas.Where(x => x.thue_vat == thue_suat.ten_thue_suat).Select(x => x.thanh_tien).Sum();
                if (isAllChietKhau)
                {
                    thanh_tien = -1 * thanh_tien;
                }
                thue_suat.thanh_tien = thanh_tien.ConvertToStringAndRemoveZeroPart();
                thue_suat.tien_thue = (thanh_tien * phanTramThue / 100).ConvertToStringAndRemoveZeroPart();
                // thue_suat.tien_thue = hangHoas.Where(x => x.thue_vat == thue_suat.ten_thue_suat)
                // .Select(x => Math.Round(x.thanh_tien * phanTramThue / 100, 0, MidpointRounding.AwayFromZero))
                // .Sum().ConvertToStringAndRemoveZeroPart();
                if (isApDungDieuChinh5DongVaoThueSuat && hoaDon.so_tien_tang_giam_tien_thue != 0)
                {
                    thue_suat.tien_thue += hoaDon.so_tien_tang_giam_tien_thue;
                }
                if (isAllChietKhau)
                {
                    thue_suat.tien_thue = ((decimal)(thue_suat.tien_thue.ConvertToDouble())).ConvertToStringAndRemoveZeroPart();
                }
                if (hoaDon.loai_tien == "VND")
                {
                    thue_suat.tien_thue = ((decimal)thue_suat.tien_thue.ConvertToDouble(0)).ConvertToStringAndRemoveZeroPart();
                    thue_suat.thanh_tien = ((decimal)thue_suat.thanh_tien.ConvertToDouble(0)).ConvertToStringAndRemoveZeroPart();
                }
            }
            // LogWriter.Writer("thue_suats done", $"{hoaDon.id}", "");
            if (hoaDon.hoa_don_dang_ky_phat_hanh_ky_hieu_goc.ConvertToString() != "" && hoaDon.ngay_hoa_don_goc.HasValue)
            {
                // var hoaDonGoc = await this.SelectByIdAsync(obj.hoa_don_id_goc);
                hoaDonData.hoa_don.du_lieu_hoa_don.thong_tin_chung.thong_tin_lien_quan = new ThongTinLienQuan()
                {
                    KHHDCLQuan = hoaDon.hoa_don_dang_ky_phat_hanh_ky_hieu_goc,
                    KHMSHDCLQuan = hoaDon.hoa_don_dang_ky_phat_hanh_mau_so_goc,
                    LHDCLQuan = hoaDon.hoa_don_nghi_dinh_id_goc == 123 ? "1" : "3",
                    NLHDCLQuan = hoaDon.ngay_hoa_don_goc.HasValue ? hoaDon.ngay_hoa_don_goc.Value.ToString("yyyy-MM-dd") : null,
                    SHDCLQuan = hoaDon.ma_so_hoa_don_goc.ToString(),
                    TCHDon = hoaDon.hoa_don_hinh_thuc_id == 3 ? "2" : "1",
                };
            }
            // LogWriter.Writer($"CreatePreviewHtmlAsync", "hoa_don_dang_ky_phat_hanh_ky_hieu_goc", "");
            var tong_tien_thanh_toan_bang_so = hoaDon.tong_tien_thanh_toan; ;
            if (hoaDon.loai_tien == "VND")
            {
                // tong_tien_thanh_toan_bang_so = (hoaDon.tong_tien_truong_thue + hoaDon.tong_tien_thue).ConvertToDouble(0).ConvertToDecimal();
            }
            // LogWriter.Writer("ThongTinLienQuan done", $"{hoaDon.id}", "");
            hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.thong_tin_thanh_toan = new ThongTinThanhToan()
            {
                tong_tien_thue = hoaDon.tong_tien_thue.ConvertToStringAndRemoveZeroPart(),
                tong_tien_chua_thue = hoaDon.tong_tien_truong_thue.ConvertToStringAndRemoveZeroPart(),
                tong_tien_thanh_toan_bang_chu = await tong_tien_thanh_toan_bang_so.ConvertToTextAsync(
                    hoaDon.loai_tien.ConvertToString() != "" ? hoaDon.loai_tien.ConvertToString() : "VND"),
                tong_tien_thanh_toan_bang_so = tong_tien_thanh_toan_bang_so.ConvertToStringAndRemoveZeroPart(),
                tong_tien_chiet_khau = hoaDon.tong_tien_chiet_khau.ConvertToStringAndRemoveZeroPart(),
                // tong_tien_thanh_toan_bang_so = hoaDon.loai_tien == "VND"
                // ? (hoaDon.hoang_hoas.Select(x => x.thanh_tien).Sum() + thue_suats.Select(x => x.tien_thue.ConvertToDecimal()).Sum()).ConvertToStringAndRemoveZeroPart()
                // : hoaDon.tong_tien_thanh_toan.ConvertToStringAndRemoveZeroPart(),
                thong_tin_thue_suat = new THTTLTSuat()
                {
                    thue_suats = thue_suats
                },
                thong_tin_phis = loaiPhis.Count() > 0 ? new DSLPhi()
                {
                    loai_phis = loaiPhis.Select(lp =>
                    {
                        return new LPhi()
                        {
                            ten_loai_phi = lp.ten_le_phi,
                            tien_phi = lp.so_tien.ConvertToStringAndRemoveZeroPart()
                        };
                    }).ToList()
                } : null

            };
            // LogWriter.Writer("CreatePreviewHtmlAsync hoaDonData done", $"{hoaDon.id}", "");
            if (hoaDon.loai_tien == "VND")
            {
                hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.thong_tin_thanh_toan.tong_tien_thanh_toan_bang_chu = await
    hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.thong_tin_thanh_toan.tong_tien_thanh_toan_bang_so.ConvertToDouble(0).ConvertToTextAsync(hoaDon.loai_tien.ConvertToString() != "" ? hoaDon.loai_tien.ConvertToString() : "VND");
            }
            if (hoaDon.hoa_don_dang_ky_phat_hanh_mau_so == "6")
            {
                //Phiếu xuất kho kiêm vận chuyển nội bộ
                if (hoaDon.loai_hoa_don_ct_id == 9)
                {
                    hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.LDDNBo = hoaDon.xuat_kho_vc_lenh_dieu_dong_noi_bo;
                    hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.PTVChuyen = hoaDon.xuat_kho_phuong_tien_van_chuyen;

                    hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.HDSo = hoaDon.xuat_kho_hop_dong_so;
                    hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.HVTNXHang = hoaDon.xuat_kho_nguoi_xuat_hang;
                    hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.TNVChuyen = hoaDon.xuat_kho_nguoi_van_chuyen;
                    hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.dia_chi = hoaDon.xuat_kho_dia_chi;
                }

                //Phiếu xuất kho đại lý
                if (hoaDon.loai_hoa_don_ct_id == 10)
                {
                    hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.HDKTSo = hoaDon.xuat_kho_dl_hop_dong_kinh_te_so;
                    if (hoaDon.xuat_kho_dl_hop_dong_ngay.HasValue)
                        hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.HDKTNgay =
                            hoaDon.xuat_kho_dl_hop_dong_ngay.Value.ToString("yyyy-MM-dd");
                    hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.PTVChuyen = hoaDon.xuat_kho_phuong_tien_van_chuyen;

                    hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.HDSo = hoaDon.xuat_kho_hop_dong_so;
                    hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.HVTNXHang = hoaDon.xuat_kho_nguoi_xuat_hang;
                    hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.TNVChuyen = hoaDon.xuat_kho_nguoi_van_chuyen;
                    hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.dia_chi = hoaDon.xuat_kho_dia_chi;
                }
            }
            if (hoaDon.hoa_don_dang_ky_phat_hanh_mau_so == "6")
            {
                //phiếu xuất kho
                var tong_tien_thanh_toan_bang_chu = hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.thong_tin_thanh_toan != null
                ? hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.thong_tin_thanh_toan.tong_tien_thanh_toan_bang_chu
                : await tong_tien_thanh_toan_bang_so.ConvertToTextAsync(
                        hoaDon.loai_tien.ConvertToString() != "" ? hoaDon.loai_tien.ConvertToString() : "VND"
                    ); ;
                hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.thong_tin_thanh_toan = null;
                if (hoaDonData.hoa_don.du_lieu_hoa_don.thong_tin_khac == null)
                {
                    hoaDonData.hoa_don.du_lieu_hoa_don.thong_tin_khac = new ThongTinKhac();
                    hoaDonData.hoa_don.du_lieu_hoa_don.thong_tin_khac.thong_tin_khac_noi_dung = new List<ThongTinKhacNoiDung>();
                }
                hoaDonData.hoa_don.du_lieu_hoa_don.thong_tin_khac.thong_tin_khac_noi_dung.Add(
                    new ThongTinKhacNoiDung()
                    {
                        du_lieu = tong_tien_thanh_toan_bang_so.ConvertToStringAndRemoveZeroPart(),
                        kieu_du_lieu = "numeric",
                        thong_tin_truong = "TgTTTBSo"
                    });
                hoaDonData.hoa_don.du_lieu_hoa_don.thong_tin_khac.thong_tin_khac_noi_dung.Add(
                    new ThongTinKhacNoiDung()
                    {
                        du_lieu = tong_tien_thanh_toan_bang_chu,
                        kieu_du_lieu = "string",
                        thong_tin_truong = "TgTTTBChu"
                    });
            }
            hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.stk = hoaDon.nguoi_ban_stk;
            hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.email = hoaDon.nguoi_ban_email;
            hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban.dien_thoai = hoaDon.nguoi_ban_dien_thoai;
            hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_mua.stk = hoaDon.nguoi_mua_stk;
            hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_mua.ngan_hang = hoaDon.nguoi_mua_ngan_hang;
            if (hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_mua.so_ho_chieu.ConvertToString() == "")
            {
                if (hoaDon.so_ho_chieu.ConvertToString() != "") hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_mua.so_ho_chieu = hoaDon.so_ho_chieu.ConvertToString();
            }
            LogWriter.Writer("CreatePreviewHtmlAsync tong_tien_thanh_toan_bang_so done", $"{hoaDon.id}", "");
            var xsltArgument = new XsltArgumentList();
            xsltArgument.AddParam("paramlien", "", "0");
            LogWriter.Writer($"CreatePreviewHtmlAsync Html Start", "api/hoa-don/{id}/print", "");
            if (mauHoaDon.xml_version.ConvertToInt() == 1)
            {
                return await this.CreatePreviewHtmlV1Async(mauHoaDon, hoaDonData, hoaDon, xsltArgument);
            }
            // var html = await _serviceWrapper.HoaDon.LoaiHoaDonCTTemplate.GeneratePrintHtmlAsync(loaiHoaDonCTTemplate.id, hoaDonData, xsltArgument);
            var html = await _serviceWrapper.HoaDon.LoaiHoaDonCTTemplate.GeneratePrintHtmlAsync(mauHoaDon, hoaDonData, xsltArgument);
            LogWriter.Writer("CreatePreviewHtmlAsync GeneratePrintHtmlAsync done", $"{hoaDon.id}", "");
            // var bgstyle = "width:900px;margin:auto; border:2px solid black; padding-top:20px;z-index:1;position: relative;";
            var bgstyle = "margin:auto; border:2px solid black; padding-top:0px;z-index:1;position: relative;";
            bgstyle = bgstyle + "background-image: url('{paramWaterMark}'); background-size:80%; background-position: center;background-color: hsla(0,0%,100%,paramOpacity;);background-blend-mode: overlay;background-repeat:no-repeat";
            var noidungdisabled = "&#160;";
            var styledisabled = "position:absolute;z-index:0;width:300px;height:140px;border:5px solid red;background:transparent;display:none;top:45%;left:40%;color:red;font-size:70pt;text-align:center;padding-top:10px;";
            var stylemau = "position:absolute;z-index:0;width:300px;height:140px;border:5px solid red;background:transparent;top:45%;left:40%;color:red;font-size:70pt;text-align:center;padding-top:10px;";
            if (!isShowMau)
            {
                styledisabled = "position:absolute;z-index:0;width:300px;height:140px;border:5px solid red;background:transparent;display:none;top:45%;left:40%;color:red;font-size:70pt;text-align:center;padding-top:10px;";
                stylemau = "position:absolute;z-index:0;width:300px;height:140px;border:5px solid red;background:transparent;display:none;top:45%;left:40%;color:red;font-size:70pt;text-align:center;padding-top:10px;";
                html = html.Replace("paramSign", "display:none");
            }
            var paramsubtitle = "normal";
            // var paramsubtitle = "none";
            var paramSubtitleDiv = "none";
            var paramsubtitlecontent = hoaDon.ngay_hoa_don.ToNgayThangNamText();
            var paramSubtitleContentDiv = "&#160;";


            html = html.Replace("viewstyle", bgstyle).Replace("paramLogo", "{paramLogo}")
           .Replace("paramChuyendoi", "display:none")
           .Replace("paramMau", stylemau)
           .Replace("min-height: 100%;background-image:url(paramVien);", "background-image:url(paramVien);")
           .Replace("paramNguoiCD", "width:100%;text-align:center;display:none")
           .Replace("paramdisable", styledisabled).Replace("contentDisable", noidungdisabled)
           .Replace("param1_1", paramsubtitle)
           .Replace("param1", paramsubtitlecontent)
           .Replace("param2_2", paramSubtitleDiv)
           .Replace("param2", paramSubtitleContentDiv)
           .Replace("paramdisable", styledisabled)
           .Replace("contentDisable", noidungdisabled)
           .Replace("paramlien", "0").Replace("paramdisplay", "display:none");

            if (mauHoaDon.is_show_wattermark_inner_table == true)
            {
                html = html.Replace("{paramLogo}", mauHoaDon.logo_path.ConvertToString().Replace('\\', '/') ?? "")
                                .Replace("paramWaterMarkTable;", mauHoaDon.watermark_path.ConvertToString().Replace('\\', '/') ?? "");
            }
            else
            {
                html = html.Replace("{paramLogo}", mauHoaDon.logo_path?.ConvertToString().Replace('\\', '/') ?? "")
                                .Replace("{paramWaterMark}", mauHoaDon.watermark_path?.ConvertToString().Replace('\\', '/') ?? "");
            }
            if (mauHoaDon.vien_path.ConvertToString() != "")
            {
                html = html.Replace("{paramVien}", mauHoaDon.vien_path?.ConvertToString().Replace('\\', '/') ?? "");
                html = html.Replace("paramVien", mauHoaDon.vien_path?.ConvertToString().Replace('\\', '/') ?? "");

            }
            if (mauHoaDon.logo_position.ConvertToString() == "right")
            {
                html = html.Replace("paramOpacityHeaderFlexDirection;", "row-reverse");
            }
            var paramOpacity = (1 - (mauHoaDon.watermark_opacity * 1.0 / 100).ConvertToDouble(2)).ToString().Replace(",", ".");
            html = html.Replace("paramOpacity;", paramOpacity);
            html = html.Replace("paramOpacity;", paramOpacity);
            var advancedSettings = mauHoaDon.advanced_settings_json.ConvertToString().TryDeserializeObject<CssEditorElementData[]>();
            html = html.Replace("12pt", "12px");
            html = html.Replace("<table style=\"width:100%;line-height:25px;font-size:12pt\">", "<table style=\"width:100%;line-height:20px;font-size:12px\">");
            html = html.Replace("line-height:25px", "line-height:20px");
            foreach (var ad in advancedSettings)
            {
                var keyCss = $"{ad.elementId}_css;";
                var keyCssDisplay = $"{ad.elementId}_css_display;";
                var css = new List<string>()
                {
                    $"font-weight:{(ad.cssValue?.isBold==true ? "bold" : "normal")}",
                    $"font-style:{(ad.cssValue?.isItalic==true ? "italic" : "normal")}",
                    $"font-size:{ad.cssValue?.fontSize}px",
                    $"color:{ad.cssValue?.color}",
                    $"text-align:{ad.cssValue?.align}"
                }.Join(";");
                html = html.Replace(keyCss, css);
                html = html.Replace(keyCssDisplay, ad.isDisplay ? "" : "display:none");
            }
            html = html.Replace("NaN", "");
            return new SuccessResult<string>(html);
        }
        private XElement RemoveNamespaces(XElement element)
        {
            // Tạo một XElement mới với tên cục bộ (bỏ namespace)
            var newElement = new XElement(element.Name.LocalName);

            // Sao chép các thuộc tính (bỏ thuộc tính liên quan đến namespace)
            newElement.Add(element.Attributes()
                .Where(attr => !attr.IsNamespaceDeclaration));

            // Sao chép các phần tử con và nội dung
            foreach (var node in element.Nodes())
            {
                if (node is XElement childElement)
                {
                    // Đệ quy để xử lý các phần tử con
                    newElement.Add(RemoveNamespaces(childElement));
                }
                else
                {
                    // Sao chép các node khác (text, comment, v.v.)
                    newElement.Add(node);
                }
            }

            return newElement;
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
        private async Task<string> GetXmlConntentFormTvanAsync(hoa_don hoaDon, Boolean isCoMa)
        {
            using (var client = Helper.WSInterTRCA2Helper.GetClient())
            {
                await client.OpenAsync();
                var authHeader = Helper.WSInterTRCA2Helper.GetAuthHeader();
                try
                {
                    string base64ResultString = string.Empty;

                    if (isCoMa)
                    {
                        var response = await client.GetXMLAsync(authHeader, $"{hoaDon.donvi_ma_dv}_{hoaDon.hoa_don_dang_ky_phat_hanh_mau_so}{hoaDon.hoa_don_dang_ky_phat_hanh_ky_hieu}_{hoaDon.ma_so_hoa_don}_000_");
                        base64ResultString = response.GetXMLResult.ConvertToString();
                    }
                    else
                    {
                        var response = await client.GetXML_MTTAsync(authHeader, $"{hoaDon.invoice_id}");
                        base64ResultString = response.GetXML_MTTResult.ConvertToString();
                    }
                    if (base64ResultString.Length > 2)
                    {
                        byte[] xmlBytes = Convert.FromBase64String(base64ResultString);

                        string xmlString = Encoding.UTF8.GetString(xmlBytes);
                        return xmlString;
                    }
                    return string.Empty;

                }
                catch (System.Exception ex)
                {
                    // LogWriter.Writer("GetXmlConntentFormTvanAsync", $"{}", "");
                    return string.Empty;
                }
                finally
                {
                    await client.CloseAsync();
                }
            }
        }
        private async Task<string> GetXmlFromTVANAsync(hoa_don hoaDon, List<hoa_don_log> logs)
        {
            var logCQTs = logs.Where(x => x.hoa_don_log_type_id == (int)e_hoa_don_log_type.CO_QUAN_THUE_CHAP_NHAN).ToList();
            foreach (var log in logCQTs)
            {
                var xmlPath = log.file_thong_diep_url;
                var thongDiep = await this.ReadXmlContentFromUrlAsync($"https://ca2einv.nacencomm.vn/{xmlPath}");
                var ketQuaThongDiepRespone = thongDiep.ConvertToObject<Model.Respone.Xml.KetQuaThongDiepRespone>();
                if (ketQuaThongDiepRespone?.TTChung?.MLTDiep == "202" ||
                    ketQuaThongDiepRespone?.TTChung?.MLTDiep == "204")
                {
                    string patternMTDTChieu = @"<MTDTChieu>(.*?)</MTDTChieu>";

                    var match = Regex.Match(thongDiep, patternMTDTChieu, RegexOptions.Singleline);
                    if (match.Success)
                    {
                        var MTDTChieu = match.Groups[1].Value;
                        if (MTDTChieu.ConvertToString() != "")
                        {
                            using (var client = Helper.WSInterTRCA2Helper.GetClient())
                            {
                                await client.OpenAsync();
                                var authHeader = Helper.WSInterTRCA2Helper.GetAuthHeader();
                                try
                                {
                                    var getXmlResultBase64 = await client.GetxmlThongdiepAsync(hoaDon.donvi_ma_dv, MTDTChieu);
                                    if (getXmlResultBase64.ConvertToString().Length > 2)
                                    {
                                        byte[] xmlBytes = Convert.FromBase64String(getXmlResultBase64);

                                        string xmlString = Encoding.UTF8.GetString(xmlBytes);
                                        return xmlString;
                                    }

                                }
                                catch (System.Exception ex)
                                {
                                    return string.Empty;
                                }
                                finally
                                {
                                    await client.CloseAsync();
                                }
                            }
                        }
                    }

                    break;
                }
            }
            return string.Empty;
        }
        public async Task<FunctionResult<string>> CreatePrintHtmlV1Async(hoa_don hoaDon, mau_hoa_don mauHoaDon, int soHoaDonTrenTrang = 10, MauHoaDonInChuyenDoiParam chuyenDoiParam = null)
        {
            var xsltContent = "";
            if (File.Exists(mauHoaDon.xslt_path))
            {
                xsltContent = await File.ReadAllTextAsync(mauHoaDon.xslt_path);
            }
            if (hoaDon.is_ky_so_succes == true)
            {
                xsltContent = xsltContent.Replace("paramSign", "display:normal");
            }
            else
            {
                xsltContent = xsltContent.Replace("paramSign", "display:none");
            }
            xsltContent = xsltContent.Replace("{paramtiengiam}", hoaDon.giam_thue_ghi_chu.ConvertToString());
            xsltContent = xsltContent.Replace("paramLogo", mauHoaDon.logo_path.ConvertToString());
            // .Replace("paramChuyendoi", chuyenDoiParam != null ? "display:normal" : "display:none")

            //    .Replace("paramNguoiCD", chuyenDoiParam != null ? "width:100%;text-align:center;display:normal" : "width:100%;text-align:center;display:none")
            xsltContent = xsltContent.Replace("paramMau", "display:none");
            xsltContent = xsltContent.Replace("paramNguoiCD", chuyenDoiParam != null ? "width:100%;text-align:center;display:normal" : "width:100%;text-align:center;display:none");
            xsltContent = xsltContent.Replace("paramChuyendoi", chuyenDoiParam != null ? "display:normal" : "display:none");
            xsltContent = xsltContent.Replace("hoadon78.nacencomm.vn", "ca2einvoice.nacencomm.vn");
            xsltContent = xsltContent.Replace("paramqrcode", $"https://api.qrserver.com/v1/create-qr-code/?size=100x100&amp;data={hoaDon.CreateQRCode()}");
            if (mauHoaDon.is_show_wattermark_inner_table == true)
            {

                xsltContent = xsltContent.Replace("viewstyle", "position:relative;width:900px;margin:auto; border:2px solid black; padding-top:20px;z-index:1;background-image: url(''); background-size:80%; background-position: center;width:900px;background-color: hsla(0,0%,100%,0.60);background-blend-mode: overlay;background-repeat:no-repeatwidth:900px;margin:auto; border:2px solid black; padding-top:20px;z-index:1;  background-image: url('" + mauHoaDon.watermark_path.ConvertToString() + "'); background-size:80%; background-position: center;width:900px;background-color: hsla(0,0%,100%,0.60);background-blend-mode: overlay;background-repeat:no-repeat");
                xsltContent = xsltContent.Replace("paramTableBG", "background-image: url('" + mauHoaDon.watermark_path.ConvertToString() + "'); background-size:cover; background-position: center;background-color: hsla(0,0%,100%,0.60);background-blend-mode: overlay;background-repeat:no-repeat");
            }
            else
            {
                xsltContent = xsltContent.Replace("viewstyle", "position:relative;width:900px;margin:auto; border:2px solid black; padding-top:20px;z-index:1;  background-image: url('" + mauHoaDon.watermark_path + "'); background-size:80%; background-position: center;width:900px;background-color: hsla(0,0%,100%,0.60);background-blend-mode: overlay;background-repeat:no-repeat");
                xsltContent = xsltContent.Replace("paramTableBG", "");

            }
            if (hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_DIEU_CHINH)
            {
                xsltContent = xsltContent.Replace("param1", "(Hóa đơn điều chỉnh)");
                xsltContent = xsltContent.Replace("param1_1", "normal");
                xsltContent = xsltContent.Replace("param2_2", "normal");
                xsltContent = xsltContent.Replace("param2", $"Hóa đơn điều chỉnh cho hóa đơn số {hoaDon.ma_so_hoa_don_goc}, mẫu số  {hoaDon.hoa_don_dang_ky_phat_hanh_mau_so_goc}, ký hiệu {hoaDon.hoa_don_dang_ky_phat_hanh_ky_hieu_goc}, ngày hóa đơn {(hoaDon.ngay_hoa_don_goc?.ToString("dd/MM/yyyy") ?? "")}");
            }
            if (hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_THAY_THE)
            {
                xsltContent = xsltContent.Replace("param1", "(Hóa đơn thay thế)");
                xsltContent = xsltContent.Replace("param1_1", "normal");
                xsltContent = xsltContent.Replace("param2_2", "normal");
                xsltContent = xsltContent.Replace("param2", $"Hóa đơn thay thế cho hóa đơn số {hoaDon.ma_so_hoa_don_goc}, mẫu số  {hoaDon.hoa_don_dang_ky_phat_hanh_mau_so_goc}, ký hiệu {hoaDon.hoa_don_dang_ky_phat_hanh_ky_hieu_goc}, ngày hóa đơn {(hoaDon.ngay_hoa_don_goc?.ToString("dd/MM/yyyy") ?? "")}");
            }
            if (hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_BI_DIEU_CHINH || hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_BI_THAY_THE)
            {
                xsltContent = xsltContent.Replace("param1", "");
                xsltContent = xsltContent.Replace("param1_1", "none");
                xsltContent = xsltContent.Replace("param2_2", "normal");
                xsltContent = xsltContent.Replace("param2", $"");
                xsltContent = xsltContent.Replace("contentDisable", hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_BI_DIEU_CHINH ? $"HÓA ĐƠN BỊ ĐIỀU CHỈNH" : "HÓA ĐƠN BỊ THAY THẾ");
                xsltContent = xsltContent.Replace("paramdisable", $"position:absolute;z-index:0; width:auto; height:70px; border:4px solid red;  background:transparent; display:block;top:45%;left:50%;transform: translate(-50%, -50%);color:red;font-size:25pt;font-weight:bold;text-align:center;padding-top:10px;opacity:0.5");
            }
            if (hoaDon.hoa_don_hinh_thuc_id != (int)e_hoa_don_hinh_thuc.HOA_DON_BI_DIEU_CHINH && hoaDon.hoa_don_hinh_thuc_id != (int)e_hoa_don_hinh_thuc.HOA_DON_BI_THAY_THE && hoaDon.hoa_don_hinh_thuc_id != (int)e_hoa_don_hinh_thuc.DA_GUI_TBSS_THAY_THE)
            {
                xsltContent = xsltContent.Replace("param1", "");
                xsltContent = xsltContent.Replace("param1_1", "none");
                xsltContent = xsltContent.Replace("param2_2", "normal");
                xsltContent = xsltContent.Replace("param2", $"");
                xsltContent = xsltContent.Replace("contentDisable", $"&#160;");
                xsltContent = xsltContent.Replace("paramdisable", $"position:absolute;z-index:0 ; width:300px; height:100px; border:3px solid red; background:transparent; display:none;  top:45%; left:40%; color:red;font-size:70pt;text-align:center;padding-top:10px;");
            }
            if (hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.DA_GUI_TBSS_THAY_THE)
            {
                xsltContent = xsltContent.Replace("param1", "");
                xsltContent = xsltContent.Replace("param1_1", "none");
                xsltContent = xsltContent.Replace("param2_2", "normal");
                xsltContent = xsltContent.Replace("param2", $"");
                xsltContent = xsltContent.Replace("contentDisable", "HÓA ĐƠN BỊ THAY THẾ");
                xsltContent = xsltContent.Replace("paramdisable", $"position:absolute;z-index:0; width:auto; height:70px; border:4px solid red;  background:transparent; display:block;top:45%;left:50%;transform: translate(-50%, -50%);color:red;font-size:25pt;font-weight:bold;text-align:center;padding-top:10px;opacity: 0.5");
            }

            var html = "";

            var xsltArgument = new XsltArgumentList();
            // xsltArgument.AddParam("paramlien", "", "0");
            XDocument doc = null;
            if (hoaDon.hoa_don_trang_thai_id != (int)e_hoa_don_trang_thai.DA_PHAT_HANH)
            {
                var xmlNhapResult = await _serviceWrapper.HoaDon.HoaDon.CreateXmlKySoAsync(hoaDon.id, true);
                if (xmlNhapResult.is_success)
                {
                    doc = XDocument.Parse(xmlNhapResult.data);
                }

            }
            else
            {
                var isCoMa = hoaDon.hoa_don_hinh_thuc_code == "C";
                var hoaDongLogs = await _serviceWrapper.HoaDon.HoaDonLog.SelectByHoaDonAsync(hoaDon.id);
                //var xmlDataFile = hoaDongLogs.Where(x => x.hoa_don_log_type_id == (isCoMa ? (int)e_hoa_don_log_type.CO_QUAN_THUE_CHAP_NHAN : (int)e_hoa_don_log_type.GUI_THONG_DIEP)).LastOrDefault();
                //     var xmlDataFile = hoaDongLogs.LastOrDefault(x =>
                //     isCoMa
                //     ? (x.hoa_don_log_type_id == (int)e_hoa_don_log_type.CO_QUAN_THUE_CHAP_NHAN && x.mltdiep == "202")
                //     : (x.hoa_don_log_type_id == (int)e_hoa_don_log_type.GUI_THONG_DIEP)
                // );

                hoa_don_log xmlDataFile = null;

                if (isCoMa)
                {
                    // BƯỚC 1: Tìm ưu tiên theo dữ liệu đã lưu trong DB (nhanh nhất)
                    xmlDataFile = hoaDongLogs.LastOrDefault(x =>
                        x.hoa_don_log_type_id == (int)e_hoa_don_log_type.CO_QUAN_THUE_CHAP_NHAN
                        && x.mltdiep == "202");

                    // BƯỚC 2: Nếu không tìm thấy trong DB, bắt đầu check File XML
                    if (xmlDataFile == null)
                    {
                        // Lấy danh sách các log CQT chấp nhận mà có đường dẫn file để kiểm tra
                        // Sắp xếp giảm dần theo ID hoặc thời gian để lấy bản ghi mới nhất trước (tương tự LastOrDefault)
                        var candidates = hoaDongLogs
                            .Where(x => x.hoa_don_log_type_id == (int)e_hoa_don_log_type.CO_QUAN_THUE_CHAP_NHAN
                                     && !string.IsNullOrEmpty(x.file_thong_diep_url))
                            .OrderByDescending(x => x.id) // Giả sử có trường ID hoặc NgayTao
                            .ToList();

                        foreach (var log in candidates)
                        {
                            try
                            {
                                if (File.Exists(log.file_thong_diep_url))
                                {
                                    // Đọc file XML
                                    var xDoc = XDocument.Load(log.file_thong_diep_url);

                                    // Tìm thẻ <MLTDiep> xem có phải 202 không
                                    // Cấu trúc: TDiep -> TTChung -> MLTDiep
                                    var mltDiepVal = xDoc.Descendants("MLTDiep").FirstOrDefault()?.Value;

                                    if (mltDiepVal == "202")
                                    {
                                        xmlDataFile = log;
                                        break; // Đã tìm thấy, thoát vòng lặp ngay
                                    }
                                }
                            }
                            catch
                            {
                                // Nếu lỗi đọc file (file lỗi, không quyền truy cập...) thì bỏ qua check file tiếp theo
                                continue;
                            }
                        }
                    }
                }
                else
                {
                    // Logic cũ cho trường hợp không có mã (Gửi thông điệp)
                    xmlDataFile = hoaDongLogs.LastOrDefault(x =>
                        x.hoa_don_log_type_id == (int)e_hoa_don_log_type.GUI_THONG_DIEP || x.hoa_don_log_type_id == (int)e_hoa_don_log_type.KY_SO_SUCCESS);
                }


                var xmlStringRoot = "";
                if (xmlDataFile != null)
                {
                    html = "";
                    if (!File.Exists(xmlDataFile.file_thong_diep_url))
                    {
                        var xml = await this.GetXmlFromTVANAsync(hoaDon, hoaDongLogs.ToList());
                        if (xml.ConvertToString() == "")
                        {
                            xml = await this.GetXmlConntentFormTvanAsync(hoaDon, isCoMa);
                        }
                        if (xml != string.Empty)
                        {
                            var filePath = xmlDataFile.file_thong_diep_url;
                            var directoryPath = Path.GetDirectoryName(filePath);
                            if (!Directory.Exists(directoryPath))
                            {
                                Directory.CreateDirectory(directoryPath);
                            }
                            await File.WriteAllTextAsync(filePath, xml);
                        }



                    }
                    if (!File.Exists(xmlDataFile.file_thong_diep_url))
                    {
                        return new FunctionResult<string>(false, "File XML không tồn tại");
                    }
                    //check empty
                    var xmlContent = await File.ReadAllTextAsync(xmlDataFile.file_thong_diep_url);
                    var isDocCreateedFormContent = false;
                    if (xmlContent.ConvertToString() == string.Empty)
                    {
                        // xmlContent = await this.GetXmlConntentFormTvanAsync($"{hoaDon.donvi_ma_dv}_{hoaDon.hoa_don_dang_ky_phat_hanh_mau_so}{hoaDon.hoa_don_dang_ky_phat_hanh_ky_hieu}_{hoaDon.ma_so_hoa_don}_000_");
                        xmlContent = await this.GetXmlConntentFormTvanAsync(hoaDon, isCoMa);
                        if (xmlContent != string.Empty)
                        {
                            var filePath = xmlDataFile.file_thong_diep_url;
                            var directoryPath = Path.GetDirectoryName(filePath);
                            try
                            {
                                if (!Directory.Exists(directoryPath))
                                {
                                    Directory.CreateDirectory(directoryPath);
                                }
                                await File.WriteAllTextAsync(filePath, xmlContent);
                            }
                            catch (System.Exception ex)
                            {
                                LogWriter.Writer("UpdateXmlContentFromTVan", $"{hoaDon.id}", "");
                                doc = XDocument.Parse(xmlContent);
                                isDocCreateedFormContent = true;
                            }
                        }
                    }
                    if (!isDocCreateedFormContent)
                        doc = XDocument.Load(xmlDataFile.file_thong_diep_url);



                }
            }
            if (doc == null) return new ErrorResult<string>();
            string xpath = "TDiep/DLieu/HDon/DLHDon/NDHDon/DSHHDVu";
            XElement hangHoasData = doc.XPathSelectElement(xpath);
            if (hangHoasData == null)
            {

                hangHoasData = doc.XPathSelectElement("HDon/DLHDon/NDHDon/DSHHDVu");
            }
            if (hoaDon.hoa_don_hinh_thuc_code == "M")
            {
                var hDonElement = doc.XPathSelectElement($"//HDon[DLHDon/@Id='_{hoaDon.id}']");
                if (hDonElement != null)
                {
                    // xpath = "DLHDon/HDon/DLHDon/NDHDon/DSHHDVu";
                    hangHoasData = hDonElement.XPathSelectElement("DLHDon/NDHDon/DSHHDVu");
                }

            }
            if (hangHoasData != null)
            {
                var hangHoas = hangHoasData.Elements("HHDVu");
                if (hangHoas.Count() <= 0)
                {
                    hangHoas = hangHoasData.Elements().Where(e => e.Name.LocalName == "HHDVu");
                }
                var soTrang = hangHoas.Count() / soHoaDonTrenTrang;
                if (soTrang * soHoaDonTrenTrang < hangHoas.Count()) soTrang += 1;
                if (soTrang <= 0) soTrang = 1;
                for (int trang = 0; trang < soTrang; trang++)
                {
                    var xsltContentTrang = xsltContent.Replace("paramlien", (trang + 1).ToString());
                    xsltContentTrang = xsltContentTrang.Replace("paramLien", (trang + 1).ToString());
                    xsltContentTrang = xsltContentTrang.Replace("param3", $"Trang {trang + 1}/{soTrang}");
                    xsltContentTrang = xsltContentTrang.Replace("paramSotrangdisplay", $"normal");
                    var xsltArgumentTrang = new XsltArgumentList();
                    if (trang == soTrang - 1)
                    {
                        //trang cuối
                        xsltContentTrang = xsltContentTrang.Replace("paramfooter", $"normal");
                    }
                    else
                    {
                        xsltContentTrang = xsltContentTrang.Replace("paramfooter", $"none");
                    }

                    // var hangHoasTrenTrang = hangHoas.Skip(trang * soHoaDonTrenTrang).Take(soHoaDonTrenTrang);
                    // XElement hangHoasDataTrenTrang = new XElement("DSHHDVu");
                    // foreach (var item in hangHoasTrenTrang)
                    // {
                    //     hangHoasDataTrenTrang.Add(new XElement(item));
                    // }
                    var docTrang = XDocument.Parse(doc.ToString());
                    // var docTrang = XDocument.Load(xmlDataFile.file_thong_diep_url);
                    var SignatureElementValueMTT = "";
                    if (hoaDon.hoa_don_hinh_thuc_code == "M")
                    {

                        // var hDonElement = docTrang.XPathSelectElement("TDiep/DLieu/HDon");
                        var hDonElement = docTrang.XPathSelectElement($"//HDon[DLHDon/@Id='_{hoaDon.id}']");
                        //lấy hDonElement theo invoice_id từ v1
                        if (hDonElement == null) hDonElement = docTrang.XPathSelectElement($"//HDon[DLHDon/@Id='_{hoaDon.invoice_id.ConvertToString().Split("_").FirstOrDefault()}']");
                        if (hDonElement != null)
                        {

                            var SignatureElement = docTrang.XPathSelectElement("/TDiep/CKSNNT/*[local-name()='Signature']");
                            docTrang = new XDocument(new XElement("DLHDon", hDonElement));
                            var xmlTest = docTrang.ToString();
                            if (SignatureElement != null)
                            {
                                XElement currentRoot = docTrang.Root;
                                currentRoot.Add(SignatureElement);
                                SignatureElementValueMTT = SignatureElement.Value;

                            }

                            // var parentElementM = docTrang.XPathSelectElement("DLHDon/HDon/DLHDon/NDHDon/DSHHDVu");
                            // if (parentElementM != null)
                            // {
                            //     parentElementM.ReplaceWith(hangHoasDataTrenTrang);
                            // }
                        }


                    }
                    // XElement parentElement = docTrang.XPathSelectElement(xpath);
                    // if (parentElement != null)
                    // {
                    //     parentElement.ReplaceWith(hangHoasDataTrenTrang);
                    // }



                    var xmlString = GetCompactXmlString(docTrang);
                    xmlString = xmlString.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    if (hoaDon.ngay_hoa_don >= new DateTime(2025, 6, 17))
                    {
                        xmlString = xmlString.Replace("Nghị quyết số 174/2024/QH15", "Nghị quyết số 204/2025/QH15");
                    }
                    xmlString = xmlString.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    var htmlTrang = await _serviceWrapper.HoaDon.LoaiHoaDonCTTemplate.GeneratePrintHtmlFromXsltContentAsyncV1(xsltContentTrang, xmlString, xsltArgumentTrang);
                    if (SignatureElementValueMTT != "")
                    {
                        htmlTrang = htmlTrang.Replace(SignatureElementValueMTT, "");
                    }

                    if (trang > 0)
                    {
                        htmlTrang = "<div class=\"page-break\"></div>" + htmlTrang;
                    }

                    html += htmlTrang;
                }


            }
            var css = @"
<style>
@media print {
    .page-break {
        page-break-before: always;
    }
}
</style>";

            html = css + html;

            html = html.Replace("NaN", "");
            return new SuccessResult<string>(html);
        }
        public async Task<FunctionResult<string>> CreatePrintHtmlAsync(hoa_don hoaDon, int soHoaDonTrenTrang = 10, MauHoaDonInChuyenDoiParam chuyenDoiParam = null)
        {

            var mauHoaDon = await this.SelectMauActiveByDonVAsync(hoaDon.donvi_ma_dv, hoaDon.loai_hoa_don_ct_id);
            if (mauHoaDon == null) return new ErrorResult<string>("Không có mẫu hóa đơn");
            // var loaiHoaDonCTTemplate = await _serviceWrapper.HoaDon.LoaiHoaDonCTTemplate.SelectByIdAsync(mauHoaDon.loai_hoa_don_ct_template_id);
            // if (loaiHoaDonCTTemplate == null) return new ErrorResult<string>("Không có template");
            if (mauHoaDon.xml_version.ConvertToInt() == 1)
            {
                return await this.CreatePrintHtmlV1Async(hoaDon, mauHoaDon, soHoaDonTrenTrang, chuyenDoiParam);
            }
            var html = "";
            var xsltArgument = new XsltArgumentList();
            xsltArgument.AddParam("paramlien", "", "0");

            if (hoaDon.hoa_don_trang_thai_id == (int)e_hoa_don_trang_thai.DA_PHAT_HANH || hoaDon.hoa_don_trang_thai_id == (int)e_hoa_don_trang_thai.CHUA_GUI_CQT)
            {
                var isCoMa = hoaDon.hoa_don_hinh_thuc_code == "C";
                var hoaDongLogs = await _serviceWrapper.HoaDon.HoaDonLog.SelectByHoaDonAsync(hoaDon.id);

                // var xmlDataFile = hoaDongLogs.LastOrDefault(x =>
                //     isCoMa
                //     ? (x.hoa_don_log_type_id == (int)e_hoa_don_log_type.CO_QUAN_THUE_CHAP_NHAN && x.mltdiep == "202")
                //     : (x.hoa_don_log_type_id == (int)e_hoa_don_log_type.GUI_THONG_DIEP)
                // );

                hoa_don_log xmlDataFile = null;

                if (isCoMa)
                {
                    // BƯỚC 1: Tìm ưu tiên theo dữ liệu đã lưu trong DB (nhanh nhất)
                    xmlDataFile = hoaDongLogs.LastOrDefault(x =>
                        x.hoa_don_log_type_id == (int)e_hoa_don_log_type.CO_QUAN_THUE_CHAP_NHAN
                        && x.mltdiep == "202");

                    // BƯỚC 2: Nếu không tìm thấy trong DB, bắt đầu check File XML
                    if (xmlDataFile == null)
                    {
                        // Lấy danh sách các log CQT chấp nhận mà có đường dẫn file để kiểm tra
                        // Sắp xếp giảm dần theo ID hoặc thời gian để lấy bản ghi mới nhất trước (tương tự LastOrDefault)
                        var candidates = hoaDongLogs
                            .Where(x => x.hoa_don_log_type_id == (int)e_hoa_don_log_type.CO_QUAN_THUE_CHAP_NHAN
                                     && !string.IsNullOrEmpty(x.file_thong_diep_url))
                            .OrderByDescending(x => x.id) // Giả sử có trường ID hoặc NgayTao
                            .ToList();

                        foreach (var log in candidates)
                        {
                            try
                            {
                                if (File.Exists(log.file_thong_diep_url))
                                {
                                    // Đọc file XML
                                    var xDoc = XDocument.Load(log.file_thong_diep_url);

                                    // Tìm thẻ <MLTDiep> xem có phải 202 không
                                    // Cấu trúc: TDiep -> TTChung -> MLTDiep
                                    var mltDiepVal = xDoc.Descendants("MLTDiep").FirstOrDefault()?.Value;

                                    if (mltDiepVal == "202")
                                    {
                                        xmlDataFile = log;
                                        break; // Đã tìm thấy, thoát vòng lặp ngay
                                    }
                                }
                            }
                            catch
                            {
                                // Nếu lỗi đọc file (file lỗi, không quyền truy cập...) thì bỏ qua check file tiếp theo
                                continue;
                            }
                        }
                    }
                }
                else
                {
                    // Logic cũ cho trường hợp không có mã (Gửi thông điệp)
                    xmlDataFile = hoaDongLogs.LastOrDefault(x =>
                  x.hoa_don_log_type_id == (int)e_hoa_don_log_type.GUI_THONG_DIEP || x.hoa_don_log_type_id == (int)e_hoa_don_log_type.KY_SO_SUCCESS);
                }


                var xmlStringRoot = "";

                if (xmlDataFile != null)
                {

                    // xmlStringRoot = File.ReadAllText(xmlDataFile.file_thong_diep_url);
                    // html = await _serviceWrapper.HoaDon.LoaiHoaDonCTTemplate.GeneratePrintHtmlAsync(loaiHoaDonCTTemplate.id, xmlStringRoot, xsltArgument);
                    html = "";
                    //1. kiểm tra số lượng hàng hóa -> tính số trang
                    //2. Mỗi trang tối đa 10 mặt hàng
                    //3. Trang cuối mới hiển thị tổng tiền
                    if (!File.Exists(xmlDataFile.file_thong_diep_url))
                    {
                        var xml = await this.GetXmlFromTVANAsync(hoaDon, hoaDongLogs.ToList());
                        if (xml.ConvertToString() == "")
                        {
                            xml = await this.GetXmlConntentFormTvanAsync(hoaDon, isCoMa);
                        }
                        if (xml != string.Empty)
                        {
                            var filePath = xmlDataFile.file_thong_diep_url;
                            var directoryPath = Path.GetDirectoryName(filePath);
                            if (!Directory.Exists(directoryPath))
                            {
                                Directory.CreateDirectory(directoryPath);
                            }
                            await File.WriteAllTextAsync(filePath, xml);
                        }

                    }
                    if (!File.Exists(xmlDataFile.file_thong_diep_url))
                    {
                        return new FunctionResult<string>(false, "File XML không tồn tại");
                    }
                    var doc = XDocument.Load(xmlDataFile.file_thong_diep_url);
                    if (hoaDon.hoa_don_hinh_thuc_code == "M")
                    {
                        // chỉ giữ lại thẻ <HDon></HDon>
                    }
                    // Tìm thẻ <DSHHDVu>
                    string xpath = "TDiep/DLieu/HDon/DLHDon/NDHDon/DSHHDVu";
                    XElement hangHoasData = doc.XPathSelectElement(xpath);
                    if (hangHoasData == null)
                    {

                        hangHoasData = doc.XPathSelectElement("HDon/DLHDon/NDHDon/DSHHDVu");
                    }
                    if (hoaDon.hoa_don_hinh_thuc_code == "M")
                    {
                        var hDonElement = doc.XPathSelectElement($"//HDon[DLHDon/@Id='_{hoaDon.id}']");
                        if (hDonElement != null)
                        {
                            // xpath = "DLHDon/HDon/DLHDon/NDHDon/DSHHDVu";
                            hangHoasData = hDonElement.XPathSelectElement("DLHDon/NDHDon/DSHHDVu");
                        }

                    }
                    if (hangHoasData != null)
                    {
                        var hangHoas = hangHoasData.Elements("HHDVu");
                        if (hangHoas.Count() <= 0)
                        {
                            hangHoas = hangHoasData.Elements().Where(e => e.Name.LocalName == "HHDVu");
                        }
                        var soTrang = hangHoas.Count() / soHoaDonTrenTrang;
                        if (soTrang * soHoaDonTrenTrang < hangHoas.Count()) soTrang += 1;

                        if (soTrang <= 0) soTrang = 1;


                        for (int trang = 0; trang < soTrang; trang++)
                        {
                            var xsltArgumentTrang = new XsltArgumentList();
                            xsltArgumentTrang.AddParam("paramlien", "", trang.ToString());
                            var hangHoasTrenTrang = hangHoas.Skip(trang * soHoaDonTrenTrang).Take(soHoaDonTrenTrang);
                            XElement hangHoasDataTrenTrang = new XElement("DSHHDVu");
                            foreach (var item in hangHoasTrenTrang)
                            {
                                hangHoasDataTrenTrang.Add(new XElement(item));
                            }
                            var docTrang = XDocument.Load(xmlDataFile.file_thong_diep_url);
                            var SignatureElementValueMTT = "";
                            if (hoaDon.hoa_don_hinh_thuc_code == "M")
                            {

                                // var hDonElement = docTrang.XPathSelectElement("TDiep/DLieu/HDon");
                                var hDonElement = docTrang.XPathSelectElement($"//HDon[DLHDon/@Id='_{hoaDon.id}']");
                                //lấy hDonElement theo invoice_id từ v1
                                if (hDonElement == null) hDonElement = docTrang.XPathSelectElement($"//HDon[DLHDon/@Id='_{hoaDon.invoice_id.ConvertToString().Split("_").FirstOrDefault()}']");
                                if (hDonElement != null)
                                {

                                    var SignatureElement = docTrang.XPathSelectElement("/TDiep/CKSNNT/*[local-name()='Signature']");
                                    docTrang = new XDocument(new XElement("DLHDon", hDonElement));
                                    var xmlTest = docTrang.ToString();
                                    if (SignatureElement != null)
                                    {
                                        XElement currentRoot = docTrang.Root;
                                        currentRoot.Add(SignatureElement);
                                        SignatureElementValueMTT = SignatureElement.Value;
                                        // SignatureElement.Descendants().Where(x => x.Name.LocalName == "DigestValue").Remove();
                                        // SignatureElement.Descendants().Where(x => x.Name.LocalName == "SignatureValue").Remove();
                                        // SignatureElement.Descendants().Where(x => x.Name.LocalName == "X509Certificate").Remove();
                                        // SignatureElement.Descendants("DigestValue");
                                        // XElement signatureWithoutNamespace = RemoveNamespaces(SignatureElement);
                                        // currentRoot.Add(signatureWithoutNamespace);
                                    }

                                    var parentElementM = docTrang.XPathSelectElement("DLHDon/HDon/DLHDon/NDHDon/DSHHDVu");
                                    if (parentElementM != null)
                                    {
                                        parentElementM.ReplaceWith(hangHoasDataTrenTrang);
                                    }
                                }


                            }
                            XElement parentElement = docTrang.XPathSelectElement(xpath);
                            if (parentElement != null)
                            {
                                parentElement.ReplaceWith(hangHoasDataTrenTrang);
                            }



                            var xmlString = GetCompactXmlString(docTrang);
                            xmlString = xmlString.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                            if (hoaDon.ngay_hoa_don >= new DateTime(2025, 6, 17))
                            {
                                xmlString = xmlString.Replace("Nghị quyết số 174/2024/QH15", "Nghị quyết số 204/2025/QH15");
                            }
                            xmlString = xmlString.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                            var htmlTrang = await _serviceWrapper.HoaDon.LoaiHoaDonCTTemplate.GeneratePrintHtmlAsync(mauHoaDon, xmlString, xsltArgumentTrang);
                            if (SignatureElementValueMTT != "")
                            {
                                htmlTrang = htmlTrang.Replace(SignatureElementValueMTT, "");
                            }
                            if (trang != soTrang - 1)
                            {
                                htmlTrang = htmlTrang.Replace("paramfooter", "none");
                            }
                            htmlTrang = htmlTrang.Replace("param3", $"Trang {trang + 1}/{soTrang}");
                            if (trang > 0)
                            {
                                htmlTrang = "<div class=\"page-break\"></div>" + htmlTrang;
                            }
                            // if (hoaDon.hoa_don_hinh_thuc_code == "M")
                            // {
                            //     htmlTrang = htmlTrang.Replace("paramSign", "display:none");
                            // }
                            html += htmlTrang;
                        }
                    }

                }

            }
            else
            {
                var hangHoas = await _serviceWrapper.HoaDon.HoaDonHangHoa.SelectByHoaDonIdAsync(hoaDon.id);
                var donVi = await this.GetCurrentDonViAsync();
                var hoaDonData = new MauHoaDonCreateHtmlInput();
                hoaDonData.hoa_don = new Model.Request.Xml.HoaDon();
                hoaDonData.hoa_don.du_lieu_hoa_don.thong_tin_chung = new ThongTinChung()
                {
                    ngay_lap = hoaDon.ngay_hoa_don.ToString("yyyy-MM-dd"),
                    ky_hieu_mau_so_hoa_don = hoaDon.hoa_don_dang_ky_phat_hanh_mau_so,
                    don_vi_tien_te = hoaDon.loai_tien,
                    hinh_thuc_thanh_toan = hoaDon.hinh_thuc_tt,
                    ky_hieu_hoa_don = hoaDon.hoa_don_dang_ky_phat_hanh_ky_hieu,
                    so_hoa_don = hoaDon.ma_so_hoa_don.ToString(),
                    ten_hoa_don = hoaDon.ten_hoa_don
                };
                if (hoaDon.giam_thue_ghi_chu.ConvertToString() != "")
                {
                    if (hoaDonData.hoa_don.du_lieu_hoa_don.thong_tin_chung.thong_tin_khac == null)
                    {
                        hoaDonData.hoa_don.du_lieu_hoa_don.thong_tin_chung.thong_tin_khac = new ThongTinKhac();
                        hoaDonData.hoa_don.du_lieu_hoa_don.thong_tin_chung.thong_tin_khac.thong_tin_khac_noi_dung = new List<ThongTinKhacNoiDung>();
                    }
                    if (hoaDonData.hoa_don.du_lieu_hoa_don.thong_tin_chung.thong_tin_khac.thong_tin_khac_noi_dung == null)
                    {
                        hoaDonData.hoa_don.du_lieu_hoa_don.thong_tin_chung.thong_tin_khac.thong_tin_khac_noi_dung = new List<ThongTinKhacNoiDung>();
                    }
                    hoaDonData.hoa_don.du_lieu_hoa_don.thong_tin_chung.thong_tin_khac.thong_tin_khac_noi_dung.Add(new ThongTinKhacNoiDung()
                    {
                        thong_tin_truong = "GhiChu",
                        kieu_du_lieu = "string",
                        du_lieu = hoaDon.giam_thue_ghi_chu.ConvertToString(),
                    });

                }
                hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban = new NguoiBan()
                {
                    dia_chi = hoaDon.nguoi_ban_dia_chi,
                    mst = hoaDon.nguoi_ban_mst,
                    ten_nguoi_ban = hoaDon.nguoi_ban_ten_donvi,
                    dien_thoai = hoaDon.nguoi_ban_dien_thoai,
                    fax = hoaDon.nguoi_ban_fax,
                    ngan_hang = hoaDon.nguoi_ban_ngan_hang,
                    stk = hoaDon.nguoi_ban_stk,
                    website = hoaDon.nguoi_ban_website

                };
                hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_mua = new NguoiMua()
                {
                    dia_chi = hoaDon.nguoi_mua_dia_chi,
                    mst = hoaDon.nguoi_mua_mst,
                    ten_don_vi = hoaDon.nguoi_mua_ten_donvi,
                    ho_ten_nguoi_mua_hang = hoaDon.nguoi_mua_ten,
                    dien_thoai = hoaDon.nguoi_mua_dien_thoai,
                    fax = hoaDon.nguoi_mua_fax,
                    ngan_hang = hoaDon.nguoi_mua_ngan_hang,
                    stk = hoaDon.nguoi_mua_stk,
                    website = hoaDon.nguoi_mua_website,


                };
                hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.danh_sach_hang_hoa_dich_vu = new DanhSachHangHoaDichVu();

                hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.danh_sach_hang_hoa_dich_vu.hang_hoa_dich_vus =
                hangHoas.Select(x => new HangHoaDichVu()
                {
                    don_gia = x.don_gia.ConvertToStringAndRemoveZeroPart(),
                    don_vi_tinh = x.dvt,
                    ma_hang_hoa_dich_vu = x.ma_hang,
                    so_luong = x.so_luong.ConvertToStringAndRemoveZeroPart(),
                    stt = x.stt.ToString(),
                    ten_hang_hoa_dich_vu = x.ten_hang,
                    thanh_tien = x.thanh_tien.ConvertToStringAndRemoveZeroPart(),
                    thue_suat = x.thue_vat,
                    tinh_chat = x.hang_hoa_tinh_chat_id,
                    ty_le_chiet_khau = x.ty_le_chiet_khau.ConvertToStringAndRemoveZeroPart(),
                    so_tien_chiet_khau = x.tien_chiet_khau.ConvertToStringAndRemoveZeroPart()
                }).ToList();
                var thue_suats = hangHoas.Select(x => x.thue_vat).Distinct().Where(x => x.Contains("%")).ToList().Select(x => new LTSuat()
                {
                    ten_thue_suat = x
                }).ToList();
                foreach (var thue_suat in thue_suats)
                {
                    var phanTramThue = thue_suat.ten_thue_suat.Replace("KHAC:", "").Replace("%", "").Trim().ConvertToDouble(2);
                    var thanh_tien = hangHoas.Where(x => x.thue_vat == thue_suat.ten_thue_suat).Select(x => x.thanh_tien).Sum();
                    thue_suat.thanh_tien = thanh_tien.ConvertToStringAndRemoveZeroPart();
                    thue_suat.tien_thue = ((double)thanh_tien * phanTramThue / 100).ConvertToDouble(0).ConvertToDecimal().ConvertToStringAndRemoveZeroPart();
                }
                hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.thong_tin_thanh_toan = new ThongTinThanhToan()
                {
                    tong_tien_thue = hoaDon.tong_tien_thue.ConvertToStringAndRemoveZeroPart(),
                    tong_tien_chua_thue = hoaDon.tong_tien_truong_thue.ConvertToStringAndRemoveZeroPart(),
                    tong_tien_thanh_toan_bang_chu = await hoaDon.tong_tien_thanh_toan.ConvertToTextAsync(
                         hoaDon.loai_tien.ConvertToString() != "" ? hoaDon.loai_tien.ConvertToString() : "VND"
                    ),
                    tong_tien_thanh_toan_bang_so = hoaDon.tong_tien_thanh_toan.ConvertToStringAndRemoveZeroPart(),
                    tong_tien_chiet_khau = hoaDon.tong_tien_chiet_khau.ConvertToStringAndRemoveZeroPart(),
                    thong_tin_thue_suat = new THTTLTSuat()
                    {
                        thue_suats = thue_suats
                    }

                };
                if (hoaDon.hoa_don_dang_ky_phat_hanh_mau_so.ConvertToString() == "7")
                {
                    hoaDonData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.thong_tin_thanh_toan = new Model.Request.Xml.ThongTinThanhToan()
                    {
                        tong_tien_thanh_toan_bang_chu = "",
                        tong_tien_thanh_toan_bang_so = hoaDon.tong_tien_thanh_toan.ConvertToStringAndRemoveZeroPart(),

                    };
                }
                if (hoaDon.hoa_don_dang_ky_phat_hanh_ky_hieu_goc.ConvertToString() != "" && hoaDon.ngay_hoa_don_goc.HasValue)
                {
                    // var hoaDonGoc = await this.SelectByIdAsync(obj.hoa_don_id_goc);
                    hoaDonData.hoa_don.du_lieu_hoa_don.thong_tin_chung.thong_tin_lien_quan = new ThongTinLienQuan()
                    {
                        KHHDCLQuan = hoaDon.hoa_don_dang_ky_phat_hanh_ky_hieu_goc,
                        KHMSHDCLQuan = hoaDon.hoa_don_dang_ky_phat_hanh_mau_so_goc,
                        LHDCLQuan = hoaDon.hoa_don_nghi_dinh_id_goc == 123 ? "1" : "3",
                        NLHDCLQuan = hoaDon.ngay_hoa_don_goc.HasValue ? hoaDon.ngay_hoa_don_goc.Value.ToString("yyyy-MM-dd") : null,
                        SHDCLQuan = hoaDon.ma_so_hoa_don_goc.ToString(),
                        TCHDon = hoaDon.hoa_don_hinh_thuc_id == 3 ? "2" : "1",
                    };
                }
                // html = await _serviceWrapper.HoaDon.LoaiHoaDonCTTemplate.GeneratePrintHtmlAsync(loaiHoaDonCTTemplate.id, hoaDonData, xsltArgument);
                html = await _serviceWrapper.HoaDon.LoaiHoaDonCTTemplate.GeneratePrintHtmlAsync(mauHoaDon, hoaDonData, xsltArgument);
            }
            // var bgstyle = "width:900px;margin:auto; border:2px solid black; padding-top:20px;z-index:1;position: relative;";
            var bgstyle = "margin:auto; border:2px solid black; padding-top:0px;z-index:1;position: relative;";
            bgstyle = bgstyle + "background-image: url('{paramWaterMark}'); background-size:80%; background-position: center;background-color: hsla(0,0%,100%,paramOpacity;);background-blend-mode: overlay;background-repeat:no-repeat";
            var noidungdisabled = "&#160;";
            var styledisabled = "position:absolute;z-index:0;width:300px;height:140px;border:5px solid red;background:transparent;display:none;top:45%;left:40%;color:red;font-size:70pt;text-align:center;padding-top:10px;";
            var stylemau = $"position:absolute;z-index:0;width:300px;height:140px;border:5px solid red;background:transparent;display:none;top:45%;left:40%;color:red;font-size:70pt;text-align:center;padding-top:10px;";
            var paramsubtitle = "none";
            var paramSubtitleDiv = "none";
            var paramsubtitlecontent = String.Empty;
            var paramSubtitleContentDiv = "&#160;";
            if (hoaDon.hoa_don_trang_thai_id == (int)e_hoa_don_trang_thai.DA_HUY)
            {
                string pattern = @"<div\s+[^>]*style=['""]paramMau['""]\s*>\s*(.*?)\s*</div>";
                string replacement = @"<div id='background' style='paramMau'>
            <div style='padding:10px;border:3px solid red;font-weight:bold;transform: rotate(-25deg);margin-top: -50px;'> HÓA ĐƠN ĐÃ HỦY</div>
            </div>";
                html = Regex.Replace(html, pattern, replacement, RegexOptions.IgnoreCase);
                stylemau = "position:absolute;z-index:0;width:100%;height:100%;background:transparent;display:flex;justify-content:center;align-items:center;color:red;font-size:25px;text-align:center;";
            }
            if (hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_BI_THAY_THE)
            {
                string pattern = @"<div\s+[^>]*style=['""]paramMau['""]\s*>\s*(.*?)\s*</div>";
                string replacement = @"<div id='background' style='paramMau'>
            <div style='padding:10px;border:3px solid red;font-weight:bold;transform: rotate(-25deg);margin-top: -50px;opacity:0.5'> ĐÃ BỊ THAY THẾ</div>
            </div>";
                html = Regex.Replace(html, pattern, replacement, RegexOptions.IgnoreCase);
                stylemau = "position:absolute;z-index:0;width:100%;height:100%;background:transparent;display:flex;justify-content:center;align-items:center;color:red;font-size:25px;text-align:center;";
            }
            if (hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_BI_DIEU_CHINH)
            {
                string pattern = @"<div\s+[^>]*style=['""]paramMau['""]\s*>\s*(.*?)\s*</div>";
                string replacement = @"<div id='background' style='paramMau'>
            <div style='padding:10px;border:3px solid red;font-weight:bold;transform: rotate(-25deg);margin-top: -50px;opacity:0.5'> ĐÃ BỊ ĐIỀU CHỈNH</div>
            </div>";
                html = Regex.Replace(html, pattern, replacement, RegexOptions.IgnoreCase);
                stylemau = "position:absolute;z-index:0;width:100%;height:100%;background:transparent;display:flex;justify-content:center;align-items:center;color:red;font-size:25px;text-align:center;";
            }

            html = html.Replace("viewstyle", bgstyle)
            .Replace("paramLogo", "{paramLogo}")
            .Replace("min-height: 100%;background-image:url(paramVien);", "background-image:url(paramVien);")
            .Replace("paramVien", "{paramVien}")
           .Replace("paramChuyendoi", chuyenDoiParam != null ? "display:normal" : "display:none")
           .Replace("paramSign", "display:normal")
           .Replace("paramMau", stylemau)
           .Replace("paramNguoiCD", chuyenDoiParam != null ? "width:100%;text-align:center;display:normal" : "width:100%;text-align:center;display:none")
           .Replace("paramdisable", styledisabled).Replace("contentDisable", noidungdisabled)
           .Replace("param1_1", paramsubtitle)
           .Replace("param1", paramsubtitlecontent)
           .Replace("param2_2", paramSubtitleDiv)
           .Replace("param2", paramSubtitleContentDiv)
           .Replace("paramdisable", styledisabled)
           .Replace("contentDisable", noidungdisabled)
           .Replace("paramlien", "0").Replace("paramdisplay", "display:none")
           .Replace("paramqrcode", $"https://api.qrserver.com/v1/create-qr-code/?size=100x100&amp;data={hoaDon.CreateQRCode()}");


            string transparentImg = "data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7";

            string logoSrc = !string.IsNullOrEmpty(mauHoaDon.logo_path?.ToString())
             ? mauHoaDon.logo_path.ConvertToString().Replace('\\', '/')
             : transparentImg; // <-- Nếu rỗng thì dùng ảnh trong suốt

            if (mauHoaDon.is_show_wattermark_inner_table == true)
            {
                html = html.Replace("{paramLogo}", logoSrc)
                                .Replace("paramWaterMarkTable;", mauHoaDon.watermark_path.ConvertToString().Replace('\\', '/') ?? "");
            }
            else
            {
                html = html.Replace("{paramLogo}", logoSrc)
                                .Replace("{paramWaterMark}", mauHoaDon.watermark_path?.ConvertToString().Replace('\\', '/') ?? "");
            }
            if (mauHoaDon.vien_path.ConvertToString() != "")
            {
                html = html.Replace("{paramVien}", mauHoaDon.vien_path?.ConvertToString().Replace('\\', '/') ?? "");

            }
            if (mauHoaDon.logo_position.ConvertToString() == "right")
            {
                html = html.Replace("paramOpacityHeaderFlexDirection;", "row-reverse");
            }
            var paramOpacity = (1 - (mauHoaDon.watermark_opacity * 1.0 / 100).ConvertToDouble(2)).ToString().Replace(",", ".");
            html = html.Replace("paramOpacity;", paramOpacity);
            html = html.Replace("paramOpacity;", paramOpacity);
            html = html.Replace("12pt", "12px");
            html = html.Replace("<table style=\"width:100%;line-height:25px;font-size:12pt\">", "<table style=\"width:100%;line-height:20px;font-size:12px\">");
            html = html.Replace("line-height:25px", "line-height:20px");
            var advancedSettings = mauHoaDon.advanced_settings_json.ConvertToString().TryDeserializeObject<CssEditorElementData[]>();
            foreach (var ad in advancedSettings)
            {
                var keyCss = $"{ad.elementId}_css;";
                var keyCssDisplay = $"{ad.elementId}_css_display;";
                var css = new List<string>()
                {
                    $"font-weight:{(ad.cssValue?.isBold==true ? "bold" : "normal")}",
                    $"font-style:{(ad.cssValue?.isItalic==true ? "italic" : "normal")}",
                    $"font-size:{ad.cssValue?.fontSize}px",
                    $"color:{ad.cssValue?.color}",
                    $"text-align:{ad.cssValue?.align}"
                }.Join(";");
                html = html.Replace(keyCss, css);
                html = html.Replace(keyCssDisplay, ad.isDisplay ? "" : "display:none");
            }
            html = html.Replace("NaN", "");
            return new SuccessResult<string>(html);
        }

        public async Task<MauHoaDonCreateHtmlInput> CreateSampleData(mau_hoa_don mauHoaDon)
        {
            var donVi = await this.GetCurrentDonViAsync();
            var sampleData = new MauHoaDonCreateHtmlInput();
            var loaiHoaDonCT = await _repositoryWrapper.HoaDon.LoaiHoaDonCTTemplate.SelectVmByIdAsync(mauHoaDon.loai_hoa_don_ct_template_id);
            sampleData.hoa_don = new Model.Request.Xml.HoaDon();
            sampleData.hoa_don.du_lieu_hoa_don.thong_tin_chung.ngay_lap = DateTime.Now.ToString("yyyy-MM-dd");
            sampleData.hoa_don.du_lieu_hoa_don.thong_tin_chung.ten_hoa_don = loaiHoaDonCT?.loai_hoa_don_ct_name ?? "";

            var dSHoaDonDangKyPhatHang = await _serviceWrapper.HoaDon.HoaDonDangKyPhatHanh.SelectByDonViAsync(donVi.ma_dv);

            var hoaDonDangKyPhatHanh = dSHoaDonDangKyPhatHang
              .Where(x => x.loai_hoa_don_ct_id == loaiHoaDonCT.loai_hoa_don_ct_id).LastOrDefault();

            sampleData.hoa_don.du_lieu_hoa_don.thong_tin_chung.ky_hieu_mau_so_hoa_don = hoaDonDangKyPhatHanh.mau_so;
            sampleData.hoa_don.du_lieu_hoa_don.thong_tin_chung.ky_hieu_hoa_don = hoaDonDangKyPhatHanh.ky_hieu;



            sampleData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_ban = new NguoiBan()
            {
                dia_chi = donVi.dia_chi,
                mst = donVi.mst,
                ten_nguoi_ban = donVi.ten_dv,
                dien_thoai = donVi.dien_thoai,
                fax = donVi.fax,
                ngan_hang = donVi.ngan_hang,
                stk = donVi.stk,
                website = donVi.website

            };
            sampleData.hoa_don.du_lieu_hoa_don.noi_dung_hoa_don.nguoi_mua = new NguoiMua()
            {
                dia_chi = "Số 1, Đường A, Tỉnh B",
                mst = "12312313",
                ten_don_vi = "Công ty A",
                ho_ten_nguoi_mua_hang = "Nguyễn Văn A",
                dien_thoai = "0922888999",
                fax = "ABCXYZ",
                ngan_hang = "Ngân hàng ABC",
                stk = "1425334134",
                website = "congtya.com.vn",
                cccd = "123456789012",
                ma_dv_ngan_sach = "ABCCC"

            };
            return sampleData;
        }

        public async Task<IEnumerable<mau_hoa_don_vm>> SelectByDonViAsync(string donvi_ma_dv)
        {
            return await _repositoryWrapper.HoaDon.MauHoaDon.SelectByDonViAsync(donvi_ma_dv);
        }

        public async Task<mau_hoa_don_vm> SelectMauActiveByDonVAsync(string donvi_ma_dv, int loai_hoa_don_ct_id)
        {
            var list = await this.SelectByDonViAsync(donvi_ma_dv);
            return list.Where(x => x.is_active && x.loai_hoa_don_ct_id == loai_hoa_don_ct_id).FirstOrDefault();
        }

        public async Task<FunctionResult<string>> CreatePreviewHtmlAsync(int hoaDonId, bool isShowMau = true)
        {
            var hoaDon = await _serviceWrapper.HoaDon.HoaDon.SelectByIdAsync(hoaDonId);
            if (hoaDon == null) return new ErrorResult<string>("Không tìm thấy hóa đơn");
            var mauHoaDon = await this.SelectMauActiveByDonVAsync(hoaDon.donvi_ma_dv, hoaDon.loai_hoa_don_ct_id);
            if (mauHoaDon == null) return new ErrorResult<string>("Không có mẫu hóa đơn");
            if (mauHoaDon.xml_version.ConvertToInt() == 1)
            {
                return await this.CreatePreviewHtmlV1Async(mauHoaDon, hoaDon);
            }
            else
            {
                var getXmlKySoPreview = await _serviceWrapper.HoaDon.HoaDon.CreateXmlKySoAsync(hoaDon.id, true);
                if (!getXmlKySoPreview.is_success) return new ErrorResult<string>(getXmlKySoPreview.message, null);
                var html = await _serviceWrapper.HoaDon.LoaiHoaDonCTTemplate.GeneratePrintHtmlAsync(mauHoaDon, getXmlKySoPreview.data, new XsltArgumentList());
                var bgstyle = "margin:auto; border:2px solid black; padding-top:0px;z-index:1;position: relative;";
                bgstyle = bgstyle + "background-image: url('{paramWaterMark}'); background-size:80%; background-position: center;background-color: hsla(0,0%,100%,paramOpacity;);background-blend-mode: overlay;background-repeat:no-repeat";
                var noidungdisabled = "&#160;";
                var styledisabled = "position:absolute;z-index:0;width:300px;height:140px;border:5px solid red;background:transparent;display:none;top:45%;left:40%;color:red;font-size:70pt;text-align:center;padding-top:10px;";
                var stylemau = "position:absolute;z-index:0;width:300px;height:140px;border:5px solid red;background:transparent;top:45%;left:40%;color:red;font-size:70pt;text-align:center;padding-top:10px;";
                if (!isShowMau)
                {
                    styledisabled = "position:absolute;z-index:0;width:300px;height:140px;border:5px solid red;background:transparent;display:none;top:45%;left:40%;color:red;font-size:70pt;text-align:center;padding-top:10px;";
                    stylemau = "position:absolute;z-index:0;width:300px;height:140px;border:5px solid red;background:transparent;display:none;top:45%;left:40%;color:red;font-size:70pt;text-align:center;padding-top:10px;";
                    html = html.Replace("paramSign", "display:none");
                }
                var paramsubtitle = "normal";
                // var paramsubtitle = "none";
                var paramSubtitleDiv = "none";
                //var paramsubtitlecontent = hoaDon.ngay_hoa_don.ToNgayThangNamText();
                var paramsubtitlecontent = "";
                var paramSubtitleContentDiv = "&#160;";


                html = html.Replace("viewstyle", bgstyle).Replace("paramLogo", "{paramLogo}")
               .Replace("paramChuyendoi", "display:none")
               .Replace("paramMau", stylemau)
               .Replace("min-height: 100%;background-image:url(paramVien);", "background-image:url(paramVien);")
               .Replace("paramNguoiCD", "width:100%;text-align:center;display:none")
               .Replace("paramdisable", styledisabled).Replace("contentDisable", noidungdisabled)
               .Replace("param1_1", paramsubtitle)
               .Replace("param1", paramsubtitlecontent)
               .Replace("param2_2", paramSubtitleDiv)
               .Replace("param2", paramSubtitleContentDiv)
               .Replace("paramdisable", styledisabled)
               .Replace("contentDisable", noidungdisabled)
               .Replace("paramlien", "0").Replace("paramdisplay", "display:none")
               .Replace("paramqrcode", $"https://api.qrserver.com/v1/create-qr-code/?size=100x100&amp;data={hoaDon.CreateQRCode()}");


                string transparentImg = "data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7";

                string logoSrc = !string.IsNullOrEmpty(mauHoaDon.logo_path?.ToString())
                 ? mauHoaDon.logo_path.ConvertToString().Replace('\\', '/')
                 : transparentImg; // <-- Nếu rỗng thì dùng ảnh trong suốt

                if (mauHoaDon.is_show_wattermark_inner_table == true)
                {
                    html = html.Replace("{paramLogo}", logoSrc)
                               .Replace("paramWaterMarkTable;", mauHoaDon.watermark_path.ConvertToString().Replace('\\', '/') ?? "");
                }
                else
                {
                    html = html.Replace("{paramLogo}", logoSrc)
                        .Replace("{paramWaterMark}", mauHoaDon.watermark_path?.ConvertToString().Replace('\\', '/') ?? "");
                }
                if (mauHoaDon.vien_path.ConvertToString() != "")
                {
                    html = html.Replace("{paramVien}", mauHoaDon.vien_path?.ConvertToString().Replace('\\', '/') ?? "");
                    html = html.Replace("paramVien", mauHoaDon.vien_path?.ConvertToString().Replace('\\', '/') ?? "");

                }
                if (mauHoaDon.logo_position.ConvertToString() == "right")
                {
                    html = html.Replace("paramOpacityHeaderFlexDirection;", "row-reverse");
                }
                var paramOpacity = (1 - (mauHoaDon.watermark_opacity * 1.0 / 100).ConvertToDouble(2)).ToString().Replace(",", ".");
                html = html.Replace("paramOpacity;", paramOpacity);
                html = html.Replace("paramOpacity;", paramOpacity);
                var advancedSettings = mauHoaDon.advanced_settings_json.ConvertToString().TryDeserializeObject<CssEditorElementData[]>();
                html = html.Replace("12pt", "12px");
                html = html.Replace("<table style=\"width:100%;line-height:25px;font-size:12pt\">", "<table style=\"width:100%;line-height:20px;font-size:12px\">");
                html = html.Replace("line-height:25px", "line-height:20px");
                foreach (var ad in advancedSettings)
                {
                    var keyCss = $"{ad.elementId}_css;";
                    var keyCssDisplay = $"{ad.elementId}_css_display;";
                    var css = new List<string>()
                {
                    $"font-weight:{(ad.cssValue?.isBold==true ? "bold" : "normal")}",
                    $"font-style:{(ad.cssValue?.isItalic==true ? "italic" : "normal")}",
                    $"font-size:{ad.cssValue?.fontSize}px",
                    $"color:{ad.cssValue?.color}",
                    $"text-align:{ad.cssValue?.align}"
                }.Join(";");
                    html = html.Replace(keyCss, css);
                    html = html.Replace(keyCssDisplay, ad.isDisplay ? "" : "display:none");
                }
                html = html.Replace("NaN", "");
                return new SuccessResult<string>(html);
            }
        }
        private async Task<FunctionResult<string>> CreatePreviewHtmlV1Async(mau_hoa_don mauHoaDon, hoa_don hoaDon)
        {
            var xsltContent = "";
            if (File.Exists(mauHoaDon.xslt_path))
            {
                xsltContent = await File.ReadAllTextAsync(mauHoaDon.xslt_path);
            }
            if (hoaDon.is_ky_so_succes == true)
            {
                xsltContent = xsltContent.Replace("paramSign", "display:normal");
            }
            else
            {
                xsltContent = xsltContent.Replace("paramSign", "display:none");
            }
            xsltContent = xsltContent.Replace("{paramtiengiam}", hoaDon.giam_thue_ghi_chu.ConvertToString());
            xsltContent = xsltContent.Replace("paramLogo", mauHoaDon.logo_path.ConvertToString());
            xsltContent = xsltContent.Replace("paramMau", "display:none");
            xsltContent = xsltContent.Replace("paramNguoiCD", "display:none");
            xsltContent = xsltContent.Replace("paramChuyendoi", "display:none");
            xsltContent = xsltContent.Replace("https://hoadon78.nacencomm.vn/UploadIMG/bg", "https://ca2einvoice.nacencomm.vn/Upload");
            xsltContent = xsltContent.Replace("https://hoadon78.nacencomm.vn/UploadIMG/logo", "https://ca2einvoice.nacencomm.vn/Upload");
            // xsltContent = xsltContent.Replace("hoadon78.nacencomm.vn", "ca2einvoice.nacencomm.vn");
            var urlQrCode = $"https://api.qrserver.com/v1/create-qr-code/?size=100x100&amp;data={hoaDon.CreateQRCode()}";
            xsltContent = xsltContent.Replace("paramqrcode", urlQrCode);

            if (mauHoaDon.is_show_wattermark_inner_table == true)
            {

                xsltContent = xsltContent.Replace("viewstyle", "position:relative;width:900px;margin:auto; border:2px solid black; padding-top:20px;z-index:1;background-image: url(''); background-size:80%; background-position: center;width:900px;background-color: hsla(0,0%,100%,0.60);background-blend-mode: overlay;background-repeat:no-repeatwidth:900px;margin:auto; border:2px solid black; padding-top:20px;z-index:1;  background-image: url('" + mauHoaDon.watermark_path.ConvertToString() + "'); background-size:80%; background-position: center;width:900px;background-color: hsla(0,0%,100%,0.60);background-blend-mode: overlay;background-repeat:no-repeat");
                xsltContent = xsltContent.Replace("paramTableBG", "background-image: url('" + mauHoaDon.watermark_path.ConvertToString() + "'); background-size:cover; background-position: center;background-color: hsla(0,0%,100%,0.60);background-blend-mode: overlay;background-repeat:no-repeat");
            }
            else
            {
                xsltContent = xsltContent.Replace("viewstyle", "position:relative;width:900px;margin:auto; border:2px solid black; padding-top:20px;z-index:1;  background-image: url('" + mauHoaDon.watermark_path + "'); background-size:80%; background-position: center;width:900px;background-color: hsla(0,0%,100%,0.60);background-blend-mode: overlay;background-repeat:no-repeat");
                xsltContent = xsltContent.Replace("paramTableBG", "");

            }
            if (hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_DIEU_CHINH)
            {
                xsltContent = xsltContent.Replace("param1", "(Hóa đơn điều chỉnh)");
                xsltContent = xsltContent.Replace("param1_1", "normal");
                xsltContent = xsltContent.Replace("param2_2", "normal");
                xsltContent = xsltContent.Replace("param2", $"Hóa đơn điều chỉnh cho hóa đơn số {hoaDon.ma_so_hoa_don_goc}, mẫu số  {hoaDon.hoa_don_dang_ky_phat_hanh_mau_so_goc}, ký hiệu {hoaDon.hoa_don_dang_ky_phat_hanh_ky_hieu_goc}, ngày hóa đơn {(hoaDon.ngay_hoa_don_goc?.ToString("dd/MM/yyyy") ?? "")}");
            }
            if (hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_THAY_THE)
            {
                xsltContent = xsltContent.Replace("param1", "(Hóa đơn thay thế)");
                xsltContent = xsltContent.Replace("param1_1", "normal");
                xsltContent = xsltContent.Replace("param2_2", "normal");
                xsltContent = xsltContent.Replace("param2", $"Hóa đơn thay thế cho hóa đơn số {hoaDon.ma_so_hoa_don_goc}, mẫu số  {hoaDon.hoa_don_dang_ky_phat_hanh_mau_so_goc}, ký hiệu {hoaDon.hoa_don_dang_ky_phat_hanh_ky_hieu_goc}, ngày hóa đơn {(hoaDon.ngay_hoa_don_goc?.ToString("dd/MM/yyyy") ?? "")}");
            }
            if (hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_BI_DIEU_CHINH || hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_BI_THAY_THE)
            {
                xsltContent = xsltContent.Replace("param1", "");
                xsltContent = xsltContent.Replace("param1_1", "none");
                xsltContent = xsltContent.Replace("param2_2", "normal");
                xsltContent = xsltContent.Replace("param2", $"");
                xsltContent = xsltContent.Replace("contentDisable", $"{(hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_BI_DIEU_CHINH ? "HÓA ĐƠN BỊ ĐIỀU CHỈNH" : "HÓA ĐƠN BỊ THAY THẾ")}");
                xsltContent = xsltContent.Replace("paramdisable", $"position:absolute;z-index:0; width:auto; height:70px; border:4px solid red;  background:transparent; display:block;top:45%;left:50%;transform: translate(-50%, -50%);color:red;font-size:25pt;font-weight:bold;text-align:center;padding-top:10px;opacity: 0.5");
            }
            if (hoaDon.hoa_don_hinh_thuc_id != (int)e_hoa_don_hinh_thuc.HOA_DON_BI_DIEU_CHINH && hoaDon.hoa_don_hinh_thuc_id != (int)e_hoa_don_hinh_thuc.HOA_DON_BI_THAY_THE && hoaDon.hoa_don_hinh_thuc_id != (int)e_hoa_don_hinh_thuc.DA_GUI_TBSS_THAY_THE)
            {
                xsltContent = xsltContent.Replace("param1", "");
                xsltContent = xsltContent.Replace("param1_1", "none");
                xsltContent = xsltContent.Replace("param2_2", "normal");
                xsltContent = xsltContent.Replace("param2", $"");
                xsltContent = xsltContent.Replace("contentDisable", $"&#160;");
                xsltContent = xsltContent.Replace("paramdisable", $"position:absolute;z-index:0 ; width:300px; height:100px; border:3px solid red; background:transparent; display:none;  top:45%; left:40%; color:red;font-size:70pt;text-align:center;padding-top:10px;");
            }
            if (hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.DA_GUI_TBSS_THAY_THE)
            {
                xsltContent = xsltContent.Replace("param1", "");
                xsltContent = xsltContent.Replace("param1_1", "none");
                xsltContent = xsltContent.Replace("param2_2", "normal");
                xsltContent = xsltContent.Replace("param2", $"");
                xsltContent = xsltContent.Replace("contentDisable", "HÓA ĐƠN BỊ THAY THẾ");
                xsltContent = xsltContent.Replace("paramdisable", $"position:absolute;z-index:0; width:auto; height:70px; border:4px solid red;  background:transparent; display:block;top:45%;left:50%;transform: translate(-50%, -50%);color:red;font-size:25pt;font-weight:bold;text-align:center;padding-top:10px;opacity: 0.5");
            }
            xsltContent = xsltContent.Replace("paramlien", "0");
            var getXmlKySoPreview = await _serviceWrapper.HoaDon.HoaDon.CreateXmlKySoAsync(hoaDon.id, true);
            if (!getXmlKySoPreview.is_success) return new ErrorResult<string>(getXmlKySoPreview.message, null);
            var html = await _serviceWrapper.HoaDon.LoaiHoaDonCTTemplate.GeneratePrintHtmlFromXsltContentAsyncV1(xsltContent, getXmlKySoPreview.data, new XsltArgumentList());
            var css = @"
<style>
@media print {
    .page-break {
        page-break-before: always;
    }
}
</style>";
            html = css + html;
            html = html.Replace("NaN", "");
            return new SuccessResult<string>(html);
        }

    }
}