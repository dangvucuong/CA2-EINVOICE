using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Cache
{
    public class XmlSignSession
    {
        public string SessionId { get; set; }
        public string Xml { get; set; }
        public string SignedInfoXml { get; set; }
        public string ReferenceId { get; set; }
        public string ObjectId { get; set; }
        public string SignatureId { get; set; }
        public bool IncludeObjectReference { get; set; }
        public int HoaDonId { get; set; }
    }

}
