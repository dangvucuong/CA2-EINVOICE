using Contracts.Service.Core;
using Model.Base;
using Model.Request.Email;
using Model.Static;
using Service.Base;
using Common;
using Model.Request.RabitMQ;
namespace Service.Core
{
    public class EmailQueueService : BaseService, IEmailService
    {
        public EmailQueueService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<FunctionResult<bool>> SendEmailAsync(SendEmailRequest rq)
        {
            var emailConfig = AppSettings.EmailConfig;
            if (emailConfig.FixedEmail.ConvertToString() != "")
            {
                rq.EmailAddress = new List<string> { emailConfig.FixedEmail.ConvertToString() };
            }

            var emailPushMessage = new RabbitMQSendEmailRequest()
            {
                Body = rq.Body,
                EmailAddress = rq.EmailAddress,
                config = emailConfig,
                isHtml = rq.isHtml,
                SendByUser = rq.SendByUser,
                Subject = rq.Subject,
            };
            var isPushSuccess = await _serviceWrapper.Core.RabitMQ.SendEmailAsync(emailPushMessage);
            if (isPushSuccess)
            {
                return new SuccessResult<bool>();
            }
            return new ErrorResult<bool>();
        }

    }
}