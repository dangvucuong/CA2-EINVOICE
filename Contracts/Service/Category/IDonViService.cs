using Contracts.Service.Base;
using Model.Table;

namespace Contracts.Service.Category
{
    public interface IDonViService : ICRUDService<donvi>
    {
        Task<donvi> SelectByMaDonViAsync(string ma_dv);
        Task<donvi> GetGipInfoAsync(string ma_dv);
        Task<donvi> SyncTotalChuKySoDaMuaAsync(string ma_dv);

    }
}