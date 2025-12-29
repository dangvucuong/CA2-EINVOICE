namespace Model.Base
{
    public class FunctionResult<T>
    {
        public bool is_success { get; set; }
        public string message { get; set; }
        public T data { get; set; }
        public FunctionResult()
        {

        }
        public FunctionResult(bool is_success, string message, T data = default(T))
        {
            this.is_success = is_success;
            this.data = data;
            this.message = message;
        }
    }
}

