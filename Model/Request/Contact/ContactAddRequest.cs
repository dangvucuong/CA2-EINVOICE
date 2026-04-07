using Model.Table;

namespace Model.Request.Contact
{
    public class ContactAddRequest:contact
    {
        public string? reCaptchaToken { get; set; }
    }
}