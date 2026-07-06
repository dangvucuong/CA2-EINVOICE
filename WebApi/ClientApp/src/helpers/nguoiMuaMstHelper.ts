export interface INguoiMuaBuyerFields {
  nguoi_mua_mst?: string;
  nguoi_mua_cccd?: string;
  nguoi_mua_ten?: string;
  nguoi_mua_ten_donvi?: string;
  nguoi_mua_dia_chi?: string;
}

export interface INguoiMuaValidationResult {
  isValid: boolean;
  message?: string;
  field?: string;
  normalized?: INguoiMuaBuyerFields;
}

export function isNguoiMuaCccd(value?: string): boolean {
  const v = (value ?? "").trim();
  return /^\d{12}$/.test(v);
}

export function validateAndNormalizeNguoiMuaBuyer(
  data: INguoiMuaBuyerFields,
): INguoiMuaValidationResult {
  const mst = (data.nguoi_mua_mst ?? "").trim();
  const cccd = (data.nguoi_mua_cccd ?? "").trim();
  const ten = (data.nguoi_mua_ten ?? "").trim();
  const tenDonVi = (data.nguoi_mua_ten_donvi ?? "").trim();
  const diaChi = (data.nguoi_mua_dia_chi ?? "").trim();

  if (!mst && !cccd) {
    return {
      isValid: true,
      normalized: {
        ...data,
        nguoi_mua_mst: mst,
        nguoi_mua_cccd: cccd,
      },
    };
  }

  if (mst) {
    if (mst.length > 14) {
      return {
        isValid: false,
        message: "Mã số thuế người mua không được vượt quá 14 ký tự",
        field: "nguoi_mua_mst",
      };
    }

    if (/[^\d-]/.test(mst)) {
      return {
        isValid: false,
        message: "Mã số thuế chỉ được chứa số và dấu gạch ngang (-)",
        field: "nguoi_mua_mst",
      };
    }

    const digits = mst.replace(/-/g, "");
    if (!/^\d+$/.test(digits)) {
      return {
        isValid: false,
        message: "Mã số thuế chỉ được chứa số và dấu gạch ngang (-)",
        field: "nguoi_mua_mst",
      };
    }

    if (digits.length !== 10 && digits.length !== 12 && digits.length !== 13) {
      return {
        isValid: false,
        message: `Mã số thuế người mua không hợp lệ: phải có 10, 12 hoặc 13 chữ số (đang nhập ${digits.length} số)`,
        field: "nguoi_mua_mst",
      };
    }

    if (cccd && cccd !== mst) {
      return {
        isValid: false,
        message: "Số Căn cước công dân không khớp với Mã số thuế người mua",
        field: "nguoi_mua_cccd",
      };
    }

    if (!tenDonVi) {
      return {
        isValid: false,
        message: "Vui lòng nhập Đơn vị mua hàng khi có Mã số thuế",
        field: "nguoi_mua_ten_donvi",
      };
    }

    if (!diaChi) {
      return {
        isValid: false,
        message: "Vui lòng nhập Địa chỉ người mua hàng khi có Mã số thuế",
        field: "nguoi_mua_dia_chi",
      };
    }
  }

  if (cccd && !/^\d{12}$/.test(cccd)) {
    return {
      isValid: false,
      message: "Căn cước công dân người mua phải đúng 12 chữ số",
      field: "nguoi_mua_cccd",
    };
  }

  if (cccd && !mst) {
    if (!diaChi) {
      return {
        isValid: false,
        message: "Vui lòng nhập Địa chỉ người mua khi sử dụng CCCD",
        field: "nguoi_mua_dia_chi",
      };
    }
    if (!ten && !tenDonVi) {
      return {
        isValid: false,
        message:
          "Vui lòng nhập Họ tên người mua hoặc Đơn vị mua hàng khi sử dụng CCCD",
        field: "nguoi_mua_ten",
      };
    }
  }

  return {
    isValid: true,
    normalized: {
      ...data,
      nguoi_mua_mst: mst,
      nguoi_mua_cccd: cccd,
      nguoi_mua_ten: ten,
      nguoi_mua_ten_donvi: tenDonVi,
      nguoi_mua_dia_chi: diaChi,
    },
  };
}
