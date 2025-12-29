using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Contract.Service;
using Contracts.Service.HoaDon;
using Microsoft.AspNetCore.Mvc;
using Model.Request.HoaDon;
using Model.Request.ToKhai;
using Service.Caching;
using StackExchange.Redis;
using WebApi.Filters;
using WebApp;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/hoa-don-ncm")]
    [MustLogged]

    public class HoaDonKySoNcmController : BaseController
    {
        private IHoaDonService _hoaDonService;
        private static readonly Dictionary<string, SemaphoreSlim> donViHoaDonLock =
           new Dictionary<string, SemaphoreSlim>();
        public HoaDonKySoNcmController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
            this._hoaDonService = _serviceWrapper.HoaDon.HoaDon;
        }
        private SemaphoreSlim GetLockForDonVi(string donvi_ma_dv, string hoa_don_dang_ky_phat_hanh_mau_so,
           string hoa_don_dang_ky_phat_hanh_ky_hieu, string taskName)
        {
            var key =
                $"{donvi_ma_dv}_{hoa_don_dang_ky_phat_hanh_mau_so}_{hoa_don_dang_ky_phat_hanh_ky_hieu}_${taskName}";
            lock (donViHoaDonLock)
            {
                if (!donViHoaDonLock.ContainsKey(key))
                {
                    donViHoaDonLock[key] = new SemaphoreSlim(1, 1);
                }

                return donViHoaDonLock[key];
            }
        }
        /// <summary>
        /// Thêm mới hóa đơn
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [HttpPost]
        [MustAuthorized("[POST]api/hoa-don")]

        public async Task<ContentResult> InsertAsync([FromBody] HoaDonAddOrEditModel model)
        {
            //tạo khóa cho đơn vị, mẫu số , ký hiệu -> giới hạn số request đồng thời
            var donViHoaDonLock = GetLockForDonVi(model.donvi_ma_dv, model.hoa_don_dang_ky_phat_hanh_mau_so,
                model.hoa_don_dang_ky_phat_hanh_ky_hieu, "InsertAsync");
            try
            {
                await donViHoaDonLock.WaitAsync();
                var user = this.GetUserInfo();
                var user_id = user.id;
                var userSerialInfo = await _serviceWrapper.User.User.SelectByIdAsync(user_id);
                if (userSerialInfo == null || userSerialInfo.serial_number.ConvertToString() == "")
                {
                    return this.BadRequest("Không tìm thấy serial_number hợp lệ");
                }
                model.SetInsertInfo(user_id);
                if (user.vender_id.ConvertToString().Trim() != string.Empty)
                {
                    model.vender_id = user.vender_id;
                }
                else
                {
                    model.donvi_ma_dv = user.donvi_ma_dv;
                }

                model.hoa_don_nghi_dinh_id = 123;
                model.ma_so_hoa_don = null;
                var result = await _hoaDonService.SaveHoaDonAsync(model);
                if (result.is_success)
                {
                    await this.SaveLogAsync($"Thêm hóa đơn: {model.id}", model, user);

                    var kySoResulit = await _serviceWrapper.ApiSignHoaDon.SignHoaDonAsync(model.id, userSerialInfo.serial_number);
                    await this.SaveLogAsync($"Kết quả ký số: {model.id} = {kySoResulit.Macode}", kySoResulit, user);

                    if (kySoResulit.Macode == 1)
                    {
                        if (model.hoa_don_hinh_thuc_code == "M")
                        {
                            var hoaDon = await _hoaDonService.SelectByIdAsync(model.id);
                            var base64 = await _hoaDonService.PhatHanhMTTAsync(new HoaDonPhatHanhRequest()
                            {
                                id = model.id,
                                signed_text = kySoResulit.SignedData
                            }, hoaDon);

                        }
                        else
                        {
                            var phatHanhResult = await _hoaDonService.PhatHanhAsync(new HoaDonPhatHanhRequest()
                            {
                                id = model.id,
                                signed_text = kySoResulit.SignedData
                            });
                        }
                    }

                    var obj = await _hoaDonService.SelectByIdAsync(model.id);
                    return this.OK(obj);
                }
                return this.BadRequest(result.message);
            }
            catch (System.Exception ex)
            {
                LogWriter.Writer(Newtonsoft.Json.JsonConvert.SerializeObject(model), "api/hoa-don-ncm", ex.Message);
                return this.BadRequest(ex.Message);
            }
            finally
            {
                donViHoaDonLock.Release();
            }
        }
        [HttpPost("{id}/ky-so")]
        [MustAuthorized("[POST]api/hoa-don/phat-hanh")]
        public async Task<ContentResult> XmlKySoBase64Async([FromRoute] int id)
        {
            var user_id = this.GetUserId();
            var userSerialInfo = await _serviceWrapper.User.User.SelectByIdAsync(user_id);
            if (userSerialInfo == null || userSerialInfo.serial_number.ConvertToString() == "")
            {
                return this.BadRequest("Không tìm thấy serial_number hợp lệ");
            }
            var result = await _serviceWrapper.ApiSignHoaDon.SignHoaDonAsync(id, userSerialInfo.serial_number);
            return this.OK(result);
        }
        [HttpPost("{id}/ky-so-va-phat-hanh")]
        [MustAuthorized("[POST]api/hoa-don/phat-hanh")]
        public async Task<ContentResult> KySoVaPhatHanhAsync([FromRoute] int id)
        {
            var user_id = this.GetUserId();
            var userSerialInfo = await _serviceWrapper.User.User.SelectByIdAsync(user_id);
            if (userSerialInfo == null || userSerialInfo.serial_number.ConvertToString() == "")
            {
                return this.BadRequest("Không tìm thấy serial_number hợp lệ");
            }
            var obj = await _hoaDonService.SelectByIdAsync(id);
            var kySoResult = await _serviceWrapper.ApiSignHoaDon.SignHoaDonAsync(id, userSerialInfo.serial_number);
            if (kySoResult.Macode == 1)
            {
                if (obj.hoa_don_hinh_thuc_code == "M")
                {
                    var base64 = await _hoaDonService.PhatHanhMTTAsync(new HoaDonPhatHanhRequest()
                    {
                        id = obj.id,
                        signed_text = kySoResult.SignedData
                    }, obj);

                }
                else
                {
                    var phatHanhResult = await _hoaDonService.PhatHanhAsync(new HoaDonPhatHanhRequest()
                    {
                        id = obj.id,
                        signed_text = kySoResult.SignedData
                    });
                }
            }
            obj = await _hoaDonService.SelectByIdAsync(id);
            return this.OK(obj);
        }
        [Route("ky-so-multiple")]
        [HttpPost]
        [MustAuthorized("[POST]api/hoa-don/phat-hanh")]
        public async Task<ContentResult> XmlKySoBase64MultiplyAsync([FromBody] HoaDonDeletesRequest request)
        {
            var user_id = this.GetUserId();
            var userSerialInfo = await _serviceWrapper.User.User.SelectByIdAsync(user_id);
            if (userSerialInfo == null || userSerialInfo.serial_number.ConvertToString() == "")
            {
                return this.BadRequest("Không tìm thấy serial_number hợp lệ");
            }
            var result = await _serviceWrapper.ApiSignHoaDon.SignHoaDonsAsync(request.ids, userSerialInfo.serial_number);
            return this.OK(result);
        }



    }
}

