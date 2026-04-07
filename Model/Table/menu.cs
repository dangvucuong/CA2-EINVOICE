using Model.Base;

namespace Model.Table
{
    public class menu : modify_infor
    {
        public int id { get; set; }
        public int sub_system_id { get; set; }
        public int menu_id_parent { get; set; }
        public string name { get; set; }
        public string name_en { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
        public string path { get; set; }
        public bool is_active { get; set; }
        public string sort_idx { get; set; }
    }
}