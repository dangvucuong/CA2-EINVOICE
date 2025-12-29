using Contracts.Repository.Base;
using Model.Table;

namespace Contracts.Repository.User
{
    public interface IRoleApiRepository : ICRUDRepository<role_api>
    {
        Task<IEnumerable<role_api>> SelectByRoleAsync(int role_id, int sub_system_id);
    }
}