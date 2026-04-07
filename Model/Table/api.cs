using Model.Base;

namespace Model.Table
{
    public class api : modify_infor
    {
        public int id { get; set; }
        public int menu_id { get; set; }
        public string method { get; set; }
        public string endpoint { get; set; }
        public string description { get; set; }
        public bool is_allow_anonymous { get; set; }
        public bool is_check_login { get; set; }
        public bool is_check_authorization { get; set; }
        public bool is_active { get; set; }
        public string sort_idx { get; set; }
        public bool is_privte { get; set; }
    }
}

