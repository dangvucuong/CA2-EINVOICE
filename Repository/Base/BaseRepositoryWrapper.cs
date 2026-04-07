using Contracts.Repository.Base;

namespace Repository.Base
{
    public class BaseRepositoryWrapper : IBaseRepositoryWrapper
    {
        protected IConnectionStrings _connectionStrings;
        protected IMSSQLConnection _defaultConnection;
        public BaseRepositoryWrapper(IConnectionStrings connectionStrings)
        {
            this._connectionStrings = connectionStrings;
            this._defaultConnection = connectionStrings.Default;
        }

    }
}

