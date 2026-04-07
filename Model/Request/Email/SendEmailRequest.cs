namespace Model.Request.Email
{
    public class SendEmailRequest
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public IList<string> EmailAddress { get; set; }
        public bool isHtml { get; set; }
        public string SendByUser { get; set; }
        public SendEmailRequest()
        {

        }
        public SendEmailRequest(string Subject, string Body, IList<string> EmailAddress, bool isHtml = true, string SendByUser = "")
        {
            this.Subject = Subject;
            this.Body = Body;
            this.EmailAddress = EmailAddress;
            this.isHtml = isHtml;
            this.SendByUser = SendByUser;
        }
        public SendEmailRequest(string Subject, string Body, string EmailAddress, bool isHtml = true, string SendByUser = "")
        {
            this.Subject = Subject;
            this.Body = Body;
            this.EmailAddress = new List<string>(new string[] { EmailAddress });
            this.isHtml = isHtml;
            this.SendByUser = SendByUser;
        }
    }
}