using Contract.Repository.Core;
using Contracts.Repository.Base;
using Contracts.Repository.Core;
using Repository.Base;

namespace Repository.Core
{
    public class CoreRepositoryWrapper : BaseRepositoryWrapper, ICoreRepositoryWrapper
    {
        private IExceptionRepository _exceptionRepsitory;
        private ILocalizedResourceRepository _localizedResourceRepository;
        private IEmailSendedRepository _emailSendedRepository;

        public CoreRepositoryWrapper(IConnectionStrings connectionStrings) : base(connectionStrings)
        {
        }

        public IExceptionRepository Exception => _exceptionRepsitory ??= new ExceptionRepository(_defaultConnection);

        public ILocalizedResourceRepository LocalizedResource => _localizedResourceRepository ??= new LocalizedResourceRepository(_defaultConnection);

        public IEmailSendedRepository EmailSended => _emailSendedRepository ??= new EmailSendedRepository(_connectionStrings.Log);
    }
}

