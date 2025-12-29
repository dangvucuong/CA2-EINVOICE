using Contracts.Service.Contact;
using Model.Table;
using Service.Base;

namespace Service.Contact
{
    public class ContactStatusService : CRUDServiceWithCache<contact_status>, IContactStatusService
    {
        public ContactStatusService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.Contact.ContactStatus;
        }

        protected override void ConfigKey()
        {
            this._itemKeyField = "id";
            this._keyPrefix = "contact_status:";
        }
    }
}