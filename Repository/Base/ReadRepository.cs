using Common;
using Contracts.Repository.Base;
using Dapper;
using Model.FuncResult;
using Model.Request.Base;

namespace Repository.Base
{
    public class ReadRepository<T> : BaseRepository, IReadRepository<T>
    {
        public ReadRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public  Task<IEnumerable<T>> SelectAllAsync()
        {
            try
            {

                var tableName = typeof(T).Name;
                return  _dbConnection.SelectAsync<T>(String.Format("{0}_select_all", tableName));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<PagingResult<IEnumerable<T>>> SelectAsync(PagingRequest pagingRequest)
        {
            try
            {
                var tableName = typeof(T).Name;
                var param = new DynamicParameters();
                param.Add("@page_index", pagingRequest.page_index);
                param.Add("@page_size", pagingRequest.page_size);
                param.Add("@sort_by", pagingRequest.sort_by.ConvertToString());
                param.Add("@sort_mode", pagingRequest.sort_mode.ConvertToString());
                param.Add("@search_key", pagingRequest.search_key.ConvertToString());
                param.Add("@total_count", dbType: System.Data.DbType.Int64, direction: System.Data.ParameterDirection.Output);
                var list = await _dbConnection.SelectAsync<T>(String.Format("{0}_select_paging", tableName), param);
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
                return new PagingResult<IEnumerable<T>>(pagingResultSummaries, list);
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public async Task<T> SelectByIdAsync(int id)
        {
            try
            {
                var tableName = typeof(T).Name;

                var param = new DynamicParameters();
                param.Add("@id", id);
                var ressult = await _dbConnection.SelectAsync<T>(String.Format("{0}_select_by_id", tableName), param);
                return ressult.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<T>> SelectChangedAsync(DateTime fromTime)
        {
            try
            {
                var tableName = typeof(T).Name;

                var param = new DynamicParameters();
                param.Add("@fromTime", fromTime);
                var ressult = await _dbConnection.SelectAsync<T>(String.Format("{0}_select_changed", tableName), param);
                return ressult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

