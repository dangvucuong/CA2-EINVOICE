namespace Model.Table
{
    public class log
    {
        public int id { get; set; }
        public string donvi_ma_dv { get; set; }
        public string username { get; set; }
        public string content { get; set; }
        public string ip { get; set; }
        public string endpoint { get; set; }
        public string payload { get; set; }
        public string method { get; set; }
        public DateTime created_at { get; set; }
    }
}