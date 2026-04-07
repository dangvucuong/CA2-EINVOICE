using System.Data;
using Contracts.Service.Base;
using Model.Base;
using Model.Request.TBSS;
using Model.Respone.Upload;
using Model.Table;

namespace Contracts.Service.TBSS
{
    public interface IThongBaoSaiSotService : ICRUDService<thong_bao_sai_sot>
    {
        Task<IEnumerable<thong_bao_sai_sot>> SelectByDonViAsync(string donvi_ma_dv);
        Task<FunctionResult<thong_bao_sai_sot>> SaveChangesAsync(ThongBaoSaiSotAddOrEditRequest request);
        Task<string> CreateXmlKySoAsync(int id);
        Task<FunctionResult<bool>> KySoVaPhatHanhAsync(int id);
        Task<string> CreateXmlThongDiepAsync(int id, string signedText);
        Task<FunctionResult<bool>> PhatHanhAsync(int id, string signedText);
        Task<FunctionResult<bool>> XuLyThongDiepAsync(thong_bao_sai_sot thongBaoSaiSot, Model.Respone.Xml.KetQuaThongDiepRespone ketQuaThongDiepRespone, string xmlThongDiep);
        Task<string> GetHtmlPreviewAsync(int id);
        Task<FunctionResult<string>> CreateXmlBienBanFromTbssAsync(int id);
        Task<FunctionResult<DataTable>> ReadAndValidImportDataAsync(UploadRespone upload);
    }
}