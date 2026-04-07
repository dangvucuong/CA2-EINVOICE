namespace Contracts.Service.Base
{
    public interface IUpdateService<T> : IBaseService
    {
        Task<bool> UpdateAsync(T obj);

    }
}

