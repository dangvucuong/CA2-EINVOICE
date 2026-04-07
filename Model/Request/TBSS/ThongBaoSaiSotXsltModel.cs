using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model.Request.TBSS
{
    public class ThongBaoSaiSotXsltModel
    {
        public class DLTBao
        {
            public string MSo { get; set; } // Mẫu số
            public string TCQT { get; set; } // Tên cơ quan thuế
            public string TNNT { get; set; } // Tên người nộp thuế
            public string MST { get; set; } // Mã số thuế
            public string DDanh { get; set; } // Địa danh
            public DateTime NTBao { get; set; } // Ngày thông báo
            public List<HDon> DSHDon { get; set; } // Danh sách hóa đơn
            public string LDo { get; set; } // Lý do
        }

        public class HDon
        {
            public int STT { get; set; } // Số thứ tự
            public string MCQTCap { get; set; } // Mã CQT cấp
            public string KHMSHDon { get; set; } // Ký hiệu mẫu số hóa đơn
            public string KHHDon { get; set; } // Ký hiệu hóa đơn
            public string SHDon { get; set; } // Số hóa đơn
            public string NLAP { get; set; } // Ngày lập hóa đơn
            public int LADHDDT { get; set; } // Loại áp dụng hóa đơn điện tử
            public int TCTBao { get; set; } // Tình trạng thông báo (Hủy/Điều chỉnh/Thay thế/Giải trình/Sai sót do tổng hợp)
            // public string LDo { get; set; } // Lý do
        }

        public class SignatureInfo
        {
            public string X509SubjectName { get; set; } // Tên chủ thể chữ ký số
            public DateTime SigningTime { get; set; } // Thời gian ký
        }

        public class TBaoInfo
        {
            public DLTBao DLTBao { get; set; } // Dữ liệu thông báo
        }

        public TBaoInfo TBao { get; set; } // Thông báo
        public SignatureInfo Signature { get; set; } // Thông tin chữ ký số
    }
}