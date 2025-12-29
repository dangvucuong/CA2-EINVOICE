using Contracts.Repository.Base;
using Model.Respone.Api;
using Model.Table;

namespace Contract.Repository.User
{
    public interface IApiRepository : ICRUDRepository<api>
    {
        Task<IEnumerable<api>> SelectByUserAsync(int user_id, int sub_system_id = 0);
        Task<IEnumerable<ApiItemViewModel>> SelectAllViewModelAsync(int sub_system_id);
        Task<IEnumerable<api>> SelectBySubSystemAsync(int sub_system_id);
    }
}

