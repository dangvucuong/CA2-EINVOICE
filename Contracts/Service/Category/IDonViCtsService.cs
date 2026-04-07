using Contracts.Service.Base;
using Model;

namespace Contracts.Service.Category
{
    public interface IDonViCtsService : ICRUDService<don_vi_cts>
    {
        Task<IEnumerable<don_vi_cts>> SelectByDonViAsync(string donvi_ma_dv);
    }
}