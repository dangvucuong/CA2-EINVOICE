using Common;
using Contracts.Repository.Base;
using Contracts.Repository.HoaDon;
using Dapper;
using Model.FuncResult;
using Model.Request.HoaDon;
using Model.Respone.HoaDon;
using Model.Table;
using Repository.Base;

namespace Repository.HoaDon
{
    public class HoaDonHangHoaRepostiory : CRUDRepository<hoa_don_hang_hoa>, IHoaDonHangHoaRepostiory
    {
        public HoaDonHangHoaRepostiory(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public async Task<PagingResult<IEnumerable<hoa_don_hang_hoa_vm>>> SelectByDonViThongKePageAsync(string donvi_ma_dv, HoaDonSelectPagingRequest pagingRequest)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", donvi_ma_dv.ConvertToString());
            param.Add("@hoa_don_trang_thai_ids", pagingRequest.hoa_don_trang_thai_ids.ConvertToTableValuedParameter());
            param.Add("@loai_hoa_don_ct_id", pagingRequest.loai_hoa_don_ct_id.ConvertToInt());
            param.Add("@hoa_don_hinh_thuc_id", pagingRequest.hoa_don_hinh_thuc_id.ConvertToInt());
            param.Add("@hoa_don_dang_ky_phat_hanh_mau_so", pagingRequest.hoa_don_dang_ky_phat_hanh_mau_so.ConvertToString());
            param.Add("@hoa_don_dang_ky_phat_hanh_ky_hieu", pagingRequest.hoa_don_dang_ky_phat_hanh_ky_hieu.ConvertToString());
            param.Add("@hoa_don_hinh_thuc_code", pagingRequest.hoa_don_hinh_thuc_code.ConvertToString());
            param.Add("@tu_ngay", pagingRequest.tu_ngay);
            param.Add("@den_ngay", pagingRequest.den_ngay);
            param.Add("@page_index", pagingRequest.page_index);
            param.Add("@page_size", pagingRequest.page_size);
            param.Add("@sort_by", pagingRequest.sort_by.ConvertToString());
            param.Add("@sort_mode", pagingRequest.sort_mode.ConvertToString());
            param.Add("@search_key", pagingRequest.search_key.ConvertToString());

            param.Add("@ma_dai_ly", pagingRequest.ma_dai_ly.ConvertToString());
            param.Add("@nguoi_mua_mst", pagingRequest.nguoi_mua_mst.ConvertToString());

            param.Add("@total_count", dbType: System.Data.DbType.Int64, direction: System.Data.ParameterDirection.Output);
            var list = await _dbConnection.SelectAsync<hoa_don_hang_hoa_vm>("hoa_don_hang_hoa_select_bydonvi_paging_thongke_page", param);
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
            return new PagingResult<IEnumerable<hoa_don_hang_hoa_vm>>(pagingResultSummaries, list);
        }

        public Task<IEnumerable<hoa_don_hang_hoa>> SelectByHoaDonIdAsync(int hoa_don_id)
        {
            var param = new DynamicParameters();
            param.Add("@hoa_don_id", hoa_don_id);
            return _dbConnection.SelectAsync<hoa_don_hang_hoa>("hoa_don_hang_hoa_select_by_hoadon", param);
        }
    }
}