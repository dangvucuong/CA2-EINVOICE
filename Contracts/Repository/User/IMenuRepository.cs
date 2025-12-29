using Contracts.Repository.Base;
using Model.Table;

namespace Contract.Repository.User
{
    public interface IMenuRepository : ICRUDRepository<menu>
    {
        Task<IEnumerable<menu>> SelectByUserAsync(int user_id, int sub_system_id = 0);
    }
}

