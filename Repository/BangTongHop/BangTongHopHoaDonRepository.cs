using Common;
using Contracts.Repository.BangTongHop;
using Contracts.Repository.Base;
using Dapper;
using Model.Table;
using Repository.Base;

namespace Repository.BangTongHop
{
    public class BangTongHopHoaDonRepository : CRUDRepository<bang_tong_hop_du_lieu_hoa_don>, IBangTongHopHoaDonRepository
    {
        public BangTongHopHoaDonRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public Task<bool> DeletesAsync(IEnumerable<int> ids, int user_id)
        {
            var param = new DynamicParameters();
            param.Add("@ids", ids.ConvertToTableValuedParameter());
            param.Add("@user_id", user_id);
            return _dbConnection.ExecuteAsync("bang_tong_hop_du_lieu_hoa_don_deletes", param);
        }

        public Task<bool> InsertsAsync(IEnumerable<bang_tong_hop_du_lieu_hoa_don> duLieuHoaDons)
        {
            var param = new DynamicParameters();
            param.Add("@data", duLieuHoaDons.ConvertToTableValuedParameter("utp_bang_tong_hop_du_lieu_hoa_don"));
            return _dbConnection.ExecuteAsync("bang_tong_hop_du_lieu_hoa_don_inserts", param);
        }

        public Task<IEnumerable<bang_tong_hop_du_lieu_hoa_don>> SelectByBangTongHopAsync(int bangTongHopId)
        {
            var param = new DynamicParameters();
            param.Add("@bang_tong_hop_du_lieu_id", bangTongHopId);
            return _dbConnection.SelectAsync<bang_tong_hop_du_lieu_hoa_don>("bang_tong_hop_du_lieu_hoa_don_select", param);
        }

        public async Task<IEnumerable<int>> SelectUsedHoaDonIdsByDonViAsync(string donvi_ma_dv)
        {
            var safeDonVi = donvi_ma_dv.ConvertToString().Replace("'", "''");
            var query =
                "SELECT DISTINCT hd.hoa_don_id " +
                "FROM bang_tong_hop_du_lieu_hoa_don hd WITH(NOLOCK) " +
                "INNER JOIN bang_tong_hop_du_lieu bth WITH(NOLOCK) ON hd.bang_tong_hop_du_lieu_id = bth.id " +
                $"WHERE bth.donvi_ma_dv = '{safeDonVi}'";

            var items = await Connection.SelectByQueryAsync<HoaDonIdItem>(query);
            return items.Select(x => x.hoa_don_id);
        }

        public async Task<IEnumerable<hoa_don>> SelectHoaDonForTongHopAsync(string donvi_ma_dv, DateTime? tu_ngay, DateTime? den_ngay)
        {
            var safeDonVi = donvi_ma_dv.ConvertToString().Replace("'", "''");
            var tuNgayFilter = tu_ngay.HasValue
                ? $" AND h.ngay_hoa_don >= '{tu_ngay.Value:yyyy-MM-dd}'"
                : "";
            var denNgayFilter = den_ngay.HasValue
                ? $" AND h.ngay_hoa_don <= '{den_ngay.Value:yyyy-MM-dd}'"
                : "";
            var query =
                "SELECT TOP 2000 h.* " +
                "FROM hoa_don h WITH(NOLOCK) " +
                $"WHERE h.donvi_ma_dv = '{safeDonVi}' " +
                "AND h.hoa_don_hinh_thuc_code = 'K' " +
                "AND h.hoa_don_trang_thai_id = 1 " +
                "AND ISNULL(h.is_deleted, 0) = 0 " +
                tuNgayFilter +
                denNgayFilter +
                " AND NOT EXISTS (" +
                "   SELECT 1 FROM bang_tong_hop_du_lieu_hoa_don bthd WITH(NOLOCK) " +
                "   INNER JOIN bang_tong_hop_du_lieu bth WITH(NOLOCK) ON bthd.bang_tong_hop_du_lieu_id = bth.id " +
                $"   WHERE bthd.hoa_don_id = h.id AND bth.donvi_ma_dv = '{safeDonVi}'" +
                " ) " +
                "ORDER BY h.id DESC";

            return await Connection.SelectByQueryAsync<hoa_don>(query);
        }

        private class HoaDonIdItem
        {
            public int hoa_don_id { get; set; }
        }
    }
}