using Contracts.Repository.Base;
using Model.FuncResult;
using Model.Request.HoaDon;
using Model.Request.ThongKe;
using Model.Respone.HoaDon;
using Model.Respone.ThongKe;
using Model.Table;
using Model.Request.Xml;
using System;
using System.Collections.Generic;

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
        Task<HoaDonNgayLienKeRespone> SelectNgayHoaDonLienKeAsync(string donvi_ma_dv, string mau_so, string ky_hieu, int hoa_don_id, DateTime ngay_hoa_don);
        Task<HoaDonSoNhoHonChuaKySoRespone> SelectSoHoaDonNhoHonChuaKySoAsync(string donvi_ma_dv, string mau_so, string ky_hieu, int hoa_don_id, int ma_so_hoa_don_hien_tai, DateTime ngay_hoa_don_hien_tai, IEnumerable<int> excludeHoaDonIds = null);
        Task<HoaDonNgayChoPhepTheoSoRespone> SelectNgayHoaDonChoPhepTheoSoAsync(string donvi_ma_dv, string mau_so, string ky_hieu, int hoa_don_id, int ma_so_hoa_don);
        Task<DateTime?> SelectNgayToiThieuChuaCoSoAsync(string donvi_ma_dv, string mau_so, string ky_hieu, int hoa_don_id, DateTime ngay_hoa_don);
        Task<bool> UpdateTrangThaiAsync(int id, int hoa_don_trang_thai_id);

        Task<bool> InsertThueSuatHoaDonAsync(int id, IEnumerable<ThueSuatModel> dsThue);
        Task<bool> InsertHoaDonThongTinBoSungAsync(int id, HoaDonThongTinBoSung infor);
        Task<IEnumerable<ThueSuatModel>> SelectThueSuatHoaDonByHoaDonIdAsync(int hoaDonId);
        Task<hd_thong_tin_bo_sung> SelectHoaDonThongTinBoSungByHoaDonIdAsync(int hoaDonId);

        Task<PagingResult<IEnumerable<hoa_don_vm>>> SelectChoPhanHoiCQTAsync(string donvi_ma_dv,HoaDonSelectPagingRequest pagingRequest);

        Task<PagingResult<IEnumerable<hoa_don_vm>>> SelectChuaGuiCQTAsync(string donvi_ma_dv,HoaDonSelectPagingRequest pagingRequest);

    }
}