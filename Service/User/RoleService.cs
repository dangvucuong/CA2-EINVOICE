using Contract.Service.User;
using Model.Respone.Role;
using Model.Table;
using Service.Base;

namespace Service.User
{
    public class RoleService : CRUDServiceWithCache<role>, IRoleService
    {
        public RoleService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.User.Role;
        }

        public Task<IEnumerable<RoleViewModel>> SelectAllViewModalAsync()
        {
            return _repositoryWrapper.User.Role.SelectAllViewModalAsync();
        }

        protected override void ConfigKey()
        {
            this._itemKeyField = "id";
            this._keyPrefix = "role:";
        }
    }
}

