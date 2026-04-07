using Model.Request.Email;
using Model.Static;

namespace Model.Request.RabitMQ
{
    public class RabbitMQSendEmailRequest:SendEmailRequest
    {
        public EmailConfig config { get; set; }
        
    }
}