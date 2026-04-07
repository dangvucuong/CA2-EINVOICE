using Common;
using Contracts.Service.HoaDon.XuLyThongDiep;
using Model.Table;
using Service.Base;

namespace Service.HoaDon.XuLyThongDiep
{
    public class XuLyThongDiepProvider : BaseService, IXyLyThongDiepProvider
    {
        public XuLyThongDiepProvider(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<IXuLyThongDiepService> GetServiceAsync(hoa_don hoaDon)
        {
            var hoaDonType = GetHoaDonType(hoaDon.hoa_don_dang_ky_phat_hanh_ky_hieu);
            if (hoaDonType == "C")
                return new XuLyHoaDonCoMaService(_serviceProvider);
            if (hoaDonType == "K")
                return new XuLyHoaDonKhongCoMaService(_serviceProvider);
            if (hoaDonType == "M")
                return new XuLyHoaDonMayTinhTienService(_serviceProvider);
            return null;
        }
        private string GetHoaDonType(string kyHieu)
        {
             if (kyHieu.ConvertToString().Length >= 4)
            {
                if (kyHieu.ConvertToString().Substring(3, 1).ConvertToString().ToUpper() == "M") return "M";
            }
            if (kyHieu.ConvertToString().FirstOrDefault().ConvertToString().ToUpper() == "K") return "K";
            if (kyHieu.ConvertToString().FirstOrDefault().ConvertToString().ToUpper() == "C") return "C";
            
            return "";
        }
    }
}