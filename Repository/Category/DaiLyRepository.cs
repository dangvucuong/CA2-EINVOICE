using Common;
using Contracts.Repository.Base;
using Contracts.Repository.Category;
using Dapper;
using Model.FuncResult;
using Model.Request.Base;
using Model.Table;
using Repository.Base;

namespace Repository.Category
{
    public class DaiLyRepository : CRUDRepository<dai_ly>, IDaiLyRepository
    {
        public DaiLyRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public async Task<PagingResult<IEnumerable<dai_ly>>> SelectByDonViAsync(string donvi_ma_dv, PagingRequest pagingRequest)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", donvi_ma_dv);
            param.Add("@page_index", pagingRequest.page_index);
            param.Add("@page_size", pagingRequest.page_size);
            param.Add("@sort_by", pagingRequest.sort_by.ConvertToString());
            param.Add("@sort_mode", pagingRequest.sort_mode.ConvertToString());
            param.Add("@search_key", pagingRequest.search_key.ConvertToString());
            param.Add("@total_count", dbType: System.Data.DbType.Int64, direction: System.Data.ParameterDirection.Output);
            var list = await _dbConnection.SelectAsync<dai_ly>("dai_ly_select_bydonvi_paging", param);
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
            return new PagingResult<IEnumerable<dai_ly>>(pagingResultSummaries, list);
        }

        public Task<IEnumerable<dai_ly>> SelectByDonViHaveEmailAsync(string donvi_ma_dv)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", donvi_ma_dv);
            return _dbConnection.SelectAsync<dai_ly>("dai_ly_select_bydonvi_have_email", param);
        }
    }
}