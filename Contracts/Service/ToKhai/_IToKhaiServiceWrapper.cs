using Contracts.Service.Base;

namespace Contracts.Service.ToKhai
{
    public interface IToKhaiSerivceWrapper:IBaseService
    {
        IToKhaiLogService ToKhaiLog { get; }
        IToKhaiLogTypeService ToKhaiLogType { get; }
        IToKhaiStatusService ToKhaiStatus { get; }
        IToKhaiService ToKhai { get; }
    }
}