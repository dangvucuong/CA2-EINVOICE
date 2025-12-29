using Contracts.Repository.Base;
using Contracts.Repository.HoaDon;
using Dapper;
using Model.Table;
using Repository.Base;

namespace Repository.HoaDon
{
    public class HoaDonLoaiPhiRepository : CRUDRepository<hoa_don_loai_phi>, IHoaDonLoaiPhiRepository
    {
        public HoaDonLoaiPhiRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public  Task<IEnumerable<hoa_don_loai_phi>> SelectByHoaDonAsync(int hoa_don_id)
        {
            var param = new DynamicParameters();
            param.Add("@hoa_don_id", hoa_don_id);
            return  _dbConnection.SelectAsync<hoa_don_loai_phi>("hoa_don_loai_phi_select_by_hoadon", param);
        }
    }
}