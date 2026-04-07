using Contracts.Repository.Base;
using Contracts.Service.Base;

namespace Service.Base
{
    public class DeleteService<T> : BaseService, IDeleteService<T>
    {
        protected IDeleteRepository<T> _deleteRepository;

        public DeleteService(IServiceProvider serviceProvider, IDeleteRepository<T> deleteRepository) : base(serviceProvider)
        {
            this._deleteRepository = deleteRepository;
        }

        public Task<bool> DeleteAsync(int id)
        {
            try
            {
                var userId = this.GetCurrentUserId();
                return _deleteRepository.DeleteAsync(id, userId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}

