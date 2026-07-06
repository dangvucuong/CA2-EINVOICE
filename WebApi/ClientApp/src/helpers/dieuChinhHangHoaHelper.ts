import Big from "big.js";
import { IHoaDonHangHoa } from "../models/responses/hoa-don/IHoaDonHangHoa";
import { eTinhChatHangHoa } from "../models/commons/eTinhChatHangHoa";

const isNumber = (value: any) => !isNaN(value) && typeof value === "number";

const getThanhTienFallback = (
  so_luong: any,
  don_gia: any,
  ty_le_chiet_khau: any,
  tienTe?: string,
): number => {
  const _soLuong = isNumber(so_luong) ? parseFloat(so_luong) : 0;
  const _donGia = isNumber(don_gia) ? parseFloat(don_gia) : 0;
  const _tyLeChietKhau = isNumber(ty_le_chiet_khau)
    ? parseFloat(ty_le_chiet_khau)
    : 0;
  const chietKhauFactor = new Big(100).minus(_tyLeChietKhau).div(100);
  const value = new Big(_soLuong).times(_donGia).times(chietKhauFactor);
  const isVnd = (tienTe ?? "VND") === "VND";
  return isVnd
    ? parseFloat(value.round(2, Big.roundHalfUp).toString())
    : parseFloat(value.toString());
};

const isHangHoaTinhTien = (hangHoa: IHoaDonHangHoa) =>
  hangHoa.hang_hoa_tinh_chat_id === eTinhChatHangHoa.HANG_HOA_DICH_VU ||
  hangHoa.hang_hoa_tinh_chat_id === 5;

export const findHangHoaGoc = (
  hangHoasGoc: IHoaDonHangHoa[],
  ma_hang: string,
): IHoaDonHangHoa | undefined => {
  if (!ma_hang) return undefined;
  return hangHoasGoc.find(
    (x) => x.ma_hang === ma_hang && isHangHoaTinhTien(x),
  );
};

export const calcThanhTienDieuChinhLine = (
  line: IHoaDonHangHoa,
  allLines: IHoaDonHangHoa[],
  hangHoasGoc: IHoaDonHangHoa[],
  tienTe?: string,
): number => {
  const goc = findHangHoaGoc(hangHoasGoc, line.ma_hang ?? "");
  if (!goc || !isHangHoaTinhTien(line)) {
    return getThanhTienFallback(
      line.so_luong,
      line.don_gia,
      line.ty_le_chiet_khau,
      tienTe,
    );
  }

  const slInput = Number(line.so_luong ?? 0);
  const dgInput = Number(line.don_gia ?? 0);
  const soLuongGoc = Number(goc.so_luong ?? 0);
  const donGiaGoc = Number(goc.don_gia ?? 0);

  let thanhTienBase = 0;
  if (slInput !== 0 && dgInput === 0) {
    thanhTienBase = slInput * donGiaGoc;
  } else if (slInput === 0 && dgInput !== 0) {
    const tongSlDieuChinh = allLines
      .filter((x) => x.ma_hang === line.ma_hang)
      .reduce((sum, x) => sum + Number(x.so_luong ?? 0), 0);
    thanhTienBase = (soLuongGoc + tongSlDieuChinh) * dgInput;
  } else if (slInput !== 0 && dgInput !== 0) {
    thanhTienBase = slInput * donGiaGoc;
  }

  const tyLeChietKhau = Number(line.ty_le_chiet_khau ?? 0);
  const chietKhauFactor = new Big(100).minus(tyLeChietKhau).div(100);
  const value = new Big(thanhTienBase).times(chietKhauFactor);
  const isVnd = (tienTe ?? "VND") === "VND";
  return isVnd
    ? parseFloat(value.round(2, Big.roundHalfUp).toString())
    : parseFloat(value.toString());
};

export const recalcHangHoasDieuChinh = (
  hangHoas: IHoaDonHangHoa[],
  hangHoasGoc: IHoaDonHangHoa[],
  tienTe?: string,
): IHoaDonHangHoa[] => {
  return hangHoas.map((line) => ({
    ...line,
    thanh_tien: calcThanhTienDieuChinhLine(
      line,
      hangHoas,
      hangHoasGoc,
      tienTe,
    ),
  }));
};
