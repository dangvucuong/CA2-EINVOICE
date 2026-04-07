using Contracts.Service.HoaDon;
using Model.Request.Dashboard;
using Model.Respone.Dashboard;
using Service.Base;

namespace Service.HoaDon
{
    public class HoaDonReportService : BaseService, IHoaDonReportService
    {
        public HoaDonReportService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public Task<IEnumerable<HoaDonTrangThaiSummary>> SelectHoaDonTrangThaiAsync(HoaDonTrangThaiSummaryRequest request)
        {
            return _repositoryWrapper.HoaDon.HoaDonReport.SelectHoaDonTrangThaiAsync(request);
        }

        public Task<IEnumerable<HoaDonLichSuPhatHanhItem>> SelectLichSuPhatHanhAsync(HoaDonTrangThaiSummaryRequest request)
        {
            return _repositoryWrapper.HoaDon.HoaDonReport.SelectLichSuPhatHanh(request);

        }

        public async Task<DonViSoLuongChuKySoSummary> SelectSoLuongChuKySoSummary(string donvi_mst)
        {
            try
            {
                var tongSoDaMuaTask = _repositoryWrapper.HoaDon.HoaDonReport.SelectTongSoLuongHoaDonDaMuaAsync(donvi_mst);
                var tongSoDaSuDungTask = _repositoryWrapper.HoaDon.HoaDonReport.SelectTongSoLuongHoaDonDaSuDungAsync(donvi_mst);
                var listTask = new List<Task>() { tongSoDaMuaTask, tongSoDaSuDungTask };
                await Task.WhenAll(listTask);
                var tongSoDaMua = await tongSoDaMuaTask;
                var tongSoDaSuDung = await tongSoDaSuDungTask;
                return new DonViSoLuongChuKySoSummary()
                {
                    donvi_ma_dv = donvi_mst,
                    tong_so_luong_da_mua = tongSoDaMua,
                    tong_so_luong_da_su_dung = tongSoDaSuDung
                };
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }
    }
}