using Contracts.Service.Base;
using Model.Table;

namespace Contracts.Service.Category
{
    public interface ICoQuanThueService : ICRUDService<co_quan_thue>
    {
        Task<co_quan_thue> SelectByMaAsync(string ma_cqt);
    }
}