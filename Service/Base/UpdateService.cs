using Contracts.Repository.Base;
using Contracts.Service.Base;

namespace Service.Base
{
    public class UpdateService<T> : BaseService, IUpdateService<T>
    {
        protected IUpdateRepository<T> _updateRepository;

        public UpdateService(IServiceProvider serviceProvider, IUpdateRepository<T> updateRepository) : base(serviceProvider)
        {
            this._updateRepository = updateRepository;
        }

        public Task<bool> UpdateAsync(T obj)
        {
            return _updateRepository.UpdateAsync(obj);
        }
    }
}

