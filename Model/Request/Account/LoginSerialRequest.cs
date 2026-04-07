namespace Model.Request.Account
{
    public class LoginSerialRequest
    {
        public string mst { get; set; }
        public string serial { get; set; }
        public string? signed_text { get; set; }
        public LoginSerialRequest()
        {
        }
    }
}

