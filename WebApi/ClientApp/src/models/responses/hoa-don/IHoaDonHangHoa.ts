export interface IHoaDonHangHoa {
  id: number;
  hoa_don_id: number;
  hang_hoa_tinh_chat_id: number;
  stt: number;
  ma_hang: string;
  ten_hang: string;
  dvt: string;
  so_luong: number;
  don_gia: number;
  ty_le_chiet_khau: number;
  tien_chiet_khau: number;
  thanh_tien: number;
  thue_vat?: string;
  hoa_don_hang_hoa_trangthai_id?: number;
  hang_hoa_dac_trung_json?: string;
}

interface ValidationResult {
  isValid: boolean;
  errors: {
    field: string;
    message: string;
  }[];
}

export const IsHoaDonHangHoaValid = (hangHoa?: IHoaDonHangHoa) => {
  // if (!hangHoa?.ma_hang) return false;
  if (!hangHoa?.ten_hang) return false;
  if (!hangHoa?.hang_hoa_tinh_chat_id) return false;
  // if ((hangHoa?.so_luong ?? 0) < 0) return false;
  // if ((hangHoa?.don_gia ?? 0) < 0) return false;
  // if ((hangHoa?.ty_le_chiet_khau ?? 0) < 0) return false;
  // if ((hangHoa?.hang_hoa_tinh_chat_id ?? 0) < 0) return false;
  // if ((hangHoa?.thanh_tien ?? 0) < 0) return false;
  return true;
};

export const IsHoaDonHangHoaValidIncludeField = (
  hangHoa?: IHoaDonHangHoa
): ValidationResult => {
  const errors: { field: string; message: string }[] = [];

  // Validate tên hàng
  if (!hangHoa?.ten_hang || hangHoa.ten_hang.trim() === "") {
    errors.push({
      field: "ten_hang",
      message: "Tên hàng không được để trống",
    });
  }

  // Validate tính chất hàng hóa
  if (!hangHoa?.hang_hoa_tinh_chat_id || hangHoa.hang_hoa_tinh_chat_id <= 0) {
    errors.push({
      field: "hang_hoa_tinh_chat_id",
      message: "Vui lòng chọn tính chất hàng hóa",
    });
  }

  return {
    isValid: errors.length === 0,
    errors,
  };
};
