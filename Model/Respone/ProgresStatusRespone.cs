namespace Model.Respone
{
    public class ProcessStatusRespone<T>
    {
        public string progress_id { get; set; }
        public List<ProcessStepRespone<T>> steps { get; set; }
    }
    public class ProcessStepRespone<T>
    {
        public int id { get; set; }
        public string name { get; set; }
        public T data { get; set; }
    }
    public class ProcessStepDataBase
    {
        public int total { get; set; }
        public int success { get; set; }
        public int error { get; set; }
        public bool is_done { get; set; }
        public bool is_processing { get; set; }
    }
}