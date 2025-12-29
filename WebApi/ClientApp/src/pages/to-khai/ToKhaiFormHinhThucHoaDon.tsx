import { Box, Checkbox, FormControl } from "@primer/react";
import { useEffect, useMemo } from "react";
import {
  Control,
  Controller,
  UseFormSetValue,
  UseFormWatch,
} from "react-hook-form";
import { IToKhai } from "../../models/responses/to-khai/IToKhai";
interface ToKhaiFormHinhThucHoaDonProps {
  register: any;
  errors: any;
  control: Control<IToKhai, any>;
  watch: UseFormWatch<IToKhai>;
  setValue: UseFormSetValue<IToKhai>;
}
const ToKhaiFormHinhThucHoaDon = (props: ToKhaiFormHinhThucHoaDonProps) => {
  const { setValue, errors, control, watch } = props;
  const is_hoadon_co_ma_cqt = watch("is_hoadon_co_ma_cqt");
  const is_hoadon_co_ma_cqt_mtt = watch("is_hoadon_co_ma_cqt_mtt");
  const is_hoadon_khong_co_ma_cqt = watch("is_hoadon_khong_co_ma_cqt");

  const isValid = useMemo(() => {
    if (
      (is_hoadon_co_ma_cqt === true || is_hoadon_co_ma_cqt_mtt === true) &&
      is_hoadon_khong_co_ma_cqt === true
    ) {
      return "Không thể lựa chọn có mã và không có mã đồng thời";
    }
    if (
      !is_hoadon_co_ma_cqt &&
      !is_hoadon_co_ma_cqt_mtt &&
      !is_hoadon_khong_co_ma_cqt
    ) {
      return "Chọn tối thiểu một hình thức";
    }
    return true;
  }, [is_hoadon_co_ma_cqt, is_hoadon_co_ma_cqt_mtt, is_hoadon_khong_co_ma_cqt]);
  useEffect(() => {
    if (is_hoadon_khong_co_ma_cqt === true) {
      setValue("is_doanh_nghiep_vvn_khac", false);
      setValue("is_doanh_nghiep_vvn_kho_khan", false);
      setValue("is_khong_phai_tra_tien_dich_vu", false);
    } else {
      setValue("is_chuyen_lieu_thong_qua_to_chuc", false);
      setValue("is_chuyen_du_lieu_truc_tiep", false);
    }
  }, [is_hoadon_khong_co_ma_cqt]);
  // console.log({
  //     is_hoadon_co_ma_cqt,
  //     is_hoadon_co_ma_cqt_mtt,
  //     is_hoadon_khong_co_ma_cqt,
  //     isValid
  // });
  return (
    <Box>
      <Controller
        control={control}
        name="is_hoadon_co_ma_cqt"
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
              <FormControl.Label>Có mã của cơ quan thuế</FormControl.Label>
            </FormControl>
          );
        }}
      />
      <Controller
        control={control}
        name="is_hoadon_co_ma_cqt_mtt"
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
                Có mã của cơ quan thuế (Khởi tạo từ máy tính tiền)
              </FormControl.Label>
              {errors && errors["is_hoadon_co_ma_cqt_mtt"] && (
                <FormControl.Validation variant="error"></FormControl.Validation>
              )}
            </FormControl>
          );
        }}
      />
      <Controller
        control={control}
        name="is_hoadon_khong_co_ma_cqt"
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
                Không có mã của cơ quan thuế
              </FormControl.Label>
              {errors && errors["is_hoadon_khong_co_ma_cqt"] && (
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

export default ToKhaiFormHinhThucHoaDon;
