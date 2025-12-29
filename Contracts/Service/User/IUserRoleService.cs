using Contracts.Service.Base;
using Model.Table;

namespace Contract.Service.User
{
    public interface IUserRoleService : ICRUDService<user_role>
    {
        Task<IEnumerable<user_role>> SelectByUserIdAsync(int user_id);
    }
}

