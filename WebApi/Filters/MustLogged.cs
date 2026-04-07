using System;
using Contract.Service.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Filters
{
    public class MustLogged : ActionFilterAttribute
    {
        private IJwtTokenService _jwtTokenService;
        public MustLogged()
        {
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                _jwtTokenService = (IJwtTokenService)context.HttpContext.RequestServices.GetService(typeof(IJwtTokenService));
                var userId = _jwtTokenService.GetUserId(context.HttpContext);
                if (userId > 0)
                {
                    return;
                }
                context.Result = new UnauthorizedResult();
            }
            catch (Exception)
            {
                base.OnActionExecuting(context);
            }
        }
    }
}

