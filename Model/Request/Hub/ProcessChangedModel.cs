namespace Model.Request.Hub
{
    public class ProcessChangedModel
    {
        public string user_id { get; set; }
        public bool is_finished { get; set; }
        public object processStatus { get; set; }
    }
}