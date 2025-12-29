using Contracts.Repository.Base;
using Model.FuncResult;
using Model.Request.Contact;
using Model.Table;

namespace Contracts.Repository.Contact
{
    public interface IContactRepository:ICRUDRepository<contact>
    {
        Task<PagingResult<IEnumerable<contact>>> SelectAsync(ContactSelectRequest request);
        Task<int> SelectCountContactByStatusAsync(int contact_status_id);

    }
}