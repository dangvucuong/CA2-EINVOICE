using Common;
using Contracts.Repository.Base;
using Contracts.Repository.HoaDon;
using Dapper;
using Model.FuncResult;
using Model.Request.HoaDon;
using Model.Request.ThongKe;
using Model.Request.Xml;
using Model.Respone.HoaDon;
using Model.Respone.ThongKe;
using Model.Table;
using Repository.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WebApp;

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

        public Task<HoaDonNgayLienKeRespone> SelectNgayHoaDonLienKeAsync(string donvi_ma_dv, string mau_so, string ky_hieu, int hoa_don_id, DateTime ngay_hoa_don)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", donvi_ma_dv.ConvertToString());
            param.Add("@mau_so", mau_so.ConvertToString());
            param.Add("@ky_hieu", ky_hieu.ConvertToString());
            param.Add("@hoa_don_id", hoa_don_id);
            param.Add("@ngay_hoa_don", ngay_hoa_don.Date);
            return _dbConnection.SelectFirstOrDefaultAsync<HoaDonNgayLienKeRespone>("hoa_don_select_ngay_lien_ke", param);
        }

        public Task<HoaDonSoNhoHonChuaKySoRespone> SelectSoHoaDonNhoHonChuaKySoAsync(string donvi_ma_dv, string mau_so, string ky_hieu, int hoa_don_id, int ma_so_hoa_don_hien_tai, DateTime ngay_hoa_don_hien_tai, IEnumerable<int> excludeHoaDonIds = null)
        {
            var excludeIds = (excludeHoaDonIds ?? Enumerable.Empty<int>())
                .Where(x => x > 0)
                .Distinct()
                .ToList();
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", donvi_ma_dv.ConvertToString());
            param.Add("@mau_so", mau_so.ConvertToString());
            param.Add("@ky_hieu", ky_hieu.ConvertToString());
            param.Add("@hoa_don_id", hoa_don_id);
            param.Add("@ma_so_hoa_don_hien_tai", ma_so_hoa_don_hien_tai);
            param.Add("@ngay_hoa_don_hien_tai", ngay_hoa_don_hien_tai.Date);

            var excludeSql = "";
            if (excludeIds.Count > 0)
            {
                param.Add("@exclude_ids_wrapped", "," + string.Join(",", excludeIds) + ",");
                excludeSql = " AND CHARINDEX(',' + CAST(hd.id AS VARCHAR(20)) + ',', @exclude_ids_wrapped) = 0 ";
            }

            var sql = $@"
SELECT TOP 1
    hd.id,
    hd.ma_so_hoa_don,
    hd.ngay_hoa_don
FROM hoa_don hd
WHERE hd.donvi_ma_dv = @donvi_ma_dv
  AND hd.hoa_don_dang_ky_phat_hanh_mau_so = @mau_so
  AND hd.hoa_don_dang_ky_phat_hanh_ky_hieu = @ky_hieu
  AND hd.is_deleted = 0
  AND hd.hoa_don_trang_thai_id <> 3
  AND hd.hoa_don_hinh_thuc_id <> 5
  AND hd.ma_so_hoa_don > 0
  AND hd.hoa_don_trang_thai_id = 1
  AND ISNULL(hd.is_ky_so_succes, 0) = 0
  AND hd.ngay_hoa_don >= '2026-08-01'
  AND hd.id <> @hoa_don_id
  {excludeSql}
  AND (
      hd.ngay_hoa_don < @ngay_hoa_don_hien_tai
      OR (hd.ngay_hoa_don = @ngay_hoa_don_hien_tai AND hd.ma_so_hoa_don < @ma_so_hoa_don_hien_tai)
  )
ORDER BY hd.ngay_hoa_don ASC, hd.ma_so_hoa_don ASC";

            return _dbConnection.QueryFirstOrDefaultAsync<HoaDonSoNhoHonChuaKySoRespone>(sql, param);
        }

        public Task<HoaDonNgayChoPhepTheoSoRespone> SelectNgayHoaDonChoPhepTheoSoAsync(string donvi_ma_dv, string mau_so, string ky_hieu, int hoa_don_id, int ma_so_hoa_don)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", donvi_ma_dv.ConvertToString());
            param.Add("@mau_so", mau_so.ConvertToString());
            param.Add("@ky_hieu", ky_hieu.ConvertToString());
            param.Add("@hoa_don_id", hoa_don_id);
            param.Add("@ma_so_hoa_don", ma_so_hoa_don);
            return _dbConnection.SelectFirstOrDefaultAsync<HoaDonNgayChoPhepTheoSoRespone>("hoa_don_select_ngay_cho_phep_theo_so", param);
        }

        public Task<DateTime?> SelectNgayToiThieuChuaCoSoAsync(string donvi_ma_dv, string mau_so, string ky_hieu, int hoa_don_id, DateTime ngay_hoa_don)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", donvi_ma_dv.ConvertToString());
            param.Add("@mau_so", mau_so.ConvertToString());
            param.Add("@ky_hieu", ky_hieu.ConvertToString());
            param.Add("@hoa_don_id", hoa_don_id);
            param.Add("@ngay_hoa_don", ngay_hoa_don.Date);
            return _dbConnection.SelectFirstOrDefaultAsync<DateTime?>("hoa_don_select_ngay_toi_thieu_chua_co_so", param);
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
            param.Add("@sort_by", string.IsNullOrWhiteSpace(pagingRequest.sort_by)
                ? "ma_so_hoa_don"
                : pagingRequest.sort_by.ConvertToString());
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
            param.Add("@sort_by", string.IsNullOrWhiteSpace(pagingRequest.sort_by)
                ? "ma_so_hoa_don"
                : pagingRequest.sort_by.ConvertToString());
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

        public Task<bool> InsertThueSuatHoaDonAsync(int id, IEnumerable<ThueSuatModel> dsThue)
        {
            // Tên trong ngoặc DataTable này thường để cho rõ nghĩa, quan trọng là cấu trúc cột
            var table = new DataTable("dbo.thue_suat_hd_type");

            // Thêm cột PHẢI ĐÚNG THỨ TỰ như trong SQL Type (TSuat -> ThTien -> TThue)
            table.Columns.Add("TSuat", typeof(string));
            table.Columns.Add("ThTien", typeof(decimal));
            table.Columns.Add("TThue", typeof(decimal));

            foreach (var item in dsThue)
            {
                table.Rows.Add(
                    item.TSuat,
                    item.ThTien,
                    item.TThue
                );
            }

            var param = new DynamicParameters();
            param.Add("@HoaDonId", id);
            // Truyền đúng tên Type đã định nghĩa trong SQL
            param.Add("@ThueList", table.AsTableValuedParameter("dbo.thue_suat_hd_type"));

            // Gọi theo kiểu 2 tham số như các hàm InsertsAsync khác của bạn
            return _dbConnection.ExecuteAsync("InsertThueSuatHoaDon", param);
        }


        public Task<IEnumerable<ThueSuatModel>> SelectThueSuatHoaDonByHoaDonIdAsync(int hoaDonId)
        {
            var param = new DynamicParameters();
            param.Add("@HoaDonId", hoaDonId);

            // Sử dụng SelectAsync (hoặc QueryAsync tùy vào wrapper của bạn) 
            // để lấy toàn bộ danh sách các cột TSuat, ThTien, TThue
            return _dbConnection.SelectAsync<ThueSuatModel>("GetThueSuatHoaDonByHoaDonId", param);
        }


        public Task<bool> InsertHoaDonThongTinBoSungAsync(int id, HoaDonThongTinBoSung info)
        {
            var param = new DynamicParameters();

            // Ánh xạ chính xác tên biến với các tham số trong Stored Procedure
            param.Add("@HoaDonId", id);
            param.Add("@IsHdBanTaiSanCong", info.IsHdBanTaiSanCong);
            param.Add("@SoQuyetDinh", info.SoQuyetDinh);
            param.Add("@NgayQuyetDinh", info.NgayQuyetDinh);
            param.Add("@CoQuanBanHanhQD", info.CoQuanBanHanhQD);
            param.Add("@HinhThucBan", info.HinhThucBan);
            param.Add("@DiaDiemVCHangDen", info.DiaDiemVCHangDen);
            param.Add("@TgianVCHangDenTu", info.TgianVCHangDenTu);
            param.Add("@TgianVCHangDenDen", info.TgianVCHangDenDen);
            param.Add("@IsHdPhiThueQuan", info.IsHdPhiThueQuan);

            // Sử dụng ExecuteAsync để chạy Procedure
            return _dbConnection.ExecuteAsync("HoaDonThongTinBoSung", param);

        }

        public Task<hd_thong_tin_bo_sung> SelectHoaDonThongTinBoSungByHoaDonIdAsync(int hoaDonId)
        {
            var param = new DynamicParameters();
            param.Add("@HoaDonId", hoaDonId);
            return _dbConnection.SelectFirstOrDefaultAsync<hd_thong_tin_bo_sung>("GetHoaDonThongTinBoSungByHoaDonId", param);
        }

        //tach ds

        public async Task<PagingResult<IEnumerable<hoa_don_vm>>> SelectChoPhanHoiCQTAsync(string donvi_ma_dv, HoaDonSelectPagingRequest pagingRequest)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", donvi_ma_dv.ConvertToString());
            //param.Add("@hoa_don_trang_thai_ids", pagingRequest.hoa_don_trang_thai_ids.ConvertToTableValuedParameter());
            param.Add("@loai_hoa_don_ct_id", pagingRequest.loai_hoa_don_ct_id.ConvertToInt());
            // Chỉ add tham số này khi hoa_don_hinh_thuc_id = 1
            param.Add("@hoa_don_dang_ky_phat_hanh_mau_so", pagingRequest.hoa_don_dang_ky_phat_hanh_mau_so.ConvertToString());
            param.Add("@hoa_don_dang_ky_phat_hanh_ky_hieu", pagingRequest.hoa_don_dang_ky_phat_hanh_ky_hieu.ConvertToString());
            param.Add("@hoa_don_hinh_thuc_code", pagingRequest.hoa_don_hinh_thuc_code.ConvertToString());
            param.Add("@tu_ngay", pagingRequest.tu_ngay);
            param.Add("@den_ngay", pagingRequest.den_ngay);
            param.Add("@page_index", pagingRequest.page_index);
            param.Add("@page_size", pagingRequest.page_size);
            param.Add("@sort_by", string.IsNullOrWhiteSpace(pagingRequest.sort_by)
                ? "ma_so_hoa_don"
                : pagingRequest.sort_by.ConvertToString());
            param.Add("@sort_mode", pagingRequest.sort_mode.ConvertToString());
            param.Add("@search_key", pagingRequest.search_key.ConvertToString());

            // param.Add("@khachhang_id", pagingRequest.khachhang_id.ConvertToInt());
            // param.Add("@dai_ly_id", pagingRequest.dai_ly_id.ConvertToInt());

            param.Add("@total_count", dbType: System.Data.DbType.Int64, direction: System.Data.ParameterDirection.Output);
            string procName = "hoa_don_select_cho_phan_hoi_cqt";

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

            try
            {
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
            catch (Exception ex)
            {
                var a = ex.Message;                
                return new PagingResult<IEnumerable<hoa_don_vm>>(null, null);
            }
        }


        public async Task<PagingResult<IEnumerable<hoa_don_vm>>> SelectChuaGuiCQTAsync(string donvi_ma_dv, HoaDonSelectPagingRequest pagingRequest)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", donvi_ma_dv.ConvertToString());
            //param.Add("@hoa_don_trang_thai_ids", pagingRequest.hoa_don_trang_thai_ids.ConvertToTableValuedParameter());
            param.Add("@loai_hoa_don_ct_id", pagingRequest.loai_hoa_don_ct_id.ConvertToInt());
            // Chỉ add tham số này khi hoa_don_hinh_thuc_id = 1
            param.Add("@hoa_don_dang_ky_phat_hanh_mau_so", pagingRequest.hoa_don_dang_ky_phat_hanh_mau_so.ConvertToString());
            param.Add("@hoa_don_dang_ky_phat_hanh_ky_hieu", pagingRequest.hoa_don_dang_ky_phat_hanh_ky_hieu.ConvertToString());
            param.Add("@hoa_don_hinh_thuc_code", pagingRequest.hoa_don_hinh_thuc_code.ConvertToString());
            param.Add("@tu_ngay", pagingRequest.tu_ngay);
            param.Add("@den_ngay", pagingRequest.den_ngay);
            param.Add("@page_index", pagingRequest.page_index);
            param.Add("@page_size", pagingRequest.page_size);
            param.Add("@sort_by", string.IsNullOrWhiteSpace(pagingRequest.sort_by)
                ? "ma_so_hoa_don"
                : pagingRequest.sort_by.ConvertToString());
            param.Add("@sort_mode", pagingRequest.sort_mode.ConvertToString());
            param.Add("@search_key", pagingRequest.search_key.ConvertToString());

            // param.Add("@khachhang_id", pagingRequest.khachhang_id.ConvertToInt());
            // param.Add("@dai_ly_id", pagingRequest.dai_ly_id.ConvertToInt());

            param.Add("@total_count", dbType: System.Data.DbType.Int64, direction: System.Data.ParameterDirection.Output);
            string procName = "[hoa_don_select_chua_gui_cqt]";

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

            try
            {
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
            catch (Exception ex)
            {
                var a = ex.Message;
                return new PagingResult<IEnumerable<hoa_don_vm>>(null, null);
            }
        }

    }
}