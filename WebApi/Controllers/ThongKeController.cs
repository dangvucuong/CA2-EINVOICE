using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Contract.Service;
using Contracts.Service.ThongKe;
using Microsoft.AspNetCore.Mvc;
using Model.Enum;
using Model.Request.HoaDon;
using Model.Request.ThongKe;
using Model.Respone.HoaDon;
using Model.Table;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/thong-ke")]


    public class ThongKeController : BaseController
    {
        private IThongKeHoaDonService _thongKeHoaDonService;
        public ThongKeController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
            this._thongKeHoaDonService = _serviceWrapper.ThongKe.ThongKeHoaDon;
        }
        [MustLogged]
        [HttpPost]
        [Route("hoa-don")]
        public async Task<ContentResult> SelectHoaDonByDonViAsync([FromBody] HoaDonSelectPagingRequest pagingRequest)
        {
            var userInfo = this.GetUserInfo();
            var list = await _serviceWrapper.HoaDon.HoaDon.SelectByDonViThongKePageAsync(userInfo.donvi_ma_dv, pagingRequest);
            return this.OK(list);
        }
        [HttpPost]
        [MustLogged]
        [Route("hang-hoa")]
        public async Task<ContentResult> SelectHangHoaByDonViAsync([FromBody] HoaDonSelectPagingRequest pagingRequest)
        {
            var userInfo = this.GetUserInfo();
            var list = await _serviceWrapper.HoaDon.HoaDonHangHoa.SelectByDonViThongKePageAsync(userInfo.donvi_ma_dv, pagingRequest);
            return this.OK(list);
        }
        [MustLogged]
        [HttpPost]
        [Route("top/so-luong")]
        public async Task<ContentResult> SelectTopBySoLuong([FromBody] ThongKeTopKhachHangTheoHoaDonRequest request)
        {
            var userInfo = this.GetUserInfo();
            request.donvi_ma_dv = userInfo.donvi_ma_dv;
            var list = await _thongKeHoaDonService.GetTopKhachHangBySoLuongHDAsync(request);
            return this.OK(list);
        }
        [MustLogged]
        [HttpPost]
        [Route("top/gia-tri")]
        public async Task<ContentResult> SelectByTopGiaTri([FromBody] ThongKeTopKhachHangTheoHoaDonRequest request)
        {
            var userInfo = this.GetUserInfo();
            request.donvi_ma_dv = userInfo.donvi_ma_dv;
            var list = await _thongKeHoaDonService.GetTopKhachHangBySoGiaTriHDAsync(request);
            return this.OK(list);
        }
        [MustLogged]
        [HttpPost]
        [Route("bang-ke/export")]
        public async Task<IActionResult> ExportExcelBangKeAsync([FromBody] HoaDonSelectPagingRequest pagingRequest)
        {
            var userInfo = this.GetUserInfo();
            pagingRequest.page_index = 0;
            pagingRequest.page_size = 100000000;
            var donVi = await _serviceWrapper.Category.DonVi.SelectByMaDonViAsync(userInfo.donvi_ma_dv);
            pagingRequest.hoa_don_trang_thai_ids = new List<int>() { (int)e_hoa_don_trang_thai.DA_PHAT_HANH };
            var list = await _serviceWrapper.HoaDon.HoaDonHangHoa.SelectByDonViThongKePageAsync(userInfo.donvi_ma_dv, pagingRequest);
            var hangHoas = list.data.ToList();
            var hoaDonList = await _serviceWrapper.HoaDon.HoaDon.SelectByDonViThongKePageAsync(userInfo.donvi_ma_dv, pagingRequest);
            var giamThueTyLeByHoaDonId = hoaDonList.data
                .GroupBy(x => x.id)
                .ToDictionary(g => g.Key, g => g.First().giam_thue_ty_le);
            var loaiTienByHoaDonId = hoaDonList.data
                .GroupBy(x => x.id)
                .ToDictionary(g => g.Key, g => g.First().loai_tien.ConvertToString());

            var hinhThucHoaDons = new Dictionary<int, string>();
            hinhThucHoaDons.Add(0, "Hóa đơn gốc");
            hinhThucHoaDons.Add((int)e_hoa_don_hinh_thuc.HOA_DON_GOC, "Hóa đơn gốc");
            hinhThucHoaDons.Add((int)e_hoa_don_hinh_thuc.HOA_DON_THAY_THE, "Hóa đơn thay thế");
            hinhThucHoaDons.Add((int)e_hoa_don_hinh_thuc.HOA_DON_DIEU_CHINH, "Hóa đơn điều chỉnh");
            hinhThucHoaDons.Add((int)e_hoa_don_hinh_thuc.HOA_DON_BI_DIEU_CHINH, "Hóa đơn bị điều chỉnh");
            hinhThucHoaDons.Add((int)e_hoa_don_hinh_thuc.HOA_DON_DA_HUY_NOI_BO, "Hóa đơn đã hủy nội bộ");
            hinhThucHoaDons.Add((int)e_hoa_don_hinh_thuc.HOA_DON_BI_THAY_THE, "Hóa đơn bị thay thế");
            hinhThucHoaDons.Add((int)e_hoa_don_hinh_thuc.HOA_DON_DA_THONG_BAO_GIAI_TRINH, "Hóa đơn TBSS giải trình");
            hinhThucHoaDons.Add((int)e_hoa_don_hinh_thuc.DA_GUI_TBSS_HUY, "Hóa đơn TBSS hủy");

            // Tạo một bộ nhớ stream để lưu tệp Excel
            using (var memoryStream = new MemoryStream())
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                // Tạo và cấu hình gói EPPlus
                using (var package = new ExcelPackage(memoryStream))
                {
                    // Thêm một worksheet mới
                    var worksheet = package.Workbook.Worksheets.Add("Data");

                    // Thêm tiêu đề
                    worksheet.Cells["D1"].Value = "PHỤ LỤC";
                    worksheet.Cells["D1:G1"].Merge = true;
                    worksheet.Cells["D1:G1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells["D1:G1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells["D1:G1"].Style.Font.Bold = true;
                    worksheet.Cells["D1:G1"].Style.Font.Size = 16;

                    worksheet.Cells["H1"].Value = "Mẫu số:  01_1/GTGT \n(Ban hành kèm theo Thông tư số 119/2014 TT-BTC ngày 25/08/2014 của Bộ tài chính)";
                    worksheet.Cells["H1:J4"].Merge = true;
                    worksheet.Cells["H1:J4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells["H1:J4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                    worksheet.Cells["H1:J4"].Style.WrapText = true;

                    worksheet.Cells["D2"].Value = "BẢNG KÊ HÓA ĐƠN, CHỨNG TỪ HÀNG HÓA, DỊCH VỤ BÁN RA";
                    worksheet.Cells["D2:G2"].Merge = true;
                    worksheet.Cells["D2:G2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells["D2:G2"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells["D2:G2"].Style.Font.Bold = true;
                    worksheet.Cells["D2:G2"].Style.Font.Size = 16;

                    worksheet.Cells["D3"].Value = "( Kèm theo tờ khai thuế GTGT theo mẫu số 01/GTGT)";
                    worksheet.Cells["D3:G3"].Merge = true;
                    worksheet.Cells["D3:G3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells["D3:G3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    worksheet.Cells["D4"].Value = $"[1] Kỳ tính thuế: từ {pagingRequest.tu_ngay.Value.ToString("dd/MM/yyyy")} đến {pagingRequest.den_ngay.Value.ToString("dd/MM/yyyy")}";
                    worksheet.Cells["D4:G4"].Merge = true;
                    worksheet.Cells["D4:G4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells["D4:G4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    worksheet.Cells["A5"].Value = $"[02] Tên người nộp thuế: {donVi?.ten_dv ?? ""}";
                    worksheet.Cells["A5:J5"].Merge = true;
                    // worksheet.Cells["A5:J5"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    // worksheet.Cells["A5:J5"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells["A5:J5"].Style.Font.Bold = true;

                    worksheet.Cells["A6"].Value = $"[03] Mã số thuế: {userInfo?.donvi_ma_dv ?? ""}";
                    worksheet.Cells["A6:J6"].Merge = true;
                    // worksheet.Cells["A6:J6"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    // worksheet.Cells["A6:J6"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells["A6:J6"].Style.Font.Bold = true;

                    worksheet.Cells["A7"].Value = $"[04] Tên đại lý (nếu có)";
                    worksheet.Cells["A7:J7"].Merge = true;
                    // worksheet.Cells["A7:J7"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    // worksheet.Cells["A7:J7"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells["A7:J7"].Style.Font.Bold = true;

                    worksheet.Cells["A8"].Value = $"[05] Mã số thuế";
                    worksheet.Cells["A8:J8"].Merge = true;
                    // worksheet.Cells["A8:J8"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    // worksheet.Cells["A8:J8"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells["A8:J8"].Style.Font.Bold = true;


                    //
                    worksheet.Cells["A9"].Value = "Hoá đơn, chứng từ bán ra";
                    worksheet.Cells["A9:B9"].Merge = true;

                    worksheet.Cells["C9"].Value = "Ký hiệu";
                    worksheet.Cells["D9"].Value = "Tên người mua";
                    worksheet.Cells["E9"].Value = "MST người mua";
                    worksheet.Cells["F9"].Value = "Doanh thu chưa thuế";
                    worksheet.Cells["G9"].Value = "Thuế GTGT";
                    worksheet.Cells["H9"].Value = "Tổng tiền";
                    worksheet.Cells["I9"].Value = "Hình thức";
                    worksheet.Cells["J9"].Value = "Ghi chú";
                    worksheet.Cells["K9"].Value = "Mã ĐVNS";
                    worksheet.Cells["L9"].Value = "CCCD";

                    worksheet.Cells["A9"].Value = "Số hóa đơn";
                    worksheet.Cells["B9"].Value = "Ngày lập";
                    worksheet.Cells["C9:C10"].Merge = true;
                    worksheet.Cells["C9:C10"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells["C9:C10"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells["C9:C10"].Style.Font.Bold = true;

                    worksheet.Cells["D9:D10"].Merge = true;
                    worksheet.Cells["D9:D10"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells["D9:D10"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells["D9:D10"].Style.Font.Bold = true;

                    worksheet.Cells["E9:E10"].Merge = true;
                    worksheet.Cells["E9:E10"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells["E9:E10"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells["E9:E10"].Style.Font.Bold = true;

                    worksheet.Cells["F9:F10"].Merge = true;
                    worksheet.Cells["F9:F10"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells["F9:F10"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells["F9:F10"].Style.Font.Bold = true;

                    worksheet.Cells["G9:G10"].Merge = true;
                    worksheet.Cells["G9:G10"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells["G9:G10"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells["G9:G10"].Style.Font.Bold = true;

                    worksheet.Cells["H9:H10"].Merge = true;
                    worksheet.Cells["H9:H10"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells["H9:H10"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells["H9:H10"].Style.Font.Bold = true;

                    worksheet.Cells["I9:I10"].Merge = true;
                    worksheet.Cells["I9:I10"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells["I9:I10"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells["I9:I10"].Style.Font.Bold = true;

                    worksheet.Cells["J9:J10"].Merge = true;
                    worksheet.Cells["J9:J10"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells["J9:J10"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells["J9:J10"].Style.Font.Bold = true;

                    worksheet.Cells["K9:K10"].Merge = true;
                    worksheet.Cells["K9:K10"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells["K9:K10"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells["K9:K10"].Style.Font.Bold = true;

                    worksheet.Cells["L9:L10"].Merge = true;
                    worksheet.Cells["L9:L10"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells["L9:L10"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells["L9:L10"].Style.Font.Bold = true;


                    worksheet.Cells["A11"].Value = "(1)";
                    worksheet.Cells["B11"].Value = "(2)";
                    worksheet.Cells["C11"].Value = "(3)";
                    worksheet.Cells["D11"].Value = "(4)";
                    worksheet.Cells["E11"].Value = "(5)";
                    worksheet.Cells["F11"].Value = "(6)";
                    worksheet.Cells["G11"].Value = "(7)";
                    worksheet.Cells["H11"].Value = "(8)";
                    worksheet.Cells["I11"].Value = "(9)";
                    worksheet.Cells["J11"].Value = "(10)";
                    worksheet.Cells["K11"].Value = "(11)";
                    worksheet.Cells["L11"].Value = "(12)";


                    worksheet.Column(1).Width = 10;
                    worksheet.Column(2).Width = 10;
                    worksheet.Column(3).Width = 10;
                    worksheet.Column(4).Width = 50;
                    worksheet.Column(5).Width = 20;
                    worksheet.Column(6).Width = 20;
                    worksheet.Column(7).Width = 20;
                    worksheet.Column(8).Width = 20;
                    worksheet.Column(9).Width = 20;
                    worksheet.Column(10).Width = 10;
                    worksheet.Column(11).Width = 10;
                    worksheet.Column(12).Width = 10;


                    worksheet.Cells["A11"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells["B11"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells["C11"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells["D11"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells["E11"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells["F11"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells["G11"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells["H11"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells["I11"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells["J11"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells["K11"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells["L11"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    // var thues = new List<string>() { "KCT", "0%", "5%", "8%", "10%",  "KKKNT" };
                    var thueDictionary = new Dictionary<string, string>();
                    thueDictionary.Add("KCT", "Hàng hóa, dịch vụ không chịu thuế giá trị gia tăng (GTGT)");
                    thueDictionary.Add("0%", "Hàng hóa, dịch vụ chịu thuế suất thuế GTGT 0%");
                    thueDictionary.Add("5%", "Hàng hóa, dịch vụ chịu thuế suất thuế GTGT 5%");
                    thueDictionary.Add("8%", "Hàng hóa, dịch vụ chịu thuế suất thuế GTGT 8%");
                    thueDictionary.Add("10%", "Hàng hóa, dịch vụ chịu thuế suất thuế GTGT 10%");
                    thueDictionary.Add("KKKNT", "Hàng hóa, dịch vụ không kế khai, tính nộp thuế GTGT");
                    var rIdx = 11;
                    var sttThue = 0;
                    decimal tong_doanh_thu_chiu_thue = 0;
                    decimal tong_tien_thue = 0;
                    decimal tong_chiet_khau = 0;
                    foreach (var thue in thueDictionary.Keys)
                    {
                        var phan_tram = thue.Replace("%", "").ConvertToInt();
                        rIdx += 1;
                        worksheet.Cells[$"A{rIdx}"].Value = "";
                        worksheet.Cells[$"A{rIdx}:L{rIdx}"].Merge = true;
                        worksheet.Cells[$"A{rIdx}:L{rIdx}"].Style.Border.Top.Style = ExcelBorderStyle.Hair;
                        worksheet.Cells[$"A{rIdx}:L{rIdx}"].Style.Border.Left.Style = ExcelBorderStyle.Hair;
                        worksheet.Cells[$"A{rIdx}:L{rIdx}"].Style.Border.Right.Style = ExcelBorderStyle.Hair;
                        worksheet.Cells[$"A{rIdx}:L{rIdx}"].Style.Border.Bottom.Style = ExcelBorderStyle.Hair;
                        rIdx += 1;
                        sttThue += 1;
                        worksheet.Cells[$"A{rIdx}"].Value = $"{sttThue}. {thueDictionary[thue]}";
                        worksheet.Cells[$"A{rIdx}:L{rIdx}"].Merge = true;
                        worksheet.Cells[$"A{rIdx}:L{rIdx}"].Style.Border.Top.Style = ExcelBorderStyle.Hair;
                        worksheet.Cells[$"A{rIdx}:L{rIdx}"].Style.Border.Left.Style = ExcelBorderStyle.Hair;
                        worksheet.Cells[$"A{rIdx}:L{rIdx}"].Style.Border.Right.Style = ExcelBorderStyle.Hair;
                        worksheet.Cells[$"A{rIdx}:L{rIdx}"].Style.Border.Bottom.Style = ExcelBorderStyle.Hair;
                        worksheet.Cells[$"A{rIdx}:L{rIdx}"].Style.Font.Italic = true;
                        var hangHoasThue = hangHoas.Where(x => x.thue_vat == thue).ToList();
                        var hangHoasGroupByHoaDonId = hangHoasThue.GroupBy(p => p.hoa_don_id).ToDictionary(g => g.Key, g => g.ToList());
                        decimal tong_truoc_thue = 0;
                        decimal tong_thue = 0;
                        decimal tong_sau_thue = 0;
                        decimal truoc_thue = 0;
                        decimal tien_thue = 0;
                        decimal sau_thue = 0;
                        foreach (var hoaDonId in hangHoasGroupByHoaDonId.Keys)
                        {
                            var hangHoasHoaDon = hangHoasGroupByHoaDonId[hoaDonId];
                            rIdx += 1;
                            //var truoc_thue = hangHoasHoaDon.Select(x => x.thanh_tien).Sum();                           
                            if (hangHoasHoaDon[0].hoa_don_hinh_thuc_id != 6)
                            {
                               if (hangHoasHoaDon[0].hoa_don_dang_ky_phat_hanh_mau_so == "2")
                                {
                                    // Hóa đơn bán hàng: tính lại thành tiền và trừ giảm thuế theo nghị quyết
                                    var allHangHoasHoaDon = hangHoas.Where(x => x.hoa_don_id == hoaDonId).ToList();
                                    var giamThueTyLe = giamThueTyLeByHoaDonId.GetValueOrDefault(hoaDonId, 0);
                                    var loaiTien = loaiTienByHoaDonId.GetValueOrDefault(hoaDonId, "VND");
                                    var tienBangKeMauSo2 = TinhTienBangKeMauSo2(
                                        hangHoasHoaDon,
                                        allHangHoasHoaDon,
                                        giamThueTyLe,
                                        loaiTien);

                                    truoc_thue = tienBangKeMauSo2.thanhTienSauGiamThue;
                                    tien_thue = 0;
                                    var tong_chiet_khau_hh = tienBangKeMauSo2.tongChietKhau;
                                    tong_chiet_khau += tong_chiet_khau_hh;
                                    sau_thue = tienBangKeMauSo2.thanhTienSauGiamThue;
                                    tong_truoc_thue += truoc_thue;
                                    tong_thue = 0;
                                    tong_sau_thue += sau_thue;
                                }
                                else
                                {
                                    //các loại hdon có thuế 
                                    truoc_thue = hangHoasHoaDon.Where(x => x.hang_hoa_tinh_chat_id == 1 || x.hang_hoa_tinh_chat_id == 5).Select(x => x.thanh_tien).Sum();
                                    var tong_chiet_khau_hh = hangHoasHoaDon.Where(x => x.hang_hoa_tinh_chat_id == 3).Select(x => x.thanh_tien).Sum();
                                    tong_chiet_khau += tong_chiet_khau_hh;
                                    tien_thue = Math.Round(truoc_thue * phan_tram / 100, 0);
                                    sau_thue = truoc_thue - tong_chiet_khau_hh + tien_thue;
                                    tong_truoc_thue += truoc_thue;
                                    tong_thue += tien_thue;
                                    tong_sau_thue += sau_thue;
                                }
                                
                            }
                            else
                            {
                                truoc_thue = 0;
                                sau_thue = 0;
                                var a = "HĐ bị thay thế ko tính vào tổng tiền";
                            }


                            worksheet.Cells[$"A{rIdx}"].Value = hangHoasHoaDon[0].ma_so_hoa_don;
                            worksheet.Cells[$"B{rIdx}"].Value = hangHoasHoaDon[0].ngay_hoa_don.ToString("dd/MM/yyyy");
                            worksheet.Cells[$"C{rIdx}"].Value = hangHoasHoaDon[0].hoa_don_dang_ky_phat_hanh_ky_hieu;
                            worksheet.Cells[$"D{rIdx}"].Value = !string.IsNullOrWhiteSpace(hangHoasHoaDon[0].nguoi_mua_mst)
                           ? hangHoasHoaDon[0].nguoi_mua_ten_donvi
                           : hangHoasHoaDon[0].nguoi_mua_ten;
                            worksheet.Cells[$"E{rIdx}"].Value = hangHoasHoaDon[0].nguoi_mua_mst;
                            worksheet.Cells[$"F{rIdx}"].Value = truoc_thue;
                            worksheet.Cells[$"G{rIdx}"].Value = tien_thue;
                            worksheet.Cells[$"H{rIdx}"].Value = sau_thue;

                            worksheet.Cells[$"I{rIdx}"].Value = hinhThucHoaDons.ContainsKey(hangHoasHoaDon[0].hoa_don_hinh_thuc_id)
                            ? hinhThucHoaDons[hangHoasHoaDon[0].hoa_don_hinh_thuc_id]
                            : "";
                            if (hangHoasHoaDon[0].hoa_don_hinh_thuc_id == 2)
                            { //hd thay the
                                worksheet.Cells[$"J{rIdx}"].Value = "Thay thế cho số: " + hangHoasHoaDon[0].ma_so_hoa_don_goc + ", ký hiệu: " + hangHoasHoaDon[0].hoa_don_dang_ky_phat_hanh_ky_hieu_goc + ", mẫu số: " + hangHoasHoaDon[0].hoa_don_dang_ky_phat_hanh_mau_so_goc;
                            }
                            else if (hangHoasHoaDon[0].hoa_don_hinh_thuc_id == 3)
                            { //hd dieu chinh
                                worksheet.Cells[$"J{rIdx}"].Value = "Điều chỉnh cho số: " + hangHoasHoaDon[0].ma_so_hoa_don_goc + ", ký hiệu: " + hangHoasHoaDon[0].hoa_don_dang_ky_phat_hanh_ky_hieu_goc + ", mẫu số: " + hangHoasHoaDon[0].hoa_don_dang_ky_phat_hanh_mau_so_goc;
                            }

                            worksheet.Cells[$"K{rIdx}"].Value = hangHoasHoaDon[0].ma_dv_ngan_sach;
                            worksheet.Cells[$"L{rIdx}"].Value = hangHoasHoaDon[0].nguoi_mua_cccd;
                        }
                        if (phan_tram > 0)
                        {
                            tong_doanh_thu_chiu_thue += tong_truoc_thue;
                            tong_tien_thue += tong_thue;
                        }


                        rIdx += 1;
                        worksheet.Cells[$"A{rIdx}"].Value = $"Tổng tiền trước thuế";
                        worksheet.Cells[$"A{rIdx}:B{rIdx}"].Merge = true;
                        worksheet.Cells[$"A{rIdx}:B{rIdx}"].Style.Border.Top.Style = ExcelBorderStyle.Hair;
                        worksheet.Cells[$"A{rIdx}:B{rIdx}"].Style.Border.Left.Style = ExcelBorderStyle.Hair;
                        worksheet.Cells[$"A{rIdx}:B{rIdx}"].Style.Border.Right.Style = ExcelBorderStyle.Hair;
                        worksheet.Cells[$"A{rIdx}:B{rIdx}"].Style.Border.Bottom.Style = ExcelBorderStyle.Hair;

                        worksheet.Cells[$"C{rIdx}"].Value = $"{tong_truoc_thue.ToString("N0")}";
                        worksheet.Cells[$"C{rIdx}:L{rIdx}"].Merge = true;
                        worksheet.Cells[$"C{rIdx}:L{rIdx}"].Style.Font.Bold = true;
                        worksheet.Cells[$"C{rIdx}:L{rIdx}"].Style.Border.Top.Style = ExcelBorderStyle.Hair;
                        worksheet.Cells[$"C{rIdx}:L{rIdx}"].Style.Border.Left.Style = ExcelBorderStyle.Hair;
                        worksheet.Cells[$"C{rIdx}:L{rIdx}"].Style.Border.Right.Style = ExcelBorderStyle.Hair;
                        worksheet.Cells[$"C{rIdx}:L{rIdx}"].Style.Border.Bottom.Style = ExcelBorderStyle.Hair;
                        rIdx += 1;
                        worksheet.Cells[$"A{rIdx}"].Value = $"Tổng tiền sau thuế";
                        worksheet.Cells[$"A{rIdx}:B{rIdx}"].Merge = true;
                        worksheet.Cells[$"A{rIdx}:B{rIdx}"].Style.Border.Top.Style = ExcelBorderStyle.Hair;
                        worksheet.Cells[$"A{rIdx}:B{rIdx}"].Style.Border.Left.Style = ExcelBorderStyle.Hair;
                        worksheet.Cells[$"A{rIdx}:B{rIdx}"].Style.Border.Right.Style = ExcelBorderStyle.Hair;
                        worksheet.Cells[$"A{rIdx}:B{rIdx}"].Style.Border.Bottom.Style = ExcelBorderStyle.Hair;

                        worksheet.Cells[$"C{rIdx}"].Value = $"{tong_sau_thue.ToString("N0")}";
                        worksheet.Cells[$"C{rIdx}:L{rIdx}"].Merge = true;
                        worksheet.Cells[$"C{rIdx}:L{rIdx}"].Style.Font.Bold = true;
                        worksheet.Cells[$"C{rIdx}:L{rIdx}"].Style.Border.Top.Style = ExcelBorderStyle.Hair;
                        worksheet.Cells[$"C{rIdx}:L{rIdx}"].Style.Border.Left.Style = ExcelBorderStyle.Hair;
                        worksheet.Cells[$"C{rIdx}:L{rIdx}"].Style.Border.Right.Style = ExcelBorderStyle.Hair;
                        worksheet.Cells[$"C{rIdx}:L{rIdx}"].Style.Border.Bottom.Style = ExcelBorderStyle.Hair;

                        // worksheet.Cells[$"F{rIdx}"].Value = tong_truoc_thue;
                        // worksheet.Cells[$"G{rIdx}"].Value = tong_thue;
                        // worksheet.Cells[$"H{rIdx}"].Value = tong_sau_thue;
                    }


                    var totalRows = worksheet.Dimension.End.Row;
                    var totalColumns = worksheet.Dimension.End.Column;

                    for (int r = 9; r <= totalRows; r++)
                    {
                        for (int col = 1; col <= totalColumns; col++)
                        {
                            var cell = worksheet.Cells[r, col];
                            if (cell.Style.Border.Top.Style != ExcelBorderStyle.Hair) cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            if (cell.Style.Border.Top.Style != ExcelBorderStyle.Hair) cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            if (cell.Style.Border.Top.Style != ExcelBorderStyle.Hair) cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            if (cell.Style.Border.Top.Style != ExcelBorderStyle.Hair) cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            if (cell.Style.Border.Top.Style == ExcelBorderStyle.Hair) cell.Style.Border.Top.Style = ExcelBorderStyle.None;
                            if (cell.Style.Border.Top.Style == ExcelBorderStyle.Hair) cell.Style.Border.Left.Style = ExcelBorderStyle.None;
                            if (cell.Style.Border.Top.Style == ExcelBorderStyle.Hair) cell.Style.Border.Right.Style = ExcelBorderStyle.None;
                            if (cell.Style.Border.Top.Style == ExcelBorderStyle.Hair) cell.Style.Border.Bottom.Style = ExcelBorderStyle.None;
                        }
                    }
                    //thêm footer

                    rIdx += 1;
                    worksheet.Cells[$"A{rIdx}"].Value = $"Tổng doanh thu hàng hóa, dịch vụ bán ra chịu thuế GTGT (*):";
                    worksheet.Cells[$"A{rIdx}:D{rIdx}"].Merge = true;
                    worksheet.Cells[$"E{rIdx}"].Value = $"{tong_doanh_thu_chiu_thue.ToString("N0")}";
                    worksheet.Cells[$"E{rIdx}:L{rIdx}"].Merge = true;
                    worksheet.Cells[$"E{rIdx}:L{rIdx}"].Style.Font.Bold = true;

                    rIdx += 1;
                    worksheet.Cells[$"A{rIdx}"].Value = $"Tổng số thuế GTGT của hàng hóa, dịch vụ bán ra (**):";
                    worksheet.Cells[$"A{rIdx}:D{rIdx}"].Merge = true;
                    worksheet.Cells[$"E{rIdx}"].Value = $"{tong_tien_thue.ToString("N0")}";
                    worksheet.Cells[$"E{rIdx}:L{rIdx}"].Merge = true;
                    worksheet.Cells[$"E{rIdx}:L{rIdx}"].Style.Font.Bold = true;

                    rIdx += 1;
                    worksheet.Cells[$"H{rIdx}"].Value = $".........., ngày ........tháng ........năm ........";
                    worksheet.Cells[$"H{rIdx}:L{rIdx}"].Merge = true;
                    worksheet.Cells[$"H{rIdx}:L{rIdx}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    rIdx += 1;
                    worksheet.Cells[$"H{rIdx}"].Value = $"NGƯỜI NỘP THUẾ";
                    worksheet.Cells[$"H{rIdx}:L{rIdx}"].Merge = true;
                    worksheet.Cells[$"H{rIdx}:L{rIdx}"].Style.Font.Size = 13;
                    worksheet.Cells[$"h{rIdx}:L{rIdx}"].Style.Font.Bold = true;
                    worksheet.Cells[$"H{rIdx}:L{rIdx}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    rIdx += 1;
                    worksheet.Cells[$"H{rIdx}"].Value = $"HOẶC ĐẠI ĐIỆN HỢP PHÁP CỦA NGƯỜI NỘP THUẾ";
                    worksheet.Cells[$"H{rIdx}:L{rIdx}"].Merge = true;
                    worksheet.Cells[$"H{rIdx}:L{rIdx}"].Style.Font.Size = 13;
                    worksheet.Cells[$"h{rIdx}:L{rIdx}"].Style.Font.Bold = true;
                    worksheet.Cells[$"H{rIdx}:L{rIdx}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    rIdx += 1;
                    worksheet.Cells[$"H{rIdx}"].Value = $"(Ký, ghi rõ hộ tên; chức vụ và đóng dấu (nếu có))";
                    worksheet.Cells[$"H{rIdx}:L{rIdx}"].Merge = true;
                    worksheet.Cells[$"H{rIdx}:L{rIdx}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"H{rIdx}:L{rIdx}"].Style.Font.Italic = true;

                    // Lưu vào bộ nhớ stream
                    package.Save();
                }
                // Đặt tên và định dạng cho tệp Excel
                var fileName = "exported_data.xlsx";
                var content = memoryStream.ToArray();
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                // Trả về tệp Excel cho người dùng tải về
                var result = new FileContentResult(content, contentType)
                {
                    FileDownloadName = fileName
                };
                return result;
            }
        }
        [HttpPost]
        [MustLogged]
        [Route("hang-hoa/export")]
        public async Task<IActionResult> ExportSelectHangHoaByDonViAsync([FromBody] HoaDonSelectPagingRequest pagingRequest)
        {
            var userInfo = this.GetUserInfo();
            var donVi = await _serviceWrapper.Category.DonVi.SelectByMaDonViAsync(userInfo?.donvi_ma_dv ?? "");
            pagingRequest.page_index = 0;
            pagingRequest.page_size = 100000000;
            var list = await _serviceWrapper.HoaDon.HoaDonHangHoa.SelectByDonViThongKePageAsync(userInfo.donvi_ma_dv, pagingRequest);
            var hangHoas = list.data;
            using (var memoryStream = new MemoryStream())
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                // Tạo và cấu hình gói EPPlus
                using (var package = new ExcelPackage(memoryStream))
                {
                    // Thêm một worksheet mới
                    var worksheet = package.Workbook.Worksheets.Add("Data");
                    // Thêm tiêu đề
                    worksheet.Cells["A1"].Value = "BÁO CÁO TỔNG HỢP BÁN HÀNG";
                    worksheet.Cells["A1:J1"].Merge = true;
                    worksheet.Cells["A1:J1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells["A1:J1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells["A1:J1"].Style.Font.Bold = true;
                    worksheet.Cells["A1:J1"].Style.Font.Size = 16;

                    worksheet.Cells["A2"].Value = $"[01] Kỳ báo cáo: từ {pagingRequest.tu_ngay.Value.ToString("dd/MM/yyyy")} đến {pagingRequest.den_ngay.Value.ToString("dd/MM/yyyy")}";
                    worksheet.Cells["A2:J2"].Merge = true;
                    worksheet.Cells["A2:J2"].Style.Font.Bold = true;
                    // worksheet.Cells["A2:J2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    // worksheet.Cells["A2:J2"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    worksheet.Cells["A3"].Value = $"[02] Tên đơn vị: {donVi?.ten_dv}";
                    worksheet.Cells["A3:J3"].Merge = true;
                    worksheet.Cells["A3:J3"].Style.Font.Bold = true;
                    // worksheet.Cells["A3:J3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    // worksheet.Cells["A3:J3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    worksheet.Cells["A4"].Value = $"[03] Mã số thuế: {donVi?.mst}";
                    worksheet.Cells["A4:J4"].Merge = true;
                    worksheet.Cells["A4:J4"].Style.Font.Bold = true;
                    // worksheet.Cells["A4:J4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    // worksheet.Cells["A4:J4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    // Lưu vào bộ nhớ stream


                    worksheet.Cells["A5"].Value = "STT";
                    worksheet.Cells["B5"].Value = "Mã hàng";
                    worksheet.Cells["C5"].Value = "Tên hàng";
                    worksheet.Cells["D5"].Value = "ĐVT";
                    worksheet.Cells["E5"].Value = "Số lượng";
                    worksheet.Cells["F5"].Value = "Đơn giá";
                    worksheet.Cells["G5"].Value = "Doanh thu";
                    worksheet.Cells["H5"].Value = "Tiền thuế";
                    worksheet.Cells["I5"].Value = "Thành tiền";
                    worksheet.Cells["J5"].Value = "Ghi chú";


                    worksheet.Cells["A6"].Value = "[1]";
                    worksheet.Cells["B6"].Value = "[2]";
                    worksheet.Cells["C6"].Value = "[3]";
                    worksheet.Cells["D6"].Value = "[4]";
                    worksheet.Cells["E6"].Value = "[5]";
                    worksheet.Cells["F6"].Value = "[6]";
                    worksheet.Cells["G6"].Value = "[7]";
                    worksheet.Cells["H6"].Value = "[8]";
                    worksheet.Cells["I6"].Value = "[9]";
                    worksheet.Cells["J6"].Value = "[10]";

                    worksheet.Column(1).Width = 10;
                    worksheet.Column(2).Width = 10;
                    worksheet.Column(3).Width = 50;
                    worksheet.Column(4).Width = 20;
                    worksheet.Column(5).Width = 10;
                    worksheet.Column(6).Width = 20;
                    worksheet.Column(7).Width = 20;
                    worksheet.Column(8).Width = 20;
                    worksheet.Column(9).Width = 20;
                    worksheet.Column(10).Width = 10;
                    var rIdx = 6;
                    var groupedHangHoas = hangHoas
                    .GroupBy(s => new { s.ma_hang, s.ten_hang, s.dvt, s.don_gia, s.ty_le_chiet_khau })
                    .Select(g => new
                    {
                        ma_hang = g.Key.ma_hang,
                        ten_hang = g.Key.ten_hang,
                        dvt = g.Key.dvt,
                        don_gia = g.Key.don_gia,
                        ty_le_chiet_khau = g.Key.ty_le_chiet_khau,
                        hangHoas = g.ToList(),
                    }).ToList();
                    for (var i = 0; i < groupedHangHoas.Count(); i++)
                    {
                        rIdx += 1;
                        var rData = groupedHangHoas[i];
                        decimal tong_so_luong = 0;
                        decimal doanh_thu = 0;
                        decimal thanh_tien = 0;
                        decimal tien_thue = 0;
                        var ty_le_chiet_khau = rData.ty_le_chiet_khau;
                        foreach (var hangHoa in groupedHangHoas[i].hangHoas)
                        {
                            tong_so_luong += hangHoa.so_luong;
                            decimal iDoanhThu = hangHoa.so_luong * hangHoa.don_gia - hangHoa.tien_chiet_khau;
                            doanh_thu += iDoanhThu;
                            var vat = hangHoa.thue_vat.Replace("%", "").ConvertToInt();
                            decimal iTienThue = ((iDoanhThu * vat) / 100).ConvertToDouble(2).ConvertToDecimal();
                            tien_thue += iTienThue;
                            thanh_tien += (iDoanhThu + iTienThue);
                        }


                        worksheet.Cells[$"A{rIdx}"].Value = i + 1;
                        worksheet.Cells[$"B{rIdx}"].Value = rData.ma_hang;
                        worksheet.Cells[$"C{rIdx}"].Value = rData.ten_hang;
                        worksheet.Cells[$"D{rIdx}"].Value = rData.dvt;
                        worksheet.Cells[$"E{rIdx}"].Value = tong_so_luong;
                        worksheet.Cells[$"F{rIdx}"].Value = rData.don_gia;
                        worksheet.Cells[$"G{rIdx}"].Value = doanh_thu;
                        worksheet.Cells[$"H{rIdx}"].Value = tien_thue;
                        worksheet.Cells[$"I{rIdx}"].Value = thanh_tien;
                        worksheet.Cells[$"J{rIdx}"].Value = ty_le_chiet_khau > 0 ? $"Chiết khấu ${ty_le_chiet_khau}%" : "";

                    }
                    var totalRows = worksheet.Dimension.End.Row;
                    var totalColumns = worksheet.Dimension.End.Column;
                    for (int r = 5; r <= totalRows; r++)
                    {
                        for (int col = 1; col <= totalColumns; col++)
                        {

                            var cell = worksheet.Cells[r, col];
                            if (cell.Style.Border.Top.Style != ExcelBorderStyle.Hair) cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            if (cell.Style.Border.Top.Style != ExcelBorderStyle.Hair) cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            if (cell.Style.Border.Top.Style != ExcelBorderStyle.Hair) cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            if (cell.Style.Border.Top.Style != ExcelBorderStyle.Hair) cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            if (cell.Style.Border.Top.Style == ExcelBorderStyle.Hair) cell.Style.Border.Top.Style = ExcelBorderStyle.None;
                            if (cell.Style.Border.Top.Style == ExcelBorderStyle.Hair) cell.Style.Border.Left.Style = ExcelBorderStyle.None;
                            if (cell.Style.Border.Top.Style == ExcelBorderStyle.Hair) cell.Style.Border.Right.Style = ExcelBorderStyle.None;
                            if (cell.Style.Border.Top.Style == ExcelBorderStyle.Hair) cell.Style.Border.Bottom.Style = ExcelBorderStyle.None;

                            if (r <= 6)
                            {
                                cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            }
                        }
                    }

                    package.Save();
                }
                // Đặt tên và định dạng cho tệp Excel
                var fileName = "exported_data.xlsx";
                var content = memoryStream.ToArray();
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                // Trả về tệp Excel cho người dùng tải về
                var result = new FileContentResult(content, contentType)
                {
                    FileDownloadName = fileName
                };
                return result;
            }
        }
        [HttpPost]
        [MustLogged]
        [Route("hang-hoa/export/chi-tiet")]
        public async Task<IActionResult> ExportChiTietSelectHangHoaByDonViAsync([FromBody] HoaDonSelectPagingRequest pagingRequest)
        {
            var userInfo = this.GetUserInfo();
            var donVi = await _serviceWrapper.Category.DonVi.SelectByMaDonViAsync(userInfo?.donvi_ma_dv ?? "");
            pagingRequest.page_index = 0;
            pagingRequest.page_size = 100000000;
            var list = await _serviceWrapper.HoaDon.HoaDonHangHoa.SelectByDonViThongKePageAsync(userInfo.donvi_ma_dv, pagingRequest);
            var hangHoas = list.data.ToList();
            var hoaDonTrangThais = new Dictionary<int, string>();
            hoaDonTrangThais.Add(1, "Hóa đơn nháp");
            hoaDonTrangThais.Add(2, "Hóa đơn đã phát hành");
            hoaDonTrangThais.Add(3, "Hóa đơn đã hủy");
            hoaDonTrangThais.Add(4, "Không có KQ phản hồi");
            hoaDonTrangThais.Add(5, "Đã gửi CQT, phản hồi kỹ thuật");
            hoaDonTrangThais.Add(6, "Đã gửi CQT,chưa phản hồi dữ liệu");
            hoaDonTrangThais.Add(7, "Không đủ điều kiện cấp mã");
            hoaDonTrangThais.Add(8, "Lỗi thông điệp");
            hoaDonTrangThais.Add(9, "Chưa gửi CQT");
            using (var memoryStream = new MemoryStream())
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                // Tạo và cấu hình gói EPPlus
                using (var package = new ExcelPackage(memoryStream))
                {
                    // Thêm một worksheet mới
                    var worksheet = package.Workbook.Worksheets.Add("Data");
                    // Thêm tiêu đề
                    worksheet.Cells["A1"].Value = "BÁO CÁO CHI TIẾT BÁN HÀNG";
                    worksheet.Cells["A1:T1"].Merge = true;
                    worksheet.Cells["A1:T1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells["A1:T1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells["A1:T1"].Style.Font.Bold = true;
                    worksheet.Cells["A1:T1"].Style.Font.Size = 16;

                    worksheet.Cells["A2"].Value = $"[01] Kỳ báo cáo: từ {pagingRequest.tu_ngay.Value.ToString("dd/MM/yyyy")} đến {pagingRequest.den_ngay.Value.ToString("dd/MM/yyyy")}";
                    worksheet.Cells["A2:T2"].Merge = true;
                    worksheet.Cells["A2:T2"].Style.Font.Bold = true;
                    // worksheet.Cells["A2:J2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    // worksheet.Cells["A2:J2"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    worksheet.Cells["A3"].Value = $"[02] Tên đơn vị: {donVi?.ten_dv}";
                    worksheet.Cells["A3:T3"].Merge = true;
                    worksheet.Cells["A3:T3"].Style.Font.Bold = true;
                    // worksheet.Cells["A3:J3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    // worksheet.Cells["A3:J3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    worksheet.Cells["A4"].Value = $"[03] Mã số thuế: {donVi?.mst}";
                    worksheet.Cells["A4:T4"].Merge = true;
                    worksheet.Cells["A4:T4"].Style.Font.Bold = true;
                    // worksheet.Cells["A4:J4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    // worksheet.Cells["A4:J4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    // Lưu vào bộ nhớ stream


                    worksheet.Cells["A5"].Value = "STT";
                    worksheet.Cells["B5"].Value = "Đơn vị mua hàng";
                    worksheet.Cells["C5"].Value = "MST";
                    worksheet.Cells["D5"].Value = "Địa chỉ";
                    worksheet.Cells["E5"].Value = "Ngày bán";
                    worksheet.Cells["F5"].Value = "Mẫu số";
                    worksheet.Cells["G5"].Value = "Ký hiệu";
                    worksheet.Cells["H5"].Value = "Số hóa đơn";
                    worksheet.Cells["I5"].Value = "Trạng thái";
                    worksheet.Cells["J5"].Value = "Mã hàng";
                    worksheet.Cells["K5"].Value = "Tên hàng";
                    worksheet.Cells["L5"].Value = "ĐVT";
                    worksheet.Cells["M5"].Value = "Số lượng";
                    worksheet.Cells["N5"].Value = "Đơn giá";
                    worksheet.Cells["O5"].Value = "Doanh thu";
                    worksheet.Cells["P5"].Value = "Thuế suất";
                    worksheet.Cells["Q5"].Value = "Tiền thuế";
                    worksheet.Cells["R5"].Value = "Thành tiền";
                    worksheet.Cells["S5"].Value = "Ghi chú";
                    worksheet.Cells["T5"].Value = "Mã tra cứu";
                    worksheet.Cells["U5"].Value = "CCCD";
                    worksheet.Cells["V5"].Value = "Mã đơn vị ngân sách";

                    worksheet.Cells["A6"].Value = "[1]";
                    worksheet.Cells["B6"].Value = "[2]";
                    worksheet.Cells["C6"].Value = "[3]";
                    worksheet.Cells["D6"].Value = "[4]";
                    worksheet.Cells["E6"].Value = "[5]";
                    worksheet.Cells["F6"].Value = "[6]";
                    worksheet.Cells["G6"].Value = "[7]";
                    worksheet.Cells["H6"].Value = "[8]";
                    worksheet.Cells["I6"].Value = "[9]";
                    worksheet.Cells["J6"].Value = "[10]";
                    worksheet.Cells["K6"].Value = "[11]";
                    worksheet.Cells["L6"].Value = "[12]";
                    worksheet.Cells["M6"].Value = "[13]";
                    worksheet.Cells["N6"].Value = "[14]";
                    worksheet.Cells["O6"].Value = "[15]";
                    worksheet.Cells["P6"].Value = "[16]";
                    worksheet.Cells["Q6"].Value = "[17]";
                    worksheet.Cells["R6"].Value = "[18]";
                    worksheet.Cells["S6"].Value = "[19]";
                    worksheet.Cells["T6"].Value = "[20]";
                    worksheet.Cells["U6"].Value = "[21]";
                    worksheet.Cells["V6"].Value = "[22]";

                    worksheet.Column(1).Width = 10;
                    worksheet.Column(2).Width = 30;
                    worksheet.Column(3).Width = 10;
                    worksheet.Column(4).Width = 30;
                    worksheet.Column(5).Width = 10;
                    worksheet.Column(6).Width = 20;
                    worksheet.Column(7).Width = 20;
                    worksheet.Column(8).Width = 20;
                    worksheet.Column(9).Width = 20;
                    worksheet.Column(10).Width = 10;
                    worksheet.Column(11).Width = 30;
                    worksheet.Column(12).Width = 10;
                    worksheet.Column(13).Width = 10;
                    worksheet.Column(14).Width = 10;
                    worksheet.Column(15).Width = 10;
                    worksheet.Column(16).Width = 10;
                    worksheet.Column(17).Width = 10;
                    worksheet.Column(18).Width = 10;
                    worksheet.Column(19).Width = 10;
                    worksheet.Column(20).Width = 10;
                    worksheet.Column(21).Width = 20;
                    worksheet.Column(22).Width = 15;
                    var rIdx = 6;

                    for (var i = 0; i < hangHoas.Count(); i++)
                    {
                        rIdx += 1;
                        var hangHoa = hangHoas[i];
                        decimal tong_so_luong = 0;
                        decimal doanh_thu = 0;
                        decimal thanh_tien = 0;
                        decimal tien_thue = 0;
                        var ty_le_chiet_khau = hangHoa.ty_le_chiet_khau;
                        tong_so_luong += hangHoa.so_luong;
                        decimal iDoanhThu = hangHoa.so_luong * hangHoa.don_gia - hangHoa.tien_chiet_khau;
                        doanh_thu += iDoanhThu;
                        var vat = hangHoa.thue_vat.Replace("%", "").ConvertToInt();
                        decimal iTienThue = ((iDoanhThu * vat) / 100).ConvertToDouble(2).ConvertToDecimal();
                        tien_thue += iTienThue;
                        thanh_tien += (iDoanhThu + iTienThue);


                        worksheet.Cells[$"A{rIdx}"].Value = i + 1;
                        worksheet.Cells[$"B{rIdx}"].Value = (hangHoa.nguoi_mua_mst.ConvertToString() != "") ? hangHoa.nguoi_mua_ten_donvi : hangHoa.nguoi_mua_ten;
                        worksheet.Cells[$"C{rIdx}"].Value = hangHoa.nguoi_mua_mst;
                        worksheet.Cells[$"D{rIdx}"].Value = hangHoa.nguoi_mua_dia_chi;
                        worksheet.Cells[$"E{rIdx}"].Value = hangHoa.ngay_hoa_don.ToString("dd/MM/yyyy");
                        worksheet.Cells[$"F{rIdx}"].Value = hangHoa.hoa_don_dang_ky_phat_hanh_mau_so;
                        worksheet.Cells[$"G{rIdx}"].Value = hangHoa.hoa_don_dang_ky_phat_hanh_ky_hieu;
                        worksheet.Cells[$"H{rIdx}"].Value = hangHoa.ma_so_hoa_don;
                        worksheet.Cells[$"I{rIdx}"].Value = hoaDonTrangThais.ContainsKey(hangHoa.hoa_don_trang_thai_id) ?
                        hoaDonTrangThais[hangHoa.hoa_don_trang_thai_id] : "";
                        worksheet.Cells[$"J{rIdx}"].Value = hangHoa.ma_hang;
                        worksheet.Cells[$"K{rIdx}"].Value = hangHoa.ten_hang;
                        worksheet.Cells[$"L{rIdx}"].Value = hangHoa.dvt;
                        worksheet.Cells[$"M{rIdx}"].Value = hangHoa.so_luong;
                        worksheet.Cells[$"N{rIdx}"].Value = hangHoa.don_gia;
                        worksheet.Cells[$"O{rIdx}"].Value = doanh_thu;
                        worksheet.Cells[$"P{rIdx}"].Value = hangHoa.thue_vat;
                        worksheet.Cells[$"Q{rIdx}"].Value = tien_thue;
                        worksheet.Cells[$"R{rIdx}"].Value = thanh_tien;
                        worksheet.Cells[$"S{rIdx}"].Value = "";
                        worksheet.Cells[$"T{rIdx}"].Value = hangHoa.ma_tra_cuu;
                        worksheet.Cells[$"U{rIdx}"].Value = hangHoa.nguoi_mua_cccd;
                        worksheet.Cells[$"V{rIdx}"].Value = hangHoa.ma_dv_ngan_sach;

                    }
                    var totalRows = worksheet.Dimension.End.Row;
                    var totalColumns = worksheet.Dimension.End.Column;
                    for (int r = 5; r <= totalRows; r++)
                    {
                        for (int col = 1; col <= totalColumns; col++)
                        {

                            var cell = worksheet.Cells[r, col];
                            if (cell.Style.Border.Top.Style != ExcelBorderStyle.Hair) cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            if (cell.Style.Border.Top.Style != ExcelBorderStyle.Hair) cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            if (cell.Style.Border.Top.Style != ExcelBorderStyle.Hair) cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            if (cell.Style.Border.Top.Style != ExcelBorderStyle.Hair) cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            if (cell.Style.Border.Top.Style == ExcelBorderStyle.Hair) cell.Style.Border.Top.Style = ExcelBorderStyle.None;
                            if (cell.Style.Border.Top.Style == ExcelBorderStyle.Hair) cell.Style.Border.Left.Style = ExcelBorderStyle.None;
                            if (cell.Style.Border.Top.Style == ExcelBorderStyle.Hair) cell.Style.Border.Right.Style = ExcelBorderStyle.None;
                            if (cell.Style.Border.Top.Style == ExcelBorderStyle.Hair) cell.Style.Border.Bottom.Style = ExcelBorderStyle.None;

                            if (r <= 6)
                            {
                                cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            }
                        }
                    }

                    package.Save();
                }
                // Đặt tên và định dạng cho tệp Excel
                var fileName = "exported_data.xlsx";
                var content = memoryStream.ToArray();
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                // Trả về tệp Excel cho người dùng tải về
                var result = new FileContentResult(content, contentType)
                {
                    FileDownloadName = fileName
                };
                return result;
            }
        }

        private static decimal TinhThanhTienHangHoaBangKe(hoa_don_hang_hoa hangHoa)
        {
            if (hangHoa.hang_hoa_tinh_chat_id == 1 || hangHoa.hang_hoa_tinh_chat_id == 5)
            {
                if (hangHoa.so_luong > 0 || hangHoa.don_gia > 0)
                {
                    var tongTienGoc = hangHoa.so_luong * hangHoa.don_gia;
                    var tienChietKhau = (hangHoa.ty_le_chiet_khau / 100) * tongTienGoc;
                    return tongTienGoc - tienChietKhau;
                }

                return hangHoa.thanh_tien;
            }

            if (hangHoa.hang_hoa_tinh_chat_id == 3)
            {
                if (hangHoa.so_luong > 0 || hangHoa.don_gia > 0)
                    return hangHoa.so_luong * hangHoa.don_gia;

                return hangHoa.thanh_tien;
            }

            return 0;
        }

        private static (decimal thanhTienSauGiamThue, decimal tongChietKhau) TinhTienBangKeMauSo2(
            List<hoa_don_hang_hoa_vm> hangHoasThueNhom,
            List<hoa_don_hang_hoa_vm> allHangHoasHoaDon,
            int giamThueTyLe,
            string loaiTien)
        {
            var isVnd = string.IsNullOrWhiteSpace(loaiTien) || loaiTien == "VND" || loaiTien == "VNĐ";

            var tongThanhTienHoaDon = allHangHoasHoaDon
                .Where(x => x.hang_hoa_tinh_chat_id == 1 || x.hang_hoa_tinh_chat_id == 5)
                .Sum(TinhThanhTienHangHoaBangKe);

            var tongTienMatHangChietKhau = allHangHoasHoaDon
                .Where(x => x.hang_hoa_tinh_chat_id == 3)
                .Sum(TinhThanhTienHangHoaBangKe);

            decimal giamThueThanhTien = 0;
            if (giamThueTyLe > 0)
            {
                giamThueThanhTien = ((double)tongThanhTienHoaDon *
                                     (giamThueTyLe / 100.0) *
                                     0.2).ConvertToDecimal();
            }

            if (isVnd)
            {
                giamThueThanhTien = Math.Round(giamThueThanhTien, 0, MidpointRounding.AwayFromZero);
                tongThanhTienHoaDon = Math.Round(tongThanhTienHoaDon, 0, MidpointRounding.AwayFromZero);
                tongTienMatHangChietKhau = Math.Round(tongTienMatHangChietKhau, 0, MidpointRounding.AwayFromZero);
            }

            var thanhTienHangHoaNhom = hangHoasThueNhom
                .Where(x => x.hang_hoa_tinh_chat_id == 1 || x.hang_hoa_tinh_chat_id == 5)
                .Sum(TinhThanhTienHangHoaBangKe);

            var chietKhauNhom = hangHoasThueNhom
                .Where(x => x.hang_hoa_tinh_chat_id == 3)
                .Sum(TinhThanhTienHangHoaBangKe);

            if (isVnd)
            {
                thanhTienHangHoaNhom = Math.Round(thanhTienHangHoaNhom, 0, MidpointRounding.AwayFromZero);
                chietKhauNhom = Math.Round(chietKhauNhom, 0, MidpointRounding.AwayFromZero);
            }

            decimal giamThueNhom = 0;
            if (giamThueThanhTien > 0 && tongThanhTienHoaDon > 0)
            {
                giamThueNhom = giamThueThanhTien * thanhTienHangHoaNhom / tongThanhTienHoaDon;
                if (isVnd)
                    giamThueNhom = Math.Round(giamThueNhom, 0, MidpointRounding.AwayFromZero);
            }

            decimal thanhTienSauGiamThue;
            if (tongTienMatHangChietKhau > 0)
            {
                // Chiết khấu là mặt hàng độc lập, trừ vào cộng tiền hàng
                thanhTienSauGiamThue = thanhTienHangHoaNhom - chietKhauNhom - giamThueNhom;
            }
            else
            {
                // Chiết khấu đã trừ theo từng mặt hàng
                thanhTienSauGiamThue = thanhTienHangHoaNhom - giamThueNhom;
            }

            if (isVnd)
                thanhTienSauGiamThue = Math.Round(thanhTienSauGiamThue, 0, MidpointRounding.AwayFromZero);

            return (thanhTienSauGiamThue, chietKhauNhom);
        }
    }
}

