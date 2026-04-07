using System.Data;
using Contracts.Service.Base;
using Model.Base;
using Model.FuncResult;
using Model.Request.HoaDon;
using Model.Respone.HoaDon;
using Model.Respone.Upload;
using Model.Table;

namespace Contracts.Service.HoaDon
{
    public interface IHoaDonHangHoaService : ICRUDService<hoa_don_hang_hoa>
    {
        Task<IEnumerable<hoa_don_hang_hoa>> SelectByHoaDonIdAsync(int hoaDonId);
        Task<FunctionResult<DataTable>> ReadAndValidImportDataAsync(UploadRespone upload);
        Task<PagingResult<IEnumerable<hoa_don_hang_hoa_vm>>> SelectByDonViThongKePageAsync(string donvi_ma_dv, HoaDonSelectPagingRequest pagingRequest);
    }
}