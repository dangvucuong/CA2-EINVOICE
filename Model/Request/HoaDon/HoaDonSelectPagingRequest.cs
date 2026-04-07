using Model.Request.Base;
using Swashbuckle.AspNetCore.Annotations;

namespace Model.Request.HoaDon
{
    public class HoaDonSelectPagingRequest : PagingRequest
    {

        [SwaggerSchema(Description = "Lọc theo 1 hoặc nhiều trạng thái của hóa đơn.  NHAP = 1,DA_PHAT_HANH = 2,DA_HUY = 3,CHUA_CO_KET_QUA_PHAN_HOI = 4,DA_GUI_LEN_CQT_PHAN_HOI_KY_THUAT = 5,DA_GUI_LEN_CQT_CHUA_PHAN_HOI_KIEM_TRA_DU_LIEU = 6,KHONG_HOP_LE = 7,LOI_THONG_DIEP = 8,CHUA_GUI_CQT = 9. Ví dụ [1,2,3]")]
        public List<int> hoa_don_trang_thai_ids { get; set; }

        [SwaggerSchema(Description = "Loại hóa đơn. Xem danh sách loại hóa đơn tại [GET] /api/loai-hoa-don-ct")]
        public int? loai_hoa_don_ct_id { get; set; }
        [SwaggerSchema(Description = "HOA_DON_GOC = 1,HOA_DON_THAY_THE = 2,HOA_DON_DIEU_CHINH = 3,HOA_DON_BI_DIEU_CHINH = 4,HOA_DON_DA_HUY_NOI_BO = 5,HOA_DON_BI_THAY_THE = 6,HOA_DON_DA_THONG_BAO_GIAI_TRINH = 7,LOI_THONG_DIEP = 8,DA_GUI_TBSS_HUY = 9,")]
        public int? hoa_don_hinh_thuc_id { get; set; }
        [SwaggerSchema(Description = "Mẫu số hóa đơn")]
        public string? hoa_don_dang_ky_phat_hanh_mau_so { get; set; }
        [SwaggerSchema(Description = "Ký hiệu hóa đơn")]
        public string? hoa_don_dang_ky_phat_hanh_ky_hieu { get; set; }
        [SwaggerSchema(Description = "Lọc theo ngày hóa đơn từ ngày")]
        public DateTime? tu_ngay { get; set; }
        [SwaggerSchema(Description = "Lọc theo ngày hóa đơn đến ngày")]
        public DateTime? den_ngay { get; set; }
        [SwaggerSchema(Description = "Hóa đơn có mã= C, hóa đơn không mã=K, hóa đơn máy tính tiền= M")]
        public string? hoa_don_hinh_thuc_code { get; set; }
        [SwaggerSchema(Description = "Lọc theo MST người mua")]
        public string? nguoi_mua_mst { get; set; }
        [SwaggerSchema(Description = "Lọc theo mã đại lý")]
        public string? ma_dai_ly { get; set; }




        //     : number,
        //     hoa_don_dang_ky_phat_hanh_mau_so: string,
        //     hoa_don_dang_ky_phat_hanh_ky_hieu: string,
        //     tu_ngay?:string,
        //     den_ngay?:string       
    }
}