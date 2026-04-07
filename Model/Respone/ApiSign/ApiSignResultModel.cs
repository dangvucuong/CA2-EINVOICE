namespace Model.Respone.ApiSign
{
    public class ApiSignResultModel
    {
        public int HoadonId { get; set; }
        public int Macode { get; set; }
        public string Message { get; set; }
        public string SignedData { get; set; }
    }
}
