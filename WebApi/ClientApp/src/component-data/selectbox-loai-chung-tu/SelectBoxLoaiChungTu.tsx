import { TriangleDownIcon, XCircleFillIcon } from "@primer/octicons-react";

import { Box, Button, SelectPanel } from "@primer/react";
import { useState } from "react";

interface ISelectBoxLoaiChungTuProps {
  onValueChanged: (id: number) => void;
  value: number;
  maxWidth?: any;
  isShowClearBtn?: boolean;
  isOnlyShowDaThietLapMau?: boolean;
}

const SelectBoxLoaiChungTu = (props: ISelectBoxLoaiChungTuProps) => {
  const [open, setOpen] = useState(false);
  const filterdData = [
    { id: 1, text: "Chứng từ khấu trừ thuế thu nhập cá nhân theo ND70" },
  ];

  return (
    <>
      <SelectPanel
        renderAnchor={({
          children,
          "aria-labelledby": ariaLabelledBy,
          ...anchorProps
        }) => (
          <Button
            sx={{
              maxWidth: 300,
            }}
            trailingAction={TriangleDownIcon}
            aria-labelledby={` ${ariaLabelledBy}`}
            {...anchorProps}
          >
            <p
              style={{
                maxWidth: props.maxWidth,
                overflow: "hidden",
                textOverflow: "ellipsis",
              }}
            >
              {children || "Chọn loại chứng từ"}
            </p>
          </Button>
        )}
        title={
          <>
            <Box sx={{ display: "flex", alignItems: "center" }}>
              <Box sx={{ flex: 1 }}>Chọn loại chứng từ</Box>
              {props.isShowClearBtn && props.value > 0 && (
                <Button
                  trailingVisual={XCircleFillIcon}
                  variant="invisible"
                  sx={{
                    color: "danger.emphasis",
                  }}
                  onClick={() => {
                    props.onValueChanged(0);
                  }}
                >
                  Bỏ chọn
                </Button>
              )}
            </Box>
          </>
        }
        placeholderText="Search"
        open={open}
        onOpenChange={setOpen}
        items={filterdData}
        selected={filterdData[0]}
        onSelectedChange={(data: any) => {
          props.onValueChanged(1);
        }}
        onFilterChange={() => {}}
        showItemDividers={true}
        overlayProps={{ width: "medium", height: "medium" }}
      />
    </>
  );
};

export default SelectBoxLoaiChungTu;
