using Common;
using Contracts.Repository.Base;
using Contracts.Repository.HoaDon;
using Dapper;
using Model.FuncResult;
using Model.Request.HoaDon;
using Model.Request.ThongKe;
using Model.Respone.HoaDon;
using Model.Respone.ThongKe;
using Model.Table;
using Repository.Base;

namespace Repository.HoaDon
{
    public class HoaDonRepository : CRUDRepository<hoa_don>, IHoaDonRepository
    {
        public HoaDonRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {

        }

        public Task<string> GetMaxMaSoHoaDon(string donvi_ma_dv, string mau_so, string ky_hieu)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", donvi_ma_dv.ConvertToString());
            param.Add("@mau_so", mau_so);
            param.Add("@ky_hieu", ky_hieu);
            return _dbConnection.SelectFirstOrDefaultAsync<string>("hoa_don_get_max_ma_so_hoa_don", param);
        }
        public Task<string> GetMaxMaSoHoaDonMTT(string donvi_ma_dv, string mau_so, int year)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", donvi_ma_dv.ConvertToString());
            param.Add("@mau_so", mau_so);
            param.Add("@year", year);
            return _dbConnection.SelectFirstOrDefaultAsync<string>("hoa_don_get_max_ma_so_hoa_don_mtt", param);
        }

        public Task<DateTime?> GetMaxNgayHoaDon(string donvi_ma_dv, string mau_so, string ky_hieu)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", donvi_ma_dv.ConvertToString());
            param.Add("@mau_so", mau_so);
            param.Add("@ky_hieu", ky_hieu);
            return _dbConnection.SelectFirstOrDefaultAsync<DateTime?>("hoa_don_get_max_ngay_hoa_don", param);
        }

        public Task<DateTime?> GetNgayHoaDonPhatHanhMaxAsynsc(string donvi_ma_dv, string hoa_don_dang_ky_phat_hanh_mau_so, string hoa_don_dang_ky_phat_hanh_ky_hieu)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", donvi_ma_dv.ConvertToString());
            param.Add("@mau_so", hoa_don_dang_ky_phat_hanh_mau_so);
            param.Add("@ky_hieu", hoa_don_dang_ky_phat_hanh_ky_hieu);
            return _dbConnection.SelectFirstOrDefaultAsync<DateTime?>("hoa_don_get_max_ngay_hoa_don_da_phat_hanh", param);
        }

        public Task<IEnumerable<ThongKeTopKhachHangTheoSoLuongHoaDonRespone>> GetTopKhachHangBySoGiaTriHDAsync(ThongKeTopKhachHangTheoHoaDonRequest request)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", request.donvi_ma_dv.ConvertToString());
            param.Add("@from_date", request.from_date);
            param.Add("@to_date", request.to_date);
            param.Add("@top", request.top);
            return _dbConnection.SelectAsync<ThongKeTopKhachHangTheoSoLuongHoaDonRespone>("hoa_don_select_report_top_gia_tri", param);
        }

        public Task<IEnumerable<ThongKeTopKhachHangTheoSoLuongHoaDonRespone>> GetTopKhachHangBySoLuongHDAsync(ThongKeTopKhachHangTheoHoaDonRequest request)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", request.donvi_ma_dv.ConvertToString());
            param.Add("@from_date", request.from_date);
            param.Add("@to_date", request.to_date);
            param.Add("@top", request.top);
            return _dbConnection.SelectAsync<ThongKeTopKhachHangTheoSoLuongHoaDonRespone>("hoa_don_select_report_top_so_luong", param);
        }



        public Task<hoa_don> SelectAnyHoaDonAsync(string donvi_ma_dv, string mau_so, string ky_hieu)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", donvi_ma_dv.ConvertToString());
            param.Add("@mau_so", mau_so.ConvertToString());
            param.Add("@ky_hieu", ky_hieu.ConvertToString());
            return _dbConnection.SelectFirstOrDefaultAsync<hoa_don>("hoa_don_select_any_by_kyhieu", param);
        }

        // public async Task<PagingResult<IEnumerable<hoa_don_vm>>> SelectByDonViAsync(string donvi_ma_dv, HoaDonSelectPagingRequest pagingRequest)
        // {
        //     var param = new DynamicParameters();
        //     param.Add("@donvi_ma_dv", donvi_ma_dv.ConvertToString());
        //     param.Add("@hoa_don_trang_thai_ids", pagingRequest.hoa_don_trang_thai_ids.ConvertToTableValuedParameter());
        //     param.Add("@loai_hoa_don_ct_id", pagingRequest.loai_hoa_don_ct_id.ConvertToInt());
        //     param.Add("@hoa_don_hinh_thuc_id", pagingRequest.hoa_don_hinh_thuc_id.ConvertToInt());
        //     param.Add("@hoa_don_dang_ky_phat_hanh_mau_so", pagingRequest.hoa_don_dang_ky_phat_hanh_mau_so.ConvertToString());
        //     param.Add("@hoa_don_dang_ky_phat_hanh_ky_hieu", pagingRequest.hoa_don_dang_ky_phat_hanh_ky_hieu.ConvertToString());
        //     param.Add("@hoa_don_hinh_thuc_code", pagingRequest.hoa_don_hinh_thuc_code.ConvertToString());
        //     param.Add("@tu_ngay", pagingRequest.tu_ngay);
        //     param.Add("@den_ngay", pagingRequest.den_ngay);
        //     param.Add("@page_index", pagingRequest.page_index);
        //     param.Add("@page_size", pagingRequest.page_size);
        //     param.Add("@sort_by", pagingRequest.sort_by.ConvertToString());
        //     param.Add("@sort_mode", pagingRequest.sort_mode.ConvertToString());
        //     param.Add("@search_key", pagingRequest.search_key.ConvertToString());

        //     // param.Add("@khachhang_id", pagingRequest.khachhang_id.ConvertToInt());
        //     // param.Add("@dai_ly_id", pagingRequest.dai_ly_id.ConvertToInt());

        //     param.Add("@total_count", dbType: System.Data.DbType.Int64, direction: System.Data.ParameterDirection.Output);
        //     var list = await _dbConnection.SelectAsync<hoa_don_vm>("hoa_don_select_bydonvi_paging", param);
        //     var total_count = param.Get<long>("@total_count");
        //     var page_size = pagingRequest?.page_size ?? 1;
        //     if (page_size == 0) page_size = 1;
        //     var page_count = (int)total_count / page_size;
        //     var pagingResultSummaries = new PagingResultSummary()
        //     {
        //         page_count = page_count * page_size < total_count ? (page_count + 1) : page_count,
        //         page_number = pagingRequest?.page_index ?? 0,
        //         page_size = pagingRequest?.page_size ?? 0,
        //         total_count = total_count
        //     };
        //     return new PagingResult<IEnumerable<hoa_don_vm>>(pagingResultSummaries, list);
        // }

        public async Task<PagingResult<IEnumerable<hoa_don_vm>>> SelectByDonViAsync(string donvi_ma_dv, HoaDonSelectPagingRequest pagingRequest)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", donvi_ma_dv.ConvertToString());
            param.Add("@hoa_don_trang_thai_ids", pagingRequest.hoa_don_trang_thai_ids.ConvertToTableValuedParameter());
            param.Add("@loai_hoa_don_ct_id", pagingRequest.loai_hoa_don_ct_id.ConvertToInt());
            // Chỉ add tham số này khi hoa_don_hinh_thuc_id = 1
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

            // param.Add("@khachhang_id", pagingRequest.khachhang_id.ConvertToInt());
            // param.Add("@dai_ly_id", pagingRequest.dai_ly_id.ConvertToInt());

            param.Add("@total_count", dbType: System.Data.DbType.Int64, direction: System.Data.ParameterDirection.Output);
            string procName = "hoa_don_select_bydonvi_paging";

            if (pagingRequest.hoa_don_trang_thai_ids != null
                && pagingRequest.hoa_don_trang_thai_ids.Contains(2))
            {
                if (pagingRequest.hoa_don_hinh_thuc_id.ConvertToInt() == 1)
                {
                    param.Add("@hoa_don_hinh_thuc_id", pagingRequest.hoa_don_hinh_thuc_id.ConvertToInt());
                }

                procName = pagingRequest.hoa_don_hinh_thuc_id switch
                {
                    2 => "hoa_don_select_bydonvi_paging_thaythe",
                    3 => "hoa_don_select_bydonvi_paging_dieuchinh",
                    _ => "hoa_don_select_bydonvi_paging"
                };
            }
            else
            {
                param.Add("@hoa_don_hinh_thuc_id", pagingRequest.hoa_don_hinh_thuc_id.ConvertToInt());
            }

            var list = await _dbConnection.SelectAsync<hoa_don_vm>(procName, param);


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
            return new PagingResult<IEnumerable<hoa_don_vm>>(pagingResultSummaries, list);
        }

        public async Task<PagingResult<IEnumerable<hoa_don_vm>>> SelectByDonViThongKePageAsync(string donvi_ma_dv, HoaDonSelectPagingRequest pagingRequest)
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
            var list = await _dbConnection.SelectAsync<hoa_don_vm>("hoa_don_select_bydonvi_paging_thongke_page", param);
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
            return new PagingResult<IEnumerable<hoa_don_vm>>(pagingResultSummaries, list);
        }

        public Task<IEnumerable<hoa_don>> SelectByIdsAsync(List<int> ids)
        {
            var param = new DynamicParameters();
            param.Add("@ids", ids.ConvertToTableValuedParameter());
            return _dbConnection.SelectAsync<hoa_don>("hoa_don_select_by_ids", param);
        }

        public Task<hoa_don> SelectByPhatHanhUuidAsync(string phat_hanh_uuid)
        {
            var param = new DynamicParameters();
            param.Add("@phat_hanh_uuid", phat_hanh_uuid);
            return _dbConnection.SelectFirstOrDefaultAsync<hoa_don>("hoa_don_select_by_phathanh_uuid", param);
        }

        public Task<hoa_don> SelectHoaDonDieuChinhThayTheChoHoaDonAsync(string donvi_ma_dv, string mau_so, string ky_hieu, int so_hoa_don)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", donvi_ma_dv);
            param.Add("@mau_so", mau_so);
            param.Add("@ky_hieu", ky_hieu);
            param.Add("@so_hoa_don", so_hoa_don);
            return _dbConnection.SelectFirstOrDefaultAsync<hoa_don>("hoa_don_select_by_hoa_don_goc", param);
        }

        public Task<hoa_don> SelectHoaDonGocAsync(string donvi_ma_dv, string mau_so, string ky_hieu, int so_hoa_don)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", donvi_ma_dv);
            param.Add("@mau_so", mau_so);
            param.Add("@ky_hieu", ky_hieu);
            param.Add("@so_hoa_don", so_hoa_don);
            return _dbConnection.SelectFirstOrDefaultAsync<hoa_don>("hoa_don_select_hoa_don_goc", param);
        }

        public Task<int> SelectHoaDonIdByInvoiceIdAsync(string invoice_id)
        {
            var param = new DynamicParameters();
            param.Add("@invoice_id", invoice_id);
            return _dbConnection.SelectFirstOrDefaultAsync<int>("hoa_don_select_by_invoice_id", param);
        }

        public Task<int> SelectHoaDonIdByMaTraCuuAsync(string maTraCuu)
        {
            var param = new DynamicParameters();
            param.Add("@ma_tra_cuu", maTraCuu);
            return _dbConnection.SelectFirstOrDefaultAsync<int>("hoa_don_select_id_by_ma_tra_cuu", param);
        }

        public Task<IEnumerable<hoa_don>> SelectHoaDonLoiChaPhathanhAsync()
        {
            return _dbConnection.SelectAsync<hoa_don>("hoa_don_select_loi_chua_phat_hanh");
        }

        public Task<IEnumerable<hoa_don>> SelectHoaDonLoiPhatHanhNhieuLanAsync()
        {
            return _dbConnection.SelectAsync<hoa_don>("hoa_don_select_loi_phat_hanh_nhieu_lan");
        }

        public async Task<List<hoa_don>> SelectListHoaDonByPhatHanhUuidAsync(string phat_hanh_uuid)
        {
            var param = new DynamicParameters();
            param.Add("@phat_hanh_uuid", phat_hanh_uuid);
            var list = await _dbConnection.SelectAsync<hoa_don>("hoa_don_select_by_phathanh_uuid", param);
            return list.ToList();
        }

        public Task<bool> UpdateMaSoHoaDonAsync(int id, int ma_so_hoa_don)
        {
            var param = new DynamicParameters();
            param.Add("@ma_so_hoa_don", ma_so_hoa_don);
            param.Add("@id", id);
            return _dbConnection.ExecuteAsync("hoa_don_update_ma_so_hoa_don", param);
        }

        public Task<bool> UpdateMaTraCuuAsync(int id, string ma_tra_cuu)
        {
            var param = new DynamicParameters();
            param.Add("@ma_tra_cuu", ma_tra_cuu);
            param.Add("@id", id);
            return _dbConnection.ExecuteAsync("hoa_don_update_ma_tra_cuu", param);
        }

        public Task<bool> UpdatePhatHanhBangKeAsync(List<int> ids, string phat_hanh_uuid, int user_id_phathanh)
        {
            var param = new DynamicParameters();
            param.Add("@ids", ids.ConvertToTableValuedParameter());
            param.Add("@phat_hanh_uuid", phat_hanh_uuid);
            param.Add("@user_id_phathanh", user_id_phathanh);
            return _dbConnection.ExecuteAsync("hoa_don_update_phathanh_bangkes", param);
        }

        public Task<bool> UpdateTrangThaiAsync(int id, int hoa_don_trang_thai_id)
        {
            var param = new DynamicParameters();
            param.Add("@id", id);
            param.Add("@hoa_don_trang_thai_id", hoa_don_trang_thai_id);
            return _dbConnection.ExecuteAsync("hoa_don_update_trang_thai", param);
        }

        public async Task<IEnumerable<HoaDonPdfInforResponse>> SelectByMaSoHoaDonRangeAsync(string donvi_ma_dv, string ky_hieu, int fromMaSo, int toMaSo)
        {
            var sql = @"SELECT id, ma_so_hoa_don, hoa_don_dang_ky_phat_hanh_ky_hieu, hoa_don_dang_ky_phat_hanh_mau_so, nguoi_mua_mst
                FROM hoa_don 
                WHERE donvi_ma_dv = @donvi_ma_dv 
                AND hoa_don_dang_ky_phat_hanh_ky_hieu = @ky_hieu
                AND ma_so_hoa_don BETWEEN @fromMaSo AND @toMaSo
                AND is_deleted = 0 
                ";

            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", donvi_ma_dv.ConvertToString());
            param.Add("@ky_hieu", ky_hieu.ConvertToString());
            param.Add("@fromMaSo", fromMaSo);
            param.Add("@toMaSo", toMaSo);

            return await _dbConnection.QueryAsync<HoaDonPdfInforResponse>(sql, param);

        }



    }
}