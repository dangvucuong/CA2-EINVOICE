namespace Contracts.Service.Base
{
    public interface IDeleteService<T> : IBaseService
    {
        Task<bool> DeleteAsync(int id);

    }
}

