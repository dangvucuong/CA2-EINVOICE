using Contracts.Repository.Base;
using Model.Table;

namespace Contract.Repository.User
{
    public interface IUserRoleRepository : ICRUDRepository<user_role>
    {
        Task<IEnumerable<user_role>> SelectByUserIdAsync(int user_id);
    }
}

