using Contracts.Service.Base;
using Model.Table;

namespace Contracts.Service.User
{
    public interface IRoleSubSystemService : ICRUDService<role_sub_system>
    {
        Task<IEnumerable<role_sub_system>> SelectByRoleAsync(int role_id);
    }
}