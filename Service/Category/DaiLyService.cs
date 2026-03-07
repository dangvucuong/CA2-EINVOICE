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
    public class DaiLyService : CRUDService<dai_ly>, IDaiLyService
    {
        public DaiLyService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.Category.DaiLy;
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
            var dailys = new List<dai_ly>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];
                var obj = new dai_ly()
                {
                    donvi_ma_dv = user.donvi_ma_dv,
                    email = row["email"].ConvertToString(),
                    ma_dai_ly = row["ma_dai_ly"].ConvertToString(),
                    so_tai_khoan = row["so_tai_khoan"].ConvertToString(),
                    ten_dai_ly = row["ten_dai_ly"].ConvertToString(),
                };
                obj.SetInsertInfo(user.id);
                dailys.Add(obj);
            }
            await _repositoryWrapper.Category.DaiLy.InsertsAsync(dailys);
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
            dt.Columns.Add("ten_dai_ly", typeof(string));
            dt.Columns.Add("ma_dai_ly", typeof(string));
            dt.Columns.Add("so_tai_khoan", typeof(string));
            dt.Columns.Add("email", typeof(string));
            dt.Columns.Add("ma_loi", typeof(string));

            for (int i = 0; i < excelDatas.Rows.Count; i++)
            {
                DataRow data = excelDatas.Rows[i];
                DataRow row = dt.NewRow();
                var maLois = new List<string>();
                row["ten_dai_ly"] = excelDatas.Columns.Contains("Tên đại lý") ? data["Tên đại lý"].ConvertToString() : "";
                row["so_tai_khoan"] = excelDatas.Columns.Contains("Số tài khoản") ? data["Số tài khoản"].ConvertToString() : "";

                row["ma_dai_ly"] = excelDatas.Columns.Contains("Mã đại lý") ? data["Mã đại lý"].ConvertToString() : "";

                row["email"] = excelDatas.Columns.Contains("Email") ? data["Email"].ConvertToString() : "";

                if (row["ten_dai_ly"].ConvertToString() == "" && row["ma_dai_ly"].ConvertToString() == "")
                {
                    maLois.Add("Vui lòng điền tên hoặc mã đại lý");
                }

                row["ma_loi"] = maLois.Join(";\n");
                dt.Rows.Add(row);
            }



            await _serviceWrapper.Cache.SetDataAsync(upload.url, dt, DateTime.Now.AddHours(1));
            return new SuccessResult<DataTable>(dt);
        }


        public Task<PagingResult<IEnumerable<dai_ly>>> SelectByDonViAsync(string donvi_ma_dv, PagingRequest pagingRequest)
        {
            return _repositoryWrapper.Category.DaiLy.SelectByDonViAsync(donvi_ma_dv, pagingRequest);
        }

        public Task<IEnumerable<dai_ly>> SelectByDonViHaveEmailAsync(string donvi_ma_dv)
        {
            return _repositoryWrapper.Category.DaiLy.SelectByDonViHaveEmailAsync(donvi_ma_dv);
        }
    }
}