using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Cache
{
    public class FinalizeResultResponse
    {
        public int hoaDonId { get; set; }
        public string SignedXmlBase64 { get; set; }
    }
}
