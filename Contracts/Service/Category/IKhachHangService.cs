using System.Data;
using Contracts.Service.Base;
using Model.Base;
using Model.FuncResult;
using Model.Request.Base;
using Model.Respone.Upload;
using Model.Table;

namespace Contracts.Service.Category
{
    public interface IKhachHangService : ICRUDService<khachhang>
    {
        Task<PagingResult<IEnumerable<khachhang>>> SelectByDonViAsync(string donvi_ma_dv, PagingRequest pagingRequest);
        Task<khachhang> SelectByDonViAsync(string donvi_ma_dv, string khach_hang_mst);
        Task<FunctionResult<DataTable>> ReadAndValidImportDataAsync(UploadRespone upload);
        Task<FunctionResult<string>> ImportDataAsync(UploadRespone upload);

    }
}