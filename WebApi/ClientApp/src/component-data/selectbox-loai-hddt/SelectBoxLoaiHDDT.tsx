import { TriangleDownIcon, XCircleFillIcon } from "@primer/octicons-react";

import { Box, Button, SelectPanel } from "@primer/react";
import { useMemo, useState } from "react";
import { useLoaiHoaDonDienTusHook } from "../../hooks/useLoaiHoaDonDienTuHook";
interface ISelectBoxLoaiHDDTProps {
  onValueChanged: (id: number, data?: any) => void;
  value: number;
  maxWidth?: any;
  isShowClearBtn?: boolean;
}

const SelectBoxLoaiHDDT = (props: ISelectBoxLoaiHDDTProps) => {
  const [open, setOpen] = useState(false);
  const [filter, setFilter] = useState("");
  const { loaiHoaDonTienTus } = useLoaiHoaDonDienTusHook();

  const dataSource = useMemo(() => {
    return loaiHoaDonTienTus.map((x) => ({ id: x.id, text: x.name }));
  }, [loaiHoaDonTienTus]);
  const filterdData = useMemo(() => {
    return dataSource.filter((item) =>
      item.text.toLowerCase().includes(filter.toLowerCase())
    );
  }, [dataSource, filter]);
  const _selectedData = useMemo(() => {
    return dataSource.find((item) => item.id === props.value);
  }, [props.value, dataSource]);

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
              {children || "Chọn  loại hóa đơn điện tử"}
            </p>
          </Button>
        )}
        title={
          <>
            <Box sx={{ display: "flex", alignItems: "center" }}>
              <Box sx={{ flex: 1 }}>Chọn loại hóa đơn điện tử</Box>
              {props.isShowClearBtn && props.value > 0 && (
                <Button
                  trailingVisual={XCircleFillIcon}
                  variant="invisible"
                  sx={{
                    color: "danger.emphasis",
                  }}
                  onClick={() => {
                    props.onValueChanged(0, undefined);
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
        selected={_selectedData}
        onSelectedChange={(data: any) => {
          if (data) {
            props.onValueChanged(data.id);
          }
        }}
        onFilterChange={setFilter}
        showItemDividers={true}
        overlayProps={{ width: "large", height: "medium" }}
      />
    </>
  );
};

export default SelectBoxLoaiHDDT;
