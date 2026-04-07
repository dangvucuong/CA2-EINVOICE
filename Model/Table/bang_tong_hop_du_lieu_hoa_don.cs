using Model.Base;

namespace Model.Table
{
    public class bang_tong_hop_du_lieu_hoa_don : modify_infor
    {
        public int id { get; set; }
        public int bang_tong_hop_du_lieu_id { get; set; }
        public int hoa_don_id { get; set; }
    }
}