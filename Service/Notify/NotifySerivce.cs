using Contracts.Service.Notify;
using Model.Enum;
using Model.Respone.Notify;
using Service.Base;

namespace Service.Notify
{
    public class NotifySerivce : BaseService, INotifySerivce
    {
        public NotifySerivce(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<NotifySummaryRespone> SelectNotifySummaryAsync(int user_id)
        {
            var register_new_count = 0;
            if (this.IsUserCanAccessApi("GET", "api/contact"))
            {
                register_new_count = await _serviceWrapper.Contact.Contact.SelectCountContactByStatusAsync((int)e_contact_status.NEW);
            }
            return new NotifySummaryRespone()
            {
                register_new_count = register_new_count
            };
        }
    }
}