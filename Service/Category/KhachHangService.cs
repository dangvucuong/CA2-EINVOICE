using System.Data;
using Common;
using Contracts.Service.Category;
using Model.Base;
using Model.FuncResult;
using Model.Request.Base;
using Model.Request.Upload;
using Model.Respone.Upload;
using Model.Table;
using Service.Base;

namespace Service.Category
{
    public class KhachHangService : CRUDService<khachhang>, IKhachHangService
    {
        public KhachHangService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.Category.KhachHang;
        }

        public async Task<FunctionResult<string>> ImportDataAsync(UploadRespone request)
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
            var khachHangs = new List<khachhang>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];
                var obj = new khachhang()
                {
                    dia_chi = row["dia_chi"].ConvertToString(),
                    donvi_ma_dv = user.donvi_ma_dv,
                    email = row["email"].ConvertToString(),
                    mst = row["mst"].ConvertToString(),
                    stk = row["stk"].ConvertToString(),
                    ten_don_vi = row["ten_don_vi"].ConvertToString(),
                    ten_khach_hang = row["ten_khach_hang"].ConvertToString(),
                    ma_dv_ngan_sach = row["ma_dv_ngan_sach"].ConvertToString(),
                    ccdan = row["ccdan"].ConvertToString()
                };
                obj.SetInsertInfo(user.id);
                khachHangs.Add(obj);
            }
            await _repositoryWrapper.Category.KhachHang.InsertsAsync(khachHangs);
            return new SuccessResult<string>();
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
            dt.Columns.Add("ten_khach_hang", typeof(string));
            dt.Columns.Add("ten_don_vi", typeof(string));
            dt.Columns.Add("dia_chi", typeof(string));
            dt.Columns.Add("stk", typeof(string));
            dt.Columns.Add("mst", typeof(string));
            dt.Columns.Add("email", typeof(string));
            dt.Columns.Add("ma_dv_ngan_sach", typeof(string));
            dt.Columns.Add("ccdan", typeof(string));
            dt.Columns.Add("ma_loi", typeof(string));

            for (int i = 0; i < excelDatas.Rows.Count; i++)
            {
                DataRow data = excelDatas.Rows[i];
                DataRow row = dt.NewRow();
                var maLois = new List<string>();
                row["ten_khach_hang"] = excelDatas.Columns.Contains("Tên người mua hàng") ? data["Tên người mua hàng"].ConvertToString() : "";
                row["ten_don_vi"] = excelDatas.Columns.Contains("Tên đơn vị") ? data["Tên đơn vị"].ConvertToString() : "";
                row["dia_chi"] = excelDatas.Columns.Contains("Địa chỉ") ? data["Địa chỉ"].ConvertToString() : "";

                row["stk"] = excelDatas.Columns.Contains("Số tài khoản") ? data["Số tài khoản"].ConvertToString() : "";
                row["mst"] = excelDatas.Columns.Contains("Mã số thuế") ? data["Mã số thuế"].ConvertToString() : "";
                row["dia_chi"] = excelDatas.Columns.Contains("Địa chỉ") ? data["Địa chỉ"].ConvertToString() : "";
                row["email"] = excelDatas.Columns.Contains("email") ? data["email"].ConvertToString() : "";

                row["ma_dv_ngan_sach"] = excelDatas.Columns.Contains("Mã ĐV ngân sách") ? data["Mã ĐV ngân sách"].ConvertToString() : "";
                row["ccdan"] = excelDatas.Columns.Contains("CCCD") ? data["CCCD"].ConvertToString() : "";

                if (row["ten_khach_hang"].ConvertToString() == "" && row["ten_don_vi"].ConvertToString() == "")
                {
                    maLois.Add("Vui lòng điền tên đơn vị hoặc tên khách hàng");
                }

                row["ma_loi"] = maLois.Join(";\n");
                dt.Rows.Add(row);
            }





            await _serviceWrapper.Cache.SetDataAsync(upload.url, dt, DateTime.Now.AddHours(1));
            return new SuccessResult<DataTable>(dt);
        }

        public Task<PagingResult<IEnumerable<khachhang>>> SelectByDonViAsync(string donvi_ma_dv, PagingRequest pagingRequest)
        {
            return _repositoryWrapper.Category.KhachHang.SelectByDonViAsync(donvi_ma_dv, pagingRequest);
        }

        public Task<khachhang> SelectByDonViAsync(string donvi_ma_dv, string khach_hang_mst)
        {
            return _repositoryWrapper.Category.KhachHang.SelectByDonViAsync(donvi_ma_dv, khach_hang_mst);

        }
    }
}