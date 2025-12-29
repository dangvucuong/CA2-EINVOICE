using Model.Respone.Account;
using Model.Table;
namespace Contracts.Service.Base
{
    public interface IBaseService
    {
        Task<string> MessageLocalizedAsync(string code);
        int GetCurrentUserId();
        bool IsUserCanAccessApi(string method, string path, string baseOnApis = "");
        JwtTokenInfo GetCurrentUser();
        Task<donvi> GetCurrentDonViAsync();
    }
}

