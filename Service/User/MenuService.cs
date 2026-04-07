using Contracts.Service.User;
using Model.Table;
using Service.Base;

namespace Service.User
{
    public class MenuService : BaseService, IMenuService
    {
        public MenuService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<IEnumerable<menu>> SelectBySubSystemAsync(int sub_system_id)
        {
            var list = await _repositoryWrapper.User.Menu.SelectAllAsync();
            return list.Where(x => x.sub_system_id == sub_system_id).ToList();
        }
    }
}