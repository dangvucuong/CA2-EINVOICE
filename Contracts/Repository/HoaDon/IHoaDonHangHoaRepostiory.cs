using Contracts.Repository.Base;
using Model.FuncResult;
using Model.Request.HoaDon;
using Model.Respone.HoaDon;
using Model.Table;

namespace Contracts.Repository.HoaDon
{
    public interface IHoaDonHangHoaRepostiory : ICRUDRepository<hoa_don_hang_hoa>
    {
        Task<IEnumerable<hoa_don_hang_hoa>> SelectByHoaDonIdAsync(int hoa_don_id);
         Task<PagingResult<IEnumerable<hoa_don_hang_hoa_vm>>> SelectByDonViThongKePageAsync(string donvi_ma_dv, HoaDonSelectPagingRequest pagingRequest);
    }
}