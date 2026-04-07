using System.Xml.Serialization;

namespace Model.Request.Xml
{
    [XmlRoot(ElementName = "TTHDLQuan")]
    public class ThongTinLienQuan
    {

        /// <summary>
        /// tính chất hóa đơn 1- thay thế, 2- điều chỉnh
        /// </summary>
        [XmlElement(ElementName = "TCHDon")]
        public string TCHDon { get; set; }

        /// <summary>
        /// loại hóa đơn gốc
        /// </summary>
        [XmlElement(ElementName = "LHDCLQuan")]
        public string LHDCLQuan { get; set; }

        /// <summary>
        /// ký hiệu mẫu số hóa đơn gốc
        /// </summary>
        [XmlElement(ElementName = "KHMSHDCLQuan")]
        public string KHMSHDCLQuan { get; set; }

        /// <summary>
        /// ký hiệu hóa đơn gốc
        /// </summary>
        [XmlElement(ElementName = "KHHDCLQuan")]
        public string KHHDCLQuan { get; set; }

        /// <summary>
        /// số tự tăng hóa đơn gốc
        /// </summary>
        [XmlElement(ElementName = "SHDCLQuan")]
        public string SHDCLQuan { get; set; }

        /// <summary>
        /// ngày lập hóa đơn gốc
        /// </summary>
        [XmlElement(ElementName = "NLHDCLQuan")]
        public string NLHDCLQuan { get; set; }


        public ThongTinLienQuan()
        {

        }
    }

}
