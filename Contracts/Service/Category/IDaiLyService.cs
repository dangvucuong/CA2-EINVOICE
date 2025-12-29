using Contracts.Service.Base;
using Model.FuncResult;
using Model.Request.Base;
using Model.Table;

namespace Contracts.Service.Category
{
    public interface IDaiLyService : ICRUDService<dai_ly>
    {
        Task<PagingResult<IEnumerable<dai_ly>>> SelectByDonViAsync(string donvi_ma_dv, PagingRequest pagingRequest);
        Task<IEnumerable<dai_ly>> SelectByDonViHaveEmailAsync(string donvi_ma_dv);

    }
}