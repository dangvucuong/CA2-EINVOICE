using Contracts.Repository.Base;
using Contracts.Repository.HoaDon;
using Dapper;
using Model.Table;
using Repository.Base;

namespace Repository.HoaDon
{
    public class HoaDonLogRepository : CRUDRepository<hoa_don_log>, IHoaDonLogRepository
    {
        public HoaDonLogRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public  Task<IEnumerable<hoa_don_log>> SelectByHoaDonAsync(int hoa_don_id)
        {
            var param = new DynamicParameters();
            param.Add("@hoa_don_id", hoa_don_id);
            return  _dbConnection.SelectAsync<hoa_don_log>("hoa_don_log_select_by_hoadon", param);
        }

        public Task<IEnumerable<hoa_don_log>> SelectByHoaDonAsync(int hoa_don_id, int hoa_don_log_type_id)
        {
            var param = new DynamicParameters();
            param.Add("@hoa_don_id", hoa_don_id);
            param.Add("@hoa_don_log_type_id", hoa_don_log_type_id);
            return  _dbConnection.SelectAsync<hoa_don_log>("hoa_don_log_select_by_hoadon_type", param);
        }
    }
}