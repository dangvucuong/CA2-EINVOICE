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
