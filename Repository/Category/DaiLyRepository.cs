using Common;
using Contracts.Repository.Base;
using Contracts.Repository.Category;
using Dapper;
using Model.FuncResult;
using Model.Request.Base;
using Model.Table;
using Repository.Base;
using System.Data;


namespace Repository.Category
{
    public class DaiLyRepository : CRUDRepository<dai_ly>, IDaiLyRepository
    {
        public DaiLyRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {

        }

        public async Task<bool> InsertsAsync(IEnumerable<dai_ly> dailys)
        {
            // 1. Khởi tạo DataTable với tên Type tương ứng trong SQL
            var table = new DataTable("utp_dm_daily");

            // 2. Định nghĩa các cột dựa trên cấu trúc DB trong ảnh
            table.Columns.Add("id", typeof(int));
            table.Columns.Add("donvi_ma_dv", typeof(string));
            table.Columns.Add("ma_dai_ly", typeof(string));
            table.Columns.Add("ten_dai_ly", typeof(string));
            table.Columns.Add("email", typeof(string));
            table.Columns.Add("so_tai_khoan", typeof(string));
            table.Columns.Add("is_deleted", typeof(bool));
            table.Columns.Add("created_time", typeof(DateTime));
            table.Columns.Add("created_user_id", typeof(int));
            table.Columns.Add("last_modified_times", typeof(DateTime));
            table.Columns.Add("last_modified_user_id", typeof(int));

            // 3. Đổ dữ liệu từ danh sách vào table
            foreach (var item in dailys)
            {
                table.Rows.Add(
                    0, // id: Truyền 0 nếu là cột Identity hoặc bắt buộc Not Null
                    item.donvi_ma_dv,
                    item.ma_dai_ly,
                    item.ten_dai_ly,
                    item.email,
                    item.so_tai_khoan,
                    item.is_deleted,
                    item.created_time == default(DateTime) ? DateTime.Now : item.created_time,
                    item.created_user_id,
                    item.last_modified_times == default(DateTime) ? DateTime.Now : item.last_modified_times,
                    item.last_modified_user_id
                );
            }

            try
            {
                // ...existing code...
                var param = new DynamicParameters();
                param.Add("@dailys", table.AsTableValuedParameter("utp_dm_daily"));
                var result = await _dbConnection.ExecuteAsync("dm_daily_inserts", param);
                return result;
            }
            catch (Exception ex)
            {
                // Log ra console hoặc file
                Console.WriteLine("Lỗi khi insert: " + ex.Message);
                // Có thể log thêm ex.StackTrace nếu cần
                return false;
            }
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