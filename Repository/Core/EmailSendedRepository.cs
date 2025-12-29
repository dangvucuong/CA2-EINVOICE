using Contracts.Repository.Base;
using Contracts.Repository.Core;
using Model.Table;
using Repository.Base;

namespace Repository.Core
{
    public class EmailSendedRepository : CRUDRepository<email_sended>, IEmailSendedRepository
    {
        public EmailSendedRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }
    }
}