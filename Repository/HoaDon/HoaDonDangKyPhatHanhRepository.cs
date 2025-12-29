using Contracts.Repository.Base;
using Contracts.Repository.HoaDon;
using Dapper;
using Model.Table;
using Repository.Base;

namespace Repository.HoaDon
{
    public class HoaDonDangKyPhatHanhRepository : CRUDRepository<hoa_don_dang_ky_phat_hanh>, IHoaDonDangKyPhatHanhRepository
    {
        public HoaDonDangKyPhatHanhRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public Task<IEnumerable<hoa_don_dang_ky_phat_hanh>> SelectByDonViAsync(string donvi_ma_dv)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", donvi_ma_dv);
            return _dbConnection.SelectAsync<hoa_don_dang_ky_phat_hanh>("hoa_don_dang_ky_phat_hanh_select_by_donvi", param);
        }
    }
}