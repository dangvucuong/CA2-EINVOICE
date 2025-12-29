using Common;
using Contract.Repository.User;
using Contracts.Repository.Base;
using Dapper;
using Model.FuncResult;
using Model.Request.Base;
using Model.Table;
using Repository.Base;

namespace Repository.User
{
    public class UserRepository : CRUDRepository<user>, IUserRepository
    {
        public UserRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public async Task<PagingResult<IEnumerable<user>>> SelectByDonViAsync(string donvi_ma_dv, PagingRequest pagingRequest)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", donvi_ma_dv);
            param.Add("@page_index", pagingRequest.page_index);
            param.Add("@page_size", pagingRequest.page_size);
            param.Add("@sort_by", pagingRequest.sort_by.ConvertToString());
            param.Add("@sort_mode", pagingRequest.sort_mode.ConvertToString());
            param.Add("@search_key", pagingRequest.search_key.ConvertToString());
            param.Add("@total_count", dbType: System.Data.DbType.Int64, direction: System.Data.ParameterDirection.Output);
            var list = await _dbConnection.SelectAsync<user>("user_select_bydonvi_paging", param);
            var total_count = param.Get<long>("@total_count");
            var page_size = pagingRequest?.page_size ?? 1;
            if (page_size == 0) page_size = 1;
            var page_count = (int)total_count / page_size;
            var pagingResultSummaries = new PagingResultSummary()
            {
                page_count = page_count * page_size < total_count ? (page_count + 1) : page_count,
                page_number = pagingRequest?.page_index ?? 0,
                page_size = pagingRequest?.page_size ?? 0,
                total_count = total_count
            };
            return new PagingResult<IEnumerable<user>>(pagingResultSummaries, list);
        }

        public Task<user> SelectByUsernameAsync(string donvi_ma_dv, string username)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", donvi_ma_dv);
            param.Add("@username", username);
            return _dbConnection.SelectFirstOrDefaultAsync<user>("user_select_by_donvi_username", param);
        }

        public Task<user> SelectByEmailAsync(string email)
        {
            var param = new DynamicParameters();
            param.Add("@email", email);
            return _dbConnection.SelectFirstOrDefaultAsync<user>("user_select_by_email", param);
        }

        public Task<user> SelectByEmailAsync(string donvi_ma_dv, string email)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", donvi_ma_dv);
            param.Add("@email", email);
            return _dbConnection.SelectFirstOrDefaultAsync<user>("user_select_by_email", param);
        }

        public Task<bool> ChangePWAsync(int id, string newHasedPW)
        {
            var param = new DynamicParameters();
            param.Add("@id", id);
            param.Add("@password", newHasedPW);
            return _dbConnection.ExecuteAsync("user_change_pw", param);
        }

        public Task<user> SelectBySerialAsync(string serial, string mst)
        {
            var param = new DynamicParameters();
            param.Add("@serial", serial);
            param.Add("@mst", mst);
            return _dbConnection.SelectFirstOrDefaultAsync<user>("user_select_by_mst_serial", param);

        }

        public Task<user> SelectByMaButKyAsync(string rs_ma_but_ky)
        {
            var param = new DynamicParameters();
            param.Add("@rs_ma_but_ky", rs_ma_but_ky);
            return _dbConnection.SelectFirstOrDefaultAsync<user>("user_select_by_rs_ma_but_ky", param);
        }

        public Task<IEnumerable<user>> SelectToUpdatePWFromV1Async()
        {
            return _dbConnection.SelectAsync<user>("user_select_to_update_pw_from_v1");
        }
    }
}

