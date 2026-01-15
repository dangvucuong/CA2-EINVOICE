import {
  AlertIcon,
  PlusIcon,
  TrashIcon,
  UploadIcon,
} from "@primer/octicons-react";
import {
  Box,
  Checkbox,
  FormControl,
  IconButton,
  Octicon,
  Textarea,
} from "@primer/react";
import Big from "big.js";
import { useEffect, useLayoutEffect, useMemo, useRef, useState } from "react";
import { Controller } from "react-hook-form";
import SelectBoxThueSuat from "../../component-data/selectbox-thue-suat/SelectBoxThueSuat";
import SelectBoxTinhChatHangHoa from "../../component-data/selectbox-tinh-chat-hang-hoa";
import TextInputMaHangHoa from "../../component-data/text-ma-hang-hoa-search";
import Button from "../../component-ui/button";
import Heading from "../../component-ui/heading";
import TextInputNumber from "../../component-ui/text-input-number/TextInputNumber";
import TextInput from "../../component-ui/text-input/TextInput";
import { ConvertTienChu, numberWithCommas } from "../../helpers/common";
import { eSize } from "../../models/commons/eSize";
import { eTinhChatHangHoa } from "../../models/commons/eTinhChatHangHoa";
import {
  IHoaDonHangHoa,
  IsHoaDonHangHoaValid,
  IsHoaDonHangHoaValidIncludeField,
} from "../../models/responses/hoa-don/IHoaDonHangHoa";
import HoaDonHangHoaImportModal from "./HoaDonHangHoaImportModal";
import HangHoaDacTrungForm from "./HangHoaDacTrungForm";
import HoaDonLoaiPhiList from "./HoaDonLoaiPhiList";
import AutoResizeText from "../../component-data/auto-resize-text";

interface IHoaDonHangHoaListProps {
  tienTe?: string;
  hangHoas: IHoaDonHangHoa[];
  isHoaDonBanHang: boolean;
  isSoAm?: boolean;
  limit?: number;
  onValueChanged: (hangHoas: IHoaDonHangHoa[]) => void;
  control: any;
  watch: any;
  error: any;
  giam_thue_ty_le: number;
  onGiamThueTyLeChanged: (ty_le: number) => void;
  onValueChangedLoaiPhis?: (loaiPhis: any[]) => void;
  loaiPhis?: any[];
  loaiTien?: string;
  tongTienChu?: string;
  setTongTienChu?: (data: string) => void;
  hoa_don_dang_ky_phat_hanh_mau_so?: string;
}
export function isNumber(value: any) {
  if (isNaN(value)) return false;
  return typeof value === "number";
}
const getTongTienGoc = (hangHoa: IHoaDonHangHoa) => {
  const _soLuong = hangHoa.so_luong ?? 0;
  const _donGia = hangHoa.don_gia ?? 0;
  if (_donGia > 0 || _soLuong > 0) {
    return parseFloat(new Big(_soLuong).times(new Big(_donGia)).toString());
  } else {
    return hangHoa.thanh_tien;
  }
};
const getTienChietKhau = (hangHoa: IHoaDonHangHoa) => {
  //(x.ty_le_chiet_khau / 100) * x.don_gia * x.so_luong
  const tongTienGoc = getTongTienGoc(hangHoa);
  const ty_le_chiet_khau = hangHoa.ty_le_chiet_khau ?? 0;
  return parseFloat(
    new Big(ty_le_chiet_khau)
      .div(new Big(100))
      .times(new Big(tongTienGoc))
      .toString()
  );
};
export const getDonGia = (value: number, tienTe?: string) => {
  //đơn giá để tối đa 6 chữ số sau dấu phẩy
  const roundIfVND = (value: number): number => {
    return (tienTe ?? "VND") === "VND"
      ? parseFloat(new Big(value).round(6, Big.roundHalfUp).toString())
      : value;
  };
  return roundIfVND(parseFloat(value.toString()));
};

export const getThanhTien = (
  so_luong: any,
  don_gia: any,
  ty_le_chiet_khau: any,
  tienTe?: string,
  _isLamTron?: boolean
): number => {
  const _soLuong = isNumber(so_luong) ? parseFloat(so_luong) : 0;
  const _donGia = isNumber(don_gia) ? parseFloat(don_gia) : 0;
  let _thanhTien = new Big(_soLuong).times(new Big(_donGia));

  const _tyLeChietKhau = isNumber(ty_le_chiet_khau)
    ? parseFloat(ty_le_chiet_khau)
    : 0;
  const chietKhauFactor = new Big(100).minus(_tyLeChietKhau).div(100);
  let value = _thanhTien.times(chietKhauFactor);

  const roundIfVND = (value: number): number => {
    return (tienTe ?? "VND") === "VND"
      ? parseFloat(new Big(value).round(2, Big.roundHalfUp).toString())
      : value;
  };
  const isLamTron = _isLamTron != undefined ? _isLamTron : true;
  return isLamTron
    ? roundIfVND(parseFloat(value.toString()))
    : parseFloat(value.toString());
};

