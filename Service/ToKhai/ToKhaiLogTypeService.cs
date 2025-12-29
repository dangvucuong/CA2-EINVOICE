using Contracts.Service.ToKhai;
using Model.Table;
using Service.Base;

namespace Service.ToKhai
{
    public class ToKhaiLogTypeService : CRUDService<to_khai_log_type>, IToKhaiLogTypeService
    {
        public ToKhaiLogTypeService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase =_repositoryWrapper.ToKhaiWrapper.ToKhaiLogType;
        }
    }
}