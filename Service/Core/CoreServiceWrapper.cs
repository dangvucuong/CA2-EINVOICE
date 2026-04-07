using Contract.Service.Core;
using Contracts.Service.Core;
using Microsoft.Extensions.DependencyInjection;
using Service.Base;

namespace Service.Core
{
    public class CoreServiceWrapper : BaseService, ICoreSerivceWrapper
    {
        private IExceptionService _exceptionService;
        private IJwtTokenService _jwtTokenService;
        private IAccountService _accountService;
        private ILocalizedResourceService _localizedResourceService;
        private IEmailService _emailService;
        private IEmailSenedService _emailSenedService;
        private IRabitMQProducerService _rabitMQProducerService;
        private ITaskQueueService _taskQueueService;
        public CoreServiceWrapper(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            var scope = serviceProvider.CreateScope();
            this._taskQueueService = scope.ServiceProvider.GetRequiredService<ITaskQueueService>();
        }

        public IExceptionService Exception => _exceptionService ??= new ExceptionService(_serviceProvider);

        public IAccountService Account => _accountService ??= new AccountService(_serviceProvider);

        public IJwtTokenService JwtToken => _jwtTokenService ??= new JwtTokenService();

        public ILocalizedResourceService LocalizedResource => _localizedResourceService ??= new LocalizedResourceService(_serviceProvider);

        public IEmailSenedService EmailSened => _emailSenedService ??= new EmailSenedService(_serviceProvider);

        // public IEmailService Email => _emailService??= new EmailService(_serviceProvider);
        public IEmailService Email => _emailService ??= new EmailQueueService(_serviceProvider);

        public IRabitMQProducerService RabitMQ => _rabitMQProducerService ??= new RabitMQProducerService(_serviceProvider);

        public ITaskQueueService TaskQueue => _taskQueueService;
    }
}

