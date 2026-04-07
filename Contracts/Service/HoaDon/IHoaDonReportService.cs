using Contracts.Service.Base;
using Model.Request.Dashboard;
using Model.Respone.Dashboard;

namespace Contracts.Service.HoaDon
{
    public interface IHoaDonReportService: IBaseService
    {
        Task<IEnumerable<HoaDonTrangThaiSummary>> SelectHoaDonTrangThaiAsync(HoaDonTrangThaiSummaryRequest request);
        Task<DonViSoLuongChuKySoSummary> SelectSoLuongChuKySoSummary(string donvi_mst);
        Task<IEnumerable<HoaDonLichSuPhatHanhItem>> SelectLichSuPhatHanhAsync(HoaDonTrangThaiSummaryRequest request);
    }
}