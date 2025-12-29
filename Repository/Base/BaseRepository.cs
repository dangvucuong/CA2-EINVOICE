using Contracts.Repository.Base;

namespace Repository.Base
{
    public class BaseRepository : IBaseRepository
    {
        protected IMSSQLConnection _dbConnection;
        public BaseRepository(IMSSQLConnection dbConnection)
        {
            this._dbConnection = dbConnection;
        }

    }
}

