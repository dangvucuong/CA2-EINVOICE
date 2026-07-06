export const MAU_SO_CHUNG_TU_TNCN = "03/TNCN";

export const getChungTuMadonvi = (user?: {
  donvi_ma_dv?: string;
  donvi?: { ma_dv?: string };
}) => user?.donvi_ma_dv?.trim() || user?.donvi?.ma_dv?.trim() || "";

export const resolveChungTuMauSo = (mauSo?: string) =>
  mauSo?.trim() || MAU_SO_CHUNG_TU_TNCN;
