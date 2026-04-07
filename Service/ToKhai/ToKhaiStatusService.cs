using Contracts.Service.ToKhai;
using Model.Table;
using Service.Base;

namespace Service.ToKhai
{
    public class ToKhaiStatusService : CRUDService<to_khai_status>, IToKhaiStatusService
    {
        public ToKhaiStatusService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase =_repositoryWrapper.ToKhaiWrapper.ToKhaiStatus;
        }
    }
}