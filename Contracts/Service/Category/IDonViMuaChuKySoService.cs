using Contracts.Service.Base;
using Model.Base;
using Model.Table;

namespace Contracts.Service.Category
{
    public interface IDonViMuaChuKySoService : ICRUDService<donvi_mua_chukyso>
    {
        Task<IEnumerable<donvi_mua_chukyso>> SelectByDonViAsync(string donvi_ma_dv);
        Task<FunctionResult<object>> ThuHoiGoiDvAsync(string ma_so_thue, string idHSDT, int userId = 0);
    }
}