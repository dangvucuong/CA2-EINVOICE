using System;
using Contract.Service.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Model.Static;

namespace WebApi.Filters
{
    public class MustBeEcoSystem : ActionFilterAttribute
    {
        public string _baseOnApi { get; set; }
        private IJwtTokenService _jwtTokenService;
        public MustBeEcoSystem()
        {
        }
        public MustBeEcoSystem(string BaseOnApis)
        {
            _baseOnApi = BaseOnApis;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                _jwtTokenService = (IJwtTokenService)context.HttpContext.RequestServices.GetService(typeof(IJwtTokenService));
                var token = _jwtTokenService.GetUserToken(context.HttpContext);
                if (AppSettings.FixedValue.EcoSystemToken != "" && token == AppSettings.FixedValue.EcoSystemToken)
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

