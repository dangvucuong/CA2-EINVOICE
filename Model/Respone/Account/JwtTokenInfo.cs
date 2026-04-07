namespace Model.Respone.Account
{
    public class JwtTokenInfo
    {
        public int id { get; set; }
        public string username { get; set; }
        public string full_name { get; set; }
        public string donvi_ma_dv { get; set; }
        public string vender_id { get; set; }
        public bool is_hsm_signing { get; set; }
        public bool is_remote_signing { get; set; }
        
    }
}

