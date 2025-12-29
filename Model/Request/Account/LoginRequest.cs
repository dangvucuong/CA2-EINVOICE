using Swashbuckle.AspNetCore.Annotations;

namespace Model.Request.Account
{
    public class LoginRequest
    {
        [SwaggerSchema(Description = "Mã số thuế của đơn vị")]
        public string donvi_ma_dv { get; set; }
        [SwaggerSchema(Description = "Username đăng nhập")]
        public string username { get; set; }
        [SwaggerSchema(Description = "Mật khẩu")]
        public string password { get; set; }
        [SwaggerSchema(Description = "reCaptchaToken= 6LfRAYApAAAAAO6tYfzdtFmuRNOtCOH3BxTy9pCM")]
        public string? reCaptchaToken { get; set; }
        public LoginRequest()
        {
        }
    }
}

