import { Box, Label } from "@primer/react";
import { useMemo } from "react";
import { eToKhaiStatus } from "../../models/commons/eToKhaiStatus";
interface IToKhaiStatusProps {
  id: number;
}
export const toKhaiStatus = [
  {
    id: eToKhaiStatus.TAO_MOI,
    name: "Tạo mới",
    name_en: "Tạo mới",
    color: "#ffd78e",
  },
  {
    id: eToKhaiStatus.CHO_CQT,
    name: "CQT đã tiếp nhận",
    name_en: "CQT đã tiếp nhận",
    color: "#a2eeef",
  },
  {
    id: eToKhaiStatus.CQT_TU_CHOI,
    name: "Cơ quan thuế từ chối",
    name_en: "Cơ quan thuế từ chối",
    color: "#d73a4a",
  },
  {
    id: eToKhaiStatus.CQT_DONG_Y,
    name: "Cơ quan thuế đồng ý",
    name_en: "Cơ quan thuế đồng ý",
    color: "#0cf478",
  },
  {
    id: eToKhaiStatus.CQT_TIEP_NHAN,
    name: "CQT tiếp nhận",
    name_en: "CQT tiếp nhận",
    color: "#4c6ef5",
  },

  {
    id: eToKhaiStatus.DA_GUI_CQT,
    name: "Đã gửi CQT",
    name_en: "Đã gửi CQT",
    color: "#a2eeef",
  },
  {
    id: eToKhaiStatus.LOI_THONG_DIEP,
    name: "Lỗi thông điệp",
    name_en: "Lỗi thông điệp",
    color: "#d73a4a",
  },
];

const ToKhaiStatus = (props: IToKhaiStatusProps) => {
  const status = useMemo(() => {
    return toKhaiStatus.find((x) => x.id === props.id);
  }, [props.id]);
  return (
    <Box
      sx={{
        display: "flex",
        alignItems: "center",
      }}
    >
      <Label>
        <Box
          sx={{
            mr: 1,
          }}
          color={status?.color}
        >
          <Box
            bg={status?.color}
            borderColor={status?.color}
            width={12}
            height={12}
            borderRadius={10}
            margin="auto"
            borderWidth="1px"
            borderStyle="solid"
          />
        </Box>
        <Box>{status?.name}</Box>
      </Label>
    </Box>
  );
};

export default ToKhaiStatus;
