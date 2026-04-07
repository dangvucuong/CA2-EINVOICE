using Contracts.Service.Base;
using Model.Respone.Role;
using Model.Table;

namespace Contract.Service.User
{
    public interface IRoleService : ICRUDService<role>
    {
        Task<IEnumerable<RoleViewModel>> SelectAllViewModalAsync();
    }
}

