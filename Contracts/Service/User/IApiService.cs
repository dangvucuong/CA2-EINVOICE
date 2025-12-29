using Contracts.Service.Base;
using Model.Respone.Api;
using Model.Table;

namespace Contracts.Service.User
{
    public interface IApiService : IBaseService, ICRUDService<api>
    {
        Task<IEnumerable<ApiItemViewModel>> SelectAllViewModelAsync(int sub_system_id);
        Task<IEnumerable<api>> SelectBySubSystemAsync(int sub_system_id);
        
    }
}