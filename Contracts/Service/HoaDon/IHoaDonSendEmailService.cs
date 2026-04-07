using Contracts.Service.Base;
using Model.Base;
using Model.Request.ToKhai;

namespace Contracts.Service.HoaDon
{
    public interface IHoaDonSendEmailService : IBaseService
    {
        Task<FunctionResult<bool>> SendEmailHoaDonAsync(List<int> hoaDonIds, bool isCheckSendBienBan=false);
        Task<FunctionResult<bool>> SendEmailHoaDonAsync(HoaDonSendEmailCustomRequest request);

    }
}