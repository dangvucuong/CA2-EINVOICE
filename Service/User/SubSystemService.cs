using Contract.Service.User;
using Model.Respone.SubSystem;
using Model.Table;
using Service.Base;

namespace Service.User
{
    public class SubSystemService : CRUDService<sub_system>, ISubSystemService
    {
        public SubSystemService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.User.SubSystem;
        }

        public Task<IEnumerable<SubSystemItemViewModel>> SelectAllViewModelAsync()
        {
            return _repositoryWrapper.User.SubSystem.SelectAllViewModelAsync();
        }

        public Task<IEnumerable<role_sub_system>> SelectByRoleAsync(int role_id)
        {
            return _repositoryWrapper.User.RoleSubSystem.SelectByRoleAsync(role_id);
        }
    }
}

