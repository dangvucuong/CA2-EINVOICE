using Contracts.Service.Base;
using Model.Table;

namespace Contracts.Service.ToKhai
{
    public interface IToKhaiLogService : ICRUDService<to_khai_log>
    {
        Task<IEnumerable<to_khai_log>> SelectByToKhaiAsync(int to_khai_id);
        Task<IEnumerable<to_khai_log>> SelectByToKhaiIdsAsync(List<int> to_khai_ids);
    }
}