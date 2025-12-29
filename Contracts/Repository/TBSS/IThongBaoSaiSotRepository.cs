using Contracts.Repository.Base;
using Model.Table;

namespace Contracts.Repository.TBSS
{
    public interface IThongBaoSaiSotRepository : ICRUDRepository<thong_bao_sai_sot>
    {
        Task<IEnumerable<thong_bao_sai_sot>> SelectByDonViAsync(string donvi_ma_dv);
        Task<thong_bao_sai_sot> SelectByPhatHanhUuidAsync(string phat_hanh_uuid);
    }
}