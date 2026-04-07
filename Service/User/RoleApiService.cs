using Contracts.Service.User;
using Model.Table;
using Service.Base;

namespace Service.User
{
    public class RoleApiService : CRUDServiceWithCache<role_api>, IRoleApiService
    {
        public RoleApiService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.User.RoleApi;
        }

        public Task<IEnumerable<role_api>> SelectByRoleAsync(int role_id, int sub_system_id)
        {
            return _repositoryWrapper.User.RoleApi.SelectByRoleAsync(role_id, sub_system_id);
        }

        protected override void ConfigKey()
        {
            this._keyPrefix = "role_api:";
            this._itemKeyField = "id";
        }
    }
}