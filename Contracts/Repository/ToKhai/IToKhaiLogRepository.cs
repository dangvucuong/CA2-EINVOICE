using Contracts.Repository.Base;
using Model.Table;

namespace Contracts.Repository.ToKhai
{
    public interface IToKhaiLogRepository : ICRUDRepository<to_khai_log>
    {
        Task<IEnumerable<to_khai_log>> SelectByToKhaiAsync(int to_khai_id);
        Task<IEnumerable<to_khai_log>> SelectByToKhaiIdsAsync(List<int> to_khai_ids);
    }
}