using Contracts.Repository.Base;
using Model.Table;

namespace Contracts.Repository.ToKhai
{
    public interface IToKhaiCTSRepository : ICRUDRepository<to_khai_cts>
    {
        Task<IEnumerable<to_khai_cts>> SelectByToKhaiAsync(int to_khai_id);
    }
}