using Contracts.Repository.Base;
using Model;

namespace Contracts.Repository.Category
{
    public interface IDonViCtsRepository:ICRUDRepository<don_vi_cts>
    {
        Task<IEnumerable<don_vi_cts>> SelectByDonViAsync(string donvi_ma_dv);
    }
}