using Contracts.Service.Contact;
using Model.FuncResult;
using Model.Request.Contact;
using Model.Table;
using Service.Base;

namespace Service.Contact
{
    public class ContactService : CRUDService<contact>, IContactService
    {
        public ContactService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase =_repositoryWrapper.Contact.Contact;
        }

        public Task<PagingResult<IEnumerable<contact>>> SelectAsync(ContactSelectRequest request)
        {
           return _repositoryWrapper.Contact.Contact.SelectAsync(request);
        }

        public Task<int> SelectCountContactByStatusAsync(int contact_status_id)
        {
             return _repositoryWrapper.Contact.Contact.SelectCountContactByStatusAsync(contact_status_id);
        }
    }
}