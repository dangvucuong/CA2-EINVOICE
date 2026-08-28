using Model.Base;
using Swashbuckle.AspNetCore.Annotations;

namespace Model.Table
{
    /// <summary>
    /// Gói chữ ký số đơn vị đã mua (HSDT đồng bộ vào bảng donvi_mua_chukyso).
    /// </summary>
    public class donvi_mua_chukyso : modify_infor
    {
        [SwaggerSchema(Description = "Id tự tăng. Bỏ qua khi thêm mới")]
        public int id { get; set; }
        [SwaggerSchema(Description = "Mã số thuế đơn vị")]
        public string donvi_mst { get; set; }
        [SwaggerSchema(Description = "Người yêu cầu")]
        public string nguoi_yeu_cau { get; set; }
        [SwaggerSchema(Description = "Mã gói dịch vụ")]
        public string ma_goi_dich_vu { get; set; }
        [SwaggerSchema(Description = "Ngày mua")]
        public DateTime ngay_mua { get; set; }
        [SwaggerSchema(Description = "Số lượng mua")]
        public int so_luong { get; set; }
        [SwaggerSchema(Description = "Số lượng khuyến mại")]
        public int so_luong_khuyen_mai { get; set; }
        [SwaggerSchema(Description = "Tổng số lượng. Backend tự tính = so_luong + so_luong_khuyen_mai")]
        public int? tong_so_luong { get; set; }
        [SwaggerSchema(Description = "Loại sản phẩm")]
        public string loai_san_pham { get; set; }
        [SwaggerSchema(Description = "Trạng thái hồ sơ")]
        public int trang_thai_hs_id { get; set; }
        [SwaggerSchema(Description = "Trạng thái thanh toán")]
        public int trang_thai_tt_id { get; set; }
        [SwaggerSchema(Description = "Trạng thái đối soát")]
        public int trang_thai_ds_id { get; set; }
        [SwaggerSchema(Description = "Trạng thái xuất hóa đơn")]
        public int tt_xuat_hd_id { get; set; }
        [SwaggerSchema(Description = "Serial chữ ký số")]
        public string serial_number { get; set; }
        [SwaggerSchema(Description = "Tên công ty")]
        public string ten_cong_ty { get; set; }
        [SwaggerSchema(Description = "Địa chỉ")]
        public string dia_chi { get; set; }
        [SwaggerSchema(Description = "Mã giao dịch HSDT (idHSDT)")]
        public string transaction_id { get; set; }
        [SwaggerSchema(Description = "Trạng thái hóa đơn. Thu hồi sẽ set = 0")]
        public int trang_thai_hd_id { get; set; }
    }
}