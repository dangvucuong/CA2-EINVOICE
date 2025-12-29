using Contracts.Service.Base;

namespace Contracts.Service.Notify
{
    public interface INotifyServiceWrapper : IBaseService
    {
        INotifySerivce Notify { get; }
    }
}