export const getTongTienData = (
  _hangHoas: IHoaDonHangHoa[],
  tienTe?: string,
  giamThueTyLe?: number,
  loaiPhis: any[] = [],
  hoa_don_dang_ky_phat_hanh_mau_so?: string
) => {
  let thuesuatck = "";
  let co_hang_hoa_dv = 0;

  const tempHangHoas = [
    ..._hangHoas.map((x) => {
      if (x?.hang_hoa_tinh_chat_id === eTinhChatHangHoa.CHIET_KHAU) {
        thuesuatck = x.thue_vat ?? "";
      }

      if (x?.hang_hoa_tinh_chat_id === eTinhChatHangHoa.HANG_HOA_DICH_VU) {
        co_hang_hoa_dv = 1;
      }

      if (x?.hang_hoa_tinh_chat_id === eTinhChatHangHoa.GHI_CHU_DIEN_GIAI) {
        return {
          ...x,
          thanh_tien: 0,
          so_luong: 0,
          don_gia: 0,
          ty_le_chiet_khau: 0,
        };
      }
      return {
        ...x,
        thanh_tien: isNumber(x.thanh_tien) ? x.thanh_tien : 0,
        so_luong: isNumber(x.so_luong) ? x.so_luong : 0,
        don_gia: isNumber(x.don_gia) ? x.don_gia : 0,
        ty_le_chiet_khau: isNumber(x.ty_le_chiet_khau) ? x.ty_le_chiet_khau : 0,
      };
    }),
  ];
  const isAllChietKhau =
    tempHangHoas
      .filter((x) => x.hang_hoa_tinh_chat_id !== 4)
      .find((x) => x.hang_hoa_tinh_chat_id !== 3) === undefined;

  const hangHoas = tempHangHoas.map((h) => {
    return {
      ...h,
      thanh_tien_khong_lam_tron: getThanhTien(
        h.so_luong,
        h.don_gia,
        h.ty_le_chiet_khau,
        tienTe,
        false
      ),
    };
  });

  const roundIfVND = (value: number): number => {
    return (tienTe ?? "VND") === "VND"
      ? parseFloat(new Big(value).round(0, Big.roundHalfUp).toString())
      : value;
  };

  const tongTienChietKhauTungMatHang = roundIfVND(
    hangHoas
      .filter((x) => x.hang_hoa_tinh_chat_id !== eTinhChatHangHoa.CHIET_KHAU)
      .map((x) => getTienChietKhau(x))
      .reduce((a, b) => a + b, 0)
  );

  const tongMatHangChietKhau = roundIfVND(
    hangHoas
      .filter((x) => x.hang_hoa_tinh_chat_id === eTinhChatHangHoa.CHIET_KHAU)
      .map((x) => x.thanh_tien_khong_lam_tron)
      .reduce((a, b) => a + b, 0)
  );

  const vats = [...Array.from(new Set(hangHoas.map((x) => x.thue_vat)))];

  // const vats_detail = vats
  //   .filter((x) => x !== undefined && x !== null)
  //   .map((x: any) => {
  //     const phan_tram = isNaN(parseInt(x.replace("%", "")))
  //       ? 0
  //       : parseInt(x.replace("%", ""));
  //     const tong_tien_hang_khong_lam_tron = hangHoas
  //       .filter((h) => h.thue_vat === x)
  //       .map((h) => h.thanh_tien_khong_lam_tron)
  //       .reduce((a, b) => a + b, 0);
  //     const tong_tien_vat = roundIfVND(
  //       parseFloat(
  //         new Big(tong_tien_hang_khong_lam_tron)
  //           .times(phan_tram)
  //           .div(100)
  //           .toString()
  //       )
  //     );
  //     return {
  //       vat: x,
  //       phan_tram,
  //       tong_tien_vat: roundIfVND(tong_tien_vat * (isAllChietKhau ? -1 : 1)),
  //     };
  //   })
  //   .filter((x) => x.phan_tram > 0);

  // ✅ Lấy các loại thuế suất duy nhất
  // const vats = [...new Set(hangHoas.map((x) => x.thue_vat))];

  const vats_detail = vats
    .filter((x) => x !== undefined && x !== null)
    .map((x: any) => {
      const phan_tram = isNaN(parseInt(x.replace("%", "")))
        ? 0
        : parseInt(x.replace("%", ""));

      // ✅ Tổng tiền hàng hóa (tính chất = 1) theo từng loại thuế
      const tongHangHoaTheoThue = hangHoas
        .filter(
          (h) =>
            (h.hang_hoa_tinh_chat_id === eTinhChatHangHoa.HANG_HOA_DICH_VU ||
              h.hang_hoa_tinh_chat_id ===
                eTinhChatHangHoa.HANG_HOA_DAC_TRUNG) &&
            h.thue_vat === x
        )
        .map((h) => h.thanh_tien)
        .reduce((a, b) => a + b, 0);

      // ✅ Tổng tiền chiết khấu theo từng loại thuế
      const tongChietKhauTheoThue = hangHoas
        .filter(
          (h) =>
            h.hang_hoa_tinh_chat_id === eTinhChatHangHoa.CHIET_KHAU &&
            h.thue_vat === x
        )
        .map((h) => h.thanh_tien_khong_lam_tron)
        .reduce((a, b) => a + b, 0);

      // ✅ Tiền tính thuế = hàng hóa - chiết khấu
      const tienTinhThue = tongHangHoaTheoThue - tongChietKhauTheoThue;
      // ✅ Tiền VAT
      const tong_tien_vat = roundIfVND(
        parseFloat(new Big(tienTinhThue).times(phan_tram).div(100).toString())
      );

      return {
        vat: x,
        phan_tram,
        tong_tien_vat: roundIfVND(tong_tien_vat * (isAllChietKhau ? -1 : 1)),
      };
    })
    .filter((x) => x.phan_tram > 0);

  let vats_total = roundIfVND(
    vats_detail.map((x) => x.tong_tien_vat).reduce((a, b) => a + b, 0) ?? 0
  );

  if (hoa_don_dang_ky_phat_hanh_mau_so === "2") {
    vats_total = 0;
  }

  const congTienHang = roundIfVND(
    hangHoas
      .filter(
        (x) =>
          x.hang_hoa_tinh_chat_id === eTinhChatHangHoa.HANG_HOA_DICH_VU ||
          x.hang_hoa_tinh_chat_id === eTinhChatHangHoa.HANG_HOA_DAC_TRUNG
      )
      .map((x) => x.thanh_tien)
      // .map((x) =>
      //   x.don_gia > 0 || x.so_luong > 0 ? x.don_gia * x.so_luong : x.thanh_tien
      // )
      .reduce((a, b) => a + b, 0)
  );
  let tienGiamThueTheoNghiDinh = 0;
  const giam_thue_ty_le = giamThueTyLe ?? 0;
  if (giam_thue_ty_le > 0) {
    const temp = new Big(congTienHang)
      .times(new Big(giam_thue_ty_le).div(100))
      .times(0.2);
    tienGiamThueTheoNghiDinh = parseFloat(
      temp.round(0, Big.roundHalfUp).toString()
    );
    tienGiamThueTheoNghiDinh = roundIfVND(tienGiamThueTheoNghiDinh);
  }
  const tongTienChietKhau = roundIfVND(
    tongTienChietKhauTungMatHang + tongMatHangChietKhau
  );

  const tongTienPhi = loaiPhis.reduce(
    (sum, item) => sum + (item?.so_tien || 0),
    0
  );

  let tongThanhTienSauCKVaVAT = 0;

  const congTienHangValid = isNumber(congTienHang) ? congTienHang : 0;
  const tongTienChietKhauValid = isNumber(tongTienChietKhau)
    ? tongTienChietKhau
    : 0;

  let cong_tien_hang = 0;

  if (thuesuatck === "0%" || thuesuatck === "") {
    cong_tien_hang = congTienHangValid;
    tongThanhTienSauCKVaVAT = roundIfVND(
      congTienHang +
        vats_total -
        tongMatHangChietKhau -
        tienGiamThueTheoNghiDinh +
        tongTienPhi
    );
  } else {
    if (co_hang_hoa_dv === 1) {
      cong_tien_hang = congTienHangValid - tongTienChietKhauValid;
      tongThanhTienSauCKVaVAT = roundIfVND(
        congTienHang +
          vats_total +
          tongTienPhi -
          tongMatHangChietKhau -
          tienGiamThueTheoNghiDinh
      );
    } else {
      if (isAllChietKhau) {
        cong_tien_hang = tongTienChietKhauValid * -1;
        tongThanhTienSauCKVaVAT = roundIfVND(
          vats_total * -1 + tongMatHangChietKhau * -1 - tienGiamThueTheoNghiDinh
        );
        vats_total = vats_total * -1;
      }
    }
  }

  return {
    //cong tien hang sẽ trừ đi tổng tiền chiết khấu nữa
    cong_tien_hang: cong_tien_hang,
    tong_tien_chiet_khau: tongTienChietKhauValid,
    tong_thanh_tien: isNumber(tongThanhTienSauCKVaVAT)
      ? tongThanhTienSauCKVaVAT
      : 0,
    vats_detail,
    vats_total,
    tienGiamThueTheoNghiDinh,
  };
};

