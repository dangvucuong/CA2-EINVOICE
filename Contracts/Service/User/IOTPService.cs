using Contracts.Service.Base;
using Model.Table;

namespace Contracts.Service.User
{
    public interface IOTPService : ICRUDService<app_otp>
    {
        Task<app_otp> SelectByPhoneNumberOrEmailAsync(string donvi_ma_dv, string phone_number_or_email);
        Task<IEnumerable<app_otp>> SelectByPhoneNumberOrEmailAsync(string donvi_ma_dv,string phone_number_or_email, DateTime date);
    }
}