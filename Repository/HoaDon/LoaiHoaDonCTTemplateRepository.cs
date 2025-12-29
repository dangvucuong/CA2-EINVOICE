using Contracts.Repository.Base;
using Contracts.Repository.HoaDon;
using Dapper;
using Model.Respone.HoaDon;
using Model.Table;
using Repository.Base;

namespace Repository.HoaDon
{
    public class LoaiHoaDonCTTemplateRepository : CRUDRepository<loai_hoa_don_ct_template>, ILoaiHoaDonCTTemplateRepository
    {
        public LoaiHoaDonCTTemplateRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public Task<loai_hoa_don_ct_template_vm> SelectVmByIdAsync(int id)
        {
            var param = new DynamicParameters();
            param.Add("@id", id);
            return _dbConnection.SelectFirstOrDefaultAsync<loai_hoa_don_ct_template_vm>("loai_hoa_don_ct_template_select_vm_by_id", param);
        }
    }
}