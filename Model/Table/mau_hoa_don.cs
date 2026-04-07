using Model.Base;
using Swashbuckle.AspNetCore.Annotations;

namespace Model.Table
{
    public class mau_hoa_don : modify_infor
    {
        [SwaggerSchema(Description = "id của mẫu hóa đơn")]
        public int id { get; set; }
        [SwaggerSchema(Description = "Loại mẫu hóa đơn. Xem chi tiết tại [GET]api/loai-hoa-don-ct-template")]
        ///loai-hoa-don-ct-template
        public int loai_hoa_don_ct_template_id { get; set; }
        [SwaggerSchema(Description = "MST của đơn vị")]
        public string donvi_ma_dv { get; set; }
        [SwaggerSchema(Description = "Tên mẫu hóa đơn")]
        public string name { get; set; }
        [SwaggerSchema(Description = "Số quyết định")]
        public string? so_qd { get; set; }
        [SwaggerSchema(Description = "Ngày quyết định")]
        public DateTime? ngay_qd { get; set; }
        [SwaggerSchema(Description = "Link ảnh logo")]
        public string? logo_path { get; set; }
        [SwaggerSchema(Description = "Link ảnh watermark")]
        public string? watermark_path { get; set; }
        [SwaggerSchema(Description = "Link ảnh viền")]
        public string? vien_path { get; set; }
        [SwaggerSchema(Description = "Show watermark trong bảng hàng hóa hay không")]
        public bool? is_show_wattermark_inner_table { get; set; }
        [SwaggerSchema(Description = "Đường dẫn đến file xslt (hệ thống sinh tự động)")]
        public string? xslt_path { get; set; }
        [SwaggerSchema(Description = "Vị trí logo: left hoặc right")]
        public string? logo_position { get; set; }
        public bool? is_locked { get; set; }
        [SwaggerSchema(Description = "Trạng thái đang sử dụng hay không")]

        public bool is_active { get; set; }
        [SwaggerSchema(Description = "Độ nét của watermark")]
        public int? watermark_opacity { get; set; }
        [SwaggerSchema(Description = "Để null hoặc []")]
        public string? advanced_settings_json { get; set; }
        public int xml_version { get; set; }
    }
}