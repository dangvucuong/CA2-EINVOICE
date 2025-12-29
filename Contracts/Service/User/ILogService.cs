using Contracts.Service.Base;
using Model.FuncResult;
using Model.Request.Base;
using Model.Table;

namespace Contracts.Service.User
{
    public interface ILogService : ICRUDService<log>
    {
        Task<PagingResult<IEnumerable<log>>> SelectByDonViAsync(string donvi_ma_dv, PagingRequest request);

    }
}