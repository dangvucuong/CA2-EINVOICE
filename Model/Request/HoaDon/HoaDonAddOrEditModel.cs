using Model.Table;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Model.Request.ToKhai
{
    public class HoaDonAddOrEditModel : hoa_don
    {
        [SwaggerSchema(Description = "Danh sách hàng hóa")]

        public List<hoa_don_hang_hoa> hoang_hoas { get; set; }
        [SwaggerSchema(Description = "Danh sách loại phí")]

        // Hóa đơn thông tin bổ sung
        public int? IsHdPhiThueQuan { get; set; }
        [SwaggerSchema(Description = "Hóa đơn dành cho khu phi thuế quan")]
        public int? IsHdBanTaiSanCong { get; set; }
        [SwaggerSchema(Description = "Hóa đơn bán tài sản công")]
        public string? SoQuyetDinh { get; set; }
        [SwaggerSchema(Description = "Số quyết định")]
        public string? NgayQuyetDinh { get; set; }
        [SwaggerSchema(Description = "Ngày quyết định")]
        public string? CoQuanBanHanhQD { get; set; }
        [SwaggerSchema(Description = "Cơ quan ban hành quyết định")]
        public string? HinhThucBan { get; set; }
        [SwaggerSchema(Description = "Hình thức bán")]
        public string? DiaDiemVCHangDen { get; set; }
        [SwaggerSchema(Description = "Địa điểm vận chuyển hàng đến")]
        public string? TgianVCHangDenTu { get; set; }
        [SwaggerSchema(Description = "Thời gian vận chuyển hàng đến từ ngày")]
        public string? TgianVCHangDenDen { get; set; }
        [SwaggerSchema(Description = "Thời gian vận chuyển hàng đến đến ngày")]
        // Hóa đơn thông tin bổ sung


        public List<hoa_don_loai_phi> loai_phis { get; set; }
        [JsonIgnore]
        public object? thong_tin_khac { get; set; }
        public HoaDonAddOrEditModel()
        {
            this.hoang_hoas = new List<hoa_don_hang_hoa>();
            this.loai_phis = new List<hoa_don_loai_phi>();
        }
        public void CheckAndSetThongTinKhacJson()
        {
            if (this.thong_tin_khac != null)
            {
                this.thong_tin_khac_json = this.thong_tin_khac.ToString();
            }
        }





    }
}