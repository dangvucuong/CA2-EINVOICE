using Contracts.Repository.Base;
using Model.Table;

namespace Contracts.Repository.Category
{
    public interface IDonViMuaChuKySoRepository : ICRUDRepository<donvi_mua_chukyso>
    {
        Task<IEnumerable<donvi_mua_chukyso>> SelectByDonViAsync(string donvi_ma_dv);
    }
}