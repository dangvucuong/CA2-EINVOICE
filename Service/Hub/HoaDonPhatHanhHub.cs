using Model.Enum;
using Model.Request.Hub;

namespace Service.Hub
{
    public class HoaDonPhatHanhHub : BaseHub
    {
        public async Task<bool> OnNewNotifyCreated(HoaDonPhatHanhPushNotifyModel request)
        {
            var eventName = e_notify_hub_event.THONG_DIEP_HAS_RESULT.ToString();
            await SendMessageToUser(request.user_id.ToString(), eventName, request);
            return true;
        }
        public async Task<bool> OnTBSSNotifyCreated(TBSSPhatHanhPushNotifyModel request)
        {
            var eventName = e_notify_hub_event.TBSS_HAS_RESULT.ToString();
            await SendMessageToUser(request.user_id.ToString(), eventName, request);
            return true;
        }
        public async Task<bool> OnToKhaiNotifyCreated(TBSSPhatHanhPushNotifyModel request)
        {
            var eventName = e_notify_hub_event.TOKHAI_HAS_RESULT.ToString();
            await SendMessageToUser(request.user_id.ToString(), eventName, request);
            return true;
        }
        public async Task<bool> OnRemoteSigningSuccess(RemoteSigningSuccess request)
        {
            try
            {
                var eventName = e_notify_hub_event.REMOTE_SIGNING_SUCCESS.ToString();
                await SendMessageToUser(request.user_id.ToString(), eventName, request);
                return true;
            }
            catch (System.Exception ex)
            {
                return false;

            }
        }
    }
}