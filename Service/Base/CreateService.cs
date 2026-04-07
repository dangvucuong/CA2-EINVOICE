using Contracts.Repository.Base;
using Contracts.Service.Base;

namespace Service.Base
{
    public class CreateService<T> : BaseService, ICreateService<T>
    {
        protected ICreateRepository<T> _createRepository;

        public CreateService(IServiceProvider serviceProvider, ICreateRepository<T> createRepository) : base(serviceProvider)
        {
            this._createRepository = createRepository;
        }

        public Task<int> InsertAsync(T obj)
        {
            return _createRepository.InsertAsync(obj);
        }
    }
}

