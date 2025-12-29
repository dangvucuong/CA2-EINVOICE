using Contracts.Repository.Base;

namespace Contracts.Repository.BangTongHop
{
    public interface IBangTongHopDuLieuRepositoryWrapper: IBaseRepositoryWrapper
    {
        IBangTongHopRepository BangTongHop { get; }
        IBangTongHopHoaDonRepository BangTongHopHoaDon { get; }
        IBangTongHopLogRepository BangTongHopLog {get;}
    }
}