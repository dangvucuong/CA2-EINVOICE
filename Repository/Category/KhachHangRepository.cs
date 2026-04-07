using Contract.Repository.Category;
using Contracts.Repository.Base;
using Dapper;
using Model.Request.Base;
using Model.Table;
using Repository.Base;
using Common;
using Model.FuncResult;
using System.Data;


namespace Repository.Category
{
    public class KhachHangRepository : CRUDRepository<khachhang>, IKhachHangRepository
    {
        public KhachHangRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        // public Task<bool> InsertsAsync(IEnumerable<khachhang> khachhangs)
        // {
        //     var param = new DynamicParameters();
        //     param.Add("@khachhangs", khachhangs.ConvertToTableValuedParameter("utp_khachhang_edit"));
        //     return _dbConnection.ExecuteAsync("khachhang_inserts", param);
        // }

        public async Task<bool> InsertsAsync(IEnumerable<khachhang> khachHangs)
        {
            var table = new DataTable("utp_dm_khach_hang");

            table.Columns.Add("id", typeof(int));
            table.Columns.Add("donvi_ma_dv", typeof(string));
            table.Columns.Add("ten_khach_hang", typeof(string));
            table.Columns.Add("ten_don_vi", typeof(string));
            table.Columns.Add("dia_chi", typeof(string));
            table.Columns.Add("stk", typeof(string));
            table.Columns.Add("mst", typeof(string));
            table.Columns.Add("email", typeof(string));
            table.Columns.Add("is_deleted", typeof(bool));
            table.Columns.Add("created_time", typeof(DateTime));
            table.Columns.Add("created_user_id", typeof(int));
            table.Columns.Add("last_modified_times", typeof(DateTime));
            table.Columns.Add("last_modified_user_id", typeof(int));
            table.Columns.Add("ma_dv_ngan_sach", typeof(string));
            table.Columns.Add("ccdan", typeof(string));

            // 2. Đổ dữ liệu
            foreach (var item in khachHangs)
            {
                table.Rows.Add(
                    0, // id = 0 nếu DB tự tăng
                    item.donvi_ma_dv,
                    item.ten_khach_hang,
                    item.ten_don_vi,
                    item.dia_chi,
                    item.stk,
                    item.mst,
                    item.email,
                    item.is_deleted,
                    item.created_time == default ? DateTime.Now : item.created_time,
                    item.created_user_id,
                    item.last_modified_times == default ? DateTime.Now : item.last_modified_times,
                    item.last_modified_user_id,
                    item.ma_dv_ngan_sach,
                    item.ccdan
                );
            }

            try
            {
                // ...existing code...
                var param = new DynamicParameters();
                param.Add("@dskhachhang", table.AsTableValuedParameter("utp_dm_khach_hang"));
                var result = await _dbConnection.ExecuteAsync("sp_inserts_khach_hang", param);
                return result;
            }
            catch (Exception ex)
            {
                // log thực tế nên dùng ILogger / Serilog
                Console.WriteLine("Lỗi import khách hàng: " + ex.Message);
                throw; // giữ stack trace
            }
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