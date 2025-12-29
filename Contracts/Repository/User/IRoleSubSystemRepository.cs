using Contracts.Repository.Base;
using Model.Table;

namespace Contract.Repository.User
{
    public interface IRoleSubSystemRepository : ICRUDRepository<role_sub_system>
    {
        Task<IEnumerable<role_sub_system>> SelectByRoleAsync(int role_id);
    }
}

