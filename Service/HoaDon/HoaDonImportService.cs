using System.Data;
using Common;
using Contracts.Service.HoaDon;
using Model.Base;
using Model.Request.HoaDon;
using Model.Request.ToKhai;
using Model.Request.Upload;
using Model.Respone.Upload;
using Model.Table;
using Service.Base;

namespace Service.HoaDon
{
    public class HoaDonImportRow
    {
        public int from_idx { get; set; }
        public int to_idx { get; set; }

    }
    public class HoaDonImportService : BaseService, IHoaDonImportService
    {
        public HoaDonImportService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<FunctionResult<string>> ImportDataAsync(HoaDonImportRequest request)
        {
            var user = this.GetCurrentUser();
            DataTable dt = await _serviceWrapper.Cache.GetDataAsync<DataTable>(request.url);
            if (dt == null)
            {
                var validateResult = await this.ReadAndValidImportDataAsync(request);
                if (validateResult.is_success)
                    dt = validateResult.data;
            }
            if (dt == null) return new ErrorResult<string>("Không tải được dữ liệu");

            var hoaDonImportRows = new List<HoaDonImportRow>();
            var from_idx = -1;
            var to_idx = -1;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var is_break_hoadon = dt.Rows[i]["is_break_hoadon"].ConvertToBoolean();
                if (!is_break_hoadon && from_idx == -1)
                {
                    from_idx = i;
                    for (var j = i + 1; j < dt.Rows.Count; j++)
                    {
                        var is_break_hoadon_j = dt.Rows[j]["is_break_hoadon"].ConvertToBoolean();
                        if (is_break_hoadon_j)
                        {
                            to_idx = j - 1;
                        }
                        if (j == dt.Rows.Count - 1)
                        {
                            to_idx = j;
                        }
                        if (to_idx > -1)
                        {
                            hoaDonImportRows.Add(new HoaDonImportRow() { from_idx = from_idx, to_idx = to_idx });
                            i = to_idx + 1;
                            from_idx = -1;
                            to_idx = -1;

                            break;
                        }
                    }
                    if (i == dt.Rows.Count - 1 && !is_break_hoadon)
                    {
                        hoaDonImportRows.Add(new HoaDonImportRow() { from_idx = from_idx, to_idx = i });

                    }
                }
            }
            var tasks = new List<Task>();
            var CKM = this.GetHoaDonType(request.hoa_don_dang_ky_phat_hanh_ky_hieu);
            var daiLyHaveEmails = (await _serviceWrapper.Category.DaiLy.SelectByDonViHaveEmailAsync(user.donvi_ma_dv)).ToList();



            foreach (var row in hoaDonImportRows)
            {
                if (CKM == "M")
                {
                    await ImportHoaDonTask(request, row, dt, user.donvi_ma_dv, daiLyHaveEmails);
                }
                else
                {
                    // await ImportHoaDonTask(request, row, dt, user.donvi_ma_dv);
                    // O là đa luồng, 1 là đơn luồng
                    if (request.importType == "0")
                    {
                        tasks.Add(ImportHoaDonTask(request, row, dt, user.donvi_ma_dv, daiLyHaveEmails));
                    }
                    else
                    {
                        await ImportHoaDonTask(request, row, dt, user.donvi_ma_dv, daiLyHaveEmails);
                    }
                }
            }

