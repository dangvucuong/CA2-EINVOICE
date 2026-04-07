using Common;
using Contracts.Repository.HoaDon;
using Contracts.Service.HoaDon;
using Model.Enum;
using Model.Table;
using Service.Base;

namespace Service.HoaDon
{
    public class HoaDonDangKyPhatHanhService : CRUDService<hoa_don_dang_ky_phat_hanh>, IHoaDonDangKyPhatHanhService
    {
        IHoaDonDangKyPhatHanhRepository _hoaDonDangKyPhatHanhRepository;
        public HoaDonDangKyPhatHanhService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.HoaDon.HoaDonDangKyPhatHanh;
            this._hoaDonDangKyPhatHanhRepository = _repositoryWrapper.HoaDon.HoaDonDangKyPhatHanh;
        }
        public override async Task<int> InsertAsync(hoa_don_dang_ky_phat_hanh obj)
        {

            var id = await _hoaDonDangKyPhatHanhRepository.InsertAsync(obj);
            if (id > 0)
            {
                var cacheKey = $"{e_redis_cache_key.HOADON_DANGKY_PHATHANH}_{obj.donvi_ma_dv}_dictionary";
                await _serviceWrapper.Cache.RemoveDataAsync(cacheKey);
            }
            return id;
        }
        public override async Task<bool> UpdateAsync(hoa_don_dang_ky_phat_hanh obj)
        {
            var isUpdated = await _hoaDonDangKyPhatHanhRepository.UpdateAsync(obj);
            if (isUpdated)
            {
                var cacheKey = $"{e_redis_cache_key.HOADON_DANGKY_PHATHANH}_{obj.donvi_ma_dv}_dictionary";
                await _serviceWrapper.Cache.RemoveDataAsync(cacheKey);
            }
            return isUpdated;
        }
        public override async Task<bool> DeleteAsync(int id)
        {
            var userId = this.GetCurrentUserId();
            var isDeleted = await _hoaDonDangKyPhatHanhRepository.DeleteAsync(id, userId);
            if (id > 0)
            {
                var obj = await this.SelectByIdAsync(id);
                if (obj != null)
                {
                    var cacheKey = $"{e_redis_cache_key.HOADON_DANGKY_PHATHANH}_{obj.donvi_ma_dv}_dictionary";
                    await _serviceWrapper.Cache.RemoveDataAsync(cacheKey);
                }
            }
            return isDeleted;
        }

        public async Task<IEnumerable<hoa_don_dang_ky_phat_hanh>> SelectByDonViAsync(string donvi_ma_dv)
        {
            var cacheKey = $"{e_redis_cache_key.HOADON_DANGKY_PHATHANH}_{donvi_ma_dv}_dictionary";
            // var cachedData = await _serviceWrapper.Cache.GetListDataAsync<hoa_don_dang_ky_phat_hanh>(cacheKey + "*");
            var cachedData = await _serviceWrapper.Cache.GetDataAsync<List<hoa_don_dang_ky_phat_hanh>>(cacheKey);
            if (cachedData != null && cachedData.Count() > 0)
            {
                return cachedData;
            }
            else
            {
                var list = await _repositoryWrapper.HoaDon.HoaDonDangKyPhatHanh.SelectByDonViAsync(donvi_ma_dv);
                await _serviceWrapper.Cache.SetDataAsync<List<hoa_don_dang_ky_phat_hanh>>(cacheKey, list.ToList(), DateTimeOffset.Now.AddHours(1));
                return list;
            }
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
        public async Task<bool> CheckIfPhatHanhDaSuDung(string donvi_ma_dv, string mau_so, string ky_hieu)
        {
            var hoaDon = await _repositoryWrapper.HoaDon.HoaDon.SelectAnyHoaDonAsync(donvi_ma_dv, mau_so, ky_hieu);
            return hoaDon != null;
            // throw new NotImplementedException();
        }

        public async Task<bool> CheckIfSoHoaDonValid(string donvi_ma_dv, string mau_so, string ky_hieu, int so_bat_dau)
        {
            var CKM = this.GetHoaDonType(ky_hieu);
            if (CKM == "M")
            {
                var soHoaDonMax = (await _repositoryWrapper.HoaDon.HoaDon.GetMaxMaSoHoaDonMTT(donvi_ma_dv, mau_so, DateTime.Now.Year)).ConvertToInt();
                if (so_bat_dau <= soHoaDonMax)
                {
                    return false;
                }
            }
            else
            {
                var soHoaDonMax = (await _repositoryWrapper.HoaDon.HoaDon.GetMaxMaSoHoaDon(donvi_ma_dv, mau_so, ky_hieu)).ConvertToInt();
                if (so_bat_dau <= soHoaDonMax)
                {
                    return false;
                }
            }

            return true;
        }
    }
}