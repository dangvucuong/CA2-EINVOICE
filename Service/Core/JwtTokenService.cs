using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common;
using Contract.Service.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Model.Base;
using Model.Enum;
using Model.Respone.Account;
using Model.Static;

namespace Service.Core
{
    public class JwtTokenService : IJwtTokenService
    {


        public async Task<string> CreateAccessTokenAsync(JwtTokenInfo obj)
        {
            try
            {
                string key = AppSettings.JwtConfig.Key;
                var issuer = AppSettings.JwtConfig.Issuer;

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var permClaims = new List<Claim>();
                permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                permClaims.Add(new Claim("id", obj.id.ToString()));
                permClaims.Add(new Claim("full_name", obj.full_name.ToString()));
                permClaims.Add(new Claim("username", obj.username.ToString()));
                permClaims.Add(new Claim("donvi_ma_dv", obj.donvi_ma_dv.ToString()));
                permClaims.Add(new Claim("vender_id", obj.vender_id.ConvertToString()));
                permClaims.Add(new Claim("is_hsm_signing", obj.is_hsm_signing.ConvertToString()));
                permClaims.Add(new Claim("is_remote_signing", obj.is_remote_signing.ConvertToString()));
                var token = new JwtSecurityToken(issuer,
                                issuer,
                                permClaims,
                                expires: DateTime.Now.AddDays(7),
                                signingCredentials: credentials);
                var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
                return jwt_token;
            }
            catch (Exception ex)
            {
                //ex.SaveLog("TokenService/GetIP");
                return string.Empty;
            }
        }

        public async Task<string> CreateRefreshTokenAsync(JwtRefreshTokenInfo model)
        {
            try
            {
                var accessToken = model.access_token;
                var user_id = model.user_id;
                string key = AppSettings.JwtConfig.Key;
                var issuer = AppSettings.JwtConfig.Issuer;

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var permClaims = new List<Claim>();
                permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                permClaims.Add(new Claim("access_token", accessToken));
                permClaims.Add(new Claim("user_id", user_id.ToString()));
                var token = new JwtSecurityToken(issuer,
                                issuer,
                                permClaims,
                                expires: DateTime.Now.AddYears(1),
                                signingCredentials: credentials);
                var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
                return jwt_token;
            }
            catch (Exception ex)
            {
                //ex.SaveLog("TokenService/GetIP");

                return string.Empty;
            }
        }

        public string GetIP(HttpContext httpContext)
        {
            try
            {
                var IP = httpContext.Connection.RemoteIpAddress.ToString();
                return IP;
            }
            catch (Exception ex)
            {
                //ex.SaveLog("TokenService/GetIP");
                return string.Empty;
            }
        }

        public async Task<string> GetIPAsync(HttpContext httpContext)
        {
            return this.GetIP(httpContext);
        }

        public e_language GetLanguage(HttpContext httpContext)
        {
            try
            {
                StringValues language;
                if (httpContext.Request.Headers.TryGetValue("language", out language))
                {
                    var language_value = language.ToString();
                    foreach (string name in Enum.GetNames(typeof(e_language)))
                    {
                        if (name == language_value)
                        {
                            return (e_language)Enum.Parse(typeof(e_language), name);
                        }
                    }
                }
                return e_language.vi;
            }
            catch (Exception ex)
            {
                //ex.SaveLog("TokenService/GetLanguage");
                return e_language.vi;
            }
        }

        public async Task<e_language> GetLanguageAsync(HttpContext httpContext)
        {
            return this.GetLanguage(httpContext);
        }


        public int GetUserId(HttpContext httpContext)
        {
            try
            {
                var userToken = this.GetUserToken(httpContext);
                var validateTokenResult = this.ReadAccessToken(userToken);
                if (validateTokenResult.is_success)
                {
                    return validateTokenResult.data.id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                //ex.SaveLog("TokenService/GetUserId");
                return 0;
            }
        }

        public async Task<int> GetUserIdAsync(HttpContext httpContext)
        {
            return this.GetUserId(httpContext);
        }

        public JwtTokenInfo GetUserInfo(HttpContext httpContext)
        {
            var token = this.GetUserToken(httpContext);
            var result = this.ReadAccessToken(token);
            if (result.is_success)
            {
                return result.data;
            }
            return null;
        }

        public string GetUserToken(HttpContext httpContext)
        {
            try
            {
                var request = httpContext.Request;
                var jwt_token = request.Headers[HeaderNames.Authorization].ConvertToString().Replace("Bearer ", "");
                return jwt_token;
            }
            catch (Exception ex)
            {
                //ex.SaveLog("TokenService/GetUserToken");
                return string.Empty;
            }
        }

        public async Task<string> GetUserTokenAsync(HttpContext httpContext)
        {
            return this.GetUserToken(httpContext);
        }

        public FunctionResult<JwtTokenInfo> ReadAccessToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(AppSettings.JwtConfig.Key);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var id = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
                var full_name = jwtToken.Claims.First(x => x.Type == "full_name").Value;
                var username = jwtToken.Claims.First(x => x.Type == "username").Value;
                var donvi_ma_dv = jwtToken.Claims.First(x => x.Type == "donvi_ma_dv").Value;
                var vender_id = jwtToken.Claims.FirstOrDefault(x => x.Type == "vender_id")?.Value ?? "";
                var is_hsm_signing = jwtToken.Claims.FirstOrDefault(x => x.Type == "is_hsm_signing")?.Value ?? "false";
                var is_remote_signing = jwtToken.Claims.FirstOrDefault(x => x.Type == "is_remote_signing")?.Value ?? "false";

                return new SuccessResult<JwtTokenInfo>(new JwtTokenInfo()
                {
                    id = id,
                    full_name = full_name,
                    username = username,
                    donvi_ma_dv = donvi_ma_dv,
                    vender_id = vender_id,
                    is_hsm_signing= is_hsm_signing.ConvertToBoolean(),
                    is_remote_signing= is_remote_signing.ConvertToBoolean()
                });


            }

            catch (Exception ex)
            {
                //ex.SaveLog("TokenService/ValidateAccessToken");
                return new ErrorResult<JwtTokenInfo>();

            }
        }

        public async Task<FunctionResult<JwtTokenInfo>> ReadAccessTokenAsync(string token)
        {
            return this.ReadAccessToken(token);
        }

        public FunctionResult<JwtRefreshTokenInfo> ReadRefreshToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(AppSettings.JwtConfig.Key);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var user_id = int.Parse(jwtToken.Claims.First(x => x.Type == "user_id").Value);
                var access_token = jwtToken.Claims.First(x => x.Type == "access_token").Value;

                return new SuccessResult<JwtRefreshTokenInfo>(new JwtRefreshTokenInfo()
                {
                    access_token = access_token,
                    user_id = user_id
                });


            }

            catch (Exception ex)
            {
                //ex.SaveLog("TokenService/ValidateAccessToken");
                return new ErrorResult<JwtRefreshTokenInfo>();

            }
        }

        public async Task<FunctionResult<JwtRefreshTokenInfo>> ReadRefreshTokenAsync(string token)
        {
            return this.ReadRefreshToken(token);
        }
    }
}

