using Contracts.Service.Base;
using Model.Request.RabitMQ;
using Model.Static;

namespace Contracts.Service.Core
{
    public interface IRabitMQProducerService : IBaseService, IDisposable
    {
        Task<bool> SendEmailAsync(RabbitMQSendEmailRequest request);
        bool SendMessages<T>(IEnumerable<T> messages, RabbitMqConfiguration configuration);
        bool SendMessage<T>(T message, RabbitMqConfiguration configuration);

    }
}