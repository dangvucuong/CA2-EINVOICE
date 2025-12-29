using Contracts.Service.Base;
using Model.Respone.ApiSign;

namespace Contracts.Service.ApiSign
{
    public interface IApiSignHoaDonService:IBaseService
    {
        Task<ApiSignResultModel> SignAsync(string base64Xml, string mst, string serial);
        // Task<ApiSignResultModel> SignAsync(string base64Xml);
        Task<ApiSignResultModel> SignHoaDonAsync(int hoaDonId,  string serial);
        Task<IEnumerable<ApiSignResultModel>> SignHoaDonsAsync(List<int> hoaDonIds, string serial);

    }
}