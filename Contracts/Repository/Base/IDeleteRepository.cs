namespace Contracts.Repository.Base
{
    public interface IDeleteRepository<T> : IBaseRepository
    {
        Task<bool> DeleteAsync(int id, int userId);

    }
}

