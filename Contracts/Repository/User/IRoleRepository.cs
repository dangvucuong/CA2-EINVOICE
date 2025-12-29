using Contracts.Repository.Base;
using Model.Respone.Role;
using Model.Table;

namespace Contract.Repository.User
{
    public interface IRoleRepository : ICRUDRepository<role>
    {
        Task<IEnumerable<role>> SelectByUserAsync(int user_id);
        Task<IEnumerable<RoleViewModel>> SelectAllViewModalAsync();
    }
}

