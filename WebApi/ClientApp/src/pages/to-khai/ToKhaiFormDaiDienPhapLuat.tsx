import { Box, FormControl, Radio } from "@primer/react";
import Text from "../../component-ui/text";
import TextInput from "../../component-ui/text-input";
import { Control, Controller } from "react-hook-form";
import { IToKhai } from "../../models/responses/to-khai/IToKhai";
import DateInput from "../../component-ui/date-input";
import moment from "moment";
interface IToKhaiFormDaiDienPhapLuatProps {
  register: any;
  errors: any;
  control: Control<IToKhai, any>;
}
const ToKhaiFormDaiDienPhapLuat = (props: IToKhaiFormDaiDienPhapLuatProps) => {
  const { register, errors, control } = props;
  // const { control, register, handleSubmit, formState: { errors } } = useFormContext<IToKhai>();
  return (
    <Box sx={{ display: "grid", gap: 2, mt: 3 }}>
      <Text
        text="Đại diện pháp luật"
        sx={{
          fontSize: 16,
          fontWeight: 600,
        }}
      />
      <Box sx={{ display: "grid", gridTemplateColumns: "1fr 1fr 1fr", gap: 2 }}>
        <FormControl>
          <FormControl.Label>Họ tên</FormControl.Label>
          <TextInput
            register={register}
            name="dai_dien_phap_luat_ho_ten"
            block
            required
            errors={errors}
            validateMessage="Vui lòng điện Họ tên người đại diện pháp luật"
          />
        </FormControl>
        <FormControl>
          <FormControl.Label>Điện thoại</FormControl.Label>
          <TextInput
            register={register}
            name="dai_dien_phap_luat_dien_thoai"
            block
            required
            errors={errors}
            validateMessage="Vui lòng điện Điện thoại người đại diện pháp luật"
          />
        </FormControl>
        <FormControl>
          <FormControl.Label>Căn cước công dân</FormControl.Label>
          <TextInput
            register={register}
            name="dai_dien_phap_luat_dien_cccd"
            block
            // required
            errors={errors}
            // validateMessage="Vui lòng điện Số CCCD, định danh người đại diện pháp luật"
          />
        </FormControl>
      </Box>
      <Box sx={{ display: "grid", gridTemplateColumns: "1fr 1fr 1fr", gap: 2 }}>
        <Controller
          control={control}
          name="dai_dien_phap_luat_dien_ngay_sinh"
          rules={{
            required: true,
          }}
          render={({ field }) => {
            return (
              <FormControl>
                <FormControl.Label>Ngày sinh</FormControl.Label>
                <DateInput
                  value={
                    field.value
                      ? moment(field.value).format("DD/MM/YYYY")
                      : undefined
                  }
                  onValueChanged={(text, date) => {
                    if (date) {
                      field.onChange(moment(date).format("YYYY-MM-DD"));
                    }
                  }}
                />
                {errors && errors["dai_dien_phap_luat_dien_ngay_sinh"] && (
                  <FormControl.Validation variant="error">
                    Vui lòng điền ngày sinh
                  </FormControl.Validation>
                )}
              </FormControl>
            );
          }}
        />

        <Controller
          control={control}
          name="dai_dien_phap_luat_dien_gioi_tinh"
          rules={{
            required: true,
          }}
          render={({ field }) => {
            return (
              <FormControl>
                <FormControl.Label>Giới tính</FormControl.Label>
                <Box
                  sx={{
                    display: "grid",
                    gap: 2,
                    gridTemplateColumns: "1fr 1fr",
                    pt: 1,
                  }}
                >
                  <FormControl>
                    <Radio
                      value="0"
                      checked={field.value === 0}
                      onChange={(e) => {
                        if (e.target.checked) {
                          field.onChange(0);
                        }
                      }}
                    />
                    <FormControl.Label>Nữ</FormControl.Label>
                  </FormControl>
                  <FormControl>
                    <Radio
                      value="1"
                      checked={field.value === 1}
                      onChange={(e) => {
                        if (e.target.checked) {
                          field.onChange(1);
                        }
                      }}
                    />
                    <FormControl.Label>Nam</FormControl.Label>
                  </FormControl>
                </Box>
                {errors && errors["dai_dien_phap_luat_dien_gioi_tinh"] && (
                  <FormControl.Validation variant="error">
                    Vui lòng điền giới tính
                  </FormControl.Validation>
                )}
              </FormControl>
            );
          }}
        />
      </Box>
    </Box>
  );
};

export default ToKhaiFormDaiDienPhapLuat;