const PlusIconAccent = () => {
  return (
    <Box sx={{ color: "accent.fg" }}>
      <PlusIcon />
    </Box>
  );
};
const HoaDonHangHoaList = (props: IHoaDonHangHoaListProps) => {
  const [isShowImportModal, setIsShowImportModal] = useState(false);
  const {
    hangHoas,
    loaiPhis,
    error,
    loaiTien,
    tongTienChu,
    setTongTienChu = () => {},
    onValueChangedLoaiPhis = () => {},
    hoa_don_dang_ky_phat_hanh_mau_so,
  } = props;
  const setHangHoas = (hangHoas: IHoaDonHangHoa[]) => {
    props.onValueChanged(hangHoas);
  };

  const isApDungDieuChinh5DongVaoThueSuat = useMemo(() => {
    const vats = [...Array.from(new Set(hangHoas.map((x) => x.thue_vat)))];
    return vats.length === 1;
  }, [hangHoas]);
  const dragItem = useRef<number | null>(null);
  const dragOverItem = useRef<number | null>(null);

  const handleDragStart = (index: number) => {
    dragItem.current = index;
  };
  const handleDragEnter = (index: number) => {
    dragOverItem.current = index;
  };

  const handleDragEnd = () => {
    const from = dragItem.current;
    const to = dragOverItem.current;

    if (from !== null && to !== null && from !== to) {
      let updated = [...hangHoas];

      // lấy cả 2 object trước khi splice
      const moved: any = { ...updated[from] };
      const target: any = { ...updated[to] };

      if (target) {
        const allKeys = new Set([
          ...Object.keys(moved),
          ...Object.keys(target),
        ]);
        const numberKeys = ["don_gia", "so_luong", "thanh_tien"];

        allKeys.forEach((key) => {
          if (!(key in moved)) {
            moved[key] = numberKeys.includes(key) ? "0" : "";
          }
          if (!(key in target)) {
            target[key] = numberKeys.includes(key) ? "0" : "";
          }
        });

        // cập nhật lại object đã đồng bộ vào mảng
        updated[from] = moved;
        updated[to] = target;
      }

      // thực hiện hoán đổi
      const [removed] = updated.splice(from, 1);
      updated.splice(to, 0, removed);
      let stt = 1;
      updated = updated.map((item) => {
        if (item.hang_hoa_tinh_chat_id !== eTinhChatHangHoa.GHI_CHU_DIEN_GIAI) {
          return { ...item, stt: stt++ };
        }
        return item;
      });

      setHangHoas(updated);
    }

    dragItem.current = null;
    dragOverItem.current = null;
  };

  const so_tien_tang_giam = props.watch("so_tien_tang_giam")
    ? parseInt(props.watch("so_tien_tang_giam"))
    : 0;
  const so_tien_tang_giam_tien_hang = props.watch("so_tien_tang_giam_tien_hang")
    ? parseInt(props.watch("so_tien_tang_giam_tien_hang"))
    : 0;
  const so_tien_tang_giam_tien_thue = props.watch("so_tien_tang_giam_tien_thue")
    ? parseInt(props.watch("so_tien_tang_giam_tien_thue"))
    : 0;

  // const [hangHoas, setHangHoas] = useState<IHoaDonHangHoa[]>(props.hangHoas);
  useEffect(() => {
    props.onValueChanged(hangHoas);

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [hangHoas]);

  useEffect(() => {
    setTongTienChu(
      ConvertTienChu(
        tongTienData.tong_thanh_tien +
          so_tien_tang_giam +
          so_tien_tang_giam_tien_hang +
          so_tien_tang_giam_tien_thue,
        loaiTien
      )
    );
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [hangHoas, loaiTien, loaiPhis, props.giam_thue_ty_le]);

  const tongTienData = getTongTienData(
    hangHoas,
    props.tienTe ?? "VND",
    props.giam_thue_ty_le,
    loaiPhis,
    hoa_don_dang_ky_phat_hanh_mau_so
  );
  const isAllChietKhau = useMemo(() => {
    return hangHoas.find((x) => x.hang_hoa_tinh_chat_id !== 3) === undefined;
  }, [hangHoas]);

  return (
    <Box>
      <Box
        sx={{
          mb: 3,
          display: "flex",
          alignItems: "center",
          // flexDirection: ["column", "column", "row"],
        }}
      >
        <Box sx={{ mr: 2, flex: 1 }}>
          <Heading text="Danh sách hàng hóa" size={eSize.medium} />
          <Box
            sx={{
              display: "flex",
              mt: 0,
              flexDirection: ["column", "column", "row"],
              gap: 2,
              alignItems: "center",
            }}
          >
            <Box
              sx={{
                display: "flex",
                alignItems: "center",
                gap: 2,
              }}
            >
              <Checkbox
                checked={props.giam_thue_ty_le > 0}
                onChange={(e) => {
                  if (e.target.checked) {
                    props.onGiamThueTyLeChanged(0);
                  } else {
                    props.onGiamThueTyLeChanged(-1);
                  }
                }}
              />
              <Box
                sx={{
                  ml: 1,
                  fontWeight: "400",
                  fontSize: "14px",
                }}
              >
                Áp dụng giảm mức thuế suất giá trị gia tăng tại Nghị quyết số
                204/2025/QH15 ngày 17 tháng 06 năm 2025. Mức tỷ lệ phần trăm
                chịu thuế :
                {/* tại Nghị quyết số
                174/2024/QH15. */}
              </Box>
            </Box>
            <Box>
              {props.giam_thue_ty_le >= 0 && (
                <TextInput
                  placeholder="3"
                  type="number"
                  value={props.giam_thue_ty_le}
                  min={0}
                  onChange={(e) => {
                    props.onGiamThueTyLeChanged(parseInt(e.target.value));
                  }}
                  sx={{
                    ml: 1,
                    width: "100px",
                  }}
                  trailingVisual={
                    <img
                      src="../../images/percent.png"
                      alt="%"
                      style={{ height: "11px", width: "auto" }}
                    />
                  }
                />
              )}
            </Box>
          </Box>
        </Box>
        <Box
          sx={{
            display: "flex",
            alignItems: "center",
          }}
        >
          <Button
            text="Import từ excel"
            leadingVisual={UploadIcon}
            variant="invisible"
            size="medium"
            onClick={() => {
              setIsShowImportModal(true);
              // console.log(hangHoas, "hang hóa log");
            }}
          />
        </Box>
      </Box>

      <Box sx={{ overflowX: "auto" }}>
        {/* <TextInputNumber /> */}
        <table className="myTable">
          <thead>
            <tr>
              <td style={{ width: "50px" }}></td>
              <td style={{ textAlign: "center", width: "50px" }}>STT</td>
              <td style={{ width: "120px" }}>Mã hàng hóa</td>
              <td style={{ minWidth: "200px" }}>Tên hàng hóa</td>
              <td style={{ width: "150px" }}>Tính chất</td>
              <td style={{ width: "80px" }}>ĐVT</td>
              <td style={{ width: "100px" }}>Chiết khấu (%)</td>
              <td style={{ width: "93px" }}>Thuế suất</td>
              <td style={{ width: "100px", textAlign: "right" }}>Số lượng</td>
              <td style={{ width: "120px", textAlign: "right" }}>Đơn giá</td>
              <td style={{ width: "120px", textAlign: "right" }}>Thành tiền</td>
            </tr>
          </thead>
          <tbody>
            {hangHoas.map((hangHoa, idx) => {
              const isType4 =
                hangHoa.hang_hoa_tinh_chat_id ===
                eTinhChatHangHoa.GHI_CHU_DIEN_GIAI; // hoặc === 4

              return (
                <>
                  <tr
                    key={idx}
                    className="tr-no-padding"
                    draggable
                    onDragStart={() => handleDragStart(idx)}
                    onDragEnter={() => handleDragEnter(idx)}
                    onDragOver={(e) => e.preventDefault()}
                    onDragEnd={handleDragEnd}
                    style={{ cursor: "move" }}
                  >
                    <td style={{ width: 50 }}>
                      <IconButton
                        icon={TrashIcon}
                        aria-label={`Delete:`}
                        title={`Delete:`}
                        variant="invisible"
                        onClick={() => {
                          let arr = [...hangHoas];
                          arr.splice(idx, 1);

                          // map lại stt, nếu không phải là loại ghi chú diễn giải, bỏ qua ghi chú diễn giải,
                          let stt = 1;
                          arr = arr.map((item) => {
                            if (
                              item.hang_hoa_tinh_chat_id !==
                              eTinhChatHangHoa.GHI_CHU_DIEN_GIAI
                            ) {
                              const newItem = { ...item, stt };
                              stt += 1;
                              return newItem;
                            }
                            return item;
                          });

                          setHangHoas(arr);
                          setHangHoas(arr);
                        }}
                      />
                    </td>
                    <td style={{ textAlign: "center", width: "50px" }}>
                      {/* {!IsHoaDonHangHoaValid(hangHoa) ? (
                        <Box>
                          <Octicon
                            icon={AlertIcon}
                            sx={{
                              color: "danger.emphasis",
                            }}
                          />
                        </Box>
                      ) : (
                        <>{idx + 1}</>
                      )} */}

                      {/* {isType4 ? (
                        <>—</>
                      ) : !IsHoaDonHangHoaValid(hangHoa) ? (
                        <Box>
                          <Octicon
                            icon={AlertIcon}
                            sx={{ color: "danger.emphasis" }}
                          />
                        </Box>
                      ) : (
                        <>{idx + 1}</>
                      )} */}
                      <>
                        {hangHoa?.stt === 0 || !hangHoa?.stt
                          ? ""
                          : hangHoa?.stt}
                      </>
                    </td>
                    <td>
                      <TextInputMaHangHoa
                        className="noborder"
                        value={hangHoa.ma_hang}
                        onValueChanged={(data) => {
                          if (data.hang_hoa) {
                            setHangHoas(
                              hangHoas.map((x, i) => {
                                if (i === idx) {
                                  return {
                                    ...x,
                                    ma_hang: data.text,
                                    ten_hang: data.hang_hoa?.ten_hang_hoa ?? "",
                                    dvt: data.hang_hoa?.dvt ?? "",
                                    don_gia: data.hang_hoa?.don_gia ?? 0,
                                  };
                                }
                                return {
                                  ...x,
                                };
                              })
                            );
                          } else {
                            setHangHoas(
                              hangHoas.map((x, i) => {
                                if (i === idx) {
                                  return {
                                    ...x,
                                    ma_hang: data.text,
                                  };
                                }
                                return {
                                  ...x,
                                };
                              })
                            );
                          }
                        }}
                      />
                    </td>
                    <td>
                      <AutoResizeText
                        sx={{
                          border: IsHoaDonHangHoaValidIncludeField(
                            hangHoa
                          ).errors.find((e) => e.field === "ten_hang")
                            ? "1px solid red"
                            : undefined,
                        }}
                        value={hangHoa.ten_hang}
                        onChange={(val) => {
                          setHangHoas(
                            hangHoas.map((x, i) =>
                              i === idx
                                ? {
                                    ...x,
                                    ten_hang: val,
                                  }
                                : { ...x }
                            )
                          );
                        }}
                      />
                    </td>
                    <td>
                      <SelectBoxTinhChatHangHoa
                        sx={{
                          border: IsHoaDonHangHoaValidIncludeField(
                            hangHoa
                          ).errors.find(
                            (e) => e.field === "hang_hoa_tinh_chat_id"
                          )
                            ? "1px solid red"
                            : 0,
                          boxShadow: "none",
                        }}
                        // onValueChanged={(id) => {
                        //   setHangHoas(
                        //     hangHoas.map((x, i) => {
                        //       if (i === idx) {
                        //         return {
                        //           ...x,
                        //           hang_hoa_tinh_chat_id: id,
                        //           stt: id === 4 ? ("" as any) : idx + 1,
                        //           dvt: id === 4 ? ("" as any) : x.dvt,
                        //           don_gia: id === 4 ? ("0" as any) : x.don_gia,
                        //           so_luong: id === 4 ? ("" as any) : x.so_luong,
                        //           thanh_tien:
                        //             id === 4 ? ("" as any) : x.thanh_tien,
                        //         };
                        //       }
                        //       return {
                        //         ...x,
                        //       };
                        //     })
                        //   );
                        // }}
                        onValueChanged={(id) => {
                          let updated: any = hangHoas.map((x, i) => {
                            if (i === idx) {
                              return {
                                ...x,
                                hang_hoa_tinh_chat_id: id,
                                dvt: id === 4 ? ("" as any) : x.dvt,
                                don_gia: id === 4 ? ("0" as any) : x.don_gia,
                                so_luong: id === 4 ? ("" as any) : x.so_luong,
                                thanh_tien:
                                  id === 4 ? ("" as any) : x.thanh_tien,
                              };
                            }
                            return x;
                          });

                          // Đánh lại stt, chỉ tăng cho hàng không phải ghi chú diễn giải
                          let stt = 1;
                          updated = updated.map((item: IHoaDonHangHoa) => {
                            if (
                              item.hang_hoa_tinh_chat_id !==
                              eTinhChatHangHoa.GHI_CHU_DIEN_GIAI
                            ) {
                              return { ...item, stt: stt++ };
                            }
                            return { ...item, stt: "" };
                          });

                          setHangHoas(updated);
                        }}
                        value={hangHoa.hang_hoa_tinh_chat_id}
                      />
                    </td>
                    <td>
                      <TextInput
                        className="noborder"
                        value={isType4 ? "" : hangHoa.dvt}
                        disabled={isType4}
                        onChange={(e) => {
                          setHangHoas(
                            hangHoas.map((x, i) => {
                              if (i === idx) {
                                return {
                                  ...x,
                                  dvt: e.target.value,
                                };
                              }
                              return {
                                ...x,
                              };
                            })
                          );
                        }}
                      />
                    </td>
                    <td>
                      <TextInput
                        type="number"
                        className="noborder"
                        value={hangHoa.ty_le_chiet_khau}
                        onBlur={(e) => {
                          setHangHoas(
                            hangHoas.map((x, i) => {
                              if (i === idx) {
                                return {
                                  ...x,
                                  ty_le_chiet_khau:
                                    parseFloat(e.target.value) ?? 0,
                                  thanh_tien: getThanhTien(
                                    x.so_luong,
                                    x.don_gia,
                                    parseFloat(e.target.value) ?? 0,
                                    props.tienTe ?? "VND"
                                  ),
                                };
                              }
                              return {
                                ...x,
                              };
                            })
                          );
                        }}
                        onChange={(e) => {
                          setHangHoas(
                            hangHoas.map((x, i) => {
                              if (i === idx) {
                                return {
                                  ...x,
                                  ty_le_chiet_khau:
                                    parseFloat(e.target.value) ?? 0,
                                };
                              }
                              return {
                                ...x,
                              };
                            })
                          );
                        }}
                      />
                    </td>
                    <td>
                      <SelectBoxThueSuat
                        sx={{
                          border: 0,
                          boxShadow: "none",
                        }}
                        isReadOnly={props.isHoaDonBanHang}
                        onValueChanged={(id) => {
                          if (!props.isHoaDonBanHang) {
                            setHangHoas(
                              hangHoas.map((x, i) => {
                                if (i === idx) {
                                  return {
                                    ...x,
                                    thue_vat: id,
                                  };
                                }
                                return {
                                  ...x,
                                };
                              })
                            );
                          }
                        }}
                        value={
                          props.isHoaDonBanHang ? "0%" : hangHoa.thue_vat ?? ""
                        }
                      />
                    </td>
                    <td>
                      <TextInput
                        type="number"
                        className="noborder"
                        disabled={isType4}
                        value={hangHoa.so_luong}
                        onBlur={(e) => {
                          setHangHoas(
                            hangHoas.map((x, i) => {
                              if (i === idx) {
                                return {
                                  ...x,
                                  so_luong: parseFloat(e.target.value) ?? 0,
                                  thanh_tien: getThanhTien(
                                    parseFloat(e.target.value) ?? 0,
                                    x.don_gia,
                                    x.ty_le_chiet_khau,
                                    props.tienTe ?? "VND"
                                  ),
                                };
                              }
                              return {
                                ...x,
                              };
                            })
                          );
                        }}
                        onChange={(e) => {
                          setHangHoas(
                            hangHoas.map((x, i) => {
                              if (i === idx) {
                                return {
                                  ...x,
                                  so_luong: parseFloat(e.target.value) ?? 0,
                                };
                              }
                              return {
                                ...x,
                              };
                            })
                          );
                        }}
                      />
                    </td>
                    <td>
                      <TextInputNumber
                        // type='number'
                        isSoAm={props.isSoAm}
                        className="noborder"
                        disabled={isType4}
                        value={hangHoa.don_gia}
                        onValueChanged={(value) => {
                          setHangHoas(
                            hangHoas.map((x, i) => {
                              if (i === idx) {
                                return {
                                  ...x,
                                  don_gia: getDonGia(
                                    value,
                                    props.tienTe ?? "VND"
                                  ),
                                  thanh_tien: getThanhTien(
                                    x.so_luong,
                                    value ?? 0,
                                    x.ty_le_chiet_khau,
                                    props.tienTe ?? "VND"
                                  ),
                                };
                              }
                              return {
                                ...x,
                              };
                            })
                          );
                        }}
                      />
                    </td>
                    <td style={{ textAlign: "right" }}>
                      <Box sx={{ mr: 2 }}>
                        {/* {hangHoa.thanh_tien > 0 ? numberWithCommas(hangHoa.thanh_tien) : "-"} */}
                        {numberWithCommas(hangHoa.thanh_tien)}
                      </Box>
                    </td>
                  </tr>
                  {hangHoa.hang_hoa_tinh_chat_id ===
                    eTinhChatHangHoa.HANG_HOA_DAC_TRUNG && (
                    <tr>
                      <td></td>
                      <td colSpan={10}>
                        <HangHoaDacTrungForm
                          hangHoa={hangHoa}
                          onValueChanged={(hangHoa) => {
                            setHangHoas(
                              hangHoas.map((x, i) => {
                                if (i === idx) {
                                  return {
                                    ...x,
                                    ...hangHoa,
                                  };
                                }
                                return {
                                  ...x,
                                };
                              })
                            );
                          }}
                        />
                      </td>
                    </tr>
                  )}
                </>
              );
            })}

            {(!props.limit ||
              (props.limit && hangHoas.length < props.limit)) && (
              <tr>
                <td colSpan={11}>
                  <Box
                    sx={{
                      width: "100%",
                      display: "flex",
                      alignItems: "center",
                      justifyContent: "center",
                    }}
                  >
                    <Button
                      leadingVisual={PlusIconAccent}
                      text="Thêm hàng hóa"
                      variant="invisible"
                      size="medium"
                      sx={{
                        color: "accent.fg",
                      }}
                      onClick={() => {
                        const newHangHoa: any = {
                          hang_hoa_tinh_chat_id: 1,
                          ty_le_chiet_khau: 0,
                          dvt: "",
                          stt:
                            hangHoas.filter(
                              (item) =>
                                item?.hang_hoa_tinh_chat_id !==
                                eTinhChatHangHoa.GHI_CHU_DIEN_GIAI
                            )?.length + 1,
                        };
                        setHangHoas([...hangHoas, newHangHoa]);
                      }}
                    />
                  </Box>
                </td>
              </tr>
            )}

            {/* <tr>
                        <td colSpan={5}>
                            Bằng chữ:
                        </td>
                        <td colSpan={5} style={{ textAlign: "right" }}>
                            <b>{readMoney(tongTienData.tong_thanh_tien)}</b>
                        </td>
                    </tr> */}
          </tbody>
          {/* <tfoot>
                    <tr>
                        <td colSpan={9}>
                            <Button leadingVisual={PlusIcon} text='Thêm hàng hóa' />
                        </td>
                    </tr>
                </tfoot> */}
        </table>

        {error && error["hang_hoas"] && (
          <FormControl.Validation variant="error" sx={{ mt: 2 }}>
            Vui lòng kiểm tra lại danh sách hàng hóa
          </FormControl.Validation>
        )}

        <Box
          sx={{
            borderTopWidth: 1,
            borderTopStyle: "solid",
            borderTopColor: "border.default",
            mt: [0, 0, 3],
            pt: [2, 2, 3],
          }}
        >
          <HoaDonLoaiPhiList
            loaiPhis={loaiPhis ?? []}
            onValueChanged={onValueChangedLoaiPhis}
          />
          {error && error["loai_phis"] && (
            <FormControl.Validation variant="error" sx={{ mt: 2 }}>
              Vui lòng kiểm tra lại danh sách loại phí
            </FormControl.Validation>
          )}
        </Box>

        <table className="myTable" style={{ marginTop: 20 }}>
          <tbody>
            <tr>
              <td colSpan={3}>Cộng tiền hàng:</td>
              <td colSpan={3}>
                {props.tienTe === "VND" && (
                  <Controller
                    control={props.control}
                    rules={{
                      validate: (value) => {
                        const tong_tang_giam =
                          (value ? parseInt(value) : 0) +
                          so_tien_tang_giam +
                          so_tien_tang_giam_tien_thue;
                        if (value) {
                          if (value < -5 || value > 5) {
                            return "Số tiền tăng giảm chỉ được trong khoảng tăng giảm 5 đồng.";
                          }
                          if (tong_tang_giam < -5 || tong_tang_giam > 5) {
                            return "Tổng tiền tăng giảm chỉ được trong khoảng tăng giảm 5 đồng.";
                          }
                        }
                        return true;
                      },
                    }}
                    name="so_tien_tang_giam_tien_hang"
                    render={({ field }) => {
                      return (
                        <Box sx={{ display: "grid", gap: 2 }}>
                          <Box
                            sx={{
                              display: "flex",
                              gap: 2,
                              alignItems: "center",
                            }}
                          >
                            <Box>Điều chỉnh tăng giảm</Box>
                            <Box sx={{ color: "fg.muted" }}>
                              (Phạm vi 5 đồng):
                            </Box>
                            <TextInput
                              type="number"
                              min={-5}
                              max={5}
                              value={field.value}
                              onChange={(e) => {
                                field.onChange(e);
                              }}
                            />
                            {props.error &&
                              props.error["so_tien_tang_giam_tien_hang"] && (
                                <FormControl.Validation variant="error"></FormControl.Validation>
                              )}
                          </Box>
                        </Box>
                      );
                    }}
                  />
                )}
              </td>
              <td colSpan={6} style={{ textAlign: "right" }}>
                <b>
                  {numberWithCommas(
                    (tongTienData.cong_tien_hang ?? 0) +
                      so_tien_tang_giam_tien_hang
                  )}
                </b>
              </td>
            </tr>
            <tr>
              <td colSpan={3}>Cộng tiền VAT:</td>
              <td colSpan={3}>
                {props.tienTe === "VND" && (
                  <Controller
                    control={props.control}
                    rules={{
                      validate: (value) => {
                        const tong_tang_giam =
                          (value ? parseInt(value) : 0) +
                          so_tien_tang_giam +
                          so_tien_tang_giam_tien_hang;

                        if (value) {
                          if (value < -5 || value > 5) {
                            return "Số tiền tăng giảm chỉ được trong khoảng tăng giảm 5 đồng.";
                          }
                          if (tong_tang_giam < -5 || tong_tang_giam > 5) {
                            return "Tổng tiền tăng giảm chỉ được trong khoảng tăng giảm 5 đồng.";
                          }
                        }
                        return true;
                      },
                    }}
                    name="so_tien_tang_giam_tien_thue"
                    render={({ field }) => {
                      return (
                        <Box sx={{ display: "grid", gap: 2 }}>
                          <Box
                            sx={{
                              display: "flex",
                              gap: 2,
                              alignItems: "center",
                            }}
                          >
                            <Box>Điều chỉnh tăng giảm tổng tiền thuế</Box>
                            <Box sx={{ color: "fg.muted" }}>
                              (Phạm vi 5 đồng):
                            </Box>
                            <TextInput
                              type="number"
                              min={-5}
                              max={5}
                              value={field.value}
                              onChange={(e) => {
                                field.onChange(e);
                              }}
                            />
                            {props.error &&
                              props.error["so_tien_tang_giam_tien_thue"] && (
                                <FormControl.Validation variant="error"></FormControl.Validation>
                              )}
                          </Box>
                        </Box>
                      );
                    }}
                  />
                )}
              </td>
              <td colSpan={6} style={{ textAlign: "right" }}>
                <b>
                  {numberWithCommas(
                    tongTienData.vats_total + so_tien_tang_giam_tien_thue
                  )}
                </b>
              </td>
            </tr>
            {hoa_don_dang_ky_phat_hanh_mau_so !== "2" &&
              tongTienData.vats_detail.map((x) => {
                return (
                  <tr key={x.vat}>
                    <td colSpan={6}>{x.vat}</td>
                    <td colSpan={6} style={{ textAlign: "right" }}>
                      {isApDungDieuChinh5DongVaoThueSuat && (
                        <>
                          {numberWithCommas(
                            x.tong_tien_vat + so_tien_tang_giam_tien_thue
                          )}
                        </>
                      )}
                      {!isApDungDieuChinh5DongVaoThueSuat && (
                        <>{numberWithCommas(x.tong_tien_vat)}</>
                      )}

                      {/* {x.tong_tien_vat ? numberWithCommas(x.tong_tien_vat) : ""} */}
                    </td>
                  </tr>
                );
              })}
            <tr>
              <td colSpan={6}>Tổng tiền chiết khấu:</td>
              <td colSpan={5} style={{ textAlign: "right" }}>
                {/* <b>{tongTienData.tong_tien_chiet_khau ? numberWithCommas(tongTienData.tong_tien_chiet_khau) : ""}</b> */}
                <b>
                  {numberWithCommas(
                    tongTienData.tong_tien_chiet_khau *
                      (isAllChietKhau ? -1 : 1)
                  )}
                </b>
              </td>
            </tr>
            <tr>
              <td colSpan={3}>Tổng tiền thanh toán:</td>
              <td colSpan={3}>
                {tongTienData.tienGiamThueTheoNghiDinh > 0 && (
                  <>
                    Đã giảm{" "}
                    {numberWithCommas(tongTienData.tienGiamThueTheoNghiDinh)}đ
                    tương ứng 20% mức tỷ lệ {props.giam_thue_ty_le}% để tính
                    thuế giá trị gia tăng theo Nghị quyết số 204/2025/QH15
                  </>
                )}
              </td>
              <td colSpan={6} style={{ textAlign: "right" }}>
                <b>
                  {/* {tongTienData.tong_thanh_tien ? numberWithCommas(tongTienData.tong_thanh_tien) : ""} */}
                  {numberWithCommas(
                    tongTienData.tong_thanh_tien +
                      so_tien_tang_giam +
                      so_tien_tang_giam_tien_hang +
                      so_tien_tang_giam_tien_thue
                  )}
                </b>
              </td>
            </tr>
            <tr>
              <td colSpan={3}>Tổng tiền bằng chữ:</td>
              <td colSpan={3}>
                {tongTienData.tienGiamThueTheoNghiDinh > 0 && (
                  <>
                    Đã giảm{" "}
                    {numberWithCommas(tongTienData.tienGiamThueTheoNghiDinh)}đ
                    tương ứng 20% mức tỷ lệ {props.giam_thue_ty_le}% để tính
                    thuế giá trị gia tăng theo Nghị quyết số 204/2025/QH15
                  </>
                )}
              </td>
              <td colSpan={6} style={{ textAlign: "right" }}>
                <b>
                  {/* {tongTienData.tong_thanh_tien ? numberWithCommas(tongTienData.tong_thanh_tien) : ""} */}
                  <input
                    type="text"
                    value={tongTienChu}
                    onChange={(e) => {
                      setTongTienChu(e.target.value);
                    }}
                    style={{
                      width: "100%",
                      border: "none",
                      textAlign: "right",
                      fontWeight: 600,
                      outline: "none",
                    }}
                  />
                </b>
              </td>
            </tr>
          </tbody>
        </table>
      </Box>

      {/* {props.tienTe === "VND" &&
                <Box sx={{ mt: 3 }}>
                    <Controller
                        control={props.control}
                        rules={{
                            validate: (value) => {
                                if (value) {
                                    if (value < -5 || value > 5) {
                                        return "Số tiền tăng giảm chỉ được trong khoảng tăng giảm 5 đồng."
                                    }
                                }
                                return true;
                            }
                        }}
                        name='so_tien_tang_giam'
                        render={({ field }) => {
                            return (
                                <Box sx={{ display: "flex", gap: 2 }}>
                                    <Box sx={{ flex: 1 }}></Box>
                                    <Box sx={{ display: "grid", gap: 2, }}>
                                        <Box sx={{ display: "flex", gap: 2, alignItems: "center" }}>
                                            <Box>Điều chỉnh tăng giảm:</Box>
                                            <TextInput type='number' min={-5} max={5}
                                                value={field.value}
                                                onChange={(e) => {
                                                    field.onChange(e)
                                                }}
                                            />
                                        </Box>
                                        <FormControl.Caption>
                                            Điều chỉnh tăng/giảm thành tiền trong phạm vi 5 đồng
                                        </FormControl.Caption>
                                    </Box>
                                    <Box>
                                        <FormControl>
                                            <FormControl.Label>Tổng tiền thanh toán</FormControl.Label>
                                            <Box sx={{ width: "100%", textAlign: "right", fontWeight: 600 }}>
                                                {numberWithCommas(tongTienData.tong_thanh_tien + (so_tien_tang_giam ? parseInt(so_tien_tang_giam) : 0))}
                                            </Box>
                                        </FormControl>
                                    </Box>
                                </Box>
                            );
                        }}
                    />
                </Box>
            } */}
      {isShowImportModal && (
        <HoaDonHangHoaImportModal
          onClose={() => {
            setIsShowImportModal(false);
          }}
          onSuccess={(data) => {
            if (props.isHoaDonBanHang) {
              setHangHoas(
                data.map((x) => {
                  return {
                    ...x,
                    stt: x.stt === 0 ? ("" as any) : x.stt,
                    thue_vat: "0%",
                  };
                })
              );
            } else {
              setHangHoas(
                data.map((x) => {
                  return {
                    ...x,
                    stt: x.stt === 0 ? ("" as any) : x.stt,
                  };
                })
              );
            }
            setIsShowImportModal(false);
          }}
        />
      )}
    </Box>
  );
};

export default HoaDonHangHoaList;
