using Contracts.Repository.Base;
using Model.FuncResult;
using Model.Request.Base;
using Model.Table;

namespace Contracts.Repository.Category
{
    public interface IDaiLyRepository : ICRUDRepository<dai_ly>
    {
        Task<PagingResult<IEnumerable<dai_ly>>> SelectByDonViAsync(string donvi_ma_dv, PagingRequest pagingRequest);
        Task<IEnumerable<dai_ly>> SelectByDonViHaveEmailAsync(string donvi_ma_dv);
        Task<bool> InsertsAsync(IEnumerable<dai_ly> dailys);

    }
}