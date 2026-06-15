using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Cache
{
    public class XmlSignTarget
    {
        public int DocId { get; set; }              // ID tài liệu (Hóa đơn/vé/tờ khai) để lưu DB
        public string XmlContent { get; set; }      // Chuỗi nội dung XML thô ban đầu
        public string IdToSign { get; set; }        // Tham số thứ 2 của Helper (Ví dụ: "_1923188")
        public string ObjectId { get; set; }        // Tham số thứ 3 của Helper (Ví dụ: "Obj-NBan-1923188")
        public string AppendXPath { get; set; }     // Tham số thứ 5 của hàm Finalize (Ví dụ: "/HDon/DSCKS/NBan")
    }
}
