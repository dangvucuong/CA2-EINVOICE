namespace Contracts.Service.HoaDon.PushMessage
{
    public interface IPushMessageToVenderService
    {
        Task<bool> PushMessageAsync();
    }
}