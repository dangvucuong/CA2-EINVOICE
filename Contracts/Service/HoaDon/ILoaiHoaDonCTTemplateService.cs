using System.Xml.Xsl;
using Contracts.Service.Base;
using Model.Request.HoaDon;
using Model.Respone.HoaDon;
using Model.Table;

namespace Contracts.Service.HoaDon
{
    public interface ILoaiHoaDonCTTemplateService : ICRUDService<loai_hoa_don_ct_template>
    {
        Task<loai_hoa_don_ct_template_vm> SelectVmByIdAsync(int id);
        Task<string> GeneratePreviewAsync(mau_hoa_don mauHoaDon);
        Task<string> GeneratePrintHtmlAsync(mau_hoa_don mauHoaDon, MauHoaDonCreateHtmlInput input, XsltArgumentList xsltArgumentList);
        Task<string> GeneratePrintHtmlAsync(mau_hoa_don mauHoaDon, string xmlData, XsltArgumentList xsltArgumentList);
        Task<string> GeneratePrintHtmlFromXsltContentAsync(string xsltContent, string xmlData, XsltArgumentList xsltArgumentList);
        Task<string> GeneratePrintHtmlFromXsltContentAsyncV1(string xsltContent, string xmlData, XsltArgumentList xsltArgumentList);


        

        // Task<string> GeneratePrintHtmlAsync(int loai_hoa_don_ct_template_id, MauHoaDonCreateHtmlInput input);
        // Task<string> GeneratePrintHtmlAsync(int loai_hoa_don_ct_template_id, MauHoaDonCreateHtmlInput input, XsltArgumentList xsltArgumentList);
        // Task<string> GeneratePrintHtmlAsync(int loai_hoa_don_ct_template_id, string xmlData, XsltArgumentList xsltArgumentList);
    }
}