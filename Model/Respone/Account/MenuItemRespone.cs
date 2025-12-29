namespace Model.Respone.Account
{
    public class MenuItemRespone
    {
        public int id { get; set; }
        public string name { get; set; }
        public string name_en { get; set; }
        public string path { get; set; }
        public string icon { get; set; }
        public int menu_parent_id { get; set; }
        public bool is_active { get; set; }
        public List<MenuItemRespone> items { get; set; }
        public int alert_number { get; set; }
        public bool expanded { get; set; }
        public int sub_system_id { get; set; }
        public int menu_id
        {
            get; set;
        }
    }
}

