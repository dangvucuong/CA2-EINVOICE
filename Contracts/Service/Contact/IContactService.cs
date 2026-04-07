using Contracts.Service.Base;
using Model.FuncResult;
using Model.Request.Contact;
using Model.Table;

namespace Contracts.Service.Contact
{
    public interface IContactService:ICRUDService<contact>
    {
        Task<PagingResult<IEnumerable<contact>>> SelectAsync(ContactSelectRequest request);
        Task<int> SelectCountContactByStatusAsync(int contact_status_id);
    }
}