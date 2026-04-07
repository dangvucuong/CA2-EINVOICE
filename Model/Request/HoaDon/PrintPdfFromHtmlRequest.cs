using Model.Request.Base;
using Swashbuckle.AspNetCore.Annotations;

namespace Model.Request.HoaDon
{
    public class PrintPdfFromHtmlRequest
    {
        public string html { get; set; }    
        public string file_name { get; set; }
    }
}