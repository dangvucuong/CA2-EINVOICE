using Contracts.Service.Contact;
using Model.Table;
using Service.Base;

namespace Service.Contact
{
    public class CompanySizeService : CRUDServiceWithCache<company_size>, ICompanySizeService
    {
        public CompanySizeService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.Contact.CompanySize;
        }

        protected override void ConfigKey()
        {
            this._itemKeyField = "id";
            this._keyPrefix = "company_size:";
        }
    }
}