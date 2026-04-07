using Contracts.Repository.Base;
using Model.FuncResult;
using Model.Request.Base;
using Model.Table;

namespace Contracts.Repository.User
{
    public interface ILogRepository : ICRUDRepository<log>
    {
        Task<PagingResult<IEnumerable<log>>> SelectByDonViAsync(string donvi_ma_dv, PagingRequest request);
    }
}