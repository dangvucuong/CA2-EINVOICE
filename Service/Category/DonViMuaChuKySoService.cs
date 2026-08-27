using Common;
using Contracts.Service.Category;
using Model.Base;
using Model.Table;
using Service.Base;

namespace Service.Category
{
    public class DonViMuaChuKySoService : CRUDService<donvi_mua_chukyso>, IDonViMuaChuKySoService
    {
        public DonViMuaChuKySoService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.Category.DonViMuaChuKySo;
        }

        public Task<IEnumerable<donvi_mua_chukyso>> SelectByDonViAsync(string donvi_ma_dv)
        {
            return _repositoryWrapper.Category.DonViMuaChuKySo.SelectByDonViAsync(donvi_ma_dv);
        }

        public async Task<FunctionResult<object>> ThuHoiGoiDvAsync(string ma_so_thue, string idHSDT, int userId = 0)
        {
            ma_so_thue = ma_so_thue.ConvertToString().Trim();
            idHSDT = idHSDT.ConvertToString().Trim();

            if (ma_so_thue == "")
                return new FunctionResult<object>(false, "Mã số thuế không được để trống");

            if (idHSDT == "")
                return new FunctionResult<object>(false, "idHSDT không được để trống");

            var existing = await _repositoryWrapper.Category.DonViMuaChuKySo
                .SelectByMstAndTransactionIdAsync(ma_so_thue, idHSDT);

            if (existing == null)
                return new FunctionResult<object>(false, "Không tìm thấy gói dịch vụ với mã số thuế và idHSDT tương ứng");

            if (existing.is_deleted)
                return new FunctionResult<object>(false, "Gói dịch vụ đã được thu hồi trước đó");

            existing.is_deleted = true;
            existing.trang_thai_hd_id = 0;
            existing.SetUpdateInfo(userId);

            var isUpdated = await this.UpdateAsync(existing);
            if (!isUpdated)
                return new FunctionResult<object>(false, "Cập nhật dữ liệu thất bại");

            return new FunctionResult<object>(true, "Thu hồi gói dịch vụ thành công", new
            {
                id = existing.id,
                donvi_mst = ma_so_thue,
                transaction_id = idHSDT
            });
        }
    }
}