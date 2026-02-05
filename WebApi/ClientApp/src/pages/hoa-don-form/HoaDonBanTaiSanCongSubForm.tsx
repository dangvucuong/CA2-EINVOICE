import { Box, FormControl } from "@primer/react";
import React from "react";
import Text from "../../component-ui/text";
import TextInput from "../../component-ui/text-input";
interface IHoaDonBanTaiSanCongSubFormProps {
  register: any;
  errors: any;
}
const HoaDonBanTaiSanCongSubForm = ({
  register,
  errors,
}: IHoaDonBanTaiSanCongSubFormProps) => {
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
            <Text text="Số quyết định" />
          </FormControl.Label>
          <TextInput
            register={register}
            name="SoQuyetDinh"
            block
            // required
            validateMessage="Vui lòng điền Số quyết định"
            errors={errors}
          />
        </FormControl>
        <FormControl sx={{ ml: [0, 3] }}>
          <FormControl.Label>
            <Text text="Ngày quyết định" />
          </FormControl.Label>
          <TextInput
            register={register}
            name="NgayQuyetDinh"
            type="date"
            // required
            block
            validateMessage="Vui lòng điền Ngày quyết định"
            errors={errors}
          />
        </FormControl>
        <FormControl sx={{ ml: [0, 3] }}>
          <FormControl.Label>
            <Text text="Cơ quan ban hành quyết định" />
          </FormControl.Label>
          <TextInput
            register={register}
            name="CoQuanBanHanhQD"
            // required
            block
            validateMessage="Vui lòng điền Cơ quan ban hành quyết định"
            errors={errors}
          />
        </FormControl>
      </Box>
      <Box
        sx={{
          display: "grid",
          gridTemplateColumns: ["1fr", "4fr 4fr 3fr 3fr"],
          gap: [2, 0],
        }}
      >
        <FormControl>
          <FormControl.Label>
            <Text text="Hình thức bán" />
          </FormControl.Label>
          <TextInput
            register={register}
            // name="xuat_kho_nguoi_xuat_hang"
            name="HinhThucBan"
            block
            errors={errors}
          />
        </FormControl>
        <FormControl sx={{ ml: [0, 3] }}>
          <FormControl.Label>
            <Text text="Địa điểm vận chuyển hàng đến" />
          </FormControl.Label>
          <TextInput
            register={register}
            name="DiaDiemVCHangDen"
            // required
            block
            validateMessage="Vui lòng điền Địa điểm vận chuyển hàng đến"
            errors={errors}
          />
        </FormControl>
        <FormControl sx={{ ml: [0, 3] }}>
          <FormControl.Label>
            <Text text="Thời gian vận chuyển hàng đến từ ngày" />
          </FormControl.Label>
          <TextInput
            register={register}
            name="TgianVCHangDenTu"
            // required
            block
            validateMessage="Vui lòng điền Thời gian vận chuyển hàng đến từ ngày"
            errors={errors}
            type="date"
          />
        </FormControl>

        <FormControl sx={{ ml: [0, 3] }}>
          <FormControl.Label>
            <Text text="Thời gian vận chuyển hàng đến đến ngày" />
          </FormControl.Label>
          <TextInput
            register={register}
            name="TgianVCHangDenDen"
            // required
            block
            validateMessage="Vui lòng điền Thời gian vận chuyển hàng đến đến ngày"
            errors={errors}
            type="date"
          />
        </FormControl>
      </Box>
    </Box>
  );
};

export default HoaDonBanTaiSanCongSubForm;
