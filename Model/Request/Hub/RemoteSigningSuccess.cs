using Model.Respone.Account;

namespace Model.Request.Hub
{
    public class RemoteSigningSuccess
    {
        public string user_id { get; set; }
        public string request_code { get; set; }
        public LoginRespone data { get; set; }

    }
}