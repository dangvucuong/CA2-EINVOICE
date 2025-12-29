using Model.Base;

namespace Model.Table
{
    public class loai_hoa_don_ct_template : modify_infor
    {
        public int id { get; set; }
        public int loai_hoa_don_ct_id { get; set; }
        public string name { get; set; }
        public string name_en { get; set; }
        public string path { get; set; }
        public bool is_active { get; set; }
        public string thumbnail {get;set;}
    }
}