using Contracts.Service.Base;
using Model.Base;
using Model.Request.Email;

namespace Contracts.Service.Core
{
    public interface IEmailService:IBaseService
    {
        Task<FunctionResult<bool>> SendEmailAsync(SendEmailRequest rq);
    }
}