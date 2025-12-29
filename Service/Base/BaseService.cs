using System.Text.RegularExpressions;
using Common;
using Contract.Service;
using Contracts.Repository;
using Contracts.Service.Base;
using Contracts.Service.Core;
using Microsoft.Extensions.DependencyInjection;
using Model.Respone.Account;
using Model.Table;

namespace Service.Base
{
    public class BaseService : IBaseService
    {
        //protected ILocalizedResourceService _localizedResourceService;
        protected IServiceProvider _serviceProvider;
        protected IServiceWrapper _serviceWrapper;
        protected IRepositoryWrapper _repositoryWrapper;
        protected ITaskQueueService _taskQueueService;

        public BaseService(IServiceProvider serviceProvider)
        {
            //không được get thằng service nào mà nó lại kết thừa BaseService vì sẽ dẫn đến lỗi Stack Overflow
            var scope = serviceProvider.CreateScope();
            this._serviceProvider = scope.ServiceProvider;
            this._serviceWrapper = _serviceProvider.GetRequiredService<IServiceWrapper>();
            this._repositoryWrapper = _serviceProvider.GetRequiredService<IRepositoryWrapper>();
            this._taskQueueService = _serviceProvider.GetRequiredService<ITaskQueueService>();
        }

        public int GetCurrentUserId()
        {
            try
            {
                var httpContext = _serviceWrapper._httpContextAccessor.HttpContext;
                return _serviceWrapper.Core.JwtToken.GetUserId(httpContext);
            }
            catch (System.Exception)
            {

                return 0;
            }
        }
        public JwtTokenInfo GetCurrentUser()
        {
            try
            {
                var httpContext = _serviceWrapper._httpContextAccessor.HttpContext;
                return _serviceWrapper.Core.JwtToken.GetUserInfo(httpContext);
            }
            catch (System.Exception)
            {

                return null;
            }
        }

        public async Task<string> MessageLocalizedAsync(string code)
        {
            return code;
            //var _localizedResourceService = _serviceWrapper.MySystemWrapper.LocalizedResource;
            //var httpContext = _serviceWrapper._httpContextAccessor.HttpContext;
            //var language = _serviceWrapper.MySystemWrapper.Token.GetLanguage(httpContext);
            //return await this.MessageLocalized(code, language.ToString());
        }
        public async Task<string> MessageLocalized(string code, string language)
        {
            return code;
            //var _localizedResourceService = _serviceWrapper.MySystemWrapper.LocalizedResource;
            //language = language.ConvertToString() != string.Empty ? language : e_language.vi.ToString();
            //return await _localizedResourceService.GetValueByKeyAsync(Model.Enum.e_localized_resource_scope.SisApi, code, language.ToString());
        }

        public bool IsUserCanAccessApi(string method, string path, string baseOnApis = "")
        {
            var userId = this.GetCurrentUserId();
            var profile = _serviceWrapper.Cache.GetData<ProfileRespone>("TOKEN_PROFILE_" + userId.ToString());
            var _isAcessed = false;
            if (profile != null)
            {
                var apis = profile.apis;
                var checkApi = apis.Where(x =>
                (Regex.Match(path, $"^{Regex.Replace(x.endpoint, @"\{[^\}]*\}", @"\w*")}$", RegexOptions.IgnoreCase).Success
                || x.endpoint == path)
                 && x.method == method).FirstOrDefault();
                if (checkApi != null)
                {
                    _isAcessed = true;
                }
                if (!_isAcessed)
                {
                    var _baseOnApis = baseOnApis.ConvertToString().Split(",").Where(x => x != string.Empty).ToList();

                    foreach (var baseOnApi in _baseOnApis)
                    {
                        if (_isAcessed) break;
                        var apiMethod = baseOnApi.Substring(0, baseOnApi.LastIndexOf("]")).Replace("[", "").Replace("]", "");
                        var apiPath = baseOnApi.Replace($"[{apiMethod}]", "");
                        if (apiPath.Length > 1 && apiPath.Substring(0, 1) == "/")
                        {
                            apiPath = apiPath.Substring(1, apiPath.Length - 1);
                        }
                        var apiCheckBaseOnApi = apis.Where(x =>
               (Regex.Match(apiPath, $"^{Regex.Replace(x.endpoint, @"\{[^\}]*\}", @"\w*")}$", RegexOptions.IgnoreCase).Success
|| x.endpoint == apiPath)
                && x.method == apiMethod).FirstOrDefault();
                        if (apiCheckBaseOnApi != null)
                        {
                            _isAcessed = true;
                        }
                    }
                }
                if (_isAcessed)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<donvi> GetCurrentDonViAsync()
        {
            var userInfo = this.GetCurrentUser();
            if (userInfo != null && userInfo.donvi_ma_dv.ConvertToString() != "")
            {
                var donVi = await _serviceWrapper.Category.DonVi.SelectByMaDonViAsync(userInfo.donvi_ma_dv);
                return donVi;
            }
            return null;
        }

        public async Task<bool> ExcuteDbTasks<T>(List<Task<T>> tasks, int batchSize = 10)
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


