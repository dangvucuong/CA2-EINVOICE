using Contract.Service.User;
using Model.Table;
using Service.Base;

namespace Service.User
{
    public class UserRoleService : CRUDService<user_role>, IUserRoleService
    {
        public UserRoleService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.User.UserRole;
        }

        public Task<IEnumerable<user_role>> SelectByUserIdAsync(int user_id)
        {
            return _repositoryWrapper.User.UserRole.SelectByUserIdAsync(user_id);
        }
    }
}

