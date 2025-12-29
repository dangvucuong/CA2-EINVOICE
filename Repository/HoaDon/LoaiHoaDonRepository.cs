using Contracts.Repository.Base;
using Contracts.Repository.HoaDon;
using Model.Table;
using Repository.Base;

namespace Repository.HoaDon
{
    public class LoaiHoaDonRepository : CRUDRepository<loai_hoa_don>, ILoaiHoaDonRepository
    {
        public LoaiHoaDonRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }
    }
}