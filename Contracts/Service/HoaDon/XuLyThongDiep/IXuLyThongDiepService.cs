using Contracts.Service.Base;
using Model.Base;
using Model.Respone.HoaDon;
using Model.Table;

namespace Contracts.Service.HoaDon.XuLyThongDiep
{
    public interface IXuLyThongDiepService :IBaseService
    {
        Task<FunctionResult<HoaDonPhatHanhRespone>> XuLyThongDiepAsync(hoa_don hoaDon, Model.Respone.Xml.KetQuaThongDiepRespone thongDiepRespone, string xmlKetQua);
        
    }
}