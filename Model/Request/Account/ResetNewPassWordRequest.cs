namespace Model.Request.Account
{
    public class ResetNewPassWordRequest
    {
        public string donvi_ma_dv { get; set; }
        public string email { get; set; }
        public string otp { get; set; }
    }
}