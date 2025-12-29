using Contracts.Service.BangTongHop;
using Model.Table;
using Service.Base;

namespace Service.BangTongHop
{
    public class BangTongHopLogService : CRUDService<bang_tong_hop_du_lieu_log>, IBangTongHopLogService
    {
        public BangTongHopLogService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.BangTongHopDuLieu.BangTongHopLog;
        }

        public Task<IEnumerable<bang_tong_hop_du_lieu_log>> SelectByBangTongHopIdAsync(int bangTongHopId)
        {
            return _repositoryWrapper.BangTongHopDuLieu.BangTongHopLog.SelectByBangTongHopIdAsync(bangTongHopId);
        }
    }
}