namespace Model.Base
{
    public class ErrorResult<T> : FunctionResult<T>
    {
        public ErrorResult(string message = "", T data = default(T)) : base(false, message, data)
        {
        }
    }
}

