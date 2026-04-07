using Model.Respone.Upload;

namespace Model.Request.HoaDon
{
    public class HoaDonImportRequest : UploadRespone
    {
        public int loai_hoa_don_ct_id { get; set; }
        public string? hoa_don_dang_ky_phat_hanh_mau_so { get; set; }
        public string? hoa_don_dang_ky_phat_hanh_ky_hieu { get; set; }
        public string? ten_hoa_don { get; set; }
        public string? template { get; set; }

        public string? importType { get; set; }



    }
}