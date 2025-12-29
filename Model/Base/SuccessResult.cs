namespace Model.Base
{
    public class SuccessResult<T> : FunctionResult<T>
    {
        public SuccessResult(string message = "", T data = default(T)) : base(true, message, data)
        {
        }
        public SuccessResult(T data) : base(true, string.Empty, data)
        {
        }
    }
}

