import cases from "./tienThueHelper.cases.json";
import { phanBoTienThueVnd } from "./tienThueHelper";
import { IHoaDonHangHoa } from "../models/responses/hoa-don/IHoaDonHangHoa";

// @primer/react gọi CSS.supports ngay khi import, jsdom không có hàm này; @primer/react/drafts chỉ có bản ESM.
(globalThis as any).CSS = { supports: () => false };
jest.mock("@primer/react/drafts", () => ({}));
const { getTongTienData } = require("../pages/hoa-don-form/HoaDonHangHoaList");

describe("phanBoTienThueVnd", () => {
  test.each(cases.map((x) => [x.name, x] as const))("%s", (_, x) => {
    expect(phanBoTienThueVnd(x.items)).toEqual(x.expected);
  });
});

describe("getTongTienData VND", () => {
  const hangHoa = (thanh_tien: number, thue_vat: string, hang_hoa_tinh_chat_id = 1) =>
    ({
      hang_hoa_tinh_chat_id,
      so_luong: 1,
      don_gia: thanh_tien,
      thanh_tien,
      ty_le_chiet_khau: 0,
      thue_vat,
    }) as unknown as IHoaDonHangHoa;

  test("Tổng thanh toán bằng tổng tiền hàng cộng tổng các mức thuế", () => {
    const data = getTongTienData(
      [
        hangHoa(373091, "10%"),
        hangHoa(110435, "5%"),
        hangHoa(11043, "KKKNT"),
        hangHoa(9091, "10%"),
        hangHoa(136364, "10%"),
      ],
      "VND",
    );
    const tongCacMuc = data.vats_detail.reduce((a: number, b: any) => a + b.tong_tien_vat, 0);
    expect(data.cong_tien_hang).toBe(640024);
    expect(tongCacMuc).toBe(57376);
    expect(data.vats_total).toBe(57376);
    expect(data.tong_thanh_tien).toBe(697400);
  });

  test("Chiết khấu thương mại trừ vào tiền tính thuế cùng mức", () => {
    const data = getTongTienData(
      [hangHoa(518546, "10%"), hangHoa(110435, "5%"), hangHoa(1000, "10%", 3)],
      "VND",
    );
    expect(data.vats_total).toBe(data.vats_detail.reduce((a: number, b: any) => a + b.tong_tien_vat, 0));
    expect(data.vats_total).toBe(Math.round(517546 * 0.1 + 110435 * 0.05));
  });
});
