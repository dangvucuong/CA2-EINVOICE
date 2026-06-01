using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Cache
{
    public class HoaDonFinalizeHashSignRequest
    {
        public string SessionId { get; set; }
        public string SignatureValue { get; set; }
    }
}