            await Task.WhenAll(tasks);
            return new SuccessResult<string>();
        }
        private async Task ImportHoaDonTask(HoaDonImportRequest request, HoaDonImportRow rows, DataTable dt, string donvi_ma_dv, List<dai_ly> daiLys)
        {

            var firstRow = dt.Rows[rows.from_idx];
            var model = new HoaDonAddOrEditModel();
            model.ngay_hoa_don = (dt.Columns.Contains("ngay_hoa_don") && firstRow["ngay_hoa_don"].ConvertToString().ConvertToDate() != null) ? firstRow["ngay_hoa_don"].ConvertToString().ConvertToDate().Value : DateTime.Now;
            model.hoa_don_dang_ky_phat_hanh_mau_so = request.hoa_don_dang_ky_phat_hanh_mau_so;
            model.hoa_don_dang_ky_phat_hanh_ky_hieu = request.hoa_don_dang_ky_phat_hanh_ky_hieu;
            model.loai_hoa_don_ct_id = request.loai_hoa_don_ct_id;
            model.ten_hoa_don = request.ten_hoa_don;
            model.donvi_ma_dv = donvi_ma_dv;
            model.ma_dai_ly = firstRow["ma_dai_ly"].ConvertToString();
            model.ten_dai_ly = firstRow["ten_dai_ly"].ConvertToString();
            model.nguoi_mua_mst = firstRow["ma_so_thue"].ConvertToString();
            model.nguoi_mua_ten = firstRow["ten_nguoi_mua"].ConvertToString();
            model.nguoi_mua_ten_donvi = firstRow["ten_cong_ty"].ConvertToString();
            model.nguoi_mua_dia_chi = firstRow["dia_chi"].ConvertToString();
            model.nguoi_mua_email = firstRow["email"].ConvertToString();
            model.nguoi_mua_stk = firstRow["so_tai_khoan"].ConvertToString();
            model.nguoi_mua_ngan_hang = firstRow["ngan_hang"].ConvertToString();
            model.hinh_thuc_tt = firstRow["hinh_thuc_thanh_toan"].ConvertToString();
            model.ma_dv_ngan_sach = firstRow["ma_dv_ngan_sach"].ConvertToString();
            model.nguoi_mua_cccd = firstRow["nguoi_mua_cccd"].ConvertToString();
            model.hoa_don_hinh_thuc_id = 1;


            model.hoang_hoas = new List<hoa_don_hang_hoa>();
            model.loai_phis = new List<hoa_don_loai_phi>();
            var stt = 0;
            for (int i = rows.from_idx; i <= rows.to_idx; i++)
            {
                stt += 1;
                var hangHoa = new hoa_don_hang_hoa()
                {
                    don_gia = dt.Rows[i]["don_gia"].ConvertToDecimal(),
                    dvt = dt.Rows[i]["don_vi_tinh"].ConvertToString(),
                    hang_hoa_tinh_chat_id = dt.Rows[i]["tinh_chat_hang_hoa"].ConvertToInt(),
                    ma_hang = dt.Rows[i]["ma_san_pham"].ConvertToString(),
                    ten_hang = dt.Rows[i]["ten_san_pham"].ConvertToString(),
                    so_luong = dt.Rows[i]["so_luong"].ConvertToDecimal(),
                    stt = stt,
                    ty_le_chiet_khau = dt.Rows[i]["ty_le_chiet_khau"].ConvertToDecimal(),
                    thue_vat = dt.Rows[i]["thue_vat"].ConvertToString(),
                    thanh_tien = 0,
                    tien_chiet_khau = 0

                };


                hangHoa.tien_chiet_khau = hangHoa.so_luong * hangHoa.don_gia * (hangHoa.ty_le_chiet_khau / 100);
                if (hangHoa.hang_hoa_tinh_chat_id != 4)
                {

                    hangHoa.thanh_tien = hangHoa.so_luong * hangHoa.don_gia * (1 - (hangHoa.ty_le_chiet_khau / 100));
                }
                if (hangHoa.hang_hoa_tinh_chat_id == 0)
                {
                    if (hangHoa.ma_hang.ConvertToString().ToUpper() == "PHÍ" || hangHoa.ma_hang.ConvertToString().ToUpper() == "PHI")
                    {
                        model.loai_phis.Add(new hoa_don_loai_phi()
                        {
                            ten_le_phi = hangHoa.ten_hang,
                            so_tien = hangHoa.don_gia
                        });
                    }
                }
                else
                {
                    model.hoang_hoas.Add(hangHoa);
                }
            }
            var thueSuatValids = new List<string>() { "5%", "8%", "10%" };
            decimal tong_tien_thue = 0;
            foreach (var thueSuat in thueSuatValids)
            {
                var phanTramThue = thueSuat.Replace("%", "").ConvertToInt();
                var tienThue = model.hoang_hoas.Where(x => x.thue_vat == thueSuat).Select(x => x.thanh_tien * phanTramThue / 100).Sum();
                tong_tien_thue += tienThue;
            }
            decimal tong_tien_hang = model.hoang_hoas.Select(x => x.thanh_tien).Sum();
            model.tong_tien_thue = tong_tien_thue;
            model.tong_tien_truong_thue = tong_tien_hang;
            model.tong_tien_thanh_toan = tong_tien_hang + tong_tien_thue;
            model.tong_tien_phi = 0;
            var isSaved = await _serviceWrapper.HoaDon.HoaDon.SaveHoaDonAsync(model);
            if (model.ma_dai_ly.ConvertToString() != null && isSaved.is_success)
            {
                var hoaDonId = isSaved.data;
                var daiLy = daiLys.Where(x => x.ma_dai_ly == model.ma_dai_ly.ConvertToString()).FirstOrDefault();
                if (daiLy != null)
                {
                    await _serviceWrapper.HoaDon.HoaDonSendEmail.SendEmailHoaDonAsync(new List<int>() { hoaDonId });
                }
            }
            // await _serviceWrapper.HoaDon.HoaDon.se
            return;
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

        public async Task<FunctionResult<DataTable>> ReadAndValidImportDataAsync(UploadRespone upload)
        {
            var excelDatas = await _serviceWrapper.Upload.ReadUploadedExcelFile(new ReadUploadedExcelFileRequest()
            {
                file_path = upload.url,
                sheetIndex = 0
            });
            if (excelDatas == null)
            {
                return new ErrorResult<DataTable>("Không được được nội dung file excel");
            }
            DataTable dt = new DataTable();
            dt.Columns.Add("ngay_hoa_don", typeof(string));
            dt.Columns.Add("ma_so_thue", typeof(string));
            dt.Columns.Add("ma_dai_ly", typeof(string));
            dt.Columns.Add("ten_dai_ly", typeof(string));
            dt.Columns.Add("ten_nguoi_mua", typeof(string));
            dt.Columns.Add("ten_cong_ty", typeof(string));
            dt.Columns.Add("dia_chi", typeof(string));
            dt.Columns.Add("email", typeof(string));
            dt.Columns.Add("so_tai_khoan", typeof(string));
            dt.Columns.Add("ma_du_lieu", typeof(string));
            dt.Columns.Add("ngan_hang", typeof(string));
            dt.Columns.Add("hinh_thuc_thanh_toan", typeof(string));
            dt.Columns.Add("ma_dv_ngan_sach", typeof(string));
            dt.Columns.Add("nguoi_mua_cccd", typeof(string));

            dt.Columns.Add("ma_san_pham", typeof(string));
            dt.Columns.Add("ten_san_pham", typeof(string));
            dt.Columns.Add("tinh_chat_hang_hoa", typeof(int));
            dt.Columns.Add("don_vi_tinh", typeof(string));
            dt.Columns.Add("so_luong", typeof(decimal));
            dt.Columns.Add("don_gia", typeof(decimal));
            dt.Columns.Add("ty_le_chiet_khau", typeof(decimal));
            dt.Columns.Add("thue_vat", typeof(string));
            dt.Columns.Add("ma_loi", typeof(string));
            dt.Columns.Add("is_break_hoadon", typeof(bool));
            var thueSuatValid = new List<string>() { "0%", "5%", "8%", "10%", "KCT", "KKKNT" };
            for (int i = 0; i < excelDatas.Rows.Count; i++)
            {
                DataRow data = excelDatas.Rows[i];
                DataRow row = dt.NewRow();
                var maLois = new List<string>();
                row["ngay_hoa_don"] = excelDatas.Columns.Contains("ngay_hoa_don") ? data["ngay_hoa_don"].ConvertToString() : DateTime.Now.ToString("dd/MM/yyyy");
                row["ma_so_thue"] = excelDatas.Columns.Contains("ma_so_thue") ? data["ma_so_thue"].ConvertToString() : "";

                row["ma_dai_ly"] = excelDatas.Columns.Contains("ma_dai_ly") ? data["ma_dai_ly"].ConvertToString() : "";
                row["ten_dai_ly"] = excelDatas.Columns.Contains("ten_dai_ly") ? data["ten_dai_ly"].ConvertToString() : "";

                row["ten_nguoi_mua"] = excelDatas.Columns.Contains("ten_nguoi_mua") ? data["ten_nguoi_mua"].ConvertToString() : "";
                row["ten_cong_ty"] = excelDatas.Columns.Contains("ten_cong_ty") ? data["ten_cong_ty"].ConvertToString() : "";
                row["dia_chi"] = excelDatas.Columns.Contains("dia_chi") ? data["dia_chi"].ConvertToString() : "";
                row["email"] = excelDatas.Columns.Contains("email") ? data["email"].ConvertToString() : "";
                row["so_tai_khoan"] = excelDatas.Columns.Contains("so_tai_khoan") ? data["so_tai_khoan"].ConvertToString() : "";
                row["ma_du_lieu"] = excelDatas.Columns.Contains("ma_du_lieu") ? data["ma_du_lieu"].ConvertToString() : "";
                row["ngan_hang"] = excelDatas.Columns.Contains("ngan_hang") ? data["ngan_hang"].ConvertToString() : "";
                row["hinh_thuc_thanh_toan"] = excelDatas.Columns.Contains("hinh_thuc_thanh_toan") ? data["hinh_thuc_thanh_toan"].ConvertToString() : "";
                row["ma_dv_ngan_sach"] = excelDatas.Columns.Contains("ma_dv_ngan_sach") ? data["ma_dv_ngan_sach"].ConvertToString() : "";
                row["nguoi_mua_cccd"] = excelDatas.Columns.Contains("nguoi_mua_cccd") ? data["nguoi_mua_cccd"].ConvertToString() : "";

                row["ma_san_pham"] = excelDatas.Columns.Contains("ma_san_pham") ? data["ma_san_pham"].ConvertToString() : "";
                row["ten_san_pham"] = excelDatas.Columns.Contains("ten_san_pham") ? data["ten_san_pham"].ConvertToString() : "";
                row["tinh_chat_hang_hoa"] = excelDatas.Columns.Contains("tinh_chat_hang_hoa") ? data["tinh_chat_hang_hoa"].ConvertToInt() : "";
                row["don_vi_tinh"] = excelDatas.Columns.Contains("don_vi_tinh") ? data["don_vi_tinh"].ConvertToString() : "";

                row["so_luong"] = excelDatas.Columns.Contains("so_luong") ? data["so_luong"].ConvertToDecimal() : 0;
                row["don_gia"] = excelDatas.Columns.Contains("don_gia") ? data["don_gia"].ConvertToDecimal() : 0;
                row["ty_le_chiet_khau"] = excelDatas.Columns.Contains("ty_le_chiet_khau") ? data["ty_le_chiet_khau"].ConvertToDecimal() : 0;
                row["thue_vat"] = excelDatas.Columns.Contains("thue_vat") ? data["thue_vat"].ConvertToString() : "";

                var is_break_hoadon = true;
                if (
                    row["ma_so_thue"].ConvertToString().Trim() != ""
                || row["ten_nguoi_mua"].ConvertToString().Trim() != ""
                || row["ten_cong_ty"].ConvertToString().Trim() != ""
                || row["dia_chi"].ConvertToString().Trim() != ""
                || row["email"].ConvertToString().Trim() != ""
                || row["so_tai_khoan"].ConvertToString().Trim() != ""
                || row["ma_du_lieu"].ConvertToString().Trim() != ""
                || row["ngan_hang"].ConvertToString().Trim() != ""
                || row["hinh_thuc_thanh_toan"].ConvertToString().Trim() != ""
                || row["ma_dv_ngan_sach"].ConvertToString().Trim() != ""

                || row["ma_san_pham"].ConvertToString().Trim() != ""
                || row["ten_san_pham"].ConvertToString().Trim() != ""
                || row["tinh_chat_hang_hoa"].ConvertToString().Trim() != "0"
                || row["don_vi_tinh"].ConvertToString().Trim() != ""
                || row["so_luong"].ConvertToString().Trim() != "0"
                || row["don_gia"].ConvertToString().Trim() != "0"
                || row["ty_le_chiet_khau"].ConvertToString().Trim() != "0"
                || row["thue_vat"].ConvertToString().Trim() != ""
                )
                {
                    is_break_hoadon = false;
                }
                row["is_break_hoadon"] = is_break_hoadon;
                if (!is_break_hoadon)
                {
                    // if (row["ma_so_thue"].ConvertToString() == "") maLois.Add("Mã số thuế không được trống");
                    if (dt.Columns.Contains("ngay_hoa_don"))
                    {
                        if (row["ngay_hoa_don"].ConvertToString() != "" && row["ngay_hoa_don"].ConvertToString().ConvertToDate() == null)
                            maLois.Add("Ngày hóa đơn không hợp lệ (kiểu ngày/tháng/năm)");
                    }

                    // if (row["email"].ConvertToString() == "") maLois.Add("Email không được trống");
                    // if (row["thue_vat"].ConvertToString() == "") maLois.Add("Thuế suất không được trống");
                    // if (row["ma_san_pham"].ConvertToString() == "") maLois.Add("Mã hàng không được để trống");
                    if (row["ten_san_pham"].ConvertToString() == "") maLois.Add("Tên hàng hóa không được để trống");
                    if (row["hinh_thuc_thanh_toan"].ConvertToString() == "") maLois.Add("Hình thức thanh toán không được để trống");
                    // if (row["don_vi_tinh"].ConvertToString() == "") maLois.Add("Đơn vị tính không được để trống");
                    // if (row["so_luong"].ConvertToDecimal() < 0) maLois.Add("Số lượng phải >= 0");
                    if (row["ma_san_pham"].ConvertToString().ToUpper() == "PHI" || row["ma_san_pham"].ConvertToString().ToUpper() == "PHÍ")
                    {
                        // if (row["tinh_chat_hang_hoa"].ConvertToInt() < 1 || row["tinh_chat_hang_hoa"].ConvertToInt() > 4) maLois.Add("Tính chất không hợp lệ");
                    }
                    else
                    {
                        if (row["tinh_chat_hang_hoa"].ConvertToInt() < 1 || row["tinh_chat_hang_hoa"].ConvertToInt() > 4) maLois.Add("Tính chất không hợp lệ");
                    }

                    if (!thueSuatValid.Contains(row["thue_vat"].ConvertToString().ToUpper())) maLois.Add("Thuế suất không hợp lệ");
                    row["ma_loi"] = maLois.Join(";\n");
                }
                else
                {
                    row["ma_loi"] = String.Empty;
                }


                dt.Rows.Add(row);
            }
            await _serviceWrapper.Cache.SetDataAsync(upload.url, dt, DateTime.Now.AddHours(1));
            return new SuccessResult<DataTable>(dt);
        }

        public async Task<FunctionResult<DataTable>> ReadAndValidImportDataHocPhiAsync(UploadRespone upload)
        {
            var excelDatas = await _serviceWrapper.Upload.ReadUploadedExcelFile(new ReadUploadedExcelFileRequest()
            {
                file_path = upload.url,
                sheetIndex = 0
            });
            if (excelDatas == null)
            {
                return new ErrorResult<DataTable>("Không được được nội dung file excel");
            }
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(excelDatas);
            DataTable dt = excelDatas;
            dt.Columns.Add("ma_loi", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];

                var maLois = new List<string>();
                if (row[2].ConvertToString() == "") maLois.Add("Họ ten học sinh không được để trống");
                row["ma_loi"] = maLois.Join(";\n");
            }
            await _serviceWrapper.Cache.SetDataAsync(upload.url, dt, DateTime.Now.AddHours(1));
            return new SuccessResult<DataTable>(dt);
        }

