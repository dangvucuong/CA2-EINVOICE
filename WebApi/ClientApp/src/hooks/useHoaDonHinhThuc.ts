import { useMemo } from "react";
import { eHoaDonHinhThuc } from "../models/commons/eHoaDonHinhThuc";

export const hoaDonHinhThucs = [
  {
    id: 0,
    name: "Hóa đơn gốc",
    name_en: "Hóa đơn gốc",
    color: "#d0d7de",
  },
  {
    id: eHoaDonHinhThuc.HOA_DON_GOC,
    name: "Hóa đơn gốc",
    name_en: "Hóa đơn gốc",
    color: "#d0d7de",
  },
  {
    id: eHoaDonHinhThuc.HOA_DON_THAY_THE,
    name: "Hóa đơn thay thế",
    name_en: "Hóa đơn thay thế",
    color: "#ffd78e",
  },
  {
    id: eHoaDonHinhThuc.HOA_DON_DIEU_CHINH,
    name: "Hóa đơn điều chỉnh",
    name_en: "Hóa đơn điều chỉnh",
    color: "#a4f287",
  },
  {
    id: eHoaDonHinhThuc.HOA_DON_BI_DIEU_CHINH,
    name: "Hóa đơn bị điều chỉnh",
    name_en: "Hóa đơn bị điều chỉnh",
    color: "#ffd78e",
  },
  {
    id: eHoaDonHinhThuc.HOA_DON_DA_HUY_NOI_BO,
    name: "Hóa đơn đã hủy nội bộ",
    name_en: "Hóa đơn đã hủy nội bộ",
    color: "#ce2c85",
  },
  {
    id: eHoaDonHinhThuc.HOA_DON_BI_THAY_THE,
    name: "Hóa đơn đã bị thay thế",
    name_en: "Hóa đơn đã bị thay thế",
    color: "#ffd78e",
  },
  {
    id: eHoaDonHinhThuc.HOA_DON_DA_THONG_BAO_GIAI_TRINH,
    name: "Hóa đơn đã thông báo giải trình",
    name_en: "Hóa đơn đã thông báo giải trình",
    color: "#a4f287",
  },
  {
    id: eHoaDonHinhThuc.HOA_DONT_DA_TBSS_HUY,
    name: "Đã gửi TBSS Hủy",
    name_en: "Đã gửi TBSS Hủy",
    color: "#ce2c85",
  },

  {
    id: eHoaDonHinhThuc.DA_GUI_TBSS_THAY_THE,
    name: "Đã gửi TBSS Thay thế",
    name_en: "Đã gửi TBSS Thay thế",
    color: "#ffd78e",
  },
];

export const useHoaDonHinhThucs = () => {
  return {
    hoaDonHinhThucs,
  };
};

export const useHoaDonHinhThuc = (id: number) => {
  const { hoaDonHinhThucs } = useHoaDonHinhThucs();
  const hoaDonHinhThuc = useMemo(() => {
    return hoaDonHinhThucs.find((x) => x.id === id);
  }, [hoaDonHinhThucs, id]);
  return {
    hoaDonHinhThuc,
  };
};
