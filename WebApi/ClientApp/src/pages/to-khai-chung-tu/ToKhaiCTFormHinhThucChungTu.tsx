import { Box, Checkbox, FormControl } from "@primer/react";
import { useEffect, useMemo } from "react";
import {
  Control,
  Controller,
  UseFormSetValue,
  UseFormWatch,
} from "react-hook-form";
import { IToKhaiCT } from "./ToKhaiCTForm";

interface ToKhaiCTFormHinhThucChungTuProps {
  register: any;
  errors: any;
  control: Control<IToKhaiCT>;
  watch: UseFormWatch<IToKhaiCT>;
  setValue: UseFormSetValue<IToKhaiCT>;
}
const ToKhaiCTFormHinhThucChungTu = (
  props: ToKhaiCTFormHinhThucChungTuProps
) => {
  const { setValue, errors, control, watch } = props;
  const is_tren_cong_thong_tin_dien_tu_cua_cqt = watch(
    "is_tren_cong_thong_tin_dien_tu_cua_cqt"
  );
  const is_chuyen_du_lieu_qua_tctn = watch("is_chuyen_du_lieu_qua_tctn");

  const is_chuyen_du_lieu_qua_tctn_duoc_uy_thac = watch(
    "is_chuyen_du_lieu_qua_tctn_duoc_uy_thac"
  );

  const isValid = useMemo(() => {
    if (
      !is_tren_cong_thong_tin_dien_tu_cua_cqt &&
      !is_chuyen_du_lieu_qua_tctn &&
      !is_chuyen_du_lieu_qua_tctn_duoc_uy_thac
    ) {
      return "Chọn tối thiểu một hình thức";
    }
    return true;
  }, [
    is_tren_cong_thong_tin_dien_tu_cua_cqt,
    is_chuyen_du_lieu_qua_tctn,
    is_chuyen_du_lieu_qua_tctn_duoc_uy_thac,
  ]);

  return (
    <Box>
      <Controller
        control={control}
        defaultValue={false}
        name="is_tren_cong_thong_tin_dien_tu_cua_cqt"
        rules={{
          validate: (data) => {
            return isValid;
          },
        }}
        render={({ field }) => {
          return (
            <FormControl>
              <Checkbox
                value="one"
                checked={field.value}
                onChange={(e) => {
                  field.onChange(e.target.checked);
                }}
              />
              <FormControl.Label>
                Trên Cổng thông tin điện tử của cơ quan thuế
              </FormControl.Label>
            </FormControl>
          );
        }}
      />
      <Controller
        control={control}
        defaultValue={false}
        name="is_chuyen_du_lieu_qua_tctn"
        rules={{
          validate: (data) => {
            return isValid;
          },
        }}
        render={({ field }) => {
          return (
            <FormControl>
              <Checkbox
                value="one"
                checked={field.value}
                onChange={(e) => {
                  field.onChange(e.target.checked);
                }}
              />
              <FormControl.Label>
                Chuyển dữ liệu qua TCTN (Thông qua tổ chức cung cấp dịch vụ hóa
                đơn điện tử)
              </FormControl.Label>
            </FormControl>
          );
        }}
      />
      <Controller
        control={control}
        defaultValue={false}
        name="is_chuyen_du_lieu_qua_tctn_duoc_uy_thac"
        rules={{
          validate: (data) => {
            return isValid;
          },
        }}
        render={({ field }) => {
          return (
            <FormControl>
              <Checkbox
                value="one"
                checked={field.value}
                onChange={(e) => {
                  field.onChange(e.target.checked);
                }}
              />
              <FormControl.Label>
                Chuyển dữ liệu qua TCTN được ủy thác (Thông qua tổ chức cung cấp
                dịch vụ hóa đơn điện tử được Tổng cục Thuế ủy thác)
              </FormControl.Label>
              {errors && errors["is_hoadon_co_ma_cqt_mtt"] && (
                <FormControl.Validation variant="error"></FormControl.Validation>
              )}
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

export default ToKhaiCTFormHinhThucChungTu;
