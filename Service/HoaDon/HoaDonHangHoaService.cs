using System.Data;
using Common;
using Contracts.Service.HoaDon;
using Model.Base;
using Model.FuncResult;
using Model.Request.HoaDon;
using Model.Request.Upload;
using Model.Respone.HoaDon;
using Model.Respone.Upload;
using Model.Table;
using Service.Base;

namespace Service.HoaDon
{
    public class HoaDonHangHoaService : CRUDService<hoa_don_hang_hoa>, IHoaDonHangHoaService
    {
        public HoaDonHangHoaService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.HoaDon.HoaDonHangHoa;
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
            dt.Columns.Add("stt", typeof(int));
            dt.Columns.Add("ma_hang", typeof(string));
            dt.Columns.Add("ten_hang", typeof(string));
            dt.Columns.Add("hang_hoa_tinh_chat_id", typeof(int));
            dt.Columns.Add("dvt", typeof(string));
            dt.Columns.Add("thue_suat", typeof(string));
            dt.Columns.Add("so_luong", typeof(decimal));
            dt.Columns.Add("don_gia", typeof(decimal));
            dt.Columns.Add("ty_le_chiet_khau", typeof(decimal));
            dt.Columns.Add("thanh_tien", typeof(decimal));
            dt.Columns.Add("ma_loi", typeof(string));
            for (int i = 0; i < excelDatas.Rows.Count; i++)
            {
                DataRow data = excelDatas.Rows[i];
                DataRow row = dt.NewRow();
                var maLois = new List<string>();
                row["stt"] = excelDatas.Columns.Contains("STT") ? data["STT"].ConvertToInt() : 0;
                row["ma_hang"] = excelDatas.Columns.Contains("Mã hàng hóa") ? data["Mã hàng hóa"].ConvertToString() : "";
                row["ten_hang"] = excelDatas.Columns.Contains("Tên hàng hóa") ? data["Tên hàng hóa"].ConvertToString() : "";
                row["dvt"] = excelDatas.Columns.Contains("ĐVT") ? data["ĐVT"].ConvertToString() : "";
                row["thue_suat"] = excelDatas.Columns.Contains("Thuế suất") ? data["Thuế suất"].ConvertToString() : "";
                row["so_luong"] = excelDatas.Columns.Contains("Số lượng") ? data["Số lượng"].ConvertToDecimal() : 0;
                row["don_gia"] = excelDatas.Columns.Contains("Đơn giá") ? data["Đơn giá"].ConvertToDecimal() : 0;
                row["ty_le_chiet_khau"] = excelDatas.Columns.Contains("Tỷ lệ chiết khấu") ? data["Tỷ lệ chiết khấu"].ConvertToDecimal() : 0;
                row["thanh_tien"] = excelDatas.Columns.Contains("Thành tiền") ? data["Thành tiền"].ConvertToDecimal() : 0;

                var tinh_chat = excelDatas.Columns.Contains("Tính chất") ? data["Tính chất"].ConvertToString() : "";
                var hang_hoa_tinh_chat_id = 0;
                if (tinh_chat == "Hàng hóa, dịch vụ") hang_hoa_tinh_chat_id = 1;
                if (tinh_chat == "Khuyến mại") hang_hoa_tinh_chat_id = 2;
                if (tinh_chat == "Chiết khấu") hang_hoa_tinh_chat_id = 3;
                if (tinh_chat == "Ghi chú, diễn giải") hang_hoa_tinh_chat_id = 4;
                row["hang_hoa_tinh_chat_id"] = hang_hoa_tinh_chat_id;


                // if (row["stt"].ConvertToInt() == 0) maLois.Add("Số thứ tự không được trống");
                // if (row["thue_suat"].ConvertToString() == "") maLois.Add("Thuế suất không được trống");
                // if (row["ma_hang"].ConvertToString() == "") maLois.Add("Mã hàng không được để trống");
                if (row["ten_hang"].ConvertToString() == "") maLois.Add("Tên hàng hóa không được để trống");
                // if (row["dvt"].ConvertToString() == "") maLois.Add("Đơn vị tính không được để trống");
                // if (row["so_luong"].ConvertToDecimal() < 0) maLois.Add("Số lượng phải >= 0");
                if (row["hang_hoa_tinh_chat_id"].ConvertToInt() <= 0) maLois.Add("Tính chất không khớp");
                row["ma_loi"] = maLois.Join(";\n");
                dt.Rows.Add(row);
            }
            return new SuccessResult<DataTable>(dt);
        }

        public Task<PagingResult<IEnumerable<hoa_don_hang_hoa_vm>>> SelectByDonViThongKePageAsync(string donvi_ma_dv, HoaDonSelectPagingRequest pagingRequest)
        {
            return _repositoryWrapper.HoaDon.HoaDonHangHoa.SelectByDonViThongKePageAsync(donvi_ma_dv,pagingRequest);
        }

        public Task<IEnumerable<hoa_don_hang_hoa>> SelectByHoaDonIdAsync(int hoaDonId)
        {
            return _repositoryWrapper.HoaDon.HoaDonHangHoa.SelectByHoaDonIdAsync(hoaDonId);
        }
    }
}