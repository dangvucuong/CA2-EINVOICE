using Model.Base;
using Swashbuckle.AspNetCore.Annotations;

namespace Model.Table
{
    public class to_khai_cts : modify_infor
    {
        [SwaggerSchema(Description = "id tự tăng")]
        public int id { get; set; }
        [SwaggerSchema(Description = "id của tờ khai")]
        public int to_khai_id { get; set; }
        [SwaggerSchema(Description = "để bằng file name")]
        public string url { get; set; }
        [SwaggerSchema(Description = "file cer name")]
        public string file_name { get; set; }
        [SwaggerSchema(Description = "số serial của chứng thư số")]
        public string serial_number { get; set; }
        [SwaggerSchema(Description = "Subject của chứng thư sô")]
        public string subject { get; set; }
        [SwaggerSchema(Description = "người phát hành chứng thư số, không có thì để trống")]
        public string issuer { get; set; }
        [SwaggerSchema(Description = "thời hạn chứng thư số từ ngày")]
        public DateTime not_before { get; set; }
        [SwaggerSchema(Description = "thời hạn chứng thư số đến ngày")]
        public DateTime not_after { get; set; }
        [SwaggerSchema(Description = "signature algorithm của chứng thư số, không có thì để trống")]
        public string signature_algorithm { get; set; }
        [SwaggerSchema(Description = "version của chứng thư số, không có thì để trống")]
        public string version { get; set; }
    }
}