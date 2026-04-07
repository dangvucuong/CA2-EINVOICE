using Contracts.Repository.Base;
using Model.FuncResult;
using Model.Request.Base;

namespace Repository.Base
{
    public class CRUDRepository<T> : BaseRepository, ICRUDRepository<T>
    {
        private ICreateRepository<T> _createRepository;
        private IReadRepository<T> _readRepository;
        private IUpdateRepository<T> _updateRepository;
        private IDeleteRepository<T> _deleteRepository;
        public CRUDRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
            this._createRepository = new CreateRepository<T>(dbConnection);
            this._readRepository = new ReadRepository<T>(dbConnection);
            this._updateRepository = new UpdateRepository<T>(dbConnection);
            this._deleteRepository = new DeleteRepository<T>(dbConnection);

        }


        public Task<bool> DeleteAsync(int id, int userId)
        {
            return _deleteRepository.DeleteAsync(id, userId);
        }

        public Task<int> InsertAsync(T obj)
        {
            return _createRepository.InsertAsync(obj);
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

        public Task<IEnumerable<T>> SelectChangedAsync(DateTime fromTime)
        {
            return _readRepository.SelectChangedAsync(fromTime);
        }

        public Task<bool> UpdateAsync(T obj)
        {
            return _updateRepository.UpdateAsync(obj);
        }
    }
}

