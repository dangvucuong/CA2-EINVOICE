using Contracts.Service.ToKhai;
using Model.Table;
using Service.Base;

namespace Service.ToKhai
{
    public class ToKhaiLogService : CRUDService<to_khai_log>, IToKhaiLogService
    {
        public ToKhaiLogService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.ToKhaiWrapper.ToKhaiLog;
        }

        public Task<IEnumerable<to_khai_log>> SelectByToKhaiAsync(int to_khai_id)
        {
            return _repositoryWrapper.ToKhaiWrapper.ToKhaiLog.SelectByToKhaiAsync(to_khai_id);
        }

        public Task<IEnumerable<to_khai_log>> SelectByToKhaiIdsAsync(List<int> to_khai_ids)
        {
           return _repositoryWrapper.ToKhaiWrapper.ToKhaiLog.SelectByToKhaiIdsAsync(to_khai_ids);
        }
    }
}