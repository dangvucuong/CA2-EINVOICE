using Contract.Repository.Category;
using Contracts.Repository.Base;
using Dapper;
using Model.FuncResult;
using Model.Request.Base;
using Model.Table;
using Repository.Base;
using Common;
using System.Data;
namespace Repository.Category
{
    public class HangHoaRepository : CRUDRepository<dm_hanghoa>, IHangHoaRepository
    {
        public HangHoaRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public Task<bool> InsertsAsync(IEnumerable<dm_hanghoa> hangHoas)
        {
            var table = new DataTable("utm_dm_hanghoa_update");

            table.Columns.Add("create_time", typeof(DateTime));
            table.Columns.Add("create_user_id", typeof(int));
            table.Columns.Add("donvi_ma_dv", typeof(string));
            table.Columns.Add("dvt", typeof(string));
            table.Columns.Add("id", typeof(int));
            table.Columns.Add("is_deleted", typeof(bool));
            table.Columns.Add("last_modified_times", typeof(DateTime));
            table.Columns.Add("last_modified_user_id", typeof(int));
            table.Columns.Add("ma_hang_hoa", typeof(string));
            table.Columns.Add("ma_loai_hoang_hoa", typeof(string));
            table.Columns.Add("ten_hang_hoa", typeof(string));
            table.Columns.Add("don_gia", typeof(decimal));

            foreach (var item in hangHoas)
            {
                table.Rows.Add(
                    item.created_time == default(DateTime) ? DateTime.Now : item.created_time,

                    // create_user_id
                    item.created_user_id,

                    // donvi_ma_dv
                    item.donvi_ma_dv,

                    // dvt
                    item.dvt,

                    // id (Truyền 0 vì SQL bắt buộc not null)
                    0,

                    // is_deleted
                    item.is_deleted,

                    // last_modified_times
                    item.last_modified_times == default(DateTime) ? DateTime.Now : item.last_modified_times,

                    // last_modified_user_id
                    item.last_modified_user_id,

                    // ma_hang_hoa
                    item.ma_hang_hoa,

                    // ma_loai_hoang_hoa
                    item.ma_loai_hoang_hoa,

                    // ten_hang_hoa
                    item.ten_hang_hoa,

                    // don_gia
                    item.don_gia ?? 0
                );
            }

            // 4. Gọi SQL
            var param = new DynamicParameters();
            // Dùng AsTableValuedParameter thay vì ConvertTo...
            param.Add("@hangHoas", table.AsTableValuedParameter("utm_dm_hanghoa_update"));

            return _dbConnection.ExecuteAsync("dm_hangHoas_inserts", param);
        }

        //public async Task<bool> InsertsAsync(IEnumerable<dm_hanghoa> hangHoas)
        //{
        //    try
        //    {
        //        // 1. Tự tạo DataTable để kiểm soát thứ tự
        //        var table = new DataTable("utm_dm_hanghoa_update");

        //        // 2. Định nghĩa cột: PHẢI KHỚP 100% VỚI ẢNH SQL BẠN GỬI
        //        // Thứ tự: create_time -> create_user_id -> ... -> don_gia

        //        table.Columns.Add("create_time", typeof(DateTime));      // SQL tên là create_time
        //        table.Columns.Add("create_user_id", typeof(int));        // SQL tên là create_user_id
        //        table.Columns.Add("donvi_ma_dv", typeof(string));
        //        table.Columns.Add("dvt", typeof(string));
        //        table.Columns.Add("id", typeof(int));
        //        table.Columns.Add("is_deleted", typeof(bool));
        //        table.Columns.Add("last_modified_times", typeof(DateTime));
        //        table.Columns.Add("last_modified_user_id", typeof(int));
        //        table.Columns.Add("ma_hang_hoa", typeof(string));
        //        table.Columns.Add("ma_loai_hoang_hoa", typeof(string));
        //        table.Columns.Add("ten_hang_hoa", typeof(string));
        //        table.Columns.Add("don_gia", typeof(decimal));           // Cột này nằm cuối

        //        // 3. Đổ dữ liệu vào đúng thứ tự trên
        //        foreach (var item in hangHoas)
        //        {
        //            table.Rows.Add(
        //                // create_time (Lấy từ C# created_time)
        //                item.created_time == default(DateTime) ? DateTime.Now : item.created_time,

        //                // create_user_id
        //                item.created_user_id,

        //                // donvi_ma_dv
        //                item.donvi_ma_dv,

        //                // dvt
        //                item.dvt,

        //                // id (Truyền 0 vì SQL bắt buộc not null)
        //                0,

        //                // is_deleted
        //                item.is_deleted,

        //                // last_modified_times
        //                item.last_modified_times == default(DateTime) ? DateTime.Now : item.last_modified_times,

        //                // last_modified_user_id
        //                item.last_modified_user_id,

        //                // ma_hang_hoa
        //                item.ma_hang_hoa,

        //                // ma_loai_hoang_hoa
        //                item.ma_loai_hoang_hoa,

        //                // ten_hang_hoa
        //                item.ten_hang_hoa,

        //                // don_gia
        //                item.don_gia ?? 0
        //            );
        //        }

        //        // 4. Gọi SQL
        //        var param = new DynamicParameters();
        //        // Dùng AsTableValuedParameter thay vì ConvertTo...
        //        param.Add("@hangHoas", table.AsTableValuedParameter("utm_dm_hanghoa_update"));

        //        var rowsAffected = await _dbConnection.ExecuteAsync("dm_hangHoas_inserts", param);

        //        return rowsAffected;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Lỗi Insert: {ex.Message}");
        //    }
        //}

        public async Task<PagingResult<IEnumerable<dm_hanghoa>>> SelectByDonViAsync(string donvi_ma_dv, PagingRequest pagingRequest)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", donvi_ma_dv);
            param.Add("@page_index", pagingRequest.page_index);
            param.Add("@page_size", pagingRequest.page_size);
            param.Add("@sort_by", pagingRequest.sort_by.ConvertToString());
            param.Add("@sort_mode", pagingRequest.sort_mode.ConvertToString());
            param.Add("@search_key", pagingRequest.search_key.ConvertToString());
            param.Add("@total_count", dbType: System.Data.DbType.Int64, direction: System.Data.ParameterDirection.Output);
            var list = await _dbConnection.SelectAsync<dm_hanghoa>("dm_hanghoa_select_bydonvi_paging", param);
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
            return new PagingResult<IEnumerable<dm_hanghoa>>(pagingResultSummaries, list);
        }

        public Task<IEnumerable<dm_hanghoa>> SelectByDonViAsync(string donvi_ma_dv, List<string> maHangs)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", donvi_ma_dv);
            param.Add("@OtherKeys", maHangs.ConvertToTableValuedParameter("OtherKeys"));
            return _dbConnection.SelectAsync<dm_hanghoa>("dm_hanghoa_select_bydonvi_mahangs", param);

        }
    }
}