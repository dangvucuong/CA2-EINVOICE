using Contracts.Service.Base;

namespace Contracts.Service.Contact
{
    public interface IContactSerivceWrapper:IBaseService
    {
        ICompanySizeService CompanySize {get;}
        IContactStatusService ContactStatus {get;}
        IContactService Contact {get;}
    }
}