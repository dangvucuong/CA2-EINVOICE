using Contracts.Repository.Base;
using Model.FuncResult;
using Model.Request.HoaDon;
using Model.Request.ThongKe;
using Model.Respone.HoaDon;
using Model.Respone.ThongKe;
using Model.Table;

namespace Contracts.Repository.HoaDon
{
    public interface IHoaDonRepository : ICRUDRepository<hoa_don>
    {
        Task<int> SelectHoaDonIdByMaTraCuuAsync(string maTraCuu);
        Task<int> SelectHoaDonIdByInvoiceIdAsync(string invoice_id);

        Task<IEnumerable<hoa_don>> SelectByIdsAsync(List<int> ids);
        Task<PagingResult<IEnumerable<hoa_don_vm>>> SelectByDonViAsync(string donvi_ma_dv, HoaDonSelectPagingRequest pagingRequest);
        Task<PagingResult<IEnumerable<hoa_don_vm>>> SelectByDonViThongKePageAsync(string donvi_ma_dv, HoaDonSelectPagingRequest pagingRequest);
        Task<string> GetMaxMaSoHoaDon(string donvi_ma_dv, string mau_so, string ky_hieu);
        Task<DateTime?> GetMaxNgayHoaDon(string donvi_ma_dv, string mau_so, string ky_hieu);
        Task<string> GetMaxMaSoHoaDonMTT(string donvi_ma_dv, string mau_so, int year);
        Task<hoa_don> SelectByPhatHanhUuidAsync(string phat_hanh_uuid);
        Task<List<hoa_don>> SelectListHoaDonByPhatHanhUuidAsync(string phat_hanh_uuid);
        Task<hoa_don> SelectHoaDonGocAsync(string donvi_ma_dv, string mau_so, string ky_hieu, int so_hoa_don);
        Task<hoa_don> SelectHoaDonDieuChinhThayTheChoHoaDonAsync(string donvi_ma_dv, string mau_so, string ky_hieu, int so_hoa_don);

        Task<IEnumerable<ThongKeTopKhachHangTheoSoLuongHoaDonRespone>> GetTopKhachHangBySoLuongHDAsync(ThongKeTopKhachHangTheoHoaDonRequest request);
        Task<IEnumerable<ThongKeTopKhachHangTheoSoLuongHoaDonRespone>> GetTopKhachHangBySoGiaTriHDAsync(ThongKeTopKhachHangTheoHoaDonRequest request);
        Task<hoa_don> SelectAnyHoaDonAsync(string donvi_ma_dv, string mau_so, string ky_hieu);
        Task<bool> UpdateMaTraCuuAsync(int id, string ma_tra_cuu);
        Task<IEnumerable<hoa_don>> SelectHoaDonLoiPhatHanhNhieuLanAsync();
        Task<IEnumerable<hoa_don>> SelectHoaDonLoiChaPhathanhAsync();
        Task<bool> UpdatePhatHanhBangKeAsync(List<int> ids, string phat_hanh_uuid, int user_id_phathanh);
        Task<bool> UpdateMaSoHoaDonAsync(int id, int ma_so_hoa_don);
        Task<DateTime?> GetNgayHoaDonPhatHanhMaxAsynsc(string donvi_ma_dv, string hoa_don_dang_ky_phat_hanh_mau_so, string hoa_don_dang_ky_phat_hanh_ky_hieu);
        Task<bool> UpdateTrangThaiAsync(int id, int hoa_don_trang_thai_id);

        Task<IEnumerable<HoaDonPdfInforResponse>> SelectByMaSoHoaDonRangeAsync(string donvi_ma_dv, string ky_hieu, int fromMaSo, int toMaSo);



    }
}