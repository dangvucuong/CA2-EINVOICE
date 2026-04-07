using Contracts.Service.Contact;
using Service.Base;

namespace Service.Contact
{
    public class ContactSerivceWrapper : BaseService, IContactSerivceWrapper
    {
        private ICompanySizeService _companySizeService;
        private IContactStatusService _contactStatusService;
        private IContactService _contactService;
        public ContactSerivceWrapper(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public ICompanySizeService CompanySize => _companySizeService??= new CompanySizeService(_serviceProvider);

        public IContactStatusService ContactStatus => _contactStatusService??= new ContactStatusService(_serviceProvider);

        public IContactService Contact => _contactService??= new ContactService(_serviceProvider);
    }
}