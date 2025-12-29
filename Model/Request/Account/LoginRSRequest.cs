namespace Model.Request.Account
{
    public class LoginRSRequest
    {
   
        public string rs_ma_but_ky { get; set; }
        public string? session_id { get; set; }
        public string? reCaptchaToken { get; set; }
        public LoginRSRequest()
        {
        }
    }
}

