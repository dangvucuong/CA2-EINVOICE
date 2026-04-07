using Contracts.Service.Base;
using Model.Respone.Notify;

namespace Contracts.Service.Notify
{
    public interface INotifySerivce:IBaseService
    {
        Task<NotifySummaryRespone> SelectNotifySummaryAsync(int user_id);
    }
}