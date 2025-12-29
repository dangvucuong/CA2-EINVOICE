using System.Xml.Serialization;

namespace Model.Request.Xml
{
    [XmlRoot(ElementName = "TTChung")]
    public class ThongTinChung
    {

        [XmlElement(ElementName = "PBan")]
        public string phien_ban { get; set; }

        [XmlElement(ElementName = "THDon")]
        public string ten_hoa_don { get; set; }

        [XmlElement(ElementName = "KHMSHDon")]
        public string ky_hieu_mau_so_hoa_don { get; set; }

        [XmlElement(ElementName = "KHHDon")]
        public string ky_hieu_hoa_don { get; set; }

        [XmlElement(ElementName = "SHDon")]
        public string so_hoa_don { get; set; }

        [XmlElement(ElementName = "NLap")]
        public string ngay_lap { get; set; }

        [XmlElement(ElementName = "DVTTe")]
        public string don_vi_tien_te { get; set; }
        [XmlElement(ElementName = "TGia")]
        public string ty_gia { get; set; }

        [XmlElement(ElementName = "HTTToan")]
        public string hinh_thuc_thanh_toan { get; set; }

        [XmlElement(ElementName = "MSTTCGP")]
        public string ma_so_thue_co_quan_quan_ly { get; set; }

        [XmlElement(ElementName = "TTKhac")]
        public ThongTinKhac thong_tin_khac { get; set; }

        [XmlElement(ElementName = "TTHDLQuan")]
        public ThongTinLienQuan thong_tin_lien_quan { get; set; }

        /// <summary>
        /// •	Đối với hóa đơn bán hàng: mẫu số 2. Bắt buộc phải gen thêm thẻ <HDDCKPTQuan> giá trị =0
        /// </summary>
        [XmlElement(ElementName = "HDDCKPTQuan")]
        public string HDDCKPTQuan { get; set; }
        
        public ThongTinChung()
        {

        }
    }

}
