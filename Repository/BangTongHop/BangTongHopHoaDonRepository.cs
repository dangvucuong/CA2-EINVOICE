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
    }
}