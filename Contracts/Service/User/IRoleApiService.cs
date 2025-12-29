using Contracts.Service.Base;
using Model.Table;

namespace Contracts.Service.User
{
    public interface IRoleApiService : ICRUDService<role_api>
    {
        Task<IEnumerable<role_api>> SelectByRoleAsync(int role_id, int sub_system_id);
    }
}