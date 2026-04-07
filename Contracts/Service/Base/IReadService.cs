using Model.FuncResult;
using Model.Request.Base;
namespace Contracts.Service.Base
{
    public interface IReadService<T> : IBaseService
    {
        Task<IEnumerable<T>> SelectAllAsync();
   
        Task<PagingResult<IEnumerable<T>>> SelectAsync(PagingRequest pagingRequest);
        Task<T> SelectByIdAsync(int id);
    }
}

