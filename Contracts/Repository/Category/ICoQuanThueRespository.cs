using Contracts.Repository.Base;
using Model.Table;

namespace Contracts.Repository.Category
{
    public interface ICoQuanThueRespository :ICRUDRepository<co_quan_thue>
    {
        Task<co_quan_thue> SelectByMaAsync(string ma_cqt);
    }
}