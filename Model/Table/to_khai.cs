using Model.Base;
using Swashbuckle.AspNetCore.Annotations;

namespace Model.Table
{
    public class to_khai : modify_infor
    {
        [SwaggerSchema(Description = "id tự tăng của tờ khai")]
        public int id { get; set; }
        [SwaggerSchema(Description = "MST của đơn vị")]
        public string donvi_ma_dv { get; set; }
        [SwaggerSchema(Description = "Trạng thái của tờ khai  TAO_MOI = 1,CHO_CQT = 2,CQT_TU_CHOI = 3,CQT_DONG_Y = 4")]
        public int to_khai_status_id { get; set; }
        [SwaggerSchema(Description = "Loại tờ khai, DANG_KY_MOI = 1,THAY_DOI_THONG_TIN = 2")]
        public int loai_to_khai_id { get; set; }
        [SwaggerSchema(Description = "Mã tờ khai")]
        public string ma_to_khai { get; set; }
        [SwaggerSchema(Description = "Ngày lập")]
        public DateTime ngay_lap { get; set; }
        [SwaggerSchema(Description = "MST của đơn vị")]
        public string mst { get; set; }
        [SwaggerSchema(Description = " Người nộp thuế")]
        public string nguoi_nop_thue { get; set; }
        [SwaggerSchema(Description = " Người liên hệ")]
        public string nguoi_lien_he { get; set; }
        [SwaggerSchema(Description = "Cơ quan thuế")]
        public string co_quan_thue { get; set; }
        [SwaggerSchema(Description = "Địa chỉ liên hệ")]
        public string dia_chi_lien_he { get; set; }
        [SwaggerSchema(Description = "Email liên hệ")]
        public string email_lien_he { get; set; }
        [SwaggerSchema(Description = "Điện thoại liên hệ")]
        public string dien_thoai_lien_he { get; set; }
        [SwaggerSchema(Description = "Là hóa đơn có mã CQT")]
        public bool is_hoadon_co_ma_cqt { get; set; }
        [SwaggerSchema(Description = "Là hóa đơn có mã CQT khởi tạo từ máy tính tiền")]
        public bool is_hoadon_co_ma_cqt_mtt { get; set; }
        [SwaggerSchema(Description = "Là hóa đơn không có mã CQT")]
        public bool is_hoadon_khong_co_ma_cqt { get; set; }
        [SwaggerSchema(Description = "Trường hợp sử dụng hóa đơn điện tử có mã không phải trả tiền dịch vụ")]
        public bool is_khong_phai_tra_tien_dich_vu { get; set; }
        [SwaggerSchema(Description = "Doanh nghiệp vừa và nhỏ, hợp tác xã, hộ, cá nhân kinh doanh tại địa bàn có điều kiện kinh tế xã hội khó khăn và đặc biệt khó khăn")]
        public bool is_doanh_nghiep_vvn_kho_khan { get; set; }
        [SwaggerSchema(Description = "Doanh nghiệp vừa và nhỏ khác theo đề nghị của UBND tỉnh, thành phố trực thuộc trung ương gửi Bộ tài chính trừ doanh nghiệp hoạt động tại khu kinh tế, khu công nghiệp, khu công nghệ cao")]
        public bool is_doanh_nghiep_vvn_khac { get; set; }

        [SwaggerSchema(Description = "Chuyển dữ liệu hóa đơn điện tử trực tiếp đến cơ quan thuế")]
        public bool is_chuyen_du_lieu_truc_tiep { get; set; }
        [SwaggerSchema(Description = "Thông qua tổ chức cung cấp dịch vụ hóa đơn điện tử")]
        public bool is_chuyen_lieu_thong_qua_to_chuc { get; set; }

        [SwaggerSchema(Description = "Chuyển đầy đủ nội dung từng hóa đơn")]

        public bool is_chuyen_day_du_tung_hoadon { get; set; }
        [SwaggerSchema(Description = "Chuyển theo bảng tổng hợp dữ liệu hóa đơn điện tử")]

        public bool is_chuyen_theo_bang_tonghop { get; set; }

        [SwaggerSchema(Description = "Sử dụng Hóa đơn GTGT")]
        public bool is_sd_hoadon_gtgt { get; set; }
        [SwaggerSchema(Description = "Sử dụng bán hàng")]
        public bool is_sd_hoadon_banhang { get; set; }
        [SwaggerSchema(Description = "Sử dụng các loại hóa đơn khác")]
        public bool is_sd_hoadon_khac { get; set; }
        [SwaggerSchema(Description = "Sử dụng Các loại chứng từ được in, phát hành, sử dụng và quản lý như hóa đơn")]
        public bool is_sd_chungtu_giong_hoadon { get; set; }
        [SwaggerSchema(Description = "Sử dụng hóa đơn bán hàng dự trự quốc gia")]

        public bool is_ban_hang_du_tru_quoc_gia { get; set; }
        [SwaggerSchema(Description = "Sử dụng hóa đơn bán tài sản công")]
        public bool is_ban_tai_san_cong { get; set; }

        [SwaggerSchema(Description = "Nơi lập")]
        public string noi_lap { get; set; }
        [SwaggerSchema(Description = "Ngày có hiệu lực")]
        public DateTime ngay_co_hieu_luc { get; set; }
        [SwaggerSchema(Description = "Để bằng =0")]
        public int cks_user_id { get; set; }
        [SwaggerSchema(Description = "Để rỗng")]
        public string cks_serial_no { get; set; }
        [SwaggerSchema(Description = "Để rỗng")]
        public string cks_user_full_name { get; set; }
        [SwaggerSchema(Description = "Để bằng true")]
        public bool is_camket { get; set; }
        [SwaggerSchema(Description = "Ngày tạo")]
        public DateTime ngay_tao { get; set; }
        [SwaggerSchema(Description = "Người tạo")]
        public string nguoi_tao { get; set; }
        [SwaggerSchema(Description = "Mã đăng ký sau khi CQT chấp nhận")]
        public string? ma_dang_ky { get; set; }
        [SwaggerSchema(Description = "Khóa phiên khi phát hành (hệ thống tự sinh)")]
        public string phat_hanh_uuid { get; set; }
        [SwaggerSchema(Description = "Người phát hành")]
        public int user_id_phathanh { get; set; }
        [SwaggerSchema(Description = "id Cơ quan thuế quản lý. Xem danh sách CQT tại api [GET]api/co-quan-thue")]
        public int co_quan_thue_id { get; set; }
        [SwaggerSchema(Description = "Mã CQT quản lý")]
        public string ma_cqt { get; set; }

        public string dai_dien_phap_luat_ho_ten { get; set; }
        public string dai_dien_phap_luat_dien_thoai { get; set; }
        public string dai_dien_phap_luat_dien_cccd { get; set; }
        public DateTime? dai_dien_phap_luat_dien_ngay_sinh { get; set; }
        public int dai_dien_phap_luat_dien_gioi_tinh { get; set; }
        public bool is_co_quan_xu_ly_tai_san_cong { get; set; }
        public bool is_sd_hoadon_gtgt_bien_lai { get; set; }
        public bool is_sd_hoadon_banhang_bien_lai { get; set; }
        public bool is_sd_hoadon_thuong_mai { get; set; }
        public string? to_chuc_cap_giay_phep_json { get; set; }
        public string? to_chuc_truyen_nhan_json { get; set; }
        public string? so_ho_chieu { get; set; }
    }
}