using System.Net.Mail;
using System.Text.RegularExpressions;
using Contracts.Service.Core;
using Model.Base;
using Model.Request.Email;
using Model.Static;
using Service.Base;
using Common;
using System.Globalization;
using System.Net.Http;
using System.Text;

namespace Service.Core
{
    public class EmailService : BaseService, IEmailService
    {

        private static readonly HttpClient _httpClient;
        private static string taikhoan = "0103930279WEBHN";
        private static string matkhau = "2026CA2WEBHN920eab4b969a4afda9fae0da7469d668";

        public EmailService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<FunctionResult<bool>> SendEmailAsync(SendEmailRequest rq)
        {           
            // return new FunctionResultBase<bool>(true,"");
            string _FromEmailAddress = AppSettings.EmailConfig.FromEmailAddress;
            string _Host = AppSettings.EmailConfig.Host;
            int _Port = AppSettings.EmailConfig.Port;
            string _UserEmail = AppSettings.EmailConfig.UserEmail;
            string _PasswordEmail = AppSettings.EmailConfig.PasswordEmail;
            bool _EnableSsl = AppSettings.EmailConfig.EnableSsl;
            string _DisplayName = AppSettings.EmailConfig.DisplayName;
            string _FixedEmail = AppSettings.EmailConfig.FixedEmail;

            if (_FixedEmail != string.Empty)
            {
                for (int i = 0; i < rq.EmailAddress.Count(); i++)
                {
                    rq.EmailAddress[i] = _FixedEmail;
                }
            }
            var message = new MailMessage();
            var toAddress = new List<MailAddress>();
            message.From = new MailAddress(_FromEmailAddress, _DisplayName); ;
            foreach (var item in rq.EmailAddress)
            {
                if (IsValidEmail(item))
                {
                    message.To.Add(item);
                }
            }
            message.Subject = rq.Subject;
            message.Body = rq.Body;
            message.IsBodyHtml = rq.isHtml;

            if (message.To.Count > 0)
            {
                //var smtp = new SmtpClient(_Host)
                //{
                //    DeliveryMethod = SmtpDeliveryMethod.Network
                //};
                //smtp.Port = _Port;
                //smtp.UseDefaultCredentials = false;
                //smtp.Credentials = new System.Net.NetworkCredential(_FromEmailAddress, _PasswordEmail);
                //smtp.EnableSsl = _EnableSsl;
                //smtp.Send(message);
                var email_sended = new Model.Table.email_sended()
                {
                    body = rq.Body,
                    from_address = _FromEmailAddress,
                    send_at = DateTime.Now,
                    subject = rq.Subject,
                    to_address = message.To.Select(x => x.Address).Join(";"),
                    send_by_username = rq.SendByUser
                };               
                email_sended.id = await _repositoryWrapper.Core.EmailSended.InsertAsync(email_sended);
                string systemkey=Guid.NewGuid().ToString();
             
                return new FunctionResult<bool>(true, string.Empty);
            }
            return new FunctionResult<bool>(false, string.Empty);

        }
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public static string SendMailapi(string systemKey,string to,string cc,string subject,string bodyHtml,string displayName)
        {
            try
            {
                var payload = new
                {
                    taikhoan,
                    matkhau,
                    SystemKey = systemKey,
                    To = to,
                    Cc = cc,
                    Subject = subject,
                    BodyHtml = bodyHtml,
                    DisplayName = displayName
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = _httpClient
                    .PostAsync("api/SendMailCA2/SendMail", content)
                    .GetAwaiter()
                    .GetResult();

                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}