using Model.Base;

namespace Model.Table
{
    public class user : modify_infor
    {
        public int id { get; set; }
        public string donvi_ma_dv { get; set; }
        public string? serial_number { get; set; }
        public string username { get; set; }
        public string full_name { get; set; }
        public string? email { get; set; }
        public string? title { get; set; }
        public bool is_active { get; set; }

        // [JsonIgnore]
        public string? password { get; set; }
        public string? serial_remote_signing_numner { get; set; }
        public bool? is_serial_remote_signing_verified { get; set; }

        public string? rs_ma_but_ky { get; set; }
        public string? vender_id { get; set; }
        public bool is_hsm_signing { get; set; }
        
    }
}

