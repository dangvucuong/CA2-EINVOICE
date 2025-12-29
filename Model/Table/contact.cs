using Model.Base;

namespace Model.Table
{
    public class contact : modify_infor
    {
        public int id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string tax_code { get; set; }
        public string? info { get; set; }
        public string? serial { get; set; }
        public DateTime register_at { get; set; }
        public int contact_status_id { get; set; }
        public int company_size_id { get; set; }
        public string? note { get; set; }
    }
}