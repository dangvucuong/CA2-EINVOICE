using System.Xml.Serialization;

namespace Model.Request.Xml
{
    [XmlRoot(ElementName = "NMua")]
    public class NguoiMua
    {

        [XmlElement(ElementName = "HVTNMHang")]
        public string ho_ten_nguoi_mua_hang { get; set; }

        [XmlElement(ElementName = "Ten")]
        public string ten_don_vi { get; set; }

        [XmlElement(ElementName = "MST")]
        public string mst { get; set; }

        [XmlElement(ElementName = "DChi")]
        public string dia_chi { get; set; }


        [XmlElement(ElementName = "DCTDTu")]
        public string email { get; set; }

        [XmlElement(ElementName = "STKNHang")]
        public string stk { get; set; }

        [XmlElement(ElementName = "TNHang")]
        public string ngan_hang { get; set; }

        [XmlElement(ElementName = "SDThoai")]
        public string dien_thoai { get; set; }

        [XmlElement(ElementName = "Fax")]
        public string fax { get; set; }

        [XmlElement(ElementName = "Website")]
        public string website { get; set; }

        [XmlElement(ElementName = "CCCDan")]
        public string cccd { get; set; }

        [XmlElement(ElementName = "SHChieu")]
        public string so_ho_chieu { get; set; }


        [XmlElement(ElementName = "MDVQHNSach")]
        public string ma_dv_ngan_sach { get; set; }

        [XmlElement(ElementName = "TTKhac")]
        public ThongTinKhac thong_tin_khac { get; set; }

        [XmlElement(ElementName = "HVTNNHang")]
        public string HVTNNHang { get; set; }


        //hoa don ban tai san cong
        [XmlElement(ElementName = "DDVCHDen")]
        public string DiadiemVCHDen { get; set; }

        [XmlElement(ElementName = "TGVCHDTu")]
        public string TGianVCTu { get; set; }

        [XmlElement(ElementName = "TGVCHDDen")]
        public string TGianVCDen { get; set; }
    }
}
