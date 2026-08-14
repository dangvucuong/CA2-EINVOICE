/** Hóa đơn thương mại: hình thức mặc định không mã CQT, ký tự thứ 4 của ký hiệu là X */
export const isHoaDonThuongMai = (loaiHoaDonCT?: {
  code?: string;
  name?: string;
  name_en?: string;
}): boolean => {
  if (!loaiHoaDonCT) {
    return false;
  }
  const code = (loaiHoaDonCT.code ?? "").trim().toUpperCase();
  if (code === "X") {
    return true;
  }
  const text = `${loaiHoaDonCT.name ?? ""} ${loaiHoaDonCT.name_en ?? ""}`
    .toLowerCase()
    .normalize("NFD")
    .replace(/[\u0300-\u036f]/g, "");
  return text.includes("thuong mai");
};

/** Ký hiệu hóa đơn MTT: ký tự thứ 4 (index 3) là M, ví dụ 1C26MNT */
export const isKyHieuMayTinhTien = (ky_hieu?: string): boolean => {
  if (!ky_hieu || ky_hieu.length < 4) {
    return false;
  }
  return ky_hieu.substring(3, 4).toUpperCase() === "M";
};

/** Đăng ký phát hành thuộc nhóm MTT (theo ký hiệu hoặc hinh_thuc_code) */
export const isDangKyPhatHanhMtt = (item: {
  ky_hieu?: string;
  hinh_thuc_code?: string;
}): boolean => {
  if (item.hinh_thuc_code?.toUpperCase() === "M") {
    return true;
  }
  return isKyHieuMayTinhTien(item.ky_hieu);
};
