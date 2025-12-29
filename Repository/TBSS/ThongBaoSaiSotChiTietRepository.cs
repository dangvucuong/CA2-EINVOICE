using Common;
using Contracts.Repository.Base;
using Contracts.Repository.TBSS;
using Dapper;
using Model.Table;
using Repository.Base;

namespace Repository.TBSS
{
    public class ThongBaoSaiSotChiTietRepository : CRUDRepository<thong_bao_sai_sot_chi_tiet>, IThongBaoSaiSotChiTietRepository
    {
        public ThongBaoSaiSotChiTietRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public Task<IEnumerable<thong_bao_sai_sot_chi_tiet>> SelectByThongBaoAsync(int thong_bao_sai_sot_id)
        {
            var param = new DynamicParameters();
            param.Add("@thong_bao_sai_sot_id", thong_bao_sai_sot_id);
            return _dbConnection.SelectAsync<thong_bao_sai_sot_chi_tiet>("thong_bao_sai_sot_chi_tiet_select_by_thongbao_id", param);
        }

        public Task<IEnumerable<thong_bao_sai_sot_chi_tiet>> SelectByThongBaoIdsAsync(List<int> thong_bao_sai_sot_ids)
        {
            var param = new DynamicParameters();
            param.Add("@thong_bao_sai_sot_ids", thong_bao_sai_sot_ids.ConvertToTableValuedParameter());
            return _dbConnection.SelectAsync<thong_bao_sai_sot_chi_tiet>("thong_bao_sai_sot_chi_tiet_select_by_thongbao_ids", param);
        }
    }
}