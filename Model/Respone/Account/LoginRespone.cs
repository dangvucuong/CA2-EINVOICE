namespace Model.Respone.Account
{
    public class LoginRespone
    {
        public TokenInfo token_info { get; set; }
        public ProfileRespone profile { get; set; }
        public bool is_verify_cert { get; set; }
    }
}

