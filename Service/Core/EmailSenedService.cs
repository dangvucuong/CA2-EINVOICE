using Contracts.Service.Core;
using Model.Table;
using Service.Base;

namespace Service.Core
{
    public class EmailSenedService : CRUDService<email_sended>, IEmailSenedService
    {
        public EmailSenedService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase=_repositoryWrapper.Core.EmailSended;
        }
    }
}