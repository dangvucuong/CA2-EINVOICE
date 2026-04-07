using Contracts.Repository.Base;
using Contracts.Repository.Core;
using Model.Table;
using Repository.Base;

namespace Repository.Core
{
    public class LocalizedResourceRepository : CRUDRepository<localized_resource>, ILocalizedResourceRepository
    {
        public LocalizedResourceRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }
    }
}