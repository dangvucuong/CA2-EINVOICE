namespace Model.Request.KysoHSM
{
    public class KySoHSMMultipleRequest
    {
        public List<int> ids { get; set; }
        public string serial_number { get; set; }
    }
}