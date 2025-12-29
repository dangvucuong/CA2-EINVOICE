namespace Contracts.Service.BangTongHop
{
    public interface IBangTongHopDuLieuServiceWrapper
    {
        IBangTongHopService BangTongHop {get;}
        IBangTongHopHoaDonService BangTongHopHoaDon {get;}
        IBangTongHopLogService BangTongHopLog {get;}
    }
}