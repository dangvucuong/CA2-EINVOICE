using Contracts.Repository.Base;
using Model.Table;

namespace Contracts.Repository.User
{
    public interface IOTPRepository : ICRUDRepository<app_otp>
    {
        Task<app_otp> SelectByPhoneNumberOrEmailAsync(string donvi_ma_dv, string phone_number);
        Task<IEnumerable<app_otp>> SelectByPhoneNumberOrEmailAsync(string donvi_ma_dv, string phone_number, DateTime date);
    }
}