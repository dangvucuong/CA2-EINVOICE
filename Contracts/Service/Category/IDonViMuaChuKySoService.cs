using Contracts.Service.Base;
using Model.Table;

namespace Contracts.Service.Category
{
    public interface IDonViMuaChuKySoService : ICRUDService<donvi_mua_chukyso>
    {
        Task<IEnumerable<donvi_mua_chukyso>> SelectByDonViAsync(string donvi_ma_dv);
    }
}