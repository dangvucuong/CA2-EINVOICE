import { Box, Checkbox, FormControl } from "@primer/react";
import { useMemo } from "react";
import {
  Control,
  Controller,
  UseFormSetValue,
  UseFormWatch,
} from "react-hook-form";
import { IToKhai } from "../../models/responses/to-khai/IToKhai";
interface ToKhaiFormPhuongThucChuyenDuLieuHoaDonProps {
  register: any;
  errors: any;
  control: Control<IToKhai, any>;
  watch: UseFormWatch<IToKhai>;
  setValue: UseFormSetValue<IToKhai>;
  is_hoadon_co_ma_cqt?: boolean;
}
const ToKhaiFormPhuongThucChuyenDuLieuHoaDon = (
  props: ToKhaiFormPhuongThucChuyenDuLieuHoaDonProps
) => {
  const { setValue, errors, control, watch } = props;
  const is_chuyen_day_du_tung_hoadon = watch("is_chuyen_day_du_tung_hoadon");
  const is_chuyen_theo_bang_tonghop = watch("is_chuyen_theo_bang_tonghop");
  const is_hoadon_co_ma_cqt = watch("is_hoadon_co_ma_cqt");
  const is_hoadon_co_ma_cqt_mtt = watch("is_hoadon_co_ma_cqt_mtt");

  const isValid = useMemo(() => {
    if (!is_chuyen_day_du_tung_hoadon && !is_chuyen_theo_bang_tonghop) {
      return "Chọn tối thiểu một phương thức";
    }
    return true;
  }, [is_chuyen_day_du_tung_hoadon, is_chuyen_theo_bang_tonghop]);
  return (
    <Box
      sx={{
        display: "grid",
        gap: 2,
      }}
    >
      <Controller
        control={control}
        name="is_chuyen_day_du_tung_hoadon"
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
                Chuyển đầy đủ nội dung từng hóa đơn
              </FormControl.Label>
            </FormControl>
          );
        }}
      />
      <Controller
        control={control}
        name="is_chuyen_theo_bang_tonghop"
        rules={{
          validate: (data) => {
            return isValid;
          },
        }}
        render={({ field }) => {
          return (
            <FormControl
              disabled={is_hoadon_co_ma_cqt || is_hoadon_co_ma_cqt_mtt}
            >
              <Checkbox
                checked={field.value}
                onChange={(e) => {
                  field.onChange(e.target.checked);
                }}
              />
              <FormControl.Label>
                Chuyển theo bảng tổng hợp dữ liệu hóa đơn điện tử
              </FormControl.Label>
              <FormControl.Caption>
                (Theo Điểm a1, Khoản 3, Điều 22 của Nghị định)
              </FormControl.Caption>
            </FormControl>
          );
        }}
      />
      {isValid !== true && (
        <FormControl.Validation variant="error" sx={{ mt: 1 }}>
          {isValid}
        </FormControl.Validation>
      )}
    </Box>
  );
};

export default ToKhaiFormPhuongThucChuyenDuLieuHoaDon;
