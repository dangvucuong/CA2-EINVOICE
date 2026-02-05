using Common;
using Model.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Table
{
  public class hoa_don : modify_infor
  {
    private string _giam_thue_ghi_chu;
    [SwaggerSchema(Description = "id hóa đơn, trường hợp thêm mới để =0")]

    public int id { get; set; }
    [SwaggerSchema(Description = "Mã số thuế của đơn vị")]

    public string donvi_ma_dv { get; set; }
    [SwaggerSchema(Description = "Phiên bản mặc định 2.0.1")]

    public string? phien_ban { get; set; }
    [SwaggerSchema(Description = "Tên hóa đơn")]

    public string ten_hoa_don { get; set; }
    [SwaggerSchema(Description = "Mẫu số hóa đơn")]

    public string hoa_don_dang_ky_phat_hanh_mau_so { get; set; }
    [SwaggerSchema(Description = "Ký hiệu hóa đơn")]

    public string hoa_don_dang_ky_phat_hanh_ky_hieu { get; set; }
    [SwaggerSchema(Description = "Loại hóa đơn. Xem danh sách loại hóa đơn tại [GET] /api/loai-hoa-don-ct")]
    public int loai_hoa_don_ct_id { get; set; }
    [SwaggerSchema(Description = "Số hóa đơn (hệ thống sinh tự động)")]
    public string? so_hoa_don { get; set; }
    [SwaggerSchema(Description = "Số hóa đơn máy tính tiền (hệ thống sinh tự động)")]
    public string? ma_so_hoa_don_mtt { get; set; }
    [SwaggerSchema(Description = "Số hóa đơn (hệ thống sinh tự động)")]
    public int? ma_so_hoa_don { get; set; }
    [SwaggerSchema(Description = "Ngày hóa đơn")]
    public DateTime ngay_hoa_don { get; set; }
    [SwaggerSchema(Description = "Loại tiền")]
    public string loai_tien { get; set; }
    [SwaggerSchema(Description = "Tỷ giá")]
    public decimal ty_gia { get; set; }
    // public string chinhanh_code { get; set; }
    [SwaggerSchema(Description = "MST người bán")]
    public string? nguoi_ban_mst { get; set; }
    [SwaggerSchema(Description = "Tên đơn vị bán hàng")]
    public string? nguoi_ban_ten_donvi { get; set; }
    [SwaggerSchema(Description = "Địa chỉ đơn vị bán hàng")]
    public string? nguoi_ban_dia_chi { get; set; }
    [SwaggerSchema(Description = "Số tài khoản người bán")]
    public string? nguoi_ban_stk { get; set; }
    [SwaggerSchema(Description = "Ngân hàng người bán")]
    public string? nguoi_ban_ngan_hang { get; set; }
    [SwaggerSchema(Description = "Điện thoại người bán")]
    public string? nguoi_ban_dien_thoai { get; set; }
    [SwaggerSchema(Description = "Fax người bán")]
    public string? nguoi_ban_fax { get; set; }
    [SwaggerSchema(Description = "Email người bán")]
    public string? nguoi_ban_email { get; set; }
    [SwaggerSchema(Description = "Website người bán")]
    public string? nguoi_ban_website { get; set; }
    [SwaggerSchema(Description = "MST người mua")]
    public string nguoi_mua_mst { get; set; }
    [SwaggerSchema(Description = "Tên đơn vị người mua")]
    public string nguoi_mua_ten_donvi { get; set; }
    [SwaggerSchema(Description = "Tên người mua")]
    public string nguoi_mua_ten { get; set; }
    [SwaggerSchema(Description = "Địa chỉ người mua")]
    public string? nguoi_mua_dia_chi { get; set; }
    [SwaggerSchema(Description = "Số tài khoản người mua")]
    public string? nguoi_mua_stk { get; set; }
    [SwaggerSchema(Description = "Ngân hàng người mua")]
    public string? nguoi_mua_ngan_hang { get; set; }
    [SwaggerSchema(Description = "Điện thoại người mua")]
    public string? nguoi_mua_dien_thoai { get; set; }
    [SwaggerSchema(Description = "Fax người mua")]
    public string? nguoi_mua_fax { get; set; }
    [SwaggerSchema(Description = "Email người mua")]
    public string? nguoi_mua_email { get; set; }
    [SwaggerSchema(Description = "Website người mua")]
    public string? nguoi_mua_website { get; set; }
    [SwaggerSchema(Description = "CCCD người mua")]
    public string? nguoi_mua_cccd { get; set; }
    [SwaggerSchema(Description = "Mã đơn vị quan hệ ngân sách")]
    //MDVQHNSach
    public string? ma_dv_ngan_sach { get; set; }
    [SwaggerSchema(Description = "Hình thức thanh toán: Tiền mặt/ Chuyển khoản, ...")]
    public string hinh_thuc_tt { get; set; }
    [SwaggerSchema(Description = "Khi thêm mới để =0.  NHAP = 1,DA_PHAT_HANH = 2,DA_HUY = 3,CHUA_CO_KET_QUA_PHAN_HOI = 4,DA_GUI_LEN_CQT_PHAN_HOI_KY_THUAT = 5,DA_GUI_LEN_CQT_CHUA_PHAN_HOI_KIEM_TRA_DU_LIEU = 6,KHONG_HOP_LE = 7,LOI_THONG_DIEP = 8,CHUA_GUI_CQT = 9. Ví dụ [1,2,3]")]
    public int hoa_don_trang_thai_id { get; set; }
    [SwaggerSchema(Description = "Tổng tiền trước thuế")]

    public decimal tong_tien_truong_thue { get; set; }
    [SwaggerSchema(Description = "Tổng tiền thuế")]

    public decimal tong_tien_thue { get; set; }
    [SwaggerSchema(Description = "Tổng tiền phí")]

    public decimal tong_tien_phi { get; set; }
    [SwaggerSchema(Description = "Tổng tiền thanh toán")]

    public decimal tong_tien_thanh_toan { get; set; }
    [SwaggerSchema(Description = "Tổng tiền chiết khấu")]

    public decimal tong_tien_chiet_khau { get; set; }
    [SwaggerSchema(Description = "Tổng tiền chữ")]

    public string? tong_tien_chu { get; set; }
    [SwaggerSchema(Description = "Mã tra cứu (hệ thống sinh tự động)")]

    public string? ma_tra_cuu { get; set; }
    [SwaggerSchema(Description = "QR code (sinh tự động khi tạo mới)")]

    public string? qr_code { get; set; }
    [SwaggerSchema(Description = "Người tạo")]
    public string? nguoi_tao { get; set; }
    [SwaggerSchema(Description = "Ngày tạo")]

    public DateTime? ngay_tao { get; set; }
    [SwaggerSchema(Description = "HOA_DON_GOC = 1,HOA_DON_THAY_THE = 2,HOA_DON_DIEU_CHINH = 3,HOA_DON_BI_DIEU_CHINH = 4,HOA_DON_DA_HUY_NOI_BO = 5,HOA_DON_BI_THAY_THE = 6,HOA_DON_DA_THONG_BAO_GIAI_TRINH = 7,LOI_THONG_DIEP = 8,DA_GUI_TBSS_HUY = 9,")]
    public int hoa_don_hinh_thuc_id { get; set; }
    [SwaggerSchema(Description = "Nghị định: 123 hoặc 51")]
    public int hoa_don_nghi_dinh_id { get; set; }

    [SwaggerSchema(Description = "Hóa đơn gốc id (trường hợp thay thế, điều chỉnh)")]
    public int hoa_don_id_goc { get; set; }
    [SwaggerSchema(Description = "Hóa đơn gốc: mẫu số")]
    public string? hoa_don_dang_ky_phat_hanh_mau_so_goc { get; set; }
    [SwaggerSchema(Description = "Hóa đơn gốc: ký hiệu")]
    public string? hoa_don_dang_ky_phat_hanh_ky_hieu_goc { get; set; }
    [SwaggerSchema(Description = "Hóa đơn gốc: số hóa đơn")]
    public string? ma_so_hoa_don_goc { get; set; }
    [SwaggerSchema(Description = "Hóa đơn gốc: ngày hóa đơn")]
    public DateTime? ngay_hoa_don_goc { get; set; }
    [SwaggerSchema(Description = "Hóa đơn gốc Nghị định: 123 hoặc 51")]
    public int hoa_don_nghi_dinh_id_goc { get; set; }

    [SwaggerSchema(Description = "Lý do điều chỉnh (trường hợp hóa đơn điều chỉnh). Điều chỉnh tăng=1, Điều chỉnh giảm=2, Điều chỉnh thông tin =3, Điều chỉnh thuế =20")]
    public int hoa_don_ly_do_dieu_chinh_id { get; set; }

    //phiếu xuất kho vận chuyển
    [SwaggerSchema(Description = "Phiếu xuất kho: Lệnh điều động nội bộ")]
    public string? xuat_kho_vc_lenh_dieu_dong_noi_bo { get; set; }
    public string? xuat_kho_dia_chi { get; set; }


    //phiếu xuất kho đại lý
    [SwaggerSchema(Description = "Phiếu xuất kho: số hợp đồng kinh tế")]
    public string? xuat_kho_dl_hop_dong_kinh_te_so { get; set; }
    [SwaggerSchema(Description = "Phiếu xuất kho: ngày hợp đồng")]
    public DateTime? xuat_kho_dl_hop_dong_ngay { get; set; }

    // phiếu xuất kho chung vận chuyển + đại lý
    [SwaggerSchema(Description = "Phiếu xuất kho: số hợp đồng")]
    public string? xuat_kho_hop_dong_so { get; set; }
    [SwaggerSchema(Description = "Phiếu xuất kho: người xuất hàng")]
    public string? xuat_kho_nguoi_xuat_hang { get; set; }
    [SwaggerSchema(Description = "Phiếu xuất kho: người vận chuyển")]
    public string? xuat_kho_nguoi_van_chuyen { get; set; }
    [SwaggerSchema(Description = "Phiếu xuất kho: phương tiện vận chuyển")]
    public string? xuat_kho_phuong_tien_van_chuyen { get; set; }

    [SwaggerSchema(Description = "Có mã:C, Không mã: K, Máy tính tiền: M")]
    /// <summary>
    /// C, K, M
    /// </summary>

    public string? hoa_don_hinh_thuc_code { get; set; }
    [SwaggerSchema(Description = "Đã ký số thành công")]
    public bool? is_ky_so_succes { get; set; }

    [SwaggerSchema(Description = "Kết quả phát hành")]
    /// <summary>
    /// Nội dung kết quả phát hành
    /// </summary>
    /// 
    public string? ket_qua_phat_hanh { get; set; }
    [SwaggerSchema(Description = "Khóa phiên phát hành")]
    public string? phat_hanh_uuid { get; set; }
    public string? phat_hanh_ma_ketqua_cqt { get; set; }
    [SwaggerSchema(Description = "Người thao tác phát hành")]
    public int user_id_phathanh { get; set; }

    [SwaggerSchema(Description = "Đã bị thay thế, điều chỉnh bởi hóa đơn")]
    public string? hoa_don_ids_thaythe_dieuchinh { get; set; }

    [SwaggerSchema(Description = "Hóa đơn nước: Mã bill")]
    public string? tt_nuoc_ma_bill { get; set; }
    [SwaggerSchema(Description = "Hóa đơn nước: Ngày đọc tháng trước")]
    public string? tt_nuoc_ngay_doc_thang_truoc { get; set; }
    [SwaggerSchema(Description = "Hóa đơn nước: Ngày đọc tháng này")]
    public string? tt_nuoc_ngay_doc_thang_nay { get; set; }
    [SwaggerSchema(Description = "Hóa đơn nước: Số")]
    public string? tt_nuoc_so_cuong { get; set; }
    [SwaggerSchema(Description = "Hóa đơn nước: Mã người mua")]
    public string? tt_nuoc_ma_nguoi_mua { get; set; }
    [SwaggerSchema(Description = "Hóa đơn nước: Chỉ số tháng trước")]
    public string? tt_nuoc_chi_so_thang_truoc { get; set; }
    [SwaggerSchema(Description = "Hóa đơn nước: Chỉ số tháng ngày")]
    public string? tt_nuoc_chi_so_thang_ngay { get; set; }
    public string? tt_nuoc_ma_nuoc { get; set; }
    [SwaggerSchema(Description = "Hóa đơn nước: Tổng số ngày")]
    public string? tt_nuoc_tong_so_ngay { get; set; }
    [SwaggerSchema(Description = "Hóa đơn nước: Tổng tiêu thụ")]
    public string? tt_nuoc_tong_tieu_thu { get; set; }
    [SwaggerSchema(Description = "Hóa đơn nước: Số hộ")]
    public string? tt_nuoc_so_ho { get; set; }
    [SwaggerSchema(Description = "Hóa đơn nước: Serial đồng hồ")]
    public string? tt_nuoc_serial_dong_ho { get; set; }

    [SwaggerSchema(Description = "Mã đại lý")]
    public string? ma_dai_ly { get; set; }
    [SwaggerSchema(Description = "Tên đại lý")]
    public string? ten_dai_ly { get; set; }

    [SwaggerSchema(Description = "Invoice Id từ hệ thống tích hợp, invoice_id dùng để check trùng khi lập hóa đơn mới")]

    public string? invoice_id { get; set; }
    public string? vender_id { get; set; }
    [SwaggerSchema(Description = "Tăng giảm trong phạm vi 5 đồng")]
    public int so_tien_tang_giam { get; set; }
    [SwaggerSchema(Description = "Tăng giảm trong phạm vi 5 đồng")]
    public int so_tien_tang_giam_tien_hang { get; set; }
    [SwaggerSchema(Description = "Tăng giảm trong phạm vi 5 đồng")]
    public int so_tien_tang_giam_tien_thue { get; set; }

    [SwaggerSchema(Description = "Phần trăm giảm thuế hóa đơn bán hàng theo nghị định (20%)")]
    public int giam_thue_phan_tram { get; set; }
    [SwaggerSchema(Description = "Tỷ lệ giảm - User tự nhập (1%, 2%,..)")]
    public int giam_thue_ty_le { get; set; }
    [SwaggerSchema(Description = "Số tiền giảm thuế")]
    public decimal giam_thue_thanh_tien { get; set; }

    [JsonConverter(typeof(ThongTinKhacJsonConverter))]
    [JsonProperty("thong_tin_khac")]
    public string? thong_tin_khac_json { get; set; }

    public string? so_ho_chieu { get; set; }
    public string? ly_do_dieu_chinh { get; set; }

    // [SwaggerSchema(Description = "Ghi chú nội dung giảm thuế")]
    public string? giam_thue_ghi_chu
    {
      get
      {
        if (giam_thue_ty_le <= 0)
        {
          return string.Empty;
        }
        return $"Đã giảm {giam_thue_thanh_tien.ToString("N0")}đ tương ứng {giam_thue_phan_tram}% mức tỷ lệ {giam_thue_ty_le}% để tính thuế giá trị gia tăng theo Nghị quyết số 204/2025/QH15 ngày 17 tháng 06 năm 2025";
      }
      set
      {
        _giam_thue_ghi_chu = value;
      }
    }

    public bool IsHoaDonDieuChinhThayThe()
    {
      if (this.hoa_don_dang_ky_phat_hanh_ky_hieu_goc.ConvertToString() != ""
      && this.hoa_don_dang_ky_phat_hanh_mau_so_goc.ConvertToString() != ""
      && this.ma_so_hoa_don_goc.ConvertToString() != ""
      )
      {
        return true;
      }
      return false;
    }


    public string CreateQRCode()
    {
      var MSTNguoiban = this.nguoi_ban_mst.ConvertToString();
      var KHMSHDon = this.hoa_don_dang_ky_phat_hanh_mau_so;
      var KHHDon = this.hoa_don_dang_ky_phat_hanh_ky_hieu;
      var SHDon = this.ma_so_hoa_don.ToString();
      var Tongtienthanhtoan = this.loai_tien == "VND"
      ? (this.tong_tien_truong_thue + this.tong_tien_thue).ConvertToDouble(0).ToString()
      : this.tong_tien_thanh_toan.ToString();

      string kq = "";
      string chuoidacta = "000201";
      string code = System.Guid.NewGuid().ToString().Replace("-", "");
      code = code.ToUpper();
      string chuoi_GUI = "0032" + code;
      MSTNguoiban = MSTNguoiban.Replace("-", "");
      string chuoiMST = "01" + MSTNguoiban.Length.ToString() + MSTNguoiban;
      string chuoi_KHMSHDon = "0201" + KHMSHDon;
      string chuoi_KHHDon = "0306" + KHHDon;
      string chuoi_SHDon = "04" + SHDon.Length.ToString() + SHDon;
      string Chuoi_NLap = "";
      if (this.ngay_tao.HasValue)
        Chuoi_NLap = "0508" + this.ngay_tao.Value.ToString("yyyyMMdd");
      string chuoi_Tongtien = "06" + Tongtienthanhtoan.Length.ToString() + Tongtienthanhtoan;
      string chuoi_CRC = "6304" + "383C";
      string chuoithongtinHD = chuoi_GUI + chuoiMST + chuoi_KHMSHDon + chuoi_KHHDon + chuoi_SHDon + Chuoi_NLap + chuoi_Tongtien;
      chuoithongtinHD = "99" + chuoithongtinHD.Length.ToString() + chuoithongtinHD;
      kq = chuoidacta + chuoithongtinHD + chuoi_CRC;
      return kq;
    }


  }
  public class ThongTinKhacJsonConverter : JsonConverter<string>
  {
    public override string? ReadJson(JsonReader reader, Type objectType, string? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
      JToken token = JToken.Load(reader);
      return token.ToString(Formatting.None); // Chuyển JToken về chuỗi JSON
    }

    public override void WriteJson(JsonWriter writer, string value, JsonSerializer serializer)
    {
      if (string.IsNullOrEmpty(value))
      {
        writer.WriteNull();
        return;
      }

      try
      {
        // Parse chuỗi JSON và ghi ra như đối tượng JSON
        JToken parsedJson = JToken.Parse(value);
        parsedJson.WriteTo(writer);
      }
      catch (JsonReaderException)
      {
        // Nếu chuỗi không phải JSON hợp lệ, ghi ra như chuỗi gốc
        writer.WriteValue(value);
      }
    }
  }
}