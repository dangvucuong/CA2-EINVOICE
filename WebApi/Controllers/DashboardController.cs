using System;
using System.Threading.Tasks;
using Contract.Service;
using Contracts.Service.HoaDon;
using Microsoft.AspNetCore.Mvc;
using Model.Request.Dashboard;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    [MustLogged]

    public class DashboardController : BaseController
    {
        private IHoaDonReportService _hoaDonReportService;
        public DashboardController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
            this._hoaDonReportService = _serviceWrapper.HoaDon.HoaDonReport;
        }
        [MustAuthorized]
        [HttpGet]
        [Route("hoa-don/trang-thai/from/{fromDate}/to/{toDate}")]
        public async Task<ContentResult> ThongKeTheoTrangThaiAsync([FromRoute] DateTime fromDate, [FromRoute] DateTime toDate)
        {
            var userInfo = this.GetUserInfo();
            var request = new HoaDonTrangThaiSummaryRequest()
            {
                donvi_ma_dv = userInfo.donvi_ma_dv,
                from_date = fromDate,
                to_date = toDate
            };
            var list = await _hoaDonReportService.SelectHoaDonTrangThaiAsync(request);
            return this.OK(list);
        }
        [HttpGet]
        [Route("hoa-don/trang-thai")]
        //  [MustAuthorized]
        public async Task<ContentResult> ThongKeTheoTrangThaiAllAsync()
        {
            var userInfo = this.GetUserInfo();
            var request = new HoaDonTrangThaiSummaryRequest()
            {
                donvi_ma_dv = userInfo.donvi_ma_dv,
                from_date = null,
                to_date = null
            };
            var list = await _hoaDonReportService.SelectHoaDonTrangThaiAsync(request);
            return this.OK(list);
        }
        [HttpGet]
        [Route("hoa-don/tong-so-luong")]
        [MustAuthorized]
        public async Task<ContentResult> ThongKeTongSoLuong()
        {
            var userInfo = this.GetUserInfo();
            var list = await _hoaDonReportService.SelectSoLuongChuKySoSummary(userInfo.donvi_ma_dv);
            return this.OK(list);
        }
        [HttpGet]
        [Route("hoa-don/phat-hanh-date/from/{fromDate}/to/{toDate}")]
        [MustAuthorized]
        public async Task<ContentResult> ThongKeSoLuongPhatHanhTheoNgay([FromRoute] DateTime fromDate, [FromRoute] DateTime toDate)
        {
            var userInfo = this.GetUserInfo();
            var request = new HoaDonTrangThaiSummaryRequest()
            {
                donvi_ma_dv = userInfo.donvi_ma_dv,
                from_date = fromDate,
                to_date = toDate
            };
            var list = await _hoaDonReportService.SelectLichSuPhatHanhAsync(request);
            return this.OK(list);
        }

    }
}

