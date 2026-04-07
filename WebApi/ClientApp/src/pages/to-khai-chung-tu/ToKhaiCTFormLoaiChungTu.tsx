import { Box, Checkbox, FormControl } from "@primer/react";
import { useMemo } from "react";
import {
  Control,
  Controller,
  UseFormSetValue,
  UseFormWatch,
} from "react-hook-form";
import { IToKhaiCT } from "./ToKhaiCTForm";

interface ToKhaiCTFormLoaiChungTuProps {
  register: any;
  errors: any;
  control: Control<IToKhaiCT>;
  watch: UseFormWatch<IToKhaiCT>;
  setValue: UseFormSetValue<IToKhaiCT>;
}

const ToKhaiCTFormLoaiChungTu = (props: ToKhaiCTFormLoaiChungTuProps) => {
  const { setValue, errors, control, watch } = props;
  const is_chung_tu_dien_tu_khau_tru_tncn = watch(
    "is_chung_tu_dien_tu_khau_tru_tncn"
  );
  const is_chung_tu_thue_thuong_mai_dien_tu = watch(
    "is_chung_tu_thue_thuong_mai_dien_tu"
  );
  const is_bien_thu_thue_phi_le_phi_khong_in_san_menh_gia = watch(
    "is_bien_thu_thue_phi_le_phi_khong_in_san_menh_gia"
  );
  const is_bien_thu_thue_phi_le_phi_in_san_menh_gia = watch(
    "is_bien_thu_thue_phi_le_phi_in_san_menh_gia"
  );
  const is_bien_lai_thu_thue_phi_le_phi_ctt50 = watch(
    "is_bien_lai_thu_thue_phi_le_phi_ctt50"
  );

  const isValid = useMemo(() => {
    if (
      !is_chung_tu_dien_tu_khau_tru_tncn &&
      !is_chung_tu_thue_thuong_mai_dien_tu &&
      !is_bien_thu_thue_phi_le_phi_khong_in_san_menh_gia &&
      !is_bien_thu_thue_phi_le_phi_in_san_menh_gia &&
      !is_bien_lai_thu_thue_phi_le_phi_ctt50
    ) {
      return "Chọn tối thiểu 1 loại chứng từ điện tử";
    }
    return true;
  }, [
    is_chung_tu_dien_tu_khau_tru_tncn,
    is_chung_tu_thue_thuong_mai_dien_tu,
    is_bien_thu_thue_phi_le_phi_khong_in_san_menh_gia,
    is_bien_thu_thue_phi_le_phi_in_san_menh_gia,
    is_bien_lai_thu_thue_phi_le_phi_ctt50,
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
        defaultValue={false}
        name="is_chung_tu_dien_tu_khau_tru_tncn"
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
                Chứng từ điện tử khấu trừ thuế thu nhập cá nhân
              </FormControl.Label>
            </FormControl>
          );
        }}
      />
      <Controller
        control={control}
        defaultValue={false}
        name="is_chung_tu_thue_thuong_mai_dien_tu"
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
                Chứng từ khấu trừ thuế thương mại điện tử
              </FormControl.Label>
            </FormControl>
          );
        }}
      />
      <Controller
        control={control}
        defaultValue={false}
        name="is_bien_thu_thue_phi_le_phi_khong_in_san_menh_gia"
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
                Biên lai thu thuế, phí, lệ phí không in sẵn mệnh giá
              </FormControl.Label>
            </FormControl>
          );
        }}
      />

      <Controller
        control={control}
        defaultValue={false}
        name="is_bien_thu_thue_phi_le_phi_in_san_menh_gia"
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
                Biên lai thu thuế, phí, lệ phí in sẵn mệnh giá
              </FormControl.Label>
            </FormControl>
          );
        }}
      />
      <Controller
        control={control}
        defaultValue={false}
        name="is_bien_lai_thu_thue_phi_le_phi_ctt50"
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
                Biên lai thu thuế, phí, lệ phí (CTT50)
              </FormControl.Label>
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

export default ToKhaiCTFormLoaiChungTu;
