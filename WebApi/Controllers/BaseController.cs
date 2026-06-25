using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Contract.Service;
using Contract.Service.Core;
using Contracts.Repository;
using Contracts.Service.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Enum;
using Model.Respone;
using Model.Respone.Account;
using Model.Table;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [SecurityHeadersAttribute]
    public class BaseController : ControllerBase
    {
        protected readonly IServiceWrapper _serviceWrapper;
        protected readonly IRepositoryWrapper _repositoryWrapper;
        protected readonly IJwtTokenService _jwtTokenService;
        private readonly ITaskQueueService _taskQueue;


        public BaseController(IServiceWrapper serviceWrapper)
        {
            this._serviceWrapper = serviceWrapper;
            this._jwtTokenService = serviceWrapper.Core.JwtToken;
            this._taskQueue = serviceWrapper.Core.TaskQueue;


        }

        protected int GetUserId()
        {
            return _jwtTokenService.GetUserId(HttpContext);
        }
        protected e_language GetLanguage()
        {
            return _jwtTokenService.GetLanguage(HttpContext);
        }
        protected JwtTokenInfo GetUserInfo()
        {
            return _jwtTokenService.GetUserInfo(HttpContext);
        }
        protected ContentResult OK()
        {
            return new ResponeBaseSuccess().ToContentResult();
        }
        protected ContentResult OK(object data)
        {
            return new ResponeBaseSuccess(data).ToContentResult();
        }
        protected Task<ContentResult> OKAsync(object data)
        {
            return new ResponeBaseSuccess(data).ToContentResultAsync();
        }
        protected ContentResult BadRequest(string message = "")
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                message = "Yêu cầu không hợp lệ.";
            }
            return new ResponeBaseErr(message).ToContentResult();

        }
        protected Task<ContentResult> BadRequestAsync(string message = "")
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                message = "Yêu cầu không hợp lệ.";
            }
            return new ResponeBaseErr(message).ToContentResultAsync();
        }
        protected async Task<string> MessageLocalized(string code)
        {
            var _localizedResourceService = _serviceWrapper.Core.LocalizedResource;
            var language = this.GetLanguage();
            return await _localizedResourceService.GetValueByKeyAsync(e_localized_resource_scope.Api, code, language.ToString());
        }
        protected bool IsUserCanAccessApi(string method, string path, string baseOnApis = "")
        {
            return _serviceWrapper.BaseService.IsUserCanAccessApi(method, path, baseOnApis);
        }
        protected async Task SaveLogAsync(string message, object payload, JwtTokenInfo userInfo = null)
        {

            try
            {
                var request = HttpContext.Request;
                var path = request.Path.ToString();
                var method = request.Method.ToString();
                if (userInfo == null) userInfo = this.GetUserInfo();
                var logInfo = new log()
                {
                    content = message,
                    created_at = DateTime.Now,
                    donvi_ma_dv = userInfo.donvi_ma_dv,
                    endpoint = path,
                    id = 0,
                    method = method,
                    ip = HttpContext.Connection.RemoteIpAddress.ToString(),
                    payload = payload != null ? JsonSerializer.Serialize(payload) : string.Empty,
                    username = userInfo.username
                };
                _taskQueue.EnqueueTask(async _ =>
                     {
                         await _serviceWrapper.User.Log.InsertAsync(logInfo);
                     });
            }
            catch (Exception ex)
            {
                return;
            }
        }
        protected async Task<bool> ExcuteDbTasks<T>(List<Task<T>> tasks, int batchSize = 10)
        {
            var totalPage = (int)Math.Ceiling((double)tasks.Count / batchSize);
            for (var page_index = 0; page_index < totalPage; page_index++)
            {
                var taskPaged = tasks
                .Skip(page_index * batchSize)
                .Take(batchSize)
                .ToList();

                await Task.WhenAll(taskPaged);
            }
            return true;
        }

    }
}

