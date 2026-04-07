using System.Xml;
using System.Xml.Xsl;
using Contracts.Service.Xslt;
using Model.Base;
using Service.Base;
using Common;
using System.Text.RegularExpressions;
namespace Service.Xslt
{
    public class XsltService : BaseService, IXsltService
    {
        public XsltService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<FunctionResult<string>> FillDataAsXmlAsyncV1(string path, string xmlData, XsltArgumentList xsltArgumentList)
        {
            try
            {
                string xsltContent = File.ReadAllText(path);

                string modifiedXslt = xsltContent.Replace("<xsl:value-of select=\"substring(\r\n  concat('00000000', TTChung/TTHDLQuan/SHDCLQuan), \r\n  string-length(DLHDon/TTChung/TTHDLQuan/SHDCLQuan) + 1, \r\n  7\r\n)\"/>", "<xsl:value-of select=\"substring(\r\n\t\t\t\tconcat('0000000', DLHDon/TTChung/TTHDLQuan/SHDCLQuan),\r\n\t\t\t\tstring-length(concat('0000000', DLHDon/TTChung/TTHDLQuan/SHDCLQuan)) - 6\r\n\t\t\t\t)\"/>");

                using (XmlReader xsltReader = XmlReader.Create(new StringReader(modifiedXslt)))
                {
                    var xslt = new XslCompiledTransform();

                    xslt.Load(xsltReader, XsltSettings.TrustedXslt, new XmlUrlResolver());

                    // Xử lý XML data (giữ nguyên logic cũ của bạn)
                    using (XmlReader inputDataHelper = XmlReader.Create(new StringReader(xmlData)))
                    {
                        using (StringWriter sw = new StringWriter())
                        {
                            xslt.Transform(inputDataHelper, xsltArgumentList, sw);
                            string transformedXml = sw.ToString();
                            return new SuccessResult<string>(transformedXml);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                return new ErrorResult<string>(ex.Message);
            }
        }

        //public async Task<FunctionResult<string>> FillDataAsXmlAsync(string path, string xmlData, XsltArgumentList xsltArgumentList)
        //{
        //    try
        //    {
        //        using (XmlReader xmlReader = XmlReader.Create(new StringReader(xmlData)))
        //        {
        //            var xslt = new XslCompiledTransform();
        //            xslt.Load(path);

        //            using (StringWriter sw = new StringWriter())
        //            {
        //                xslt.Transform(xmlReader, xsltArgumentList, sw);
        //                string transformedXml = sw.ToString();
        //                return new SuccessResult<string>(transformedXml);
        //            }

        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return new ErrorResult<string>(ex.Message);
        //    }
        //}


        public async Task<FunctionResult<string>> FillDataAsXmlAsync(string path, string xmlData, XsltArgumentList xsltArgumentList)
        {
            try
            {
                string xsltContent = await File.ReadAllTextAsync(path);

                string oldExprPattern =
        @"substring\s*\(\s*concat\('00000000'\s*,\s*TTChung/TTHDLQuan/SHDCLQuan\)\s*,\s*string-length\(DLHDon/TTChung/TTHDLQuan/SHDCLQuan\)\s*\+\s*1\s*,\s*7\s*\)";

                string newExpr =
                    "substring(concat('0000000', DLHDon/TTChung/TTHDLQuan/SHDCLQuan)," +
                    " string-length(concat('', DLHDon/TTChung/TTHDLQuan/SHDCLQuan)) + 1, 7)";

                string modifiedXslt = xsltContent;

                if (Regex.IsMatch(xsltContent, oldExprPattern, RegexOptions.IgnoreCase))
                {
                    modifiedXslt = Regex.Replace(xsltContent, oldExprPattern, newExpr, RegexOptions.IgnoreCase);

                    string removeValueOfPattern =
                        @"<\s*xsl:value-of\s+select\s*=\s*""DLHDon/TTChung/TTHDLQuan/SHDCLQuan""\s*/>";

                    modifiedXslt = Regex.Replace(modifiedXslt, removeValueOfPattern, "", RegexOptions.IgnoreCase);
                }
 


                using (XmlReader xsltReader = XmlReader.Create(new StringReader(modifiedXslt)))
                {
                    var xslt = new XslCompiledTransform();
                    xslt.Load(xsltReader, XsltSettings.TrustedXslt, new XmlUrlResolver());

                    using (XmlReader xmlReader = XmlReader.Create(new StringReader(xmlData)))
                    using (StringWriter sw = new StringWriter())
                    {
                        xslt.Transform(xmlReader, xsltArgumentList, sw);
                        return new SuccessResult<string>(sw.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                return new ErrorResult<string>(ex.Message);
            }
        }

        public async Task<FunctionResult<string>> FillDataAsXmlFromXsltContentAsync(string xsltContent, string xmlData, XsltArgumentList xsltArgumentList)
        {
            try
            {
                using (var xmlReader = XmlReader.Create(new StringReader(xmlData)))
                using (var xsltReader = XmlReader.Create(new StringReader(xsltContent)))
                {
                    var xslt = new XslCompiledTransform();
                    xslt.Load(xsltReader);

                    using (var sw = new StringWriter())
                    {
                        xslt.Transform(xmlReader, xsltArgumentList, sw);
                        string transformedXml = sw.ToString();
                        return new SuccessResult<string>(transformedXml);
                    }
                }
            }
            catch (Exception ex)
            {
                return new ErrorResult<string>(ex.Message);
            }
        }



        public async Task<FunctionResult<string>> FillDataAsXmlFromXsltContentAsyncV1(string xsltContent, string xmlData, XsltArgumentList xsltArgumentList)
        {
            try
            {
                string modifiedXslt = xsltContent.Replace("<xsl:value-of select=\"substring(\r\n  concat('00000000', TTChung/TTHDLQuan/SHDCLQuan), \r\n  string-length(DLHDon/TTChung/TTHDLQuan/SHDCLQuan) + 1, \r\n  7\r\n)\"/>", "<xsl:value-of select=\"substring(\r\n\t\t\t\tconcat('0000000', DLHDon/TTChung/TTHDLQuan/SHDCLQuan),\r\n\t\t\t\tstring-length(concat('0000000', DLHDon/TTChung/TTHDLQuan/SHDCLQuan)) - 6\r\n\t\t\t\t)\"/>");

                using (var xmlReader = XmlReader.Create(new StringReader(xmlData)))
                using (var xsltReader = XmlReader.Create(new StringReader(modifiedXslt)))
                {
                    var xslt = new XslCompiledTransform();
                    xslt.Load(xsltReader);

                    using (var sw = new StringWriter())
                    {
                        xslt.Transform(xmlReader, xsltArgumentList, sw);
                        string transformedXml = sw.ToString();
                        return new SuccessResult<string>(transformedXml);
                    }
                }
            }
            catch (Exception ex)
            {
                return new ErrorResult<string>(ex.Message);
            }
        }



        public async Task<FunctionResult<string>> FillDataAsync<T>(string path, T data)
        {
            try
            {
                string xmlInput = data.SerializeToXml();
                using (XmlReader xmlReader = XmlReader.Create(new StringReader(xmlInput)))
                {
                    var xslt = new XslCompiledTransform();
                    xslt.Load(path);

                    using (StringWriter sw = new StringWriter())
                    {
                        xslt.Transform(xmlReader, null, sw);
                        string transformedXml = sw.ToString();
                        return new SuccessResult<string>(transformedXml);
                    }

                }
            }
            catch (System.Exception ex)
            {
                return new ErrorResult<string>(ex.Message);
            }
        }

        public async Task<FunctionResult<string>> FillDataAsync<T>(string path, T data, XsltArgumentList xsltArgumentList)
        {
            try
            {
                string xmlInput = data.SerializeToXml();
                using (XmlReader xmlReader = XmlReader.Create(new StringReader(xmlInput)))
                {
                    var xslt = new XslCompiledTransform();
                    xslt.Load(path);

                    using (StringWriter sw = new StringWriter())
                    {
                        xslt.Transform(xmlReader, xsltArgumentList, sw);
                        string transformedXml = sw.ToString();
                        return new SuccessResult<string>(transformedXml);
                    }

                }
            }
            catch (System.Exception ex)
            {
                return new ErrorResult<string>(ex.Message);
            }
        }
    }
}