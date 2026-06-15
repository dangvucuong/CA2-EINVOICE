using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class PrepareHashSignRequest
    {
        public int HoaDonId { get; set; }     
        public string Xml { get; set; }   
        public string ReferenceId { get; set; }
        // Object Id
        // ví dụ: Obj-NBan-1910633
        public string ObjectId { get; set; }
        // Signature Id
        // ví dụ: NBan-1910633
        public string SignatureId { get; set; }
        // Có ký object/reference thứ 2 không
        public bool IncludeObjectReference { get; set; }
    }
}
