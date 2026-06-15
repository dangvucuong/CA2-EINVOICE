using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Cache
{
    public class SignSession
    {
        public string SessionId { get; set; }

        public int HoaDonId { get; set; }

        public string XmlContent { get; set; }

        public string SignedInfoXml { get; set; }

        public string ObjectXml { get; set; }

        public string SignatureId { get; set; }

        public string HashBase64 { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsCompleted { get; set; }
    }
}
