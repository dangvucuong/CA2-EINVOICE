using Contracts.Service.Base;
using Model.Respone.SubSystem;
using Model.Table;

namespace Contract.Service.User
{
    public interface ISubSystemService : ICRUDService<sub_system>
    {
        Task<IEnumerable<SubSystemItemViewModel>> SelectAllViewModelAsync();
        
    }
}

