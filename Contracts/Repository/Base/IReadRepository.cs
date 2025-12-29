using Model.FuncResult;
using Model.Request.Base;
namespace Contracts.Repository.Base
{
    public interface IReadRepository<T> : IBaseRepository
    {
        Task<IEnumerable<T>> SelectAllAsync();
        Task<T> SelectByIdAsync(int id);
        Task<IEnumerable<T>> SelectChangedAsync(DateTime fromTime);
        Task<PagingResult<IEnumerable<T>>> SelectAsync(PagingRequest pagingRequest);

    }
}

