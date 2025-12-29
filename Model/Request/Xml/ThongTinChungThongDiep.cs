using System.Xml.Serialization;

namespace Model.Request.Xml
{
    public class ThongTinChungThongDiep
    {
        [XmlElement(ElementName = "PBan")]
        public string phien_ban { get; set; }

        /// <summary>
        /// Là MST của nơi gửi và nơi nhận thông điệp không bao gồm dấu “-” , TCGP có MST là 0107001731-001, TCTN có MST là 0107001732. Khi TCGP gửi dữ liệu đến TCTN thì MNGui là: 0107001731001, MNNhan là: 0107001732.
        /// </summary>
        [XmlElement(ElementName = "MNGui")]
        public string ma_noi_gui { get; set; }

        /// <summary>
        /// Là MST của nơi gửi và nơi nhận thông điệp không bao gồm dấu “-” , TCGP có MST là 0107001731-001, TCTN có MST là 0107001732.
        /// </summary>
        [XmlElement(ElementName = "MNNhan")]
        public string ma_noi_nhan { get; set; }

        /// <summary>
        /// Gửi yêu cầu cấp mã	= 200; Gửi hóa đơn không mã = 203
        /// </summary>
        [XmlElement(ElementName = "MLTDiep")]
        public string thong_diep { get; set; }

        /// <summary>
        /// MNGui + Chuỗi GUID bỏ dấu
        /// </summary>
        [XmlElement(ElementName = "MTDiep")]
        public string ma_noi_gui_uuid { get; set; }


        /// <summary>
        /// Mã thông điệp tham chiếu, Bắt buộc (Trừ trường hợp hệ thống của bên nhận không bóc  tách và lấy được thông điệp gốc)
        /// </summary>
        [XmlElement(ElementName = "MTDTChieu")]
        public string ma_thong_diep_tham_chieu { get; set; }


        [XmlElement(ElementName = "MST")]
        public string mst { get; set; }


        [XmlElement(ElementName = "SLuong")]
        public decimal so_luong { get; set; }
    }
}