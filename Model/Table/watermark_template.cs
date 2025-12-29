using Model.Base;

namespace Model.Table
{
    public class watermark_template : modify_infor
    {
        public int id { get; set; }
        public string name { get; set; }
        public string small_size_url { get; set; }
        public string url { get; set; }
        public int watermark_template_type_id { get; set; }
        
        
    }
}