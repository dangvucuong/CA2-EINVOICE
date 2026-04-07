using Contracts.Repository.Base;

namespace Contracts.Repository.Contact
{
    public interface IContactRepositoryWrapper : IBaseRepositoryWrapper
    {
        ICompanySizeRepository CompanySize { get; }
        IContactStatusRepository ContactStatus { get; }
        IContactRepository Contact { get; }
    }
}