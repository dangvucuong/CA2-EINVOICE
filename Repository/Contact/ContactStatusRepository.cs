using Contracts.Repository.Base;
using Contracts.Repository.Contact;
using Model.Table;
using Repository.Base;

namespace Repository.Contact
{
    public class ContactStatusRepository : CRUDRepository<contact_status>, IContactStatusRepository
    {
        public ContactStatusRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }
    }
}