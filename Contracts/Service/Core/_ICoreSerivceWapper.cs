using Contracts.Service.Base;
using Contracts.Service.Core;

namespace Contract.Service.Core
{
    public interface ICoreSerivceWrapper : IBaseService
    {
        IExceptionService Exception { get; }
        IAccountService Account { get; }
        IJwtTokenService JwtToken { get; }
        ILocalizedResourceService LocalizedResource { get; }
        IEmailSenedService EmailSened { get; }
        IEmailService Email { get; }
        IRabitMQProducerService RabitMQ { get; }
        ITaskQueueService TaskQueue { get; }
       
    }
}

