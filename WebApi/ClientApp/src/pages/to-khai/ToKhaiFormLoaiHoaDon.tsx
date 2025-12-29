import { Box, Checkbox, FormControl } from "@primer/react";
import { useMemo } from "react";
import {
  Control,
  Controller,
  UseFormSetValue,
  UseFormWatch,
} from "react-hook-form";
import { IToKhai } from "../../models/responses/to-khai/IToKhai";
interface ToKhaiFormLoaiHoaDonProps {
  register: any;
  errors: any;
  control: Control<IToKhai, any>;
  watch: UseFormWatch<IToKhai>;
  setValue: UseFormSetValue<IToKhai>;
}
const ToKhaiFormLoaiHoaDon = (props: ToKhaiFormLoaiHoaDonProps) => {
  const { setValue, errors, control, watch } = props;
  const is_ban_hang_du_tru_quoc_gia = watch("is_ban_hang_du_tru_quoc_gia");
  const is_ban_tai_san_cong = watch("is_ban_tai_san_cong");
  const is_sd_hoadon_gtgt = watch("is_sd_hoadon_gtgt");
  const is_sd_hoadon_gtgt_bien_lai = watch("is_sd_hoadon_gtgt_bien_lai");
  const is_sd_hoadon_banhang = watch("is_sd_hoadon_banhang");
  const is_sd_hoadon_banhang_bien_lai = watch("is_sd_hoadon_banhang_bien_lai");
  const is_sd_hoadon_thuong_mai = watch("is_sd_hoadon_thuong_mai");
  const is_sd_chungtu_giong_hoadon = watch("is_sd_chungtu_giong_hoadon");
  const is_sd_hoadon_khac = watch("is_sd_hoadon_khac");

  const isValid = useMemo(() => {
    if (
      !is_ban_hang_du_tru_quoc_gia &&
      !is_ban_tai_san_cong &&
      !is_sd_hoadon_gtgt &&
      !is_sd_hoadon_gtgt_bien_lai &&
      !is_sd_hoadon_banhang &&
      !is_sd_hoadon_banhang_bien_lai &&
      !is_sd_hoadon_thuong_mai &&
      !is_sd_chungtu_giong_hoadon
    ) {
      return "Chọn tối thiểu loại hóa đơn";
    }
    return true;
  }, [
    is_ban_hang_du_tru_quoc_gia,
    is_ban_tai_san_cong,
    is_sd_hoadon_gtgt,
    is_sd_hoadon_gtgt_bien_lai,
    is_sd_hoadon_banhang,
    is_sd_hoadon_banhang_bien_lai,
    is_sd_hoadon_thuong_mai,
    is_sd_chungtu_giong_hoadon,
  ]);
  return (
    <Box
      sx={{
        display: "grid",
        gap: 2,
      }}
    >
      <Controller
        control={control}
        name="is_sd_hoadon_gtgt"
        rules={{
          validate: (data) => {
            return isValid;
          },
        }}
        render={({ field }) => {
          return (
            <FormControl>
              <Checkbox
                checked={field.value}
                onChange={(e) => {
                  field.onChange(e.target.checked);
                }}
              />
              <FormControl.Label>Hóa đơn GTGT</FormControl.Label>
            </FormControl>
          );
        }}
      />
      <Controller
        control={control}
        name="is_sd_hoadon_gtgt_bien_lai"
        rules={{
          validate: (data) => {
            return isValid;
          },
        }}
        render={({ field }) => {
          return (
            <FormControl>
              <Checkbox
                checked={field.value}
                onChange={(e) => {
                  field.onChange(e.target.checked);
                }}
              />
              <FormControl.Label>
                Hóa đơn GTGT tích hợp biên lai thu thuế, phí, lệ phí
              </FormControl.Label>
            </FormControl>
          );
        }}
      />
      <Controller
        control={control}
        name="is_sd_hoadon_banhang"
        rules={{
          validate: (data) => {
            return isValid;
          },
        }}
        render={({ field }) => {
          return (
            <FormControl>
              <Checkbox
                checked={field.value}
                onChange={(e) => {
                  field.onChange(e.target.checked);
                }}
              />
              <FormControl.Label>Hóa đơn bán hàng</FormControl.Label>
            </FormControl>
          );
        }}
      />

      <Controller
        control={control}
        name="is_sd_hoadon_banhang_bien_lai"
        rules={{
          validate: (data) => {
            return isValid;
          },
        }}
        render={({ field }) => {
          return (
            <FormControl>
              <Checkbox
                checked={field.value}
                onChange={(e) => {
                  field.onChange(e.target.checked);
                }}
              />
              <FormControl.Label>
                Hóa đơn bán hàng tích hợp biên lai thu thuế, phí, lệ phí
              </FormControl.Label>
            </FormControl>
          );
        }}
      />
      <Controller
        control={control}
        name="is_sd_hoadon_thuong_mai"
        rules={{
          validate: (data) => {
            return isValid;
          },
        }}
        render={({ field }) => {
          return (
            <FormControl>
              <Checkbox
                checked={field.value}
                onChange={(e) => {
                  field.onChange(e.target.checked);
                }}
              />
              <FormControl.Label>Hóa đơn thương mại</FormControl.Label>
            </FormControl>
          );
        }}
      />
      <Controller
        control={control}
        name="is_sd_chungtu_giong_hoadon"
        rules={{
          validate: (data) => {
            return isValid;
          },
        }}
        render={({ field }) => {
          return (
            <FormControl>
              <Checkbox
                checked={field.value}
                onChange={(e) => {
                  field.onChange(e.target.checked);
                }}
              />
              <FormControl.Label>
                Các loại chứng từ được in, phát hành, sử dụng và quản lý như hóa
                đơn
              </FormControl.Label>
            </FormControl>
          );
        }}
      />
      <Controller
        control={control}
        name="is_ban_tai_san_cong"
        rules={{
          validate: (data) => {
            return isValid;
          },
        }}
        render={({ field }) => {
          return (
            <FormControl>
              <Checkbox
                checked={field.value}
                onChange={(e) => {
                  field.onChange(e.target.checked);
                }}
              />
              <FormControl.Label>Hóa đơn bán tài sản công</FormControl.Label>
            </FormControl>
          );
        }}
      />
      <Controller
        control={control}
        name="is_ban_hang_du_tru_quoc_gia"
        rules={{
          validate: (data) => {
            return isValid;
          },
        }}
        render={({ field }) => {
          return (
            <FormControl>
              <Checkbox
                checked={field.value}
                onChange={(e) => {
                  field.onChange(e.target.checked);
                }}
              />
              <FormControl.Label>
                Hóa đơn bán hàng dữ trụ quốc gia
              </FormControl.Label>
            </FormControl>
          );
        }}
      />
      <Controller
        control={control}
        name="is_sd_hoadon_khac"
        rules={{
          validate: (data) => {
            return isValid;
          },
        }}
        render={({ field }) => {
          return (
            <FormControl>
              <Checkbox
                checked={field.value}
                onChange={(e) => {
                  field.onChange(e.target.checked);
                }}
              />
              <FormControl.Label>Các loại hóa đơn khác</FormControl.Label>
            </FormControl>
          );
        }}
      />

      {isValid !== true && (
        <FormControl.Validation variant="error" sx={{ mt: 1 }}>
          {isValid}
        </FormControl.Validation>
      )}
      {/* <Box>
                                        <CheckboxGroup>
                                            <FormControl>
                                                <Checkbox value="one" checked={isSuDungHoaDonGTGT} onChange={(e) => { setIsSuDungHoaDonGTGT(e.target.checked) }} />
                                                <FormControl.Label>Hóa đơn GTGT</FormControl.Label>
                                            </FormControl>
                                            <FormControl>
                                                <Checkbox value="two" checked={isSuDungHoaDonBanHang} onChange={(e) => { setIsSuDungHoaDonBanHang(e.target.checked) }} />
                                                <FormControl.Label>Hóa đơn bán hàng</FormControl.Label>
                                            </FormControl>
                                            <FormControl>
                                                <Checkbox value="three" checked={isSuDungHoaDonKhac} onChange={(e) => { setisSuDungHoaDonKhac(e.target.checked) }} />
                                                <FormControl.Label>Các loại hóa đơn khác</FormControl.Label>
                                            </FormControl>
                                            <FormControl>
                                                <Checkbox value="four" checked={isSuDungChungTuGiongHoaDon} onChange={(e) => { setIsSuDungChungTuGiongHoaDon(e.target.checked) }} />
                                                <FormControl.Label>Các loại chứng từ được in, phát hành, sử dụng và quản lý như hóa đơn</FormControl.Label>
                                            </FormControl>
                                            <FormControl>
                                                <Checkbox value="four" checked={isBanTaiSanCong} onChange={(e) => { setIsBanTaiSanCong(e.target.checked) }} />
                                                <FormControl.Label>Hóa đơn bán tài sản công </FormControl.Label>
                                            </FormControl>
                                            <FormControl>
                                                <Checkbox value="four" checked={isBanHangDuTruQuocGia} onChange={(e) => { setIsBanHangDuTruQuocGia(e.target.checked) }} />
                                                <FormControl.Label>Hóa đơn bán hàng dữ trụ quốc gia</FormControl.Label>
                                            </FormControl>
                                        </CheckboxGroup>
                                        {
                                            (!isSuDungHoaDonGTGT && !isSuDungHoaDonBanHang && !isSuDungHoaDonKhac && !isSuDungChungTuGiongHoaDon) &&
                                            <FormControl.Validation id={"phuong_thuc"} variant="error">
                                                Vui lòng chọn Loại hóa đơn sử dụng
                                            </FormControl.Validation>
                                        }
                                    </Box> */}
    </Box>
  );
};

export default ToKhaiFormLoaiHoaDon;
