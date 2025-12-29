using Model.Base;

namespace Model
{
    public class don_vi_cts : modify_infor
    {
        public int id { get; set; }
        public string? donvi_ma_dv { get; set; }
        public string? url { get; set; }
        public string? file_name { get; set; }
        public string serial_number { get; set; }
        public string subject { get; set; }
        public string issuer { get; set; }
        public DateTime not_before { get; set; }
        public DateTime not_after { get; set; }
        public string? signature_algorithm { get; set; }
        public string? version { get; set; }
        public int serial_type_id { get; set; }
        public string? rs_ma_but_ky { get; set; }
        public int user_id { get; set; }
        public bool is_active { get; set; }
    }
}