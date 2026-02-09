using Contracts.Repository.Base;
using Model.FuncResult;
using Model.Request.Base;
using Model.Table;

namespace Contract.Repository.User
{
    public interface IUserRepository : ICRUDRepository<user>
    {
        Task<user> SelectByEmailAsync(string donvi_ma_dv, string email);
        Task<user> SelectBySerialAsync(string serial, string mst);
        Task<user> SelectByMaButKyAsync(string rs_ma_but_ky);

        Task<PagingResult<IEnumerable<user>>> SelectByDonViAsync(string donvi_ma_dv, PagingRequest pagingRequest);
        Task<user> SelectByUsernameAsync(string donvi_ma_dv, string username);
        Task<bool> ChangePWAsync(int id, string newHasedPW);
        Task<IEnumerable<user>> SelectToUpdatePWFromV1Async();

        Task<UserDeleteResult> RemoveUserAsync(int id, int user_id);



        // RemoveUserAsync
    }
}

