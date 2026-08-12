using System.Threading.Tasks;
using Common;
using Contract.Service;
using Contracts.Service.HoaDon;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Model.Enum;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/hoa-don-ky-so-remote")]
    [MustLogged]

    public class HoaDonKySoRemoteController : BaseController
    {
        private IHoaDonService _hoaDonService;
        private IHoaDonKyLoService _hoaDonKyLoService;
        public HoaDonKySoRemoteController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
            this._hoaDonService = _serviceWrapper.HoaDon.HoaDon;
            this._hoaDonKyLoService = _serviceWrapper.HoaDon.KyLo;
        }

        [HttpPost("{id}/ky-so")]
        [MustAuthorized("[POST]api/hoa-don/phat-hanh")]
        public async Task<ContentResult> KySoAsync([FromRoute] int id, [FromQuery] bool notify = false)
        {
            var userId = this.GetUserId();
            var userSerialInfo = await _serviceWrapper.User.User.SelectByIdAsync(userId);
            if (userSerialInfo.is_hsm_signing || userSerialInfo.rs_ma_but_ky.ConvertToString() != "")
            {
                var hoaDon = await _hoaDonService.SelectByIdAsync(id);
                if (hoaDon != null)
                {
                    if (hoaDon.is_ky_so_succes == true)
                    {
                        return this.BadRequest("Hóa đơn đã ký số người bán");
                    }
                    var donVi = await _serviceWrapper.Category.DonVi.SelectByMaDonViAsync(hoaDon.donvi_ma_dv);
                    if (donVi != null && donVi.total_cks_con_lai <= 0) return this.BadRequest("Đã hết chữ ký số");
                    var xmlResult = await _hoaDonService.CreateXmlKySoAsync(hoaDon);
                    if (!xmlResult.is_success)
                    {
                        return this.BadRequest(xmlResult.message);
                    }
                    var base64 = xmlResult.data.ConvertToBase64();
                    if (base64.ConvertToString() == "") return this.BadRequest("Không tạo được XML");
                    if (userSerialInfo.is_hsm_signing)
                    {
                        var kySoResult = await _hoaDonKyLoService.KySoHSMAsync(hoaDon, base64, hoaDon.donvi_ma_dv, userSerialInfo.serial_number, null);
                        if (kySoResult != null && kySoResult.Macode == 1)
                        {
                            return this.OK();
                        }
                        return this.BadRequest("Ký số thất bại");
                    }
                    if (userSerialInfo.rs_ma_but_ky.ConvertToString() != "")
                    {
                        if (notify)
                        {
                            var kySoResult = await _hoaDonKyLoService.KySoRemoteSigningBackgroundAsync(hoaDon, base64, hoaDon.donvi_ma_dv, userSerialInfo.serial_number);
                            if (kySoResult.is_success)
                            {
                                return this.OK(kySoResult.data);
                            }
                            return this.BadRequest(kySoResult.message);
                        }
                        else
                        {
                            var kySoResult = await _hoaDonKyLoService.KySoRemoteSigningAsync(hoaDon, base64, hoaDon.donvi_ma_dv, userSerialInfo.serial_number);
                            if (kySoResult.is_success)
                            {
                                return this.OK();
                            }
                            return this.BadRequest(kySoResult.message);
                        }
                    }

                }
                return this.BadRequest("Không tìm thấy hóa đơn");
            }

            return this.BadRequest("Chỉ áp dụng với tài khoản được phép ký số HSM hoặc Remote Signing");

        }
        [HttpPost("{id}/ky-so-va-phat-hanh")]
        [MustAuthorized("[POST]api/hoa-don/phat-hanh")]
        public async Task<ContentResult> KySoVaPhatHanhAsync([FromRoute] int id, [FromQuery] bool notify = false)
        {
            var userId = this.GetUserId();
            var userSerialInfo = await _serviceWrapper.User.User.SelectByIdAsync(userId);
            if (userSerialInfo.is_hsm_signing || userSerialInfo.rs_ma_but_ky.ConvertToString() != "")
            {
                var hoaDon = await _hoaDonService.SelectByIdAsync(id);
                if (hoaDon != null)
                {
                    var donVi = await _serviceWrapper.Category.DonVi.SelectByMaDonViAsync(hoaDon.donvi_ma_dv);
                    if (donVi != null && donVi.total_cks_con_lai <= 0) return this.BadRequest("Đã hết chữ ký số");
                    var base64 = "";
                    if (hoaDon.hoa_don_hinh_thuc_code == "M")
                    {
                        var base64Result = await _hoaDonService.CreateBase64MTTAsync(hoaDon);
                        base64 = base64Result.data;
                    }
                    else
                    {
                        var xmlResult = await _hoaDonService.CreateXmlKySoAsync(hoaDon);
                        if (!xmlResult.is_success) return this.BadRequest(xmlResult.message);
                        base64 = xmlResult.data.ConvertToBase64();
                    }
                    if (base64.ConvertToString() == "") return this.BadRequest("Không tạo được XML");
                    string base64BienBan = null;
                    if (hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_DIEU_CHINH || hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_THAY_THE)
                    {
                        var getBase64BienBanResult = await _hoaDonService.GetBase64BienBanAsync(id);
                        if (!getBase64BienBanResult.is_success) return this.BadRequest(getBase64BienBanResult.message);
                        base64BienBan = getBase64BienBanResult.data;
                    }
                    if (userSerialInfo.is_hsm_signing)
                    {
                        var phatHanhResult = await _hoaDonKyLoService.SignAndPhatHanhHSMAsync(hoaDon, base64, userSerialInfo.serial_number, null, null, base64BienBan);
                        if (phatHanhResult)
                        {
                            return this.OK();
                        }
                        return this.BadRequest("Thất bại");
                    }
                    if (userSerialInfo.rs_ma_but_ky.ConvertToString() != "")
                    {
                        if (notify)
                        {
                            var kySoResult = await _hoaDonKyLoService.KySoRemoteSigningThenPhatHanhBackgroundAsync(hoaDon, base64, hoaDon.donvi_ma_dv, userSerialInfo.serial_number, null, null);
                            if (kySoResult.is_success)
                            {
                                return this.OK(kySoResult.data);
                            }
                            return this.BadRequest(kySoResult.message);
                        }
                        else
                        {

                            var phatHanhResult = await _hoaDonKyLoService.SignAndPhatHanhRemoteSigningAsync(hoaDon, base64, userSerialInfo.serial_number, null, null);
                            if (phatHanhResult)
                            {
                                return this.OK();
                            }
                            return this.BadRequest("Thất bại");
                        }
                    }

                }
                return this.BadRequest("Không tìm thấy hóa đơn");
            }


            return this.BadRequest("Chỉ áp dụng với tài khoản được phép ký số HSM hoặc Remote Signing");

        }


    }
}



