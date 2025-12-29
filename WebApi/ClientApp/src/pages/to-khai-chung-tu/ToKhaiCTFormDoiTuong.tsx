import { Box, Checkbox, FormControl } from "@primer/react";
import { useEffect, useMemo } from "react";
import {
  Control,
  Controller,
  UseFormSetValue,
  UseFormWatch,
} from "react-hook-form";
import { IToKhaiCT } from "./ToKhaiCTForm";

interface ToKhaiCTFormDoiTuongProps {
  register: any;
  errors: any;
  control: Control<IToKhaiCT>;
  watch: UseFormWatch<IToKhaiCT>;
  setValue: UseFormSetValue<IToKhaiCT>;
}

const ToKhaiCTFormDoiTuong = (props: ToKhaiCTFormDoiTuongProps) => {
  const { setValue, errors, control, watch } = props;
  const is_to_chuc_ca_nhan_phat_hanh = watch("is_to_chuc_ca_nhan_phat_hanh");
  const is_co_quan_thue_phat_hanh = watch("is_co_quan_thue_phat_hanh");

  const isValid = useMemo(() => {
    if (!is_to_chuc_ca_nhan_phat_hanh && !is_co_quan_thue_phat_hanh) {
      return "Chọn tối thiểu một đối tượng sử dụng";
    }
    return true;
  }, [is_to_chuc_ca_nhan_phat_hanh, is_co_quan_thue_phat_hanh]);

  return (
    <Box>
      <Controller
        control={control}
        defaultValue={false}
        name="is_to_chuc_ca_nhan_phat_hanh"
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
              <FormControl.Label>Tổ chức, cá nhân phát hành</FormControl.Label>
            </FormControl>
          );
        }}
      />
      <Controller
        control={control}
        defaultValue={false}
        name="is_co_quan_thue_phat_hanh"
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
              <FormControl.Label>Cơ quan thuế phát hành</FormControl.Label>
              {errors && errors["is_co_quan_thue_phat_hanh"] && (
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

export default ToKhaiCTFormDoiTuong;
