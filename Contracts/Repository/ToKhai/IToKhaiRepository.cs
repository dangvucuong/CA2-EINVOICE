using Contracts.Repository.Base;
using Model.Table;

namespace Contracts.Repository.ToKhai
{
    public interface IToKhaiRepository : ICRUDRepository<to_khai>
    {
        Task<IEnumerable<to_khai>> SelectByDonViAsync(string donvi_ma_dv);
        Task<to_khai> SelectByPhatHanhUuidAsync(string phat_hanh_uuid);
    }
}