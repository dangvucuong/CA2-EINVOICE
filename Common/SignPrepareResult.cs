using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class SignPrepareResult
    {
        public string SignedInfoXml { get; set; }

        public string SignedInfoHashBase64 { get; set; }

        public string ObjectXml { get; set; }

        public string SignatureId { get; set; }
    }
}
