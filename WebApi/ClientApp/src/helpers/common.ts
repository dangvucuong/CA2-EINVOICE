import moment from "moment";

export const isNotEmpty = (value: string | undefined | null): boolean => {
  return value !== undefined && value !== null && value !== "";
};
export const getFirstDayOfMonth = (): string => {
  const today = moment();
  const firstDayOfMonth = today.startOf("month");
  return firstDayOfMonth.format("YYYY-MM-DD");
};
export const getLastDayOfMonth = (): string => {
  const today = moment();
  const endDayOfMonth = today.endOf("month");
  return endDayOfMonth.format("YYYY-MM-DD");
};
export const numberWithCommas = (x: any) => {
  return x?.toString().replace(/\B(?=(\d{3})+(?!\d))/g, " ") ?? "";
};

export function parseSoapResponse(soapXmlString: string) {
  const parser = new DOMParser();
  const xmlDoc = parser.parseFromString(soapXmlString, "text/xml");

  const resultNode = Array.from(xmlDoc.getElementsByTagName("*")).find((node) =>
    node.nodeName.endsWith("Result")
  );

  if (!resultNode || !resultNode.textContent) {
    return null;
  }

  const jsonText = resultNode.textContent.trim();
  return JSON.parse(jsonText);
}

export function formatXml(xmlString: string): string {
  let formatted = "";
  const reg = /(>)(<)(\/*)/g;
  xmlString = xmlString.replace(reg, "$1\r\n$2$3"); // thêm xuống dòng giữa thẻ
  let pad = 0;

  xmlString.split("\r\n").forEach((node) => {
    let indent = 0;
    if (node.match(/.+<\/\w[^>]*>$/)) {
      // thẻ mở & đóng trên cùng 1 dòng
      indent = 0;
    } else if (node.match(/^<\/\w/)) {
      // thẻ đóng
      if (pad !== 0) pad -= 1;
    } else if (node.match(/^<\w([^>]*[^/])?>.*$/)) {
      // thẻ mở
      indent = 1;
    } else {
      indent = 0;
    }

    const padding = new Array(pad + 1).join("  "); // 2 spaces
    formatted += padding + node + "\r\n";
    pad += indent;
  });

  return formatted.trim();
}

export function ConvertTienChu(
  number: number,
  loaiTien = "VND",
  addUnit = true
): string {
  // validate input
  if (number === null || number === undefined) return "";
  const n = Number(number);
  if (isNaN(n)) return "";

  const chuSo = [
    "không",
    "một",
    "hai",
    "ba",
    "bốn",
    "năm",
    "sáu",
    "bảy",
    "tám",
    "chín",
  ];
  const hang = ["", "nghìn", "triệu", "tỷ", "nghìn tỷ", "triệu tỷ", "tỷ tỷ"];

  const donViTien: any = {
    VND: "đồng",
    USD: "đô la mỹ",
    EUR: "Euro",
    SGD: "đô la Singapore",
    JPY: "yên Nhật",
    CHF: "franc Thụy Sĩ",
    AUD: "đô la Úc",
    GBP: "bảng Anh",
    CAD: "đô la Canada",
    CNY: "tệ",
  };

  // map đơn vị phần thập phân (nếu không có => không hiển thị phần thập phân)
  const fractionUnit: any = {
    VND: "xu",
    USD: "cent",
    EUR: "cent",
    SGD: "cent",
    AUD: "cent",
    CAD: "cent",
    GBP: "cent",
    CHF: "cent",
    JPY: "", // yên thường không có phần thập phân
    CNY: "hào", // mặc định vẫn dùng "hào" nếu cần
  };

  // map các đơn vị con (nếu currency có 2-level fractional, ví dụ CNY: hào + xu)
  const fractionSubUnits: any = {
    CNY: ["hào", "xu"], // 0.1 => 1 hào, 0.01 => 1 xu
    // có thể mở rộng cho các loại tiền khác nếu cần
  };

  // đọc 3 chữ số (0-999). forceHundred: ép hiển thị "không trăm" nếu cần.
  const readThree = (num: number, forceHundred = false): string => {
    let s = "";
    const a = Math.floor(num / 100); // hàng trăm
    const b = Math.floor((num % 100) / 10); // hàng chục
    const c = num % 10; // hàng đơn vị

    if (a > 0) {
      s += `${chuSo[a]} trăm`;
    } else if (forceHundred && (b > 0 || c > 0)) {
      // bắt buộc hiển thị "không trăm"
      s += `không trăm`;
    }

    if (b > 1) {
      s += (s ? " " : "") + `${chuSo[b]} mươi`;
      if (c === 1) s += " mốt";
      else if (c === 4) s += " tư";
      else if (c === 5) s += " lăm";
      else if (c > 0) s += " " + chuSo[c];
    } else if (b === 1) {
      s += (s ? " " : "") + "mười";
      if (c === 0) {
        // nothing
      } else if (c === 5) s += " lăm";
      else s += " " + chuSo[c];
    } else if (b === 0) {
      if (c > 0) {
        // nếu đã có "trăm" hoặc "không trăm" thì dùng "linh" (theo yêu cầu 1.001 => "linh")
        if (a > 0 || (forceHundred && s.includes("không trăm"))) {
          s += (s ? " linh " : "") + chuSo[c];
        } else {
          // trường hợp thông thường (ví dụ 101 -> "một trăm lẻ một")
          s += (s ? " lẻ " : "") + chuSo[c];
        }
      }
    }

    return s.trim();
  };

  const absN = Math.abs(n);
  const integerPart = Math.floor(absN);
  const decimalPart = Math.round((absN - integerPart) * 100); // hai chữ số thập phân

  // helper: build fractional string (xóa đơn vị chính khi gọi đệ quy)
  const buildFractionText = (dec: number, currency: string): string => {
    // nếu currency có sub-units (ví dụ CNY: ["hào","xu"]), đọc tách từng chữ số
    const sub = fractionSubUnits[currency];
    if (sub && sub.length >= 2) {
      const tens = Math.floor(dec / 10); // hào (0.1)
      const units = dec % 10; // xu (0.01)
      const parts: string[] = [];
      if (tens > 0) parts.push(`${chuSo[tens]} ${sub[0]}`);
      if (units > 0) parts.push(`${chuSo[units]} ${sub[1]}`);
      return parts.join(" ");
    }

    // mặc định: đọc số thập phân như một số (ví dụ 11 -> "mười một") + unit
    const txt = ConvertTienChu(dec, currency, false).toLowerCase().trim();
    return txt;
  };

  // xử lý trường hợp số = 0 (phần nguyên = 0)
  if (integerPart === 0) {
    let res = "Không";
    const unit = donViTien[loaiTien] ?? "";
    if (addUnit && unit) res += " " + unit;

    // chỉ hiển thị phần thập phân nếu có định nghĩa fractionUnit và khác rỗng
    const fracUnit = fractionUnit[loaiTien];
    if (decimalPart > 0 && fracUnit !== undefined && fracUnit !== "") {
      const fracText = buildFractionText(decimalPart, loaiTien);
      // nếu buildFractionText đã trả về từng phần kèm tên nhỏ, dùng trực tiếp; ngược lại thêm unit chung
      if (fractionSubUnits[loaiTien]) {
        // no " và " for multi-level fractional units (e.g. CNY => "tệ một hào một xu")
        res += " " + fracText;
      } else {
        res += " và " + fracText + (fracUnit ? " " + fracUnit : "");
      }
    }

    return res;
  }
  // chia thành nhóm 3 chữ số, groups từ thấp -> cao
  const groups: { val: number; idx: number }[] = [];
  let tmp = integerPart;
  let idx = 0;
  while (tmp > 0) {
    groups.push({ val: tmp % 1000, idx });
    tmp = Math.floor(tmp / 1000);
    idx++;
  }

  const parts: string[] = [];
  for (let i = groups.length - 1; i >= 0; i--) {
    const g = groups[i];
    if (g.val === 0) {
      // nếu nhóm = 0 và không có nhóm thấp hơn non-zero thì bỏ qua
      const anyLowerNonZero = groups.slice(0, i).some((x) => x.val > 0);
      if (anyLowerNonZero) {
        // giữ chỗ bằng cách thêm rỗng để sau này ghép suffix nếu cần
        parts.push("");
      }
      continue;
    }
    // nếu group < 100 và có nhóm cao hơn (bên trái) không bằng 0 thì ép hiển thị "không trăm"
    const hasHigherNonZero = groups.slice(i + 1).some((x) => x.val > 0);
    const forceHundred = g.val < 100 && hasHigherNonZero;
    const text = readThree(g.val, forceHundred);
    const suffix = hang[g.idx] ? " " + hang[g.idx] : "";
    parts.push((text + suffix).trim());
  }

  // lọc các phần rỗng do giữ chỗ
  let result = parts
    .filter((p) => p !== "")
    .join(" ")
    .replace(/\s+/g, " ")
    .trim();

  const unitName = donViTien[loaiTien] ?? "";
  if (addUnit && unitName) result = result + " " + unitName;

  // phần thập phân: chỉ hiển thị nếu định nghĩa fractionUnit khác rỗng
  const fracUnitAll = fractionUnit[loaiTien];
  if (decimalPart > 0 && fracUnitAll !== undefined && fracUnitAll !== "") {
    if (fractionSubUnits[loaiTien]) {
      // ghép các đơn vị con (không thêm unit chính) — no " và "
      const frac = buildFractionText(decimalPart, loaiTien);
      result = result + " " + frac;
    } else {
      const fracText = ConvertTienChu(decimalPart, loaiTien, false)
        .toLowerCase()
        .trim();
      result =
        result + " và " + fracText + (fracUnitAll ? " " + fracUnitAll : "");
    }
  }

  result = result.charAt(0).toUpperCase() + result.slice(1);
  if (n < 0) result = "Âm " + result;

  return result;
}
