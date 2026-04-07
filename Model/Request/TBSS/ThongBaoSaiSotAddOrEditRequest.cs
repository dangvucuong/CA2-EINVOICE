using Model.Table;

namespace Model.Request.TBSS
{
    public class ThongBaoSaiSotAddOrEditRequest : thong_bao_sai_sot
    {
        public List<thong_bao_sai_sot_chi_tiet> thong_bao_sai_sot_chi_tiets { get; set; }
        
        
    }
}