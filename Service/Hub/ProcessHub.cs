using Model.Enum;
using Model.Request.Hub;

namespace Service.Hub
{
    public class ProcessHub : BaseHub
    {
        public async Task<bool> OnProcessChangedAsync(ProcessChangedModel request)
        {
            try
            {
                var eventName = e_notify_hub_event.PROCESS_CHANGED.ToString();
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