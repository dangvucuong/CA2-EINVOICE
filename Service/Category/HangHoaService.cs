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
    public class HangHoaService : CRUDService<dm_hanghoa>, IHangHoaService
    {
        public HangHoaService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.Category.HangHoa;
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
            var hangHoas = new List<dm_hanghoa>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];
                var obj = new dm_hanghoa()
                {
                    ma_hang_hoa = row["ma_hang_hoa"].ConvertToString(),
                    donvi_ma_dv = user.donvi_ma_dv,
                    ten_hang_hoa = row["ten_hang_hoa"].ConvertToString(),
                    dvt = row["dvt"].ConvertToString(),
                    ma_loai_hoang_hoa = "",
                   
                };
                obj.SetInsertInfo(user.id);
                hangHoas.Add(obj);
            }
            await _repositoryWrapper.Category.HangHoa.InsertsAsync(hangHoas);
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
            dt.Columns.Add("ma_hang_hoa", typeof(string));
            dt.Columns.Add("ten_hang_hoa", typeof(string));
            dt.Columns.Add("dvt", typeof(string));
            dt.Columns.Add("ma_loi", typeof(string));
           

            for (int i = 0; i < excelDatas.Rows.Count; i++)
            {
                DataRow data = excelDatas.Rows[i];
                DataRow row = dt.NewRow();
                var maLois = new List<string>();
                row["ma_hang_hoa"] = excelDatas.Columns.Contains("Mã hàng hóa") ? data["Mã hàng hóa"].ConvertToString() : "";
                row["ten_hang_hoa"] = excelDatas.Columns.Contains("Tên hàng hóa") ? data["Tên hàng hóa"].ConvertToString() : "";
                row["dvt"] = excelDatas.Columns.Contains("Đơn vị tính") ? data["Đơn vị tính"].ConvertToString() : "";

            
                if (row["ma_hang_hoa"].ConvertToString() == "" && row["ten_hang_hoa"].ConvertToString() == "")
                {
                    maLois.Add("Vui lòng điền mã hàng hóa hoặc tên hàng hóa");
                }

                row["ma_loi"] = maLois.Join(";\n"); 
                dt.Rows.Add(row);
            }




            await _serviceWrapper.Cache.SetDataAsync(upload.url, dt, DateTime.Now.AddHours(1));
            return new SuccessResult<DataTable>(dt);
        }

        public Task<PagingResult<IEnumerable<dm_hanghoa>>> SelectByDonViAsync(string donvi_ma_dv, PagingRequest pagingRequest)
        {
            return _repositoryWrapper.Category.HangHoa.SelectByDonViAsync(donvi_ma_dv, pagingRequest);
        }

        public Task<IEnumerable<dm_hanghoa>> SelectByDonViAsync(string donvi_ma_dv, List<string> maHangs)
        {
            return _repositoryWrapper.Category.HangHoa.SelectByDonViAsync(donvi_ma_dv, maHangs);
        }
    }
}