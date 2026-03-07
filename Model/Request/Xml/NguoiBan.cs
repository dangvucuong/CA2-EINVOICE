using System.Xml.Serialization;

namespace Model.Request.Xml
{
    [XmlRoot(ElementName = "NBan")]
    public class NguoiBan
    {

        [XmlElement(ElementName = "Ten")]
        public string ten_nguoi_ban { get; set; }

        [XmlElement(ElementName = "MST")]
        public string mst { get; set; }

        [XmlElement(ElementName = "DChi")]
        public string dia_chi { get; set; }


        [XmlElement(ElementName = "STKNHang")]
        public string stk { get; set; }

        [XmlElement(ElementName = "DCTDTu")]
        public string email { get; set; }

        [XmlElement(ElementName = "TNHang")]
        public string ngan_hang { get; set; }

        [XmlElement(ElementName = "SDThoai")]
        public string dien_thoai { get; set; }

        [XmlElement(ElementName = "Fax")]
        public string fax { get; set; }

        [XmlElement(ElementName = "Website")]
        public string website { get; set; }

        [XmlElement(ElementName = "TTKhac")]
        public ThongTinKhac thong_tin_khac { get; set; }

        /// <summary>
        /// Lệnh điều động nội bộ
        /// </summary>

        [XmlElement(ElementName = "LDDNBo")]
        public string LDDNBo { get; set; }

        /// <summary>
        /// Phương tiện vận chuyển
        /// </summary>

        [XmlElement(ElementName = "PTVChuyen")]
        public string PTVChuyen { get; set; }

        /// <summary>
        /// Hợp đồng số
        /// </summary>

        [XmlElement(ElementName = "HDSo")]
        public string HDSo { get; set; }

        /// <summary>
        /// Họ và tên người xuất hàng
        /// </summary>

        [XmlElement(ElementName = "HVTNXHang")]
        public string HVTNXHang { get; set; }

        /// <summary>
        /// Tên người vận chuyển
        /// </summary>

        [XmlElement(ElementName = "TNVChuyen")]
        public string TNVChuyen { get; set; }

        /// <summary>
        /// Hợp đồng kinh tế  số
        /// </summary>

        [XmlElement(ElementName = "HDKTSo")]
        public string HDKTSo { get; set; }

        /// <summary>
        /// Hợp đồng kinh tế ngày
        /// </summary>

        [XmlElement(ElementName = "HDKTNgay")]
        public string HDKTNgay { get; set; }

        //hoa don ban tai san cong
        [XmlElement(ElementName = "SQDinh")]
        public string SoQuyetdinh { get; set; }

        [XmlElement(ElementName = "NQDinh")]
        public string NgayQuyetdinh { get; set; }

        [XmlElement(ElementName = "CQBHQDinh")]
        public string CoQuanBHQDinh { get; set; }

        [XmlElement(ElementName = "HTBan")]
        public string HThucban { get; set; }


    }

}
