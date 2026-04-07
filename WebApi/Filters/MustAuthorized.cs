using System;
using System.Linq;
using System.Text.RegularExpressions;
using Common;
using Contract.Service;
using Contract.Service.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Model.Respone.Account;

namespace WebApi.Filters
{
    public class MustAuthorized : ActionFilterAttribute
    {
        public string _baseOnApi { get; set; }
        private IJwtTokenService _jwtTokenService;
        public MustAuthorized()
        {
        }
        public MustAuthorized(string BaseOnApis)
        {
            _baseOnApi = BaseOnApis;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                _jwtTokenService = (IJwtTokenService)context.HttpContext.RequestServices.GetService(typeof(IJwtTokenService));
                var _serviceWrapper = (IServiceWrapper)context.HttpContext.RequestServices.GetService(typeof(IServiceWrapper));
                var userId = _jwtTokenService.GetUserId(context.HttpContext);
                if (userId > 0)
                {
                    var request = context.HttpContext.Request;
                    var path = request.Path.ToString();
                    if (path.Length > 1 && path.Substring(0, 1) == "/")
                    {
                        path = path.Substring(1, path.Length - 1);
                    }
                    var method = request.Method.ToString();
                    var profile = _serviceWrapper.Cache.GetData<ProfileRespone>("TOKEN_PROFILE_" + userId.ToString());
                    //string original = "api/so-diem/{id}";
                    //string requested = "api/so-diem/2";
                    //string originalPattern = Regex.Replace(original, @"\{[^\}]*\}", @"\w*");
                    //var isMatch = Regex.Match(requested, $"^{originalPattern}$", RegexOptions.IgnoreCase).Success;
                    var _isAcessed = false;
                    if (profile != null)
                    {
                        //Regex.Match(path, $"^{Regex.Replace(x.endpoint, @"\{[^\}]*\}", @"\w*")}$", RegexOptions.IgnoreCase)
                        var apis = profile.apis;
                        var checkApi = apis.Where(x =>
                        (Regex.Match(path, $"^{Regex.Replace(x.endpoint, @"\{[^\}]*\}",@"[^/]+")}$", RegexOptions.IgnoreCase).Success
                        || x.endpoint == path)
                         && x.method == method).FirstOrDefault();
                        if (checkApi != null)
                        {
                            _isAcessed = true;
                        }
                        if (!_isAcessed)
                        {
                            var baseOnApis = _baseOnApi.ConvertToString().Split(",").Where(x => x != string.Empty).ToList();

                            foreach (var baseOnApi in baseOnApis)
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
                            if (context.HttpContext.Request.Method.ToString().ToUpper() != "GET")
                            {
                                // await _serviceWrapper.MySystemWrapper.Log.InsertAsync(context.HttpContext, _user_id);
                            }
                            return;
                        }
                        else
                        {
                            context.Result = new UnauthorizedResult();
                        }
                    }

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

