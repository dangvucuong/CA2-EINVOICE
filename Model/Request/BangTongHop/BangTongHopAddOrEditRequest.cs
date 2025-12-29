using Model.Table;

namespace Model.Request.BangTongHop
{
    public class BangTongHopAddOrEditRequest: bang_tong_hop_du_lieu
    {
        public List<int> hoa_don_ids { get; set; }
        public List<hoa_don> hoa_dons { get; set; }
        
        
        
        
    }
}