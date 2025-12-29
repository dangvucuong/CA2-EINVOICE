using Contract.Repository.Category;
using Contracts.Repository.Base;
using Dapper;
using Model.Request.Base;
using Model.Table;
using Repository.Base;
using Common;
using Model.FuncResult;
namespace Repository.Category
{
    public class KhachHangRepository : CRUDRepository<khachhang>, IKhachHangRepository
    {
        public KhachHangRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public Task<bool> InsertsAsync(IEnumerable<khachhang> khachhangs)
        {
            var param = new DynamicParameters();
            param.Add("@khachhangs", khachhangs.ConvertToTableValuedParameter("utp_khachhang"));
            return _dbConnection.ExecuteAsync("khachhang_inserts", param);
        }

        public async Task<PagingResult<IEnumerable<khachhang>>> SelectByDonViAsync(string donvi_ma_dv, PagingRequest pagingRequest)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", donvi_ma_dv);
            param.Add("@page_index", pagingRequest.page_index);
            param.Add("@page_size", pagingRequest.page_size);
            param.Add("@sort_by", pagingRequest.sort_by.ConvertToString());
            param.Add("@sort_mode", pagingRequest.sort_mode.ConvertToString());
            param.Add("@search_key", pagingRequest.search_key.ConvertToString());
            param.Add("@total_count", dbType: System.Data.DbType.Int64, direction: System.Data.ParameterDirection.Output);
            var list = await _dbConnection.SelectAsync<khachhang>("khachhang_select_bydonvi_paging", param);
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
            return new PagingResult<IEnumerable<khachhang>>(pagingResultSummaries, list);
        }

        public Task<khachhang> SelectByDonViAsync(string donvi_ma_dv, string khach_hang_mst)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", donvi_ma_dv);
            param.Add("@khach_hang_mst", khach_hang_mst);
            return _dbConnection.SelectFirstOrDefaultAsync<khachhang>("khachhang_select_bydonvi_mst", param);

        }
    }
}