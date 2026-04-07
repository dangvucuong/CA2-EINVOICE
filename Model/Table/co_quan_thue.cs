using Model.Base;

namespace Model.Table
{
    public class co_quan_thue : modify_infor
    {
        public int id { get; set; }
        public string ma_cqt { get; set; }
        public string ma_cqt_ql { get; set; }
        public string tinh { get; set; }
        public string ten_viet_tat { get; set; }
        public string ten { get; set; }
        public string dia_chi { get; set; }
        public int co_quan_thue_trang_thai_id { get; set; }
    }
}