using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Contract.Service;
using Contracts.Service.HoaDon;
using Microsoft.AspNetCore.Mvc;
using Model.Table;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/dang-ky-phat-hanh-hoa-don")]
    [MustLogged]

    public class HoaDonDangKyPhatHanhController : BaseController
    {
        private IHoaDonDangKyPhatHanhService _hoaDonDangKyPhatHanh;
        public HoaDonDangKyPhatHanhController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
            this._hoaDonDangKyPhatHanh = _serviceWrapper.HoaDon.HoaDonDangKyPhatHanh;
        }
        [HttpGet]
        /// <summary>
        /// Xem danh sách các đăng ký phát hành của đơn vị
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        public async Task<ContentResult> SelectByDonViAsync()
        {
            var userInfo = this.GetUserInfo();
            var list = await _hoaDonDangKyPhatHanh.SelectByDonViAsync(userInfo.donvi_ma_dv);
            list = list.OrderByDescending(x => x.id).ToList();
            return this.OK(list);
        }
        /// <summary>
        /// Tạo mới 1 đăng ký phát hành
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [HttpPost]
        [MustAuthorized]
        public async Task<ContentResult> InsertAsync([FromBody] hoa_don_dang_ky_phat_hanh model)
        {
            var user = this.GetUserInfo();
            var user_id = user.id;
            model.SetInsertInfo(user_id);
            model.donvi_ma_dv = user.donvi_ma_dv;
            model.so_luong = model.so_ket_thuc.ConvertToInt() - model.so_bat_dau.ConvertToInt() + 1;
            model.ngay_qd = DateTime.Now;
            var isSoBatDauValid = await _hoaDonDangKyPhatHanh.CheckIfSoHoaDonValid(model.donvi_ma_dv, model.mau_so, model.ky_hieu, model.so_bat_dau.ConvertToInt());
            if (!isSoBatDauValid)
            {
                return this.BadRequest("Số bắt đầu không được nhỏ hơn số hóa đơn đã sử dụng");
            }
            model.id = await _hoaDonDangKyPhatHanh.InsertAsync(model);
            if (model.id > 0)
            {
                await this.SaveLogAsync($"Đăng ký phát hành hóa đơn ({model.id}): ký hiệu {model.ky_hieu} từ {model.so_bat_dau} đến {model.so_ket_thuc}", model);
                return this.OK(model);
            }
            return this.BadRequest();
        }
        /// <summary>
        /// Sửa đăng ký phát hành
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [MustAuthorized]
        [HttpPut]
        public async Task<ContentResult> UpdateAsync([FromBody] hoa_don_dang_ky_phat_hanh model)
        {
            var user_id = this.GetUserId();
            var obj = await _hoaDonDangKyPhatHanh.SelectByIdAsync(model.id);
            if (obj == null) return this.BadRequest();
            var isDaSuDung = await _hoaDonDangKyPhatHanh.CheckIfPhatHanhDaSuDung(obj.donvi_ma_dv, obj.mau_so, obj.ky_hieu);
            if (isDaSuDung)
            {
                if (obj.ky_hieu != model.ky_hieu)
                {
                    return this.BadRequest("Không thể sửa ký hiệu với đăng ký đã được sử dụng");
                }
                // obj.SetUpdateInfo(user_id);
                // obj.hinh_thuc_code = model.hinh_thuc_code;
                // obj.mau_so = model.mau_so;
                // obj.so_bat_dau = model.so_bat_dau;
                obj.so_ket_thuc = model.so_ket_thuc;
                obj.so_luong = model.so_ket_thuc.ConvertToInt() - obj.so_bat_dau.ConvertToInt() + 1;
                // obj.ngay_su_dung = model.ngay_su_dung;
                // obj.so_qd = model.so_qd;
                // obj.ky_hieu = model.ky_hieu;
                obj.ten_hoa_don = model.ten_hoa_don;
                // obj.hinh_thuc_code = model.hinh_thuc_code;
                // obj.is_chiu_thue = model.is_chiu_thue;
            }
            else
            {
                obj.SetUpdateInfo(user_id);
                obj.hinh_thuc_code = model.hinh_thuc_code;
                obj.mau_so = model.mau_so;
                obj.so_bat_dau = model.so_bat_dau;
                obj.so_ket_thuc = model.so_ket_thuc;
                obj.so_luong = model.so_ket_thuc.ConvertToInt() - model.so_bat_dau.ConvertToInt() + 1;
                obj.ngay_su_dung = model.ngay_su_dung;
                obj.so_qd = model.so_qd;
                obj.ky_hieu = model.ky_hieu;
                obj.ten_hoa_don = model.ten_hoa_don;
                obj.hinh_thuc_code = model.hinh_thuc_code;
                obj.is_chiu_thue = model.is_chiu_thue;
            }


            var validationError = await _hoaDonDangKyPhatHanh.ValidateSoKhoangPhatHanhAsync(obj);
            if (validationError != null)
            {
                return this.BadRequest(validationError);
            }

            var isUpdated = await _hoaDonDangKyPhatHanh.UpdateAsync(obj);
            if (isUpdated)
            {
                await this.SaveLogAsync($"Cập nhật đăng ký phát hành hóa ({model.id}) đơn ký hiệu {model.ky_hieu} từ {model.so_bat_dau} đến {model.so_ket_thuc}", model);
            }
            return isUpdated ? this.OK(obj) : this.BadRequest("Cập nhật đăng ký phát hành không thành công");
        }
        /// <summary>
        /// Xóa đăng ký phát hành
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [HttpDelete("{id}")]
        [MustAuthorized]
        public async Task<ContentResult> DeleteAsync([FromRoute] int id)
        {
            var obj = await _hoaDonDangKyPhatHanh.SelectByIdAsync(id);
            if (obj == null) return this.BadRequest();
            var isDaSuDung = await _hoaDonDangKyPhatHanh.CheckIfPhatHanhDaSuDung(obj.donvi_ma_dv, obj.mau_so, obj.ky_hieu);
            if (isDaSuDung) return this.BadRequest("Không thể xóa đăng ký đã sử dụng");
            var isDeleted = await _hoaDonDangKyPhatHanh.DeleteAsync(obj.id);
            if (isDeleted) await this.SaveLogAsync($"Xóa đăng ký phát hành hóa đơn: {obj.id}", null);
            return isDeleted ? this.OK(obj) : this.BadRequest();
        }

    }
}

