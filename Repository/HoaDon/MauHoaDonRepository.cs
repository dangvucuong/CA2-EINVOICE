using Contracts.Repository.Base;
using Contracts.Repository.HoaDon;
using Dapper;
using Model.Respone.HoaDon;
using Model.Table;
using Repository.Base;

namespace Repository.HoaDon
{
    public class MauHoaDonRepository : CRUDRepository<mau_hoa_don>, IMauHoaDonRepository
    {
        public MauHoaDonRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public Task<IEnumerable<mau_hoa_don_vm>> SelectByDonViAsync(string donvi_ma_dv)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", donvi_ma_dv);
            return _dbConnection.SelectAsync<mau_hoa_don_vm>("mau_hoa_don_select_by_donvi", param);
        }

        public Task<mau_hoa_don_vm> SelectVmByIdAsync(int id)
        {
            var param = new DynamicParameters();
            param.Add("@id", id);
            return _dbConnection.SelectFirstOrDefaultAsync<mau_hoa_don_vm>("mau_hoa_don_select_vm_by_id", param);
        }
    }
}