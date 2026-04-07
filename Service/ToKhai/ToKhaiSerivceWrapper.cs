using Contracts.Service.ToKhai;
using Service.Base;

namespace Service.ToKhai
{
    public class ToKhaiSerivceWrapper : BaseService, IToKhaiSerivceWrapper
    {
        private IToKhaiLogService _toKhaiLogService;
        private IToKhaiLogTypeService _toKhaiLogTypeService;
        private IToKhaiStatusService _toKhaiStatusService;
        private IToKhaiService _toKhaiService;
        public ToKhaiSerivceWrapper(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public IToKhaiLogService ToKhaiLog => _toKhaiLogService??= new ToKhaiLogService(_serviceProvider);

        public IToKhaiLogTypeService ToKhaiLogType => _toKhaiLogTypeService??= new ToKhaiLogTypeService(_serviceProvider);

        public IToKhaiStatusService ToKhaiStatus => _toKhaiStatusService??= new ToKhaiStatusService(_serviceProvider);

        public IToKhaiService ToKhai => _toKhaiService??= new ToKhaiService(_serviceProvider);
    }
}