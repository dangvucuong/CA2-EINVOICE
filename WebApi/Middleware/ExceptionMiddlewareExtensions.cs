using System;
using System.Net;
using Contract.Service.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Model.Respone.Account;

namespace WebApi.Middleware
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, Contract.Service.Core.IExceptionService exceptionService,
        IJwtTokenService jwtTokenService,
        IWebHostEnvironment env
        )
        // ITokenService tokenService, IExceptionService exceptionService)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    JwtTokenInfo userInfo;
                    try
                    {
                        userInfo = jwtTokenService.GetUserInfo(context);
                    }
                    catch
                    {

                        userInfo = null;
                    }
                    //var validateTokenReult = tokenService.ValidateAccessToken(userToken);
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    if (contextFeature != null)
                    {
                        try
                        {
                            await exceptionService.InsertAsync(new Model.Table.exception()
                            {
                                create_at = DateTime.Now,
                                message = contextFeature.Error.Message,
                                endpoint = contextFeature.Path.ToString(),
                                username = userInfo?.username ?? "",
                                user_id = userInfo?.id.ToString() ?? "0",
                                id = 0
                            });
                        }
                        catch
                        {

                        }
                        var errorMessage = GetExceptionMessage(contextFeature.Error, env.IsDevelopment());
                        await context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(new
                        {
                            is_success = false,
                            status_code = context.Response.StatusCode,
                            message = errorMessage
                        }));
                    }
                });
            });
        }

        private static string GetExceptionMessage(Exception ex, bool isDevelopment)
        {
            if (ex == null)
            {
                return "Có lỗi không xác định.";
            }

            var message = ex.Message;
            if (isDevelopment && ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
            {
                message = $"{message} ({ex.InnerException.Message})";
            }

            return string.IsNullOrWhiteSpace(message) ? "Có lỗi không xác định." : message;
        }
    }
}

