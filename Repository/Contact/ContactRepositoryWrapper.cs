using Contracts.Repository.Base;
using Contracts.Repository.Contact;
using Repository.Base;

namespace Repository.Contact
{
    public class ContactRepositoryWrapper : BaseRepositoryWrapper,IContactRepositoryWrapper
    {
        private ICompanySizeRepository _companySize;
        private IContactStatusRepository _contactStatus;
        private IContactRepository _contact;

        public ContactRepositoryWrapper(IConnectionStrings connectionStrings) : base(connectionStrings)
        {
        }

        public ICompanySizeRepository CompanySize => _companySize??= new CompanySizeRepository(_defaultConnection);

        public IContactStatusRepository ContactStatus => _contactStatus??= new ContactStatusRepository(_defaultConnection);

        public IContactRepository Contact => _contact??= new ContactRepository(_defaultConnection);
    }
}