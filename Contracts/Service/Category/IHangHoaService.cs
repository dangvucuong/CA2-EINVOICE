using System.Data;
using Contracts.Service.Base;
using Model.Base;
using Model.FuncResult;
using Model.Request.Base;
using Model.Respone.Upload;
using Model.Table;

namespace Contracts.Service.Category
{
    public interface IHangHoaService : ICRUDService<dm_hanghoa>
    {
        Task<PagingResult<IEnumerable<dm_hanghoa>>> SelectByDonViAsync(string donvi_ma_dv, PagingRequest pagingRequest);
        Task<IEnumerable<dm_hanghoa>> SelectByDonViAsync(string donvi_ma_dv, List<string> maHangs);
        Task<FunctionResult<DataTable>> ReadAndValidImportDataAsync(UploadRespone upload);
        Task<FunctionResult<string>> ImportDataAsync(UploadRespone upload);
    }
}