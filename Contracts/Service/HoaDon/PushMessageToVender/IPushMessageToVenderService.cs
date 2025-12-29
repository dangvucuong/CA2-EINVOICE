using Contracts.Service.Base;
using Model.Request.HoaDon.PushMessageToVender;
using Model.Table;

namespace Contracts.Service.HoaDon.PushMessageToVender
{
    public interface IPushMessageToVenderService:IBaseService
    {
        Task<bool> PushMessageAsync(PushMessageToVenderRequest request);
        Task<bool> CheckAndPushMessageAsync(hoa_don hoaDon);
    }
}