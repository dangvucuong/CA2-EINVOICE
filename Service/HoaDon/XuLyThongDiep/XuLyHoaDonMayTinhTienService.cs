using System.Text.RegularExpressions;
using Amazon.Runtime;
using Common;
using Contracts.Service.HoaDon;
using Contracts.Service.HoaDon.XuLyThongDiep;
using Microsoft.VisualBasic;
using Model.Base;
using Model.Enum;
using Model.Respone.HoaDon;
using Model.Respone.Xml;
using Model.Table;
using Service.Base;

namespace Service.HoaDon.XuLyThongDiep
{
    public class XuLyHoaDonMayTinhTienService : BaseService, IXuLyThongDiepService
    {
        private IHoaDonService _hoaDonService;
        private IHoaDonLogService _hoaDonLogService;
        public XuLyHoaDonMayTinhTienService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _hoaDonService = _serviceWrapper.HoaDon.HoaDon;
            _hoaDonLogService = _serviceWrapper.HoaDon.HoaDonLog;
        }

        public async Task<FunctionResult<HoaDonPhatHanhRespone>> XuLyThongDiepAsync(hoa_don hoaDon, KetQuaThongDiepRespone thongDiepRespone, string xmlKetQua)
        {
            //hóa đơn MTT thì phải lấy lại dữ liệu từ DB
            //vì sau khi cached, dữ liệu đã thay đổi (cached trước khi có mã MTT, giờ đổi logic ký xong mới tạo mã MTT -> sửa lại)
            var hoaDonId = hoaDon.id;
            hoaDon = await _serviceWrapper.HoaDon.HoaDon.SelectByIdAsync(hoaDonId);
            if (hoaDon == null) return new ErrorResult<HoaDonPhatHanhRespone>("Không tìm thấy hóa đơn");
            var isHopLe = false;
            if (thongDiepRespone.TTChung.MLTDiep == "204")
            {
                var LTBao = thongDiepRespone.DLieu?.TBao?.DLTBao?.LTBao ?? "";
                if (LTBao == "2")
                {
                    var maKetQuaPhatHanh = thongDiepRespone?.DLieu?.HDon?.MCCQT?.Text.ConvertToString() ?? "";
                    hoaDon.phat_hanh_ma_ketqua_cqt = maKetQuaPhatHanh;
                    hoaDon.hoa_don_trang_thai_id = (int)e_hoa_don_trang_thai.DA_PHAT_HANH;
                    hoaDon.ket_qua_phat_hanh = $"";
                    isHopLe = true;
                }
                else
                {
                    var MTLoi = thongDiepRespone.DLieu.TBao?.DLTBao?.LHDKMa?.DSHDon?.HDon?.DSLDo?.LDo?.MTLoi ?? "";
                    string pattern = @"<MTLoi>(.*?)</MTLoi>";
                    var match = Regex.Match(xmlKetQua, pattern);
                    if (match.Success)
                    {
                        var MTLoi2 = match.Groups[1].Value;
                        if (MTLoi != MTLoi2)
                        {
                            if (MTLoi == "")
                            {
                                MTLoi = MTLoi2;
                            }
                            else
                            {
                                MTLoi += ";" + MTLoi2;
                            }
                        }
                    }
                    hoaDon.hoa_don_trang_thai_id = (int)e_hoa_don_trang_thai.KHONG_HOP_LE;
                    hoaDon.ket_qua_phat_hanh = $"{MTLoi}";
                }
            }
            var mltdiep = ThongDiepHoaDonHelper.GetMLTDiep(thongDiepRespone, xmlKetQua);
            if (ThongDiepHoaDonHelper.IsLoiThongDiep(thongDiepRespone, xmlKetQua))
            {
                hoaDon.hoa_don_trang_thai_id = (int)e_hoa_don_trang_thai.LOI_THONG_DIEP;
                hoaDon.ket_qua_phat_hanh = ThongDiepHoaDonHelper.GetMTa(thongDiepRespone, xmlKetQua);
            }
            else if (mltdiep == "999")
            {
                hoaDon.hoa_don_trang_thai_id = (int)e_hoa_don_trang_thai.DA_GUI_LEN_CQT_CHUA_PHAN_HOI_KIEM_TRA_DU_LIEU;
                hoaDon.ket_qua_phat_hanh = $"";
            }

            await _hoaDonService.UpdateAsync(hoaDon);
            var logResult = await _hoaDonLogService.SaveFromPhatHanhAsync(hoaDon.id, hoaDon.ket_qua_phat_hanh, xmlKetQua, hoaDon.hoa_don_trang_thai_id == (int)e_hoa_don_trang_thai.DA_PHAT_HANH);
            if (isHopLe)
            {
                return new SuccessResult<HoaDonPhatHanhRespone>(hoaDon.ket_qua_phat_hanh, new HoaDonPhatHanhRespone()
                {
                    id = hoaDon.id,
                    hoa_don_trang_thai_id = hoaDon.hoa_don_trang_thai_id,
                    file_thong_diep_url = logResult.data,
                    ket_qua_phat_hanh = hoaDon.ket_qua_phat_hanh,
                    KetQuaThongDiep = thongDiepRespone
                });
            }
            return new ErrorResult<HoaDonPhatHanhRespone>(hoaDon.ket_qua_phat_hanh, new HoaDonPhatHanhRespone()
            {
                id = hoaDon.id,
                hoa_don_trang_thai_id = hoaDon.hoa_don_trang_thai_id,
                file_thong_diep_url = logResult.data,
                ket_qua_phat_hanh = hoaDon.ket_qua_phat_hanh,
                KetQuaThongDiep = thongDiepRespone
            });
        }
    }
}