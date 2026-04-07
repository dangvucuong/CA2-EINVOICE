using Model.Request.Base;

namespace Model.Request.Contact
{
    public class ContactSelectRequest : PagingRequest
    {
        public int? contact_status_id { get; set; }
    }
}