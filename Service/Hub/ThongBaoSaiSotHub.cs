using Model.Enum;
using Model.Request.Hub;

namespace Service.Hub
{
    public class ThongBaoSaiSotHub : BaseHub
    {
        public async Task<bool> OnNewNotifyCreated(TBSSPhatHanhPushNotifyModel request)
        {
            var eventName = e_notify_hub_event.TBSS_HAS_RESULT.ToString();
            await SendMessageToUser(request.user_id.ToString(), eventName, request);
            return true;
        }
    }
}