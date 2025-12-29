namespace Model.Respone.Account
{
    public class SendOTPRespone
    {
        public string email { get; set; }
        public DateTime expire_at { get; set; }
    }
}