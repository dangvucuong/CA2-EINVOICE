using Contracts.Repository.BangTongHop;
using Contracts.Repository.Base;
using Dapper;
using Model.Table;
using Repository.Base;

namespace Repository.BangTongHop
{
    public class BangTongHopRepository : CRUDRepository<bang_tong_hop_du_lieu>, IBangTongHopRepository
    {
        public BangTongHopRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public Task<IEnumerable<bang_tong_hop_du_lieu>> SelectByDonViAsync(string donvi_ma_dv)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", donvi_ma_dv);
            return _dbConnection.SelectAsync<bang_tong_hop_du_lieu>("bang_tong_hop_du_lieu_select_by_donvi", param);
        }
    }
}