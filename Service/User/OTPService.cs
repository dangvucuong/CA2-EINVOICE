using Contracts.Service.User;
using Model.Table;
using Service.Base;

namespace Service.User
{
    public class OTPService : CRUDService<app_otp>, IOTPService
    {
        public OTPService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.User.OTP;
        }

        public Task<app_otp> SelectByPhoneNumberOrEmailAsync(string donvi_ma_dv, string phone_number)
        {
            return _repositoryWrapper.User.OTP.SelectByPhoneNumberOrEmailAsync(donvi_ma_dv,phone_number);
        }

        public Task<IEnumerable<app_otp>> SelectByPhoneNumberOrEmailAsync(string donvi_ma_dv, string phone_number, DateTime date)
        {
            return _repositoryWrapper.User.OTP.SelectByPhoneNumberOrEmailAsync(donvi_ma_dv,phone_number, date);
        }
    }
}