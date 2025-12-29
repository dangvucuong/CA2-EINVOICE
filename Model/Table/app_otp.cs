using Model.Base;

namespace Model.Table
{
    public class app_otp :  modify_infor
    {
        public int id { get; set; }
        public string phone_number { get; set; }
        public string email { get; set; }

        public string otp { get; set; }
        public DateTime expire_at { get; set; }
    }
}