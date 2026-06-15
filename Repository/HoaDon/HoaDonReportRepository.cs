using Contracts.Repository.Base;
using Contracts.Repository.HoaDon;
using Dapper;
using Model.Request.Dashboard;
using Model.Respone.Dashboard;
using Repository.Base;

namespace Repository.HoaDon
{
    public class HoaDonReportRepository : BaseRepository, IHoaDonReportRepository
    {
        public HoaDonReportRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public  Task<IEnumerable<HoaDonTrangThaiSummary>> SelectHoaDonTrangThaiAsync(HoaDonTrangThaiSummaryRequest request)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", request.donvi_ma_dv);
            param.Add("@from_date", request.from_date);
            param.Add("@to_date", request.to_date);
            return  _dbConnection.SelectAsync<HoaDonTrangThaiSummary>("hoa_don_select_report_trangthai", param);

        }

        public  Task<IEnumerable<HoaDonLichSuPhatHanhItem>> SelectLichSuPhatHanh(HoaDonTrangThaiSummaryRequest request)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", request.donvi_ma_dv);
            param.Add("@from_date", request.from_date);
            param.Add("@to_date", request.to_date);
            return  _dbConnection.SelectAsync<HoaDonLichSuPhatHanhItem>("hoa_don_select_lich_su_phat_hanh_by_date", param);
        }

        public  Task<int> SelectTongSoLuongHoaDonDaMuaAsync(string donvi_mst)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_mst", donvi_mst);

            return  _dbConnection.SelectFirstOrDefaultAsync<int>("don_vi_select_tong_sl_chu_ky_so_da_mua", param);
        }

        public  Task<int> SelectTongSoLuongHoaDonDaSuDungAsync(string donvi_mst)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_mst", donvi_mst);

            return _dbConnection.SelectFirstOrDefaultAsync<int>("sp2026_don_vi_select_tong_sl_hoa_don_da_dung", param);

          //  return  _dbConnection.SelectFirstOrDefaultAsync<int>("don_vi_select_tong_sl_hoa_don_da_dung", param);

            
        }
    }
}