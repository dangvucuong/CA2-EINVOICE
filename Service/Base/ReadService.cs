using Contracts.Repository.Base;
using Contracts.Service.Base;
using Model.FuncResult;
using Model.Request.Base;

namespace Service.Base
{
    public class ReadService<T> : BaseService, IReadService<T>
    {
        protected IReadRepository<T> _readRepository;

        public ReadService(IServiceProvider serviceProvider, IReadRepository<T> readRepository) : base(serviceProvider)
        {
            this._readRepository = readRepository;
        }

        public Task<IEnumerable<T>> SelectAllAsync()
        {
            return _readRepository.SelectAllAsync();
        }

        public Task<PagingResult<IEnumerable<T>>> SelectAsync(PagingRequest pagingRequest)
        {
             return _readRepository.SelectAsync(pagingRequest);
        }

        public Task<T> SelectByIdAsync(int id)
        {
            return _readRepository.SelectByIdAsync(id);
        }
    }
}

