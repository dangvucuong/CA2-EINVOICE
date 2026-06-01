using System.Collections.Generic;
using System.Threading.Tasks;
using Model.Cache;

namespace Contracts.Service.HoaDon
{
    public interface IHoaDonSignService
    {
        Task<HoaDonPrepareHashSignResponse> PrepareCoreGenericAsync(XmlSignTarget target);

        // SỬA CHỖ NÀY: Thay X509Certificate2 bằng string certBase64
        Task<(string, int)> FinalizeCoreGenericAsync(HoaDonFinalizeHashSignRequest request, string certBase64, string appendXPath = "/HDon/DSCKS/NBan");
    }
}