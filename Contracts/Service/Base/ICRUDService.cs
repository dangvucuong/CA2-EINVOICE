namespace Contracts.Service.Base
{
    public interface ICRUDService<T> : IBaseService,
        ICreateService<T>,
        IReadService<T>,
        IUpdateService<T>,
        IDeleteService<T>
    {
        Task<IEnumerable<T>> ClearCacheThenSelectAllAsync();
        Task<bool> EnsureCachedDateUpdatedByLastUpdatTimeAsync();
        Task<bool> ClearCacheAsync();
    }
}

