using Contracts.Repository.Base;
using Model.Respone.SubSystem;
using Model.Table;

namespace Contract.Repository.User
{
    public interface ISubSystemRepository : ICRUDRepository<sub_system>
    {
        Task<IEnumerable<sub_system>> SelectByUserAsync(int user_id);
        Task<IEnumerable<SubSystemItemViewModel>> SelectAllViewModelAsync();
    }
}

