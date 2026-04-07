using Contracts.Repository.Base;
using Model.Table;

namespace Contracts.Repository.HoaDon
{
    public interface IHoaDonDangKyPhatHanhRepository : ICRUDRepository<hoa_don_dang_ky_phat_hanh>
    {
        Task<IEnumerable<hoa_don_dang_ky_phat_hanh>> SelectByDonViAsync(string donvi_ma_dv);
        
    }
}