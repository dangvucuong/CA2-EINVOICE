using Contracts.Service.Base;
using Model.Base;
using Model.Request.ToKhai;
using Model.Table;

namespace Contracts.Service.ToKhai
{
    public interface IToKhaiService : ICRUDService<to_khai>
    {
        Task<IEnumerable<to_khai>> SelectByDonViAsync(string donvi_ma_dv);
        Task<int> SaveToKhaiAsync(ToKhaiAddOrEditModel model);
        Task<ToKhaiAddOrEditModel> SelectViewModel(int id);

        Task<string> PhatHanhAsync(int id, string signedText, int user_id_phathanh=0);
        Task<string> CreateXmlKySoAsync(int id);
        Task<FunctionResult<string>> GetHtmlPrintAsync(int id);
        Task<FunctionResult<string>> GetHtmlPhatHanhAsync(int id);
        Task<FunctionResult<bool>> XuLyThongDiepAsync(to_khai toKhai, Model.Respone.Xml.KetQuaThongDiepRespone ketQuaThongDiepRespone, string xmlThongDiep);
        Task<FunctionResult<string>> KySoVaPhatHanhAsync(int id);
    }
}