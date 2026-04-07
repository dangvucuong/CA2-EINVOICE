using Contracts.Repository.Base;
using Model.Respone.HoaDon;
using Model.Table;

namespace Contracts.Repository.HoaDon
{
    public interface ILoaiHoaDonCTTemplateRepository : ICRUDRepository<loai_hoa_don_ct_template>
    {
        Task<loai_hoa_don_ct_template_vm> SelectVmByIdAsync(int id);
    }
}