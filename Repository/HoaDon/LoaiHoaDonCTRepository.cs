using Contracts.Repository.Base;
using Contracts.Repository.HoaDon;
using Model.Table;
using Repository.Base;

namespace Repository.HoaDon
{
    public class LoaiHoaDonCTRepository : CRUDRepository<loai_hoa_don_ct>, ILoaiHoaDonCTRepository
    {
        public LoaiHoaDonCTRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }
    }
}