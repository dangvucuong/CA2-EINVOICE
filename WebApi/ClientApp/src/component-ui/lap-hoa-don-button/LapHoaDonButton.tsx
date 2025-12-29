import { Box } from "@primer/react";
import { PlusIcon } from "@primer/octicons-react";
import Button from "../button";
import { memo, useEffect, useMemo, useState } from "react";
import { useAppSelector } from "../../hooks/useAppSelector";
import { useDispatch } from "react-redux";
import { eReducerStatusBase } from "../../state/reducer-models/eReducerStatusBase";
import { rootAction } from "../../state/actions/rootAction";
import { useHistory } from "react-router-dom";

function LapHoaDonButton() {
  const [open, setOpen] = useState(false);
  const history = useHistory();

  const { hoaDonDangKyPhatHanhs, status } = useAppSelector(
    (x) => x.hoaDon.hoaDonDangKyPhatHanhReducer
  );
  const dispatch = useDispatch();

  const dataSource = useMemo(() => {
    var uniqueData = new Set();
    hoaDonDangKyPhatHanhs
      .map((x) => ({ id: x.loai_hoa_don_ct_id, text: x.ten_hoa_don }))
      .forEach((item) => {
        uniqueData.add(JSON.stringify(item));
      });

    const result = Array.from(uniqueData)
      .map((item: any) => JSON.parse(item))
      .sort((a, b) => a.id - b.id);

    return result;
  }, [hoaDonDangKyPhatHanhs]);

  useEffect(() => {
    if (status === eReducerStatusBase.is_not_initialization) {
      dispatch(rootAction.hoaDon.hoaDonDangKyPhatHanhAction.loadStart());
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [status]);

  return (
    <Box
      sx={{ position: "relative", display: "inline-block" }}
      onMouseEnter={() => setOpen(true)}
      onMouseLeave={() => setOpen(false)}
    >
      {/* Khi hover vào btn thì hiện ra dropdown để chon loại hóa đơn */}
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
          }}
        >
          {dataSource?.map((op) => (
            <Box
              key={op?.value}
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
