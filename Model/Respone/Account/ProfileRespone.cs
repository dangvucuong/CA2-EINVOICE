using Model.Table;

namespace Model.Respone.Account
{
    public class ProfileRespone
    {
        public int user_id { get; set; }
        public string username { get; set; }
        public string full_name { get; set; }
        public string email { get; set; }
        public string donvi_ma_dv { get; set; }
        public string serial_number { get; set; }
        public string vender_id { get; set; }
        public bool is_hsm_signing { get; set; }
        public bool is_remote_signing { get; set; }
        public List<sub_system> sub_systems { get; set; }
        public List<MenuItemRespone> menus { get; set; }
        public List<api> apis { get; set; }
        public List<role> roles { get; set; }
        public donvi donvi { get; set; }
        public string? serial_remote_signing_numner { get; set; }
        public bool? is_serial_remote_signing_verified { get; set; }
    }

}

