namespace Contracts.Repository.Base
{
    public interface IUpdateRepository<T> : IBaseRepository
    {
        Task<bool> UpdateAsync(T obj);

    }
}

