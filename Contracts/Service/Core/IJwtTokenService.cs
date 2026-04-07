using Microsoft.AspNetCore.Http;
using Model.Base;
using Model.Enum;
using Model.Respone.Account;

namespace Contract.Service.Core
{
    public interface IJwtTokenService
    {
        public Task<string> CreateAccessTokenAsync(JwtTokenInfo obj);
        public Task<string> CreateRefreshTokenAsync(JwtRefreshTokenInfo obj);
        public Task<FunctionResult<JwtTokenInfo>> ReadAccessTokenAsync(string token);
        public Task<FunctionResult<JwtRefreshTokenInfo>> ReadRefreshTokenAsync(string token);
        public Task<string> GetUserTokenAsync(HttpContext httpContext);
        public Task<int> GetUserIdAsync(HttpContext httpContext);
        public Task<e_language> GetLanguageAsync(HttpContext httpContext);
        public Task<string> GetIPAsync(HttpContext httpContext);

        public JwtTokenInfo GetUserInfo(HttpContext httpContext);
        public FunctionResult<JwtTokenInfo> ReadAccessToken(string token);
        public FunctionResult<JwtRefreshTokenInfo> ReadRefreshToken(string token);
        public string GetUserToken(HttpContext httpContext);
        public int GetUserId(HttpContext httpContext);
        public e_language GetLanguage(HttpContext httpContext);
        public string GetIP(HttpContext httpContext);
    }
}

