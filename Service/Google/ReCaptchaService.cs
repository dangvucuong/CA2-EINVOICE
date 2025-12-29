using Microsoft.Extensions.Configuration;
using Model.Google;
using Model.Static;

namespace Service.Google
{
   public class ReCaptchaService
    {
        private readonly IConfiguration _configuration;
        public ReCaptchaService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public bool IsValidate(string token)
        {
            // return true;
            using (var client = new System.Net.WebClient())
            {

                // var SecretKey = _configuration["GoggleRecaptcha:SecretKey"];
                var SecretKey = AppSettings.GoogleRecaptcha.SecretKey;

                var GoogleReply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", SecretKey, token));

                var captchaResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<GoogleRecaptchaRespone>(GoogleReply);

                return captchaResponse.Success.ToLower() == "true";
            }
        }
    }
}