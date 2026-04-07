import { Box, FormControl } from "@primer/react";
import React from "react";
import Text from "../../component-ui/text";
import TextInput from "../../component-ui/text-input";
interface IPhieuXuatKhoDaiLySubFormProps {
  register: any;
  errors: any;
}
const PhieuXuatKhoDaiLySubForm = ({
  register,
  errors,
}: IPhieuXuatKhoDaiLySubFormProps) => {
  return (
    <Box
      display={"grid"}
      sx={{
        gap: 2,
        mt: 2,
      }}
    >
      <Box
        sx={{
          display: "grid",
          gridTemplateColumns: ["1fr", "1fr 1fr 1fr"],
          gap: [2, 0],
        }}
      >
        <FormControl>
          <FormControl.Label>
            <Text text="Hợp đồng kinh tế số" />
          </FormControl.Label>
          <TextInput
            register={register}
            name="xuat_kho_dl_hop_dong_kinh_te_so"
            block
            required
            validateMessage="Vui lòng điền Số hợp đồng kinh tế"
            errors={errors}
          />
        </FormControl>
        <FormControl sx={{ ml: [0, 3] }}>
          <FormControl.Label>
            <Text text="Hợp đồng kinh tế ngày" />
          </FormControl.Label>
          <TextInput
            register={register}
            name="xuat_kho_dl_hop_dong_ngay"
            type="date"
            // required
            block
            validateMessage="Vui lòng điền Ngày hợp đồng kinh tế"
            errors={errors}
          />
        </FormControl>
        <FormControl sx={{ ml: [0, 3] }}>
          <FormControl.Label>
            <Text text="Hợp đồng vận chuyển số" />
          </FormControl.Label>
          <TextInput
            register={register}
            name="xuat_kho_hop_dong_so"
            // required
            block
            validateMessage="Vui lòng điền Email"
            errors={errors}
          />
        </FormControl>
      </Box>
      <Box
        sx={{
          display: "grid",
          gridTemplateColumns: ["1fr", "1fr 1fr 1fr"],
          gap: [2, 0],
        }}
      >
        <FormControl>
          <FormControl.Label>
            <Text text="Người xuất hàng" />
          </FormControl.Label>
          <TextInput
            register={register}
            name="xuat_kho_nguoi_xuat_hang"
            block
            errors={errors}
          />
        </FormControl>
        <FormControl sx={{ ml: [0, 3] }}>
          <FormControl.Label>
            <Text text="Người vận chuyển" />
          </FormControl.Label>
          <TextInput
            register={register}
            name="xuat_kho_nguoi_van_chuyen"
            // required
            block
            validateMessage="Vui lòng điền Email"
            errors={errors}
          />
        </FormControl>
        <FormControl sx={{ ml: [0, 3] }}>
          <FormControl.Label>
            <Text text="Phương tiện vận chuyển" />
          </FormControl.Label>
          <TextInput
            register={register}
            name="xuat_kho_phuong_tien_van_chuyen"
            required
            block
            validateMessage="Vui lòng điền Phương tiện vận chuyển"
            errors={errors}
          />
        </FormControl>

        <FormControl sx={{ mt: 2 }}>
          <FormControl.Label>
            <Text text="Xuất tại kho" />
          </FormControl.Label>
          <TextInput
            register={register}
            name="xuat_kho_dia_chi"
            // required
            block
            errors={errors}
          />
        </FormControl>
      </Box>
    </Box>
  );
};

export default PhieuXuatKhoDaiLySubForm;
