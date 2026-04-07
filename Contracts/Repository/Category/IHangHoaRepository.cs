using Contracts.Repository.Base;
using Model.FuncResult;
using Model.Request.Base;
using Model.Table;

namespace Contract.Repository.Category
{
    public interface IHangHoaRepository : ICRUDRepository<dm_hanghoa>
    {
        Task<PagingResult<IEnumerable<dm_hanghoa>>> SelectByDonViAsync(string donvi_ma_dv, PagingRequest pagingRequest);
        Task<IEnumerable<dm_hanghoa>> SelectByDonViAsync(string donvi_ma_dv, List<string> maHangs);
        Task<bool> InsertsAsync(IEnumerable<dm_hanghoa> hangHoas);

    }
}