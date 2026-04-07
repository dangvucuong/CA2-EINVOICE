namespace Model.Table
{
    public class email_sended
    {
         public int id { get; set; }
        public string from_address { get; set; }
        public string to_address { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public DateTime send_at { get; set; }
        public string send_by_username { get; set; }
        public string email_type { get; set; }
        public string student_code { get; set; }
        public string other_data { get; set; }
    }
}