        public async Task<FunctionResult<string>> ImportDataHocPhiAsync(HoaDonImportRequest upload)
        {
            var user = this.GetCurrentUser();
            DataTable dt = await _serviceWrapper.Cache.GetDataAsync<DataTable>(upload.url);
            if (dt == null)
            {
                var validateResult = await this.ReadAndValidImportDataAsync(upload);
                if (validateResult.is_success)
                    dt = validateResult.data;
            }
            if (dt == null) return new ErrorResult<string>("Không tải được dữ liệu");

            var tasks = new List<Task>();
            var CKM = this.GetHoaDonType(upload.hoa_don_dang_ky_phat_hanh_ky_hieu);
            foreach (DataRow row in dt.Rows)
            {
                if (CKM == "M")
                {
                    await ImportHoaDonHocPhiTask(upload, row, dt, user.donvi_ma_dv);
                }
                else
                {
                    // tasks.Add(ImportHoaDonHocPhiTask(upload, row, dt, user.donvi_ma_dv));
                    // O là đa luồng, 1 là đơn luồng
                    if (upload.importType == "0")
                    {
                        tasks.Add(ImportHoaDonHocPhiTask(upload, row, dt, user.donvi_ma_dv));
                    }
                    else
                    {
                        await ImportHoaDonHocPhiTask(upload, row, dt, user.donvi_ma_dv);
                    }
                }
            }

            await Task.WhenAll(tasks);
            return new SuccessResult<string>();
        }
        private Task ImportHoaDonHocPhiTask(HoaDonImportRequest request, DataRow firstRow, DataTable dt, string donvi_ma_dv)
        {
            var hangHoaFromColumIdx = 4;
            var hangHoaToColumIdx = 8;
            var maHangIdxs = new List<string>() { "HP", "BTru", "NKhieu", "NDThem1", "NDThem2" };
            var model = new HoaDonAddOrEditModel();
            model.ngay_hoa_don = DateTime.Now;
            model.hoa_don_dang_ky_phat_hanh_mau_so = request.hoa_don_dang_ky_phat_hanh_mau_so;
            model.hoa_don_dang_ky_phat_hanh_ky_hieu = request.hoa_don_dang_ky_phat_hanh_ky_hieu;
            model.loai_hoa_don_ct_id = request.loai_hoa_don_ct_id;
            model.ten_hoa_don = request.ten_hoa_don;
            model.donvi_ma_dv = donvi_ma_dv;
            model.nguoi_mua_mst = string.Empty;
            model.nguoi_mua_ten = firstRow[2].ConvertToString();
            model.nguoi_mua_ten_donvi = "";// firstRow[1].ConvertToString(); bỏ mã học sinh không cần lưu
            model.nguoi_mua_dia_chi = firstRow[3].ConvertToString();
            model.nguoi_mua_email = "";
            model.nguoi_mua_stk = "";
            model.nguoi_mua_ngan_hang = "";
            model.hinh_thuc_tt = "Tiền mặt/ Chuyển khoản";

            model.hoang_hoas = new List<hoa_don_hang_hoa>();
            var stt = 0;
            var stthh = 0;
            for (int i = 0; i <= dt.Columns.Count; i++)
            {
                if (i >= hangHoaFromColumIdx && i <= dt.Columns.Count - 4)
                {
                    stthh += 1;
                    if (firstRow[i].ConvertToString().Trim() != "")
                    {
                        var don_gia = firstRow[i].ConvertToDecimal();
                        var hangHoa = new hoa_don_hang_hoa()
                        {
                            don_gia = don_gia,
                            dvt = "VND",
                            hang_hoa_tinh_chat_id = 1,
                            ma_hang = maHangIdxs[i % hangHoaFromColumIdx],
                            ten_hang = dt.Columns[i].Caption,
                            so_luong = 1,
                            ty_le_chiet_khau = 0,
                            thue_vat = "0%",
                            thanh_tien = don_gia,
                            stt = stthh,
                            tien_chiet_khau = 0

                        };
                        model.hoang_hoas.Add(hangHoa);
                    }
                }
                //stt += 1;
            }

            decimal tong_tien_thue = 0;
            decimal tong_tien_hang = model.hoang_hoas.Select(x => x.thanh_tien).Sum();
            model.tong_tien_thue = tong_tien_thue;
            model.tong_tien_truong_thue = tong_tien_hang;
            model.tong_tien_thanh_toan = tong_tien_hang + tong_tien_thue;
            model.tong_tien_phi = 0;
            return _serviceWrapper.HoaDon.HoaDon.SaveHoaDonAsync(model);
        }

