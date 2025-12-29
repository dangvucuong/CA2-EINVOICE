using Contract.Repository.Core;
using Contracts.Repository.Base;
using Repository.Base;

namespace Repository.Core
{
    public class ExceptionRepository : CRUDRepository<Model.Table.exception>, IExceptionRepository
    {
        public ExceptionRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }
    }
}

