using Contracts.Repository.Base;
using Contracts.Repository.Contact;
using Model.Table;
using Repository.Base;

namespace Repository.Contact
{
    public class CompanySizeRepository : CRUDRepository<company_size>, ICompanySizeRepository
    {
        public CompanySizeRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }
    }
}