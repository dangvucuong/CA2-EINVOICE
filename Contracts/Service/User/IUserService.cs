using Contracts.Service.Base;
using Model;
using Model.Base;
using Model.FuncResult;
using Model.Request.Base;
using Model.Respone.Account;
using Model.Respone.User;
using Model.Table;

namespace Contract.Service.User
{
    public interface IUserService : ICRUDService<user>
    {
        Task<user> SelectByUsernameAsync(string donvi_ma_dv, string username);
        Task<user> SelectBySerialAsync(string serial, string mst);
        Task<JwtTokenInfo> SelectAndFormatJwtTokenAsync(int id);
        Task<user> SelectByMaButKyAsync(string rs_ma_but_ky);
        Task<user> SelectByEmailAsync(string donvi_ma_dv, string email);
        Task<UserEditModel> SelectEditModelByIdAsync(int id);
        Task<FunctionResult<UserEditModel>> SaveChangeAsync(UserEditModel model);
        Task<FunctionResult<bool>> ValidateUserExited(UserEditModel model);
        Task<FunctionResult<bool>> UpdateRemoteSigningSerialAsync(UserUpdateRemoteSigningSerialNumberRequest model);
        Task<FunctionResult<bool>> UpdateSerialNumberAsync(UserUpdateSerialNumberRequest model);
        Task<PagingResult<IEnumerable<user>>> SelectByDonViAsync(string donvi_ma_dv, PagingRequest pagingRequest);
        Task<bool> SyncFromCtsAsync(don_vi_cts obj);
    }
}

