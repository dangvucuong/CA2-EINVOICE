using Contracts.Repository.Base;
using Contracts.Service.Base;
using Model.FuncResult;
using Model.Request.Base;

namespace Service.Base
{
    public class CRUDService<T> : BaseService, ICRUDService<T>
    {
        private ICreateService<T> _createService;
        private IReadService<T> _readService;
        private IUpdateService<T> _updateService;
        private IDeleteService<T> _deleteService;
        protected ICRUDRepository<T> _repositoryBase;

        public CRUDService(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        public virtual Task<bool> DeleteAsync(int id)
        {
            _deleteService = _deleteService ??= new DeleteService<T>(_serviceProvider, _repositoryBase);
            return _deleteService.DeleteAsync(id);
        }

        public virtual Task<int> InsertAsync(T obj)
        {
            _createService = _createService ??= new CreateService<T>(_serviceProvider, _repositoryBase);
            return _createService.InsertAsync(obj);
        }

        public Task<IEnumerable<T>> SelectAllAsync()
        {
            _readService = _readService ??= new ReadService<T>(_serviceProvider, _repositoryBase);
            return _readService.SelectAllAsync();
        }

        public Task<T> SelectByIdAsync(int id)
        {
            _readService = _readService ??= new ReadService<T>(_serviceProvider, _repositoryBase);
            return _readService.SelectByIdAsync(id);
        }

        public virtual Task<bool> UpdateAsync(T obj)
        {
            _updateService = _updateService ?? new UpdateService<T>(_serviceProvider, _repositoryBase);
            return _updateService.UpdateAsync(obj);
        }

        public Task<PagingResult<IEnumerable<T>>> SelectAsync(PagingRequest pagingRequest)
        {
            _readService = _readService ??= new ReadService<T>(_serviceProvider, _repositoryBase);
            return _readService.SelectAsync(pagingRequest);
        }

        public async Task<bool> ClearCacheAsync()
        {
            return true;
        }

        public Task<IEnumerable<T>> ClearCacheThenSelectAllAsync()
        {
            return this.SelectAllAsync();
        }

        public async Task<bool> EnsureCachedDateUpdatedByLastUpdatTimeAsync()
        {
           return true;
        }
    }
}

