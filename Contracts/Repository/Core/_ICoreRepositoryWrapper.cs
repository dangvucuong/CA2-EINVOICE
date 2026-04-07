using Contracts.Repository.Base;
using Contracts.Repository.Core;

namespace Contract.Repository.Core
{
    public interface ICoreRepositoryWrapper : IBaseRepositoryWrapper
    {
        IExceptionRepository Exception { get; }
        ILocalizedResourceRepository LocalizedResource { get; }
        IEmailSendedRepository EmailSended { get; }
    }
}

