using Contracts.Service.Base;
using Model.Table;

namespace Contracts.Service.TBSS
{
    public interface IThongBaoSaiSotChiTietService : ICRUDService<thong_bao_sai_sot_chi_tiet>
    {
        Task<IEnumerable<thong_bao_sai_sot_chi_tiet>> SelectByThongBaoIdAsync(int thong_bao_sai_sot_id);
        Task<IEnumerable<thong_bao_sai_sot_chi_tiet>> SelectByThongBaoIdsAsync(List<int> thong_bao_sai_sot_ids);
    }
}