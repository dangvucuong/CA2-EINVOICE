using System.Xml.Xsl;
using Contracts.Service.Base;
using Model.Base;

namespace Contracts.Service.Xslt
{
    public interface IXsltService : IBaseService
    {
        Task<FunctionResult<string>> FillDataAsync<T>(string path, T data);
        Task<FunctionResult<string>> FillDataAsync<T>(string path, T data, XsltArgumentList xsltArgumentList);
        Task<FunctionResult<string>> FillDataAsXmlAsync(string path, string xmlData, XsltArgumentList xsltArgumentList);
        Task<FunctionResult<string>> FillDataAsXmlAsyncV1(string path, string xmlData, XsltArgumentList xsltArgumentList);
        Task<FunctionResult<string>> FillDataAsXmlFromXsltContentAsync(string xlstContent, string xmlData, XsltArgumentList xsltArgumentList);
        Task<FunctionResult<string>> FillDataAsXmlFromXsltContentAsyncV1(string xlstContent, string xmlData, XsltArgumentList xsltArgumentList);

    }
}