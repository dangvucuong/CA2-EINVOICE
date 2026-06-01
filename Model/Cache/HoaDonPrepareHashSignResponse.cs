using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Cache
{
    public class HoaDonPrepareHashSignResponse
    {
        public string SessionId { get; set; }

        public int HoaDonId { get; set; }

        public string HashBase64 { get; set; }
    }
}
