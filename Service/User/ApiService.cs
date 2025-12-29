using Contracts.Service.User;
using Model.Respone.Api;
using Model.Table;
using Service.Base;

namespace Service.User
{
    public class ApiService : CRUDServiceWithCache<api>, IApiService
    {
        public ApiService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase =_repositoryWrapper.User.Api;
        }

        public Task<IEnumerable<ApiItemViewModel>> SelectAllViewModelAsync(int sub_system_id)
        {
            return _repositoryWrapper.User.Api.SelectAllViewModelAsync(sub_system_id);
        }

        public Task<IEnumerable<role_api>> SelectByRoleAsync(int role_id, int sub_system_id)
        {
            return _repositoryWrapper.User.RoleApi.SelectByRoleAsync(role_id, sub_system_id);
        }

        public Task<IEnumerable<api>> SelectBySubSystemAsync(int sub_system_id)
        {
            return _repositoryWrapper.User.Api.SelectBySubSystemAsync(sub_system_id);
        }

        protected override void ConfigKey()
        {
            this._keyPrefix = "api:";
            this._itemKeyField = "id";
        }
    }
}