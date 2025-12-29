using System.Data;
using Contracts.Service.Base;
using Model.Base;
using Model.Request.HoaDon;
using Model.Respone.Upload;

namespace Contracts.Service.HoaDon
{
    public interface IHoaDonImportService : IBaseService
    {
        Task<FunctionResult<DataTable>> ReadAndValidImportDataAsync(UploadRespone upload);
        Task<FunctionResult<string>> ImportDataAsync(HoaDonImportRequest upload);

        Task<FunctionResult<DataTable>> ReadAndValidImportDataHocPhiAsync(UploadRespone upload);
        Task<FunctionResult<string>> ImportDataHocPhiAsync(HoaDonImportRequest upload);
        Task<FunctionResult<DataTable>> ReadAndValidImportDataNuocAsync(UploadRespone upload);
        Task<FunctionResult<string>> ImportDataNuocAsync(HoaDonImportRequest upload);

    }
}