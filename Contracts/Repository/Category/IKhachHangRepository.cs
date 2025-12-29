using Contracts.Repository.Base;
using Model.FuncResult;
using Model.Request.Base;
using Model.Table;

namespace Contract.Repository.Category
{
    public interface IKhachHangRepository : ICRUDRepository<khachhang>
    {
        Task<PagingResult<IEnumerable<khachhang>>> SelectByDonViAsync(string donvi_ma_dv, PagingRequest pagingRequest);
        Task<khachhang> SelectByDonViAsync(string donvi_ma_dv, string khach_hang_mst);
        Task<bool> InsertsAsync(IEnumerable<khachhang> khachhangs);
    }
}