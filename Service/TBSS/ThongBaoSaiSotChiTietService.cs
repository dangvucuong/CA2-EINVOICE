using Contracts.Service.TBSS;
using Model.Table;
using Service.Base;

namespace Service.TBSS
{
    public class ThongBaoSaiSotChiTietService : CRUDService<thong_bao_sai_sot_chi_tiet>, IThongBaoSaiSotChiTietService
    {
        public ThongBaoSaiSotChiTietService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.ThongBaoSaiSot.ThongBaoSaiSotChiTiet;
        }

        public Task<IEnumerable<thong_bao_sai_sot_chi_tiet>> SelectByThongBaoIdAsync(int thong_bao_sai_sot_id)
        {
            return _repositoryWrapper.ThongBaoSaiSot.ThongBaoSaiSotChiTiet.SelectByThongBaoAsync(thong_bao_sai_sot_id);
        }

        public Task<IEnumerable<thong_bao_sai_sot_chi_tiet>> SelectByThongBaoIdsAsync(List<int> thong_bao_sai_sot_ids)
        {
            return _repositoryWrapper.ThongBaoSaiSot.ThongBaoSaiSotChiTiet.SelectByThongBaoIdsAsync(thong_bao_sai_sot_ids);
        }
    }
}