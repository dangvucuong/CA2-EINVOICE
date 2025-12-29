using Contracts.Service.Base;
using Model.Table;

namespace Contracts.Service.HoaDon
{
    public interface IHoaDonDangKyPhatHanhService : ICRUDService<hoa_don_dang_ky_phat_hanh>
    {
        Task<IEnumerable<hoa_don_dang_ky_phat_hanh>> SelectByDonViAsync(string donvi_ma_dv);
        Task<bool> CheckIfPhatHanhDaSuDung(string donvi_ma_dv, string mau_so, string ky_hieu);
        Task<bool> CheckIfSoHoaDonValid(string donvi_ma_dv, string mau_so, string ky_hieu, int so_bat_dau);

    }
}