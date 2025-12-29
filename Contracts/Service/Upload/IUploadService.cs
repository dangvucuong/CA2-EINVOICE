using System.Data;
using Contracts.Service.Base;
using Microsoft.AspNetCore.Http;
using Model.Base;
using Model.Request.Upload;
using Model.Respone.Upload;

namespace Contracts.Service.Upload
{
    public interface IUploadService:IBaseService
    {
        Task<FunctionResult<string>> UploadAsync(IFormFile formFile);
        Task<FunctionResult<UploadCerRespone>> UploadCertAsync(IFormFile formFile);
        Task<DataTable> ReadUploadedExcelFile(ReadUploadedExcelFileRequest request);


    }
}