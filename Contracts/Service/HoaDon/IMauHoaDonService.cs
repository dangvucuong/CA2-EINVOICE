using Contracts.Service.Base;
using Model.Base;
using Model.Request.HoaDon;
using Model.Request.ToKhai;
using Model.Respone.HoaDon;
using Model.Table;

namespace Contracts.Service.HoaDon
{
    public interface IMauHoaDonService : ICRUDService<mau_hoa_don>
    {
        Task<IEnumerable<mau_hoa_don_vm>> SelectByDonViAsync(string donvi_ma_dv);
        Task<mau_hoa_don_vm> SelectMauActiveByDonVAsync(string donvi_ma_dv, int loai_hoa_don_ct_id);
        Task<MauHoaDonCreateHtmlInput> CreateSampleData(mau_hoa_don mauHoaDon);
        Task<FunctionResult<string>> CreatePrintHtmlAsync(hoa_don hoaDon, int soHoaDonTrenTrang = 10, MauHoaDonInChuyenDoiParam chuyenDoiParam = null);
        Task<FunctionResult<string>> CreatePreviewHtmlAsync(HoaDonAddOrEditModel hoaDon, bool isShowMau = true);
        Task<FunctionResult<string>> CreatePreviewHtmlAsync(int hoaDonId, bool isShowMau = true);
        //
    }
}