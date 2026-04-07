namespace Contracts.Repository.Base
{
    public interface ICRUDRepository<T> : IBaseRepository,
        ICreateRepository<T>,
        IReadRepository<T>,
        IUpdateRepository<T>,
        IDeleteRepository<T>

    {
    }
}

