import { Box, Label } from "@primer/react";
import { useMemo } from "react";
interface IToKhaiStatusProps {
  id: number;
}
export const toKhaiStatus = [
  {
    id: 1,
    name: "Tạo mới",
    name_en: "Tạo mới",
    color: "#ffd78e",
  },
  {
    id: 2,
    name: "Đã ký số",
    name_en: "Chờ cơ quan thuế",
    color: "#a2eeef",
  },
  {
    id: 3,
    name: "Đã gửi cơ quan thuế",
    name_en: "Cơ quan thuế từ chối",
    color: "#0cf478",
  },
];
const ToKhaiChungTuStatus = (props: IToKhaiStatusProps) => {
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

export default ToKhaiChungTuStatus;
