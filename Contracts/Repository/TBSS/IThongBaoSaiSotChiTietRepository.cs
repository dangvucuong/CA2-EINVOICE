using Contracts.Repository.Base;
using Model.Table;

namespace Contracts.Repository.TBSS
{
    public interface IThongBaoSaiSotChiTietRepository : ICRUDRepository<thong_bao_sai_sot_chi_tiet>
    {
        Task<IEnumerable<thong_bao_sai_sot_chi_tiet>> SelectByThongBaoAsync(int thong_bao_sai_sot_id);
        Task<IEnumerable<thong_bao_sai_sot_chi_tiet>> SelectByThongBaoIdsAsync(List<int> thong_bao_sai_sot_ids);
    }
}