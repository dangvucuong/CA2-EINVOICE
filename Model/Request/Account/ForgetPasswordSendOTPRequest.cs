namespace Model.Request.Account
{
    public class ForgetPasswordSendOTPRequest
    {
        public string donvi_ma_dv { get; set; }
        public string email { get; set; }
        public string? reCaptchaToken { get; set; }
    }
}