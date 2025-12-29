namespace Contracts.Repository.Base
{
    public interface ICreateRepository<T> : IBaseRepository
    {
        Task<int> InsertAsync(T obj);
    }
}

