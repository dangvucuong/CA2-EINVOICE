using Contracts.Service.Base;
using Model.Base;
using Model.Request.HoaDon;
using Model.Request.Hub;
using Model.Respone.ApiSign;
using Model.Respone.HoaDon;
using Model.Respone.Xml;
using Model.Table;

namespace Contracts.Service.HoaDon
{
    public interface IHoaDonKyLoService : ICRUDService<hoa_don>
    {
        Task<IEnumerable<HoaDonCreateXmlKySoRespone>> CreateXmlVaPhatHanhsAsync(HoaDonKyLoRequest request, bool isRunBackgroundForRS = false);
        Task<bool> SignAndPhatHanhRemoteSigningAsync(hoa_don hoaDon, string base64, string serial, SemaphoreSlim lockStatus, ProcessChangedModel processChangedModel);
        Task<bool> SignAndPhatHanhHSMAsync(hoa_don hoaDon, string base64, string serial, SemaphoreSlim lockStatus, ProcessChangedModel processChangedModel, string? bienBanBase64);

        Task<ApiSignResultModel> KySoHSMAsync(hoa_don hoaDon, string base64, string mst, string serial, string? bienBanBase64);
        Task<FunctionResult<string>> KySoRemoteSigningAsync(hoa_don hoaDon, string base64, string mst, string serial);
        Task<FunctionResult<string>> KySoRemoteSigningBackgroundAsync(hoa_don hoaDon, string base64, string mst, string serial);
        Task<FunctionResult<string>> KySoThongDiep206MTTRemoteSigningBackgroundAsync(hoa_don hoaDon, string base64, string mst, string serial);
        Task<FunctionResult<string>> KySoRemoteSigningThenPhatHanhBackgroundAsync(hoa_don hoaDon, string base64, string mst, string serial, SemaphoreSlim lockStatus, ProcessChangedModel processChangedModel);
        Task<bool> XuLyThongDiepKySoHoaDonAsync(rs_yeu_cau_ky yeuCauKy);
        Task<bool> XuLyThongDiepKySoVaPhatHanhHoaDonAsync(rs_yeu_cau_ky yeuCauKy);
        Task<bool> XuLyThongDiepKySoVaPhatHanhHoaDonBangKeAsync(rs_yeu_cau_ky yeuCauKy);
        Task<IEnumerable<HoaDonCreateXmlKySoRespone>> CreateXmlVaPhatHanhsMTTBangKeAsync(HoaDonKyLoRequest request, List<hoa_don> hoaDonMTTs, bool isRunBackgroundForRS = false);
        Task<bool> XuLyThongDiepKetQuaPhanHanhAsync(KetQuaThongDiepRespone ketQuaThongDiepRespone, string xmlKetQua);

    }
}