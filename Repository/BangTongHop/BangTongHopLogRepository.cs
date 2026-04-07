using Contracts.Repository.BangTongHop;
using Contracts.Repository.Base;
using Dapper;
using Model.Table;
using Repository.Base;

namespace Repository.BangTongHop
{
    public class BangTongHopLogRepository : CRUDRepository<bang_tong_hop_du_lieu_log>, IBangTongHopLogRepository
    {
        public BangTongHopLogRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public Task<IEnumerable<bang_tong_hop_du_lieu_log>> SelectByBangTongHopIdAsync(int bang_tong_hop_du_lieu_id)
        {
            var param = new DynamicParameters();
            param.Add("@bang_tong_hop_du_lieu_id", bang_tong_hop_du_lieu_id);
            return _dbConnection.SelectAsync<bang_tong_hop_du_lieu_log>("bang_tong_hop_du_lieu_log_select", param);
        }
    }
}