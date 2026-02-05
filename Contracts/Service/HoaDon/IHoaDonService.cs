using Contracts.Service.Base;
using Model.Base;
using Model.FuncResult;
using Model.Request.HoaDon;
using Model.Request.ToKhai;
using Model.Respone.HoaDon;
using Model.Table;

namespace Contracts.Service.HoaDon
{
    public interface IHoaDonService : ICRUDService<hoa_don>
    {
        Task<PagingResult<IEnumerable<hoa_don_vm>>> SelectByDonViAsync(string donvi_ma_dv, HoaDonSelectPagingRequest pagingRequest);
        Task<PagingResult<IEnumerable<hoa_don_vm>>> SelectByDonViThongKePageAsync(string donvi_ma_dv, HoaDonSelectPagingRequest pagingRequest);
        Task<hoa_don_vm> SelectViewModelAsync(int id);
        Task<int> SelectHoaDonIdByMaTraCuuAsync(string maTraCuu);
        Task<IEnumerable<hoa_don>> SelectByIdsAsync(List<int> ids);
        Task<hoa_don> SelectByPhatHanhUuidAsync(string phat_hanh_uuid);
        Task<FunctionResult<int>> SaveHoaDonAsync(HoaDonAddOrEditModel model);
        Task<FunctionResult<Model.Request.Xml.HoaDon>> CreateXmlObjectKySoAsync(int id, bool isPreview = false);
        Task<FunctionResult<string>> CreateBase64MTTAsync(int id);
        Task<FunctionResult<string>> CreateBase64MTTAsync(hoa_don hoaDon);
        Task<FunctionResult<string>> CreateBase64_206MTTAsync(int id, string signedText);
        Task<FunctionResult<string>> CreateBase64_206MTTAsync(hoa_don hoaDon, string signedText);
        Task<FunctionResult<string>> CreateBase64MTTBangKeAsync(List<hoa_don> hoaDons);
        Task<FunctionResult<string>> CreateXmlKySoAsync(int id, bool isPreview = false);
        Task<FunctionResult<string>> CreateXmlKySoAsync(hoa_don hoaDon);
        Task<FunctionResult<string>> GetHtmlPrintAsync(int id, int page_size = 10, MauHoaDonInChuyenDoiParam chuyenDoiParam = null);
        Task<FunctionResult<string>> GetHtmlForDownloadAsync(int id, int page_size = 10, MauHoaDonInChuyenDoiParam chuyenDoiParam = null);

        Task<FunctionResult<string>> GetHtmlPrintBienBanAsync(int id);
        Task<FunctionResult<string>> GetBase64BienBanAsync(int id);
        Task<FunctionResult<string>> GetHtmlPreviewAsync(HoaDonAddOrEditModel model);
        Task<FunctionResult<string>> UpdteKySoSuccessAsync(HoaDonPhatHanhRequest request, int user_id = 0);
        /// <summary>
        /// Ký bảng kê thành công
        /// </summary>
        /// <param name="hoaDons"></param>
        /// <param name="signed_text"></param>
        /// <param name="user_id"></param>
        /// <returns></returns>
        Task<List<HoaDonUpdateKySoSuccessItemRespone>> UpdteKySoSuccessBangKeAsync(List<hoa_don> hoaDons, string signed_text, int user_id = 0);
        Task<FunctionResult<HoaDonPhatHanhRespone>> PhatHanhAsync(HoaDonPhatHanhRequest request, int user_id_phathanh = 0);
        Task<FunctionResult<HoaDonPhatHanhRespone>> PhatHanhMTTAsync(HoaDonPhatHanhRequest request, hoa_don hoaDon, int user_id_phathanh = 0);
        Task<FunctionResult<HoaDonPhatHanhRespone>> PhatHanhMTT_KyRiengAsync(HoaDonPhatHanhRequest request, hoa_don hoaDon, int user_id_phathanh = 0);
        /// <summary>
        /// Phát hành bảng kê
        /// </summary>
        /// <param name="hoaDons"></param>
        /// <param name="signed_text"></param>
        /// <param name="user_id_phathanh"></param>
        /// <returns></returns>
        Task<FunctionResult<HoaDonPhatHanhRespone>> PhatHanhMTTBangKeAsync(List<hoa_don> hoaDons, string signed_text, int user_id_phathanh = 0);
        Task<FunctionResult<HoaDonPhatHanhRespone>> XuLyThongDiepAsync(string thongDiep);
        Task<FunctionResult<int>> GetSoHoaDonAsyn(string donvi_ma_dv, string hoa_don_dang_ky_phat_hanh_mau_so, string hoa_don_dang_ky_phat_hanh_ky_hieu);
        Task<FunctionResult<DateTime?>> GetNgayHoaDonPhatHanhMaxAsynsc(string donvi_ma_dv, string hoa_don_dang_ky_phat_hanh_mau_so, string hoa_don_dang_ky_phat_hanh_ky_hieu);
        Task<FunctionResult<int>> UpdateHoaDonPhatHanhLoiNhieuLanAsync();
        Task<FunctionResult<int>> UpdateHoaDonPhatHanhLoiChuaPhatHanhAsync();
        Task<FunctionResult<int>> SaoChepHoaDonNghichDaoAsync();
        Task<FunctionResult<int>> XuLyLoiMaKhongLienTiepAsync();
        Task<bool> InsertThueSuatHoaDonAsync(int hoaDonId, List<ThueSuatModel> dsThue);
        Task<bool> InsertHoaDonThongTinBoSungAsync(int hoaDonId, HoaDonThongTinBoSung infor);



    }
}