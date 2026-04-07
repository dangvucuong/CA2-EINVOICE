using Contracts.Service.Base;
using Model.Base;
using Model.Request.BangTongHop;
using Model.Table;

namespace Contracts.Service.BangTongHop
{
    public interface IBangTongHopService : ICRUDService<bang_tong_hop_du_lieu>
    {
        Task<IEnumerable<bang_tong_hop_du_lieu>> SelectByDonViAsync(string donvi_ma_dv);
        Task<FunctionResult<bang_tong_hop_du_lieu>> SaveChangesAsync(BangTongHopAddOrEditRequest request);
        Task<string> CreateXmlKySoAsync(int id);
        Task<string> CreateXmlThongDiepAsync(int id, string signedText);
        Task<FunctionResult<bool>> PhatHanhAsync(int id, string signedText);
        Task<FunctionResult<bool>> XuLyThongDiepAsync(bang_tong_hop_du_lieu thongBaoSaiSot, Model.Respone.Xml.KetQuaThongDiepRespone ketQuaThongDiepRespone, string xmlThongDiep);
    }
}