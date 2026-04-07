using Contracts.Repository.BangTongHop;
using Contracts.Repository.Base;
using Repository.Base;

namespace Repository.BangTongHop
{
    public class BangTongHopDuLieuRepositoryWrapper : BaseRepositoryWrapper, IBangTongHopDuLieuRepositoryWrapper
    {
        private IBangTongHopRepository _bangTongHopRepository;
        private IBangTongHopHoaDonRepository _bangTongHopHoaDonRepository;
        private IBangTongHopLogRepository _bangTongHopLogRepository;
        public BangTongHopDuLieuRepositoryWrapper(IConnectionStrings connectionStrings) : base(connectionStrings)
        {
        }

        public IBangTongHopRepository BangTongHop => _bangTongHopRepository ??= new BangTongHopRepository(_defaultConnection);

        public IBangTongHopHoaDonRepository BangTongHopHoaDon => _bangTongHopHoaDonRepository ??= new BangTongHopHoaDonRepository(_defaultConnection);

        public IBangTongHopLogRepository BangTongHopLog => _bangTongHopLogRepository??= new BangTongHopLogRepository (_defaultConnection);
    }
}