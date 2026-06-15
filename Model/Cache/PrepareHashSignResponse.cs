using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Cache
{
    internal class PrepareHashSignResponse
    {
        public string SessionId { get; set; }
        public string HashBase64 { get; set; }
        public string SignedInfoXml { get; set; }
    }
}
