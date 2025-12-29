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
            if (this.thong_tin_khac!= null)
            {
                this.thong_tin_khac_json = this.thong_tin_khac.ToString();
            }
        }
    }
}