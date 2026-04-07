using Contracts.Service.BangTongHop;
using Service.Base;

namespace Service.BangTongHop
{
    public class BangTongHopDuLieuServiceWrapper : BaseService, IBangTongHopDuLieuServiceWrapper
    {
        private IBangTongHopService _bangTongHopService;
        private IBangTongHopHoaDonService _bangTongHopHoaDonService;
        private IBangTongHopLogService _bangTongHopLogService;
        public BangTongHopDuLieuServiceWrapper(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public IBangTongHopService BangTongHop => _bangTongHopService??=new BangTongHopService(_serviceProvider);

        public IBangTongHopHoaDonService BangTongHopHoaDon => _bangTongHopHoaDonService??=new BangTongHopHoaDonService (_serviceProvider);

        public IBangTongHopLogService BangTongHopLog => _bangTongHopLogService??= new BangTongHopLogService(_serviceProvider);
    }
}