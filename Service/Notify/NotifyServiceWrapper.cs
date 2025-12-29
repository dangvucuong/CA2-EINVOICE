using Contracts.Service.Notify;
using Service.Base;

namespace Service.Notify
{
    public class NotifyServiceWrapper : BaseService, INotifyServiceWrapper
    {
        private INotifySerivce _notifySerivce;
        public NotifyServiceWrapper(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public INotifySerivce Notify => _notifySerivce??= new NotifySerivce(_serviceProvider);
    }
}