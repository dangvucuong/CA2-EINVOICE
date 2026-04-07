using Contracts.Repository.Base;
using Model.Respone.HoaDon;
using Model.Table;

namespace Contracts.Repository.HoaDon
{
    public interface IMauHoaDonRepository : ICRUDRepository<mau_hoa_don>
    {
        Task<IEnumerable<mau_hoa_don_vm>> SelectByDonViAsync(string donvi_ma_dv);
        Task<mau_hoa_don_vm> SelectVmByIdAsync(int id);
    }
}