import { axiosClient } from "../api/axiosClient";
import { parseSoapResponse } from "./common";

export const TO_KHAI_CT_CHAP_NHAN = "CQT đã chấp nhận";

export const layDanhSachToKhaiChungTu = async (
  madonvi: string | undefined,
) => {
  const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <Laydanhsachtokhaict xmlns="http://tempuri.org/">
      <madonvi>${madonvi}</madonvi>
    </Laydanhsachtokhaict>
  </soap12:Body>
</soap12:Envelope>`;

  const res: string = await axiosClient.post(
    process.env.REACT_APP_API_CHUNG_TU as string,
    soap,
    {
      headers: {
        "Content-Type": "text/xml; charset=utf-8",
      },
    },
  );

  return parseSoapResponse(res);
};

export const hasToKhaiChungTuChapNhan = (danhSach: any[]) => {
  return danhSach?.some((x) => x.ketquaphanhoi === TO_KHAI_CT_CHAP_NHAN);
};

export const checkChungTuThayTheDieuChinh = async (
  madonvi: string | undefined,
  payload: {
    mau_so: string;
    ky_hieu: string;
    so_chung_tu_goc: string;
    loai_chung_tu: number;
  },
) => {
  const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <CheckChungTuThayTheDieuChinh xmlns="http://tempuri.org/">
      <madonvi>${madonvi}</madonvi>
      <mau_so>${payload?.mau_so}</mau_so>
      <kyhieu>${payload?.ky_hieu}</kyhieu>
      <sochungtu>${payload?.so_chung_tu_goc}</sochungtu>
      <TinhchatCT>${payload?.loai_chung_tu}</TinhchatCT>
    </CheckChungTuThayTheDieuChinh>
  </soap12:Body>
</soap12:Envelope>`;

  const res: string = await axiosClient.post(
    process.env.REACT_APP_API_CHUNG_TU as string,
    soap,
    {
      headers: {
        "Content-Type": "text/xml; charset=utf-8",
      },
    },
  );

  return parseSoapResponse(res);
};

const isChungTuDaBiThayTheHoacDieuChinh = (row: any): boolean => {
  const ghiChu = row?.GhichuCT?.toString() ?? "";
  return (
    ghiChu.includes("Bị thay thế bởi") || ghiChu.includes("Bị điều chỉnh bởi")
  );
};

export const validateDieuChinhChungTu = (row: any): string | null => {
  if (Number(row?.TrangthaicuoiCT) !== 1) {
    return "Chứng từ gốc đã bị xóa bỏ, không thể lập điều chỉnh";
  }
  const phanbiet = Number(row?.PhanbietCTValue ?? row?.PhanbietCT);
  if (phanbiet !== 0) {
    if (phanbiet === 2) {
      return "Không thể lập điều chỉnh cho chứng từ điều chỉnh";
    }
    if (phanbiet === 1) {
      return "Chứng từ gốc là chứng từ thay thế, không thể lập điều chỉnh";
    }
    return "Chỉ được lập điều chỉnh cho chứng từ gốc";
  }
  if (isChungTuDaBiThayTheHoacDieuChinh(row)) {
    return "Chứng từ gốc đã bị thay thế hoặc điều chỉnh, không thể lập tiếp chứng từ điều chỉnh";
  }
  if (Number(row?.TinhtrangCT) !== 33) {
    return "Chứng từ gốc chưa được gửi lên CQT";
  }
  return null;
};

export const validateThayTheChungTu = (row: any): string | null => {
  if (Number(row?.TrangthaicuoiCT) !== 1) {
    return "Chứng từ gốc đã bị xóa bỏ, không thể lập thay thế";
  }
  const phanbiet = Number(row?.PhanbietCTValue ?? row?.PhanbietCT);
  if (phanbiet !== 0 && phanbiet !== 1) {
    if (phanbiet === 2) {
      return "Chứng từ gốc là chứng từ điều chỉnh, không thể lập thay thế";
    }
    return "Chỉ được lập thay thế cho chứng từ gốc hoặc chứng từ thay thế";
  }
  if (isChungTuDaBiThayTheHoacDieuChinh(row)) {
    return "Chứng từ gốc đã bị thay thế hoặc điều chỉnh, không thể lập tiếp chứng từ thay thế";
  }
  if (Number(row?.TinhtrangCT) !== 33) {
    return "Chứng từ gốc chưa được gửi lên CQT";
  }
  return null;
};
