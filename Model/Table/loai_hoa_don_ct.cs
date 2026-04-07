using Model.Base;

namespace Model.Table
{
    public class loai_hoa_don_ct : modify_infor
    {
        public int id { get; set; }
        public int loai_hoa_don_id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string name_en { get; set; }
        public string desription { get; set; }
        public string sort_idx { get; set; }
        public bool is_active { get; set; }
    }
}