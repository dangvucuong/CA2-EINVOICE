using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Cache
{
    public class HoaDonSignSessionCache
    {
        public int HoaDonId { get; set; }
        public string XmlContent { get; set; }
        public string AppendXPath { get; set; }
        public string PrepareResultJson { get; set; } // Chuỗi JSON chứa đối tượng SignPrepareResult
        public bool IsCompleted { get; set; }
    }
}
