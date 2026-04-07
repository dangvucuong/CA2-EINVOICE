using Contracts.Service.User;
using Model.FuncResult;
using Model.Request.Base;
using Model.Table;
using Service.Base;

namespace Service.User
{
    public class LogService : CRUDService<log>, ILogService
    {
        public LogService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.User.Log;
        }

        public Task<PagingResult<IEnumerable<log>>> SelectByDonViAsync(string donvi_ma_dv, PagingRequest request)
        {
            return _repositoryWrapper.User.Log.SelectByDonViAsync(donvi_ma_dv, request);
        }
    }
}