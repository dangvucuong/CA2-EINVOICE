import { Box } from "@primer/react";
import { PlusIcon } from "@primer/octicons-react";
import Button from "../button";
import { memo, useMemo, useState } from "react";
import { useHistory } from "react-router-dom";
import { useHoaDonDangKyPhatHanhLoader } from "../../hooks/useHoaDonDangKyPhatHanhLoader";
import { isDangKyPhatHanhInCurrentYear } from "../../utils/dangKyPhatHanhFilter";

function LapHoaDonButton() {
  const [open, setOpen] = useState(false);
  const history = useHistory();

  const { hoaDonDangKyPhatHanhs, isLoading } = useHoaDonDangKyPhatHanhLoader();

  const dataSource = useMemo(() => {
    var uniqueData = new Set();
    hoaDonDangKyPhatHanhs
      .filter((x) => isDangKyPhatHanhInCurrentYear(x?.ngay_su_dung))
      .map((x) => ({ id: x.loai_hoa_don_ct_id, text: x.ten_hoa_don }))
      .forEach((item) => {
        uniqueData.add(JSON.stringify(item));
      });

    const result = Array.from(uniqueData)
      .map((item: any) => JSON.parse(item))
      .sort((a, b) => a.id - b.id);

    return result;
  }, [hoaDonDangKyPhatHanhs]);

  return (
    <Box
      sx={{ position: "relative", display: "inline-block" }}
      onMouseEnter={() => setOpen(true)}
      onMouseLeave={() => setOpen(false)}
    >
      <Button
        text="Lập hóa đơn mới"
        leadingVisual={PlusIcon}
        variant="primary"
        size="medium"
      />

      {open && (
        <Box
          role="menu"
          sx={{
            position: "absolute",
            top: "100%",
            left: 0,
            mt: 0,
            bg: "canvas.default",
            border: "1px solid",
            borderColor: "border.default",
            borderRadius: 6,
            boxShadow: "shadow.large",
            zIndex: 1000,
            overflow: "hidden",
            minWidth: 200,
          }}
        >
          {isLoading && (
            <Box sx={{ px: 3, py: 2, fontSize: 14, color: "fg.muted" }}>
              Đang tải...
            </Box>
          )}
          {!isLoading && dataSource.length === 0 && (
            <Box sx={{ px: 3, py: 2, fontSize: 14, color: "fg.muted" }}>
              Chưa có đăng ký phát hành
            </Box>
          )}
          {dataSource?.map((op) => (
            <Box
              key={op?.id}
              role="menuitem"
              onClick={() => {
                setOpen(false);
                history.push({
                  pathname: "../../hoa-don/form/0",
                  state: { from: "header", value: op?.id },
                });
              }}
              sx={{
                px: 3,
                py: 2,
                cursor: "pointer",
                fontSize: 14,
                "&:hover": { bg: "neutral.subtle" },
              }}
            >
              {op?.text}
            </Box>
          ))}
        </Box>
      )}
    </Box>
  );
}

export default memo(LapHoaDonButton);
