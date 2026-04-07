using Model.Table;

namespace Model.Respone.Api
{
    public class ApiItemViewModel : api
    {
        public int menu_group_id { get; set; }
        public string menu_group_name { get; set; }
        public int menu_id { get; set; }
        public string menu_name { get; set; }
        
    }
}