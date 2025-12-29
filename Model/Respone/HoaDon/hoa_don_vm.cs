using Model.Table;

namespace Model.Respone.HoaDon
{
    public class hoa_don_vm : hoa_don
    {
        public List<hoa_don_hang_hoa> hang_hoas { get; set; }
        public List<hoa_don_loai_phi> loai_phis { get; set; }
        public string link { get; set; }
        
        
    }
}