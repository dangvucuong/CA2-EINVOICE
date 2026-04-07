using Model.Base;
using Swashbuckle.AspNetCore.Annotations;

namespace Model.Table
{
    public class hoa_don_hang_hoa : modify_infor
    {
        [SwaggerSchema(Description = "id tự tăng, bỏ trống khi thêm mới")]
        public int id { get; set; }
        [SwaggerSchema(Description = "id của hóa đơn")]
        public int hoa_don_id { get; set; }
        [SwaggerSchema(Description = "Tính chất hàng hóa. Hàng hóa, dịch vụ=1,Khuyến mại=2,Chiết khấu=3,Ghi chú, diễn giải=4")]
        public int hang_hoa_tinh_chat_id { get; set; }
        [SwaggerSchema(Description = "Số thự tự")]
        public int stt { get; set; }
        [SwaggerSchema(Description = "Mã hàng")]
        public string ma_hang { get; set; }
        [SwaggerSchema(Description = "Tên hàng")]
        public string ten_hang { get; set; }
        [SwaggerSchema(Description = "Tên đơn vị tính")]
        public string dvt { get; set; }
        [SwaggerSchema(Description = "Số lượng")]
        public decimal so_luong { get; set; }
        [SwaggerSchema(Description = "Đơn giá")]
        public decimal don_gia { get; set; }
        [SwaggerSchema(Description = "Tỷ lệ chiết khấu")]
        public decimal ty_le_chiet_khau { get; set; }
        [SwaggerSchema(Description = "Số tiền được chiết khấu")]
        public decimal tien_chiet_khau { get; set; }
        [SwaggerSchema(Description = "Thành tiền")]
        public decimal thanh_tien { get; set; }
        [SwaggerSchema(Description = "Thuế: 0%, 5%, 8%, 10%, KCT, KKKNT")]
        public string? thue_vat { get; set; }
        [SwaggerSchema(Description = "Bỏ qua")]
        public int hoa_don_hang_hoa_trangthai_id { get; set; }
        [SwaggerSchema(Description = "Thông tin hàng hóa đặc trưng kiêu json")]
        public string? hang_hoa_dac_trung_json { get; set; }
    }
}