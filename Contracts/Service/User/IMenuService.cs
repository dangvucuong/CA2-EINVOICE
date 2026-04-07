using Contracts.Service.Base;
using Model.Table;

namespace Contracts.Service.User
{
    public interface IMenuService : IBaseService
    {
        Task<IEnumerable<menu>> SelectBySubSystemAsync(int sub_system_id);
    }
}