        public async Task<FunctionResult<DataTable>> ReadAndValidImportDataNuocAsync(UploadRespone upload)
        {
            var user = this.GetCurrentUser();

            var excelDatas = await _serviceWrapper.Upload.ReadUploadedExcelFile(new ReadUploadedExcelFileRequest()
            {
                file_path = upload.url,
                sheetIndex = 0
            });
            if (excelDatas == null)
            {
                return new ErrorResult<DataTable>("Không được được nội dung file excel");
            }
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(excelDatas);
            DataTable dt = excelDatas;
            dt.Columns.Add("ma_loi", typeof(string));
            var thueSuatValid = new List<string>() { "0%", "5%", "8%", "10%", "KCT", "KKKNT" };
            var donVi = await _serviceWrapper.Category.DonVi.SelectByMaDonViAsync(user.donvi_ma_dv);
            var ngay_hoa_don_max = donVi.ngay_hoa_don_max;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];

                var maLois = new List<string>();
                if (row[1].ConvertToString().Trim() == "") maLois.Add("Mã Bill không được để trống");
                var maBill = row[1].ConvertToString().Trim();
                if (maBill != string.Empty)// bỏ check mã bill
                {
                    var tt_nuoc_ngay_doc_thang_truoc = row[3].ConvertToString().Trim();
                    var tt_nuoc_ngay_doc_thang_nay = row[2].ConvertToString().Trim();
                    var ngay_hoa_don_string = row[4].ConvertToString().Trim();
                    var ngay_hoa_don = ngay_hoa_don_string.ConvertToDate();
                    var ngay_doc_thang_nay = tt_nuoc_ngay_doc_thang_nay.ConvertToDate();
                    var ngay_doc_thang_truoc = tt_nuoc_ngay_doc_thang_truoc.ConvertToDate();
                    if (!ngay_hoa_don.HasValue) maLois.Add("Ngày hóa đơn không hợp lệ (dd/MM/yyyy)");
                    if (!ngay_doc_thang_nay.HasValue) maLois.Add("Ngày đọc tháng này không hợp lệ (dd/MM/yyyy)");
                    if (!ngay_doc_thang_truoc.HasValue) maLois.Add("Ngày đọc tháng trước không hợp lệ (dd/MM/yyyy)");
                    if (ngay_hoa_don.Value.Date < ngay_hoa_don_max.Value.Date) maLois.Add($"Ngày hóa đơn phải từ ngày {ngay_hoa_don_max.Value.Date.ToString("dd/MM/yyyy")}");
                    var thue_vat = row[32].ConvertToString();
                    if (!thueSuatValid.Contains(thue_vat))
                    {
                        maLois.Add("Thuế suất không hợp lệ");
                    }
                }
                row["ma_loi"] = maLois.Join(";\n");
            }
            await _serviceWrapper.Cache.SetDataAsync(upload.url, dt, DateTime.Now.AddHours(1));
            return new SuccessResult<DataTable>(dt);
        }

        public async Task<FunctionResult<string>> ImportDataNuocAsync(HoaDonImportRequest upload)
        {
            var user = this.GetCurrentUser();
            DataTable dt = await _serviceWrapper.Cache.GetDataAsync<DataTable>(upload.url);
            if (dt == null)
            {
                var validateResult = await this.ReadAndValidImportDataAsync(upload);
                if (validateResult.is_success)
                    dt = validateResult.data;
            }
            if (dt == null) return new ErrorResult<string>("Không tải được dữ liệu");

            var tasks = new List<Task>();
            var CKM = this.GetHoaDonType(upload.hoa_don_dang_ky_phat_hanh_ky_hieu);
            foreach (DataRow row in dt.Rows)
            {
                if (CKM == "M")
                {
                    await ImportHoaDonNuocTask(upload, row, dt, user.donvi_ma_dv);
                }
                else
                {
                    // O là đa luồng, 1 là đơn luồng
                    if (upload.importType == "0")
                    {
                        tasks.Add(ImportHoaDonNuocTask(upload, row, dt, user.donvi_ma_dv));
                    }
                    else
                    {
                        await ImportHoaDonNuocTask(upload, row, dt, user.donvi_ma_dv);
                    }

                }
            }

            await Task.WhenAll(tasks);
            return new SuccessResult<string>();
        }
        private Task ImportHoaDonNuocTask(HoaDonImportRequest request, DataRow firstRow, DataTable dt, string donvi_ma_dv)
        {

            var mst = firstRow[10].ConvertToString().Trim();
            var ten_khach_hang = firstRow[9].ConvertToString().Trim();
            var dia_chi = firstRow[11].ConvertToString().Trim();
            var email = firstRow[36].ConvertToString().Trim();
            var hinh_thuc_thanh_toan = firstRow[35].ConvertToString().Trim();
            var ngay_hoa_don_string = firstRow[4].ConvertToString().Trim();
            var ten_hang_hoa = firstRow[14].ConvertToString().Trim();
            var ngay_hoa_don = ngay_hoa_don_string.ConvertToDate();

            var tt_nuoc_ma_bill = firstRow[1].ConvertToString().Trim();
            var tt_nuoc_ngay_doc_thang_truoc = firstRow[3].ConvertToString().Trim();
            var tt_nuoc_ngay_doc_thang_nay = firstRow[2].ConvertToString().Trim();
            var tt_nuoc_so_cuong = firstRow[5].ConvertToString().Trim();
            var tt_nuoc_ma_nguoi_mua = firstRow[8].ConvertToString().Trim();
            var tt_nuoc_chi_so_thang_truoc = firstRow[13].ConvertToString().Trim();
            var tt_nuoc_chi_so_thang_ngay = firstRow[12].ConvertToString().Trim();

            var tt_nuoc_ma_nuoc = firstRow[15].ConvertToString().Trim();
            var tt_nuoc_tong_tieu_thu = firstRow[16].ConvertToString().Trim();
            var tt_nuoc_tong_so_ngay = "";
            var tt_nuoc_so_ho = firstRow[6].ConvertToString().Trim();
            var tt_nuoc_serial_dong_ho = firstRow[7].ConvertToString().Trim();





            var model = new HoaDonAddOrEditModel();
            model.ngay_hoa_don = ngay_hoa_don.Value;
            model.hoa_don_dang_ky_phat_hanh_mau_so = request.hoa_don_dang_ky_phat_hanh_mau_so;
            model.hoa_don_dang_ky_phat_hanh_ky_hieu = request.hoa_don_dang_ky_phat_hanh_ky_hieu;
            model.loai_hoa_don_ct_id = request.loai_hoa_don_ct_id;
            model.ten_hoa_don = request.ten_hoa_don;
            model.donvi_ma_dv = donvi_ma_dv;
            model.nguoi_mua_mst = mst;
            model.nguoi_mua_ten = mst == "" ? ten_khach_hang : "";
            model.nguoi_mua_ten_donvi = mst != string.Empty ? ten_khach_hang : "";
            model.nguoi_mua_dia_chi = dia_chi;
            model.nguoi_mua_email = email;
            model.nguoi_mua_stk = "";
            model.nguoi_mua_ngan_hang = "";
            model.hinh_thuc_tt = hinh_thuc_thanh_toan;

            model.tt_nuoc_ma_bill = tt_nuoc_ma_bill;
            model.tt_nuoc_ngay_doc_thang_truoc = tt_nuoc_ngay_doc_thang_truoc;
            model.tt_nuoc_ngay_doc_thang_nay = tt_nuoc_ngay_doc_thang_nay;
            model.tt_nuoc_so_cuong = tt_nuoc_so_cuong;
            model.tt_nuoc_ma_nguoi_mua = tt_nuoc_ma_nguoi_mua;
            model.tt_nuoc_chi_so_thang_truoc = tt_nuoc_chi_so_thang_truoc;
            model.tt_nuoc_chi_so_thang_ngay = tt_nuoc_chi_so_thang_ngay;

            model.tt_nuoc_ma_nuoc = tt_nuoc_ma_nuoc;
            model.tt_nuoc_tong_tieu_thu = tt_nuoc_tong_tieu_thu;

            model.tt_nuoc_serial_dong_ho = tt_nuoc_serial_dong_ho;
            model.tt_nuoc_so_ho = tt_nuoc_so_ho;

            var ngay_doc_thang_nay = tt_nuoc_ngay_doc_thang_nay.ConvertToDate();
            var ngay_doc_thang_truoc = tt_nuoc_ngay_doc_thang_truoc.ConvertToDate();
            TimeSpan difference = ngay_doc_thang_nay.Value.Date - ngay_doc_thang_truoc.Value.Date;
            var tong_so_ngay = difference.TotalDays;
            model.tt_nuoc_tong_so_ngay = tong_so_ngay.ToString();

            model.hoang_hoas = new List<hoa_don_hang_hoa>();
            var soLuongIdxStart = 17;
            var donGiaIdxStart = 18;
            var thanhTienIdxStart = 19;
            for (var hangHoaIdx = 1; hangHoaIdx <= 4; hangHoaIdx++)
            {
                var maHangHoa = $"M{hangHoaIdx}";
                var soLuongIdx = soLuongIdxStart + (3 * (hangHoaIdx - 1));
                var donGiaIdx = donGiaIdxStart + (3 * (hangHoaIdx - 1));
                var thanhTienIdx = thanhTienIdxStart + (3 * (hangHoaIdx - 1));
                var thue_vat = firstRow[32].ConvertToString();
                var thanh_tien = firstRow[34].ConvertToDecimal();
                var hangHoa = new hoa_don_hang_hoa()
                {
                    don_gia = firstRow[donGiaIdx].ConvertToDecimal(),
                    dvt = "Khối",
                    hang_hoa_tinh_chat_id = 1,
                    ma_hang = maHangHoa,
                    ten_hang = ten_hang_hoa,
                    so_luong = (decimal)firstRow[soLuongIdx].ConvertToDouble(),
                    stt = hangHoaIdx,
                    ty_le_chiet_khau = 0,
                    thue_vat = thue_vat,
                    thanh_tien = firstRow[thanhTienIdx].ConvertToDecimal(),
                    tien_chiet_khau = 0

                };
                model.hoang_hoas.Add(hangHoa);
            }

            var thueSuatValids = new List<string>() { "5%", "8%", "10%" };
            decimal tong_tien_thue = 0;
            foreach (var thueSuat in thueSuatValids)
            {
                var phanTramThue = thueSuat.Replace("%", "").ConvertToInt();
                var tienThue = model.hoang_hoas.Where(x => x.thue_vat == thueSuat).Select(x => x.thanh_tien * phanTramThue / 100).Sum();
                tong_tien_thue += tienThue;
            }
            decimal tong_tien_hang = model.hoang_hoas.Select(x => x.thanh_tien).Sum();
            model.tong_tien_thue = tong_tien_thue;
            model.tong_tien_truong_thue = tong_tien_hang;
            model.tong_tien_thanh_toan = tong_tien_hang + tong_tien_thue;
            model.tong_tien_phi = 0;

            // decimal tong_tien_thue = 0;
            // decimal tong_tien_hang = model.hoang_hoas.Select(x => x.thanh_tien).Sum();
            // model.tong_tien_thue = tong_tien_thue;
            // model.tong_tien_truong_thue = tong_tien_hang;
            // model.tong_tien_thanh_toan = tong_tien_hang + tong_tien_thue;
            // model.tong_tien_phi = 0;
            return _serviceWrapper.HoaDon.HoaDon.SaveHoaDonAsync(model);
        }
    }
}