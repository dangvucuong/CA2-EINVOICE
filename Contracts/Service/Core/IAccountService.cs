using Contracts.Service.Base;
using Model.Base;
using Model.Request.Account;
using Model.Respone.Account;
using Model.Table;

namespace Contract.Service.Core
{
    public interface IAccountService : IBaseService
    {
        Task<FunctionResult<LoginRespone>> LoginAsync(LoginRequest request);
        Task<FunctionResult<LoginRespone>> LoginAsync(LoginSerialRequest request);
        Task<FunctionResult<LoginRespone>> LoginAsync(LoginRSRequest request);
        Task<FunctionResult<string>> LoginRSAsync(LoginRSRequest request);
        Task<FunctionResult<LoginRespone>> LoginRSGetResultAsync(string uuid);
        Task<ProfileRespone?> GetProfileAsync(int user_id, int sub_system_id = 0);
        Task<TokenInfo?> RefreshTokenAsync(RefreshTokenRequest request);
        Task<FunctionResult<SendOTPRespone>> SendOTPForgetPWAsync(ForgetPasswordSendOTPRequest request);
        Task<FunctionResult<bool>> ResetNewPWAsync(ResetNewPassWordRequest request);
        Task<FunctionResult<bool>> ChangePWAsync(ChangePassWordRequest request);
        Task<bool> ChangePWAsync(int user_id, string newPW);
        Task<FunctionResult<bool>> DeletePasskeyAsync(LoginRequest request);
        Task<FunctionResult<bool>> XuLyThongDiepKyRSAsync(rs_yeu_cau_ky model);
    }
}

