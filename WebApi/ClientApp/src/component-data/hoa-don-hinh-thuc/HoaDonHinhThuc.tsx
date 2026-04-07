import { Box, Label } from "@primer/react";
import { useMemo } from "react";
import { eToKhaiStatus } from "../../models/commons/eToKhaiStatus";
import { useHoaDonHinhThuc } from "../../hooks/useHoaDonHinhThuc";
interface IHoaDonHinhThucProps {
  id: number;
}
// export const hoaDonHinhThucStatus = [
//     {
//         id: eToKhaiStatus.TAO_MOI,
//         name: "Tạo mới",
//         name_en: "Tạo mới",
//         color: "#ffd78e"
//     },
//     {
//         id: eToKhaiStatus.CHO_CQT,
//         name: "Chờ cơ quan thuế",
//         name_en: "Chờ cơ quan thuế",
//         color: "#a2eeef"
//     },
//     {
//         id: eToKhaiStatus.CQT_TU_CHOI,
//         name: "Cơ quan thuế từ chối",
//         name_en: "Cơ quan thuế từ chối",
//         color: "#d73a4a"
//     },
//     {
//         id: eToKhaiStatus.CQT_DONG_Y,
//         name: "Cơ quan thuế đồng ý",
//         name_en: "Cơ quan thuế đồng ý",
//         color: "#0cf478"
//     }
// ]
const HoaDonHinhThuc = (props: IHoaDonHinhThucProps) => {
  const { hoaDonHinhThuc } = useHoaDonHinhThuc(props.id);
  return (
    <Box
      sx={{
        display: "flex",
        alignItems: "center",
      }}
    >
      {hoaDonHinhThuc && (
        <Label>
          <Box
            sx={{
              mr: 1,
            }}
            color={hoaDonHinhThuc.color}
          >
            <Box
              bg={hoaDonHinhThuc.color}
              borderColor={hoaDonHinhThuc.color}
              width={12}
              height={12}
              borderRadius={10}
              margin="auto"
              borderWidth="1px"
              borderStyle="solid"
            />
          </Box>
          <Box sx={{ fontSize: "11px" }}>{hoaDonHinhThuc.name}</Box>
        </Label>
      )}
    </Box>
  );
};

export default HoaDonHinhThuc;
