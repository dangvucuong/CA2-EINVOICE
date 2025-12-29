using Contracts.Repository.Base;
using Model.Request.Dashboard;
using Model.Respone.Dashboard;

namespace Contracts.Repository.HoaDon
{
    public interface IHoaDonReportRepository : IBaseRepository
    {
        Task<IEnumerable<HoaDonTrangThaiSummary>> SelectHoaDonTrangThaiAsync(HoaDonTrangThaiSummaryRequest request);
        Task<int> SelectTongSoLuongHoaDonDaMuaAsync(string donvi_mst);
        Task<int> SelectTongSoLuongHoaDonDaSuDungAsync(string donvi_mst);
        Task<IEnumerable<HoaDonLichSuPhatHanhItem>> SelectLichSuPhatHanh(HoaDonTrangThaiSummaryRequest request);

    }
}