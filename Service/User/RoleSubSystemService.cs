using Contracts.Service.User;
using Model.Table;
using Service.Base;

namespace Service.User
{
    public class RoleSubSystemService : CRUDService<role_sub_system>, IRoleSubSystemService
    {
        public RoleSubSystemService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase =_repositoryWrapper.User.RoleSubSystem;
        }

        public Task<IEnumerable<role_sub_system>> SelectByRoleAsync(int role_id)
        {
            return _repositoryWrapper.User.RoleSubSystem.SelectByRoleAsync(role_id);
        }
    }
}