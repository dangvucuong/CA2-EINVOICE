namespace Model.Request.Account
{
    public class RefreshTokenRequest
    {
        public string? access_token { get; set; }
        public string refresh_token { get; set; }
        public RefreshTokenRequest()
        {
        }
    }
}

