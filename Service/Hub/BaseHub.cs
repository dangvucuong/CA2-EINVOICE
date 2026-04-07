using Common;
using Microsoft.AspNetCore.SignalR;

namespace Service.Hub
{
    public class CustomUserIdProvider : Microsoft.AspNetCore.SignalR.IUserIdProvider
    {
        public CustomUserIdProvider()
        {

        }
        public string GetUserId(HubConnectionContext connection)
        {
            // var connectionId = connection.ConnectionId;
            var httpContext = connection.GetHttpContext();
            var userId = httpContext.Request.Query["userId"].ToString();
            return userId;
        }
    }
    public class BaseHub : Microsoft.AspNetCore.SignalR.Hub
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">Để trống thì gửi all thay vì gửi đến user cụ thể</param>
        /// <param name="eventName"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public async Task<bool> SendMessageToUser(string userId, string eventName, object payload)
        {
            try
            {
                if (Clients != null)
                {
                    if (userId.ConvertToString() == "")
                    {
                        await Clients.All.SendAsync(eventName, payload);
                    }
                    else
                    {
                        var proxy = Clients.User(userId).SendAsync(eventName, payload);
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                ex.SaveLog();
                return false;
            }
        }
    }
}