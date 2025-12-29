using System.Threading.Tasks;
using Common;
using Contract.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Request.Account;
using Model.Static;
using Service.Google;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : BaseController
    {
        private readonly ReCaptchaService _reCaptchaService;

        public AccountController(IServiceWrapper serviceWrapper, ReCaptchaService reCaptchaService) : base(serviceWrapper)
        {
            this._reCaptchaService = reCaptchaService;
        }
        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <remarks>
        ///Sau khi đăng nhập thành công, hệ thống trả về access_token và refresh_token.
        ///Sử dụng acces_token để xác thực khi gọi các api khác.
        ///acces_token là JWT Token, sẽ hết hạn sau 1 thời gian chỉ định. Cần kiểm tra token còn hạn hay không để đăng nhập lại hoặc sử dụng refresh_token để cấp phát access_token mới.
        ///refresh token mới bằng api [POST]token/refresh.
        /// </remarks>
        [HttpPost]
        [Route("login")]
        public async Task<ContentResult> LoginAsync([FromBody] LoginRequest request)
        {

            if (request.reCaptchaToken.ConvertToString() != AppSettings.FixedValue.EcoSystemToken)
            {
                var isValid = _reCaptchaService.IsValidate(request.reCaptchaToken);
                if (!isValid) return this.BadRequest();
            }

            var result = await _serviceWrapper.Core.Account.LoginAsync(request);
            if (result.is_success)
            {
                await this.SaveLogAsync($"Đăng nhập bằng username", new
                {
                    donvi_ma_dv = request.donvi_ma_dv,
                    username = request.username
                }, new Model.Respone.Account.JwtTokenInfo()
                {
                    donvi_ma_dv = request.donvi_ma_dv,
                    username = request.username
                });
            }

            return result.is_success ? this.OK(result.data) : this.BadRequest(result.message);
        }
        [HttpPost]
        [Route("pass-key/delete")]
        public async Task<ContentResult> DeletePasskeyAsync([FromBody] LoginRequest request)
        {
            if (request.reCaptchaToken.ConvertToString() != AppSettings.FixedValue.EcoSystemToken)
            {
                var isValid = _reCaptchaService.IsValidate(request.reCaptchaToken);
                if (!isValid) return this.BadRequest();
            }
            var result = await _serviceWrapper.Core.Account.DeletePasskeyAsync(request);
            return result.is_success ? this.OK(result.data) : this.BadRequest(result.message);
        }
        [HttpPost]
        [Route("login-mst")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ContentResult> LoginMSTAsync([FromBody] LoginSerialRequest request)
        {

            var result = await _serviceWrapper.Core.Account.LoginAsync(request);
            if (result.is_success)
            {
                await this.SaveLogAsync($"Đăng nhập bằng Serial", new
                {
                    donvi_ma_dv = request.mst,
                    serial = request.serial
                }, new Model.Respone.Account.JwtTokenInfo()
                {
                    donvi_ma_dv = request.mst,
                    username = request.serial
                });
            }
            return result.is_success ? this.OK(result.data) : this.BadRequest(result.message);
        }
        [HttpPost]
        [Route("login-rs")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ContentResult> LoginRSAsync([FromBody] LoginRSRequest request)
        {
            if (request.reCaptchaToken.ConvertToString() != AppSettings.FixedValue.EcoSystemToken)
            {
                var isValid = _reCaptchaService.IsValidate(request.reCaptchaToken);
                if (!isValid) return this.BadRequest();
            }
            var result = await _serviceWrapper.Core.Account.LoginRSAsync(request);
            if (result.is_success)
            {
                // await this.SaveLogAsync($"Đăng nhập bằng Remote Signing", new
                // {
                //     donvi_ma_dv = result.data.profile.donvi_ma_dv,
                //     ma_but_ky = request.rs_ma_but_ky
                // }, new Model.Respone.Account.JwtTokenInfo()
                // {
                //     donvi_ma_dv = result.data.profile.donvi_ma_dv,
                //     username = result.data.profile.serial_number
                // });
            }
            return result.is_success ? this.OK(result.data) : this.BadRequest(result.message);
        }
        [HttpGet]
        [Route("login-rs/{uuid}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ContentResult> LoginRSGetResultAsync(string uuid)
        {

            var result = await _serviceWrapper.Core.Account.LoginRSGetResultAsync(uuid);
            return result.is_success ? this.OK(result.data) : this.BadRequest(result.message);
        }
        /// <summary>
        /// Gửi OTP để reset mật khẩu trường hợp quên mật khẩu
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [HttpPost]
        [Route("forget-pw/send-otp")]
        public async Task<ContentResult> ForgetPWSendOTPAsync([FromBody] ForgetPasswordSendOTPRequest request)
        {
            if (request.reCaptchaToken.ConvertToString() != AppSettings.FixedValue.EcoSystemToken)
            {
                var isValid = _reCaptchaService.IsValidate(request.reCaptchaToken);
                if (!isValid) return this.BadRequest();
            }
            // var isValid = _reCaptchaService.IsValidate(request.reCaptchaToken);
            // if (!isValid) return this.BadRequest();
            var result = await _serviceWrapper.Core.Account.SendOTPForgetPWAsync(request);
            return result.is_success ? this.OK(result.data) : this.BadRequest(result.message);
        }
        /// <summary>
        /// Reset mật khẩu mới
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [HttpPost]
        [Route("reset-pw")]
        [MustLogged]
        public async Task<ContentResult> ResetPWAsync([FromBody] ResetNewPassWordRequest request)
        {
            var result = await _serviceWrapper.Core.Account.ResetNewPWAsync(request);
            return result.is_success ? this.OK(result.data) : this.BadRequest(result.message);
        }
        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [HttpPost]
        [Route("change-pw")]

        public async Task<ContentResult> ChangePWAsync([FromBody] ChangePassWordRequest request)
        {
            var result = await _serviceWrapper.Core.Account.ChangePWAsync(request);
            return result.is_success ? this.OK(result.data) : this.BadRequest(result.message);
        }
        [HttpGet]
        [Route("info")]
        [MustLogged]
        public async Task<ContentResult> GetInfoAsync()
        {
            var userId = this.GetUserId();
            var userInfo = await _serviceWrapper.Core.Account.GetProfileAsync(userId);
            return userInfo != null ? this.OK(userInfo) : this.BadRequest();
        }
        [HttpGet]
        [Route("info/sub-system/{sub_system_id}")]
        [AllowAnonymous]
        public async Task<ContentResult> GetInfoAsync(int sub_system_id)
        {
            var userId = this.GetUserId();
            var userInfo = await _serviceWrapper.Core.Account.GetProfileAsync(userId, sub_system_id);
            return userInfo != null ? this.OK(userInfo) : this.BadRequest();
        }
        /// <summary>
        /// Refresh token mới
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [HttpPost]
        [Route("token/refresh")]
        public async Task<ContentResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var tokenInfo = await _serviceWrapper.Core.Account.RefreshTokenAsync(request);
            return tokenInfo != null ? this.OK(tokenInfo) : this.BadRequest();
        }


    }
}

