import { Box, Button, SelectPanel } from "@primer/react";
import { useEffect, useMemo, useState } from "react";
import { TriangleDownIcon, XCircleFillIcon } from "@primer/octicons-react";
import { useHoaDonDangKyPhatHanhLoader } from "../../hooks/useHoaDonDangKyPhatHanhLoader";
import { isDangKyPhatHanhMtt } from "../../utils/hoaDonKyHieu";

interface ISelectBoxMauSoPhatHanhProps {
  onValueChanged: (id: string) => void;
  value: string;
  maxWidth?: any;
  loai_hoa_don_ct_id: number;
  isAutoSelectIfHasOneItem: boolean;
  isShowClearBtn?: boolean;
  onlyMtt?: boolean;
}

const SelectBoxMauSoPhatHanh = (props: ISelectBoxMauSoPhatHanhProps) => {
  const [open, setOpen] = useState(false);
  const { hoaDonDangKyPhatHanhs, isLoading } = useHoaDonDangKyPhatHanhLoader();
  const [filter, setFilter] = useState("");
  const dataSource = useMemo(() => {
    var uniqueData = new Set();
    hoaDonDangKyPhatHanhs
      .sort((a, b) => b.id - a.id)
      .filter((x) => {
        if (x.loai_hoa_don_ct_id !== props.loai_hoa_don_ct_id) {
          return false;
        }
        if (props.onlyMtt) {
          return isDangKyPhatHanhMtt(x);
        }
        return true;
      })
      .map((x) => ({ id: x.mau_so, text: x.mau_so }))
      .forEach((item) => {
        uniqueData.add(JSON.stringify(item));
      });
    var result = Array.from(uniqueData).map((item: any) => JSON.parse(item));
    return result;
  }, [hoaDonDangKyPhatHanhs, props.loai_hoa_don_ct_id, props.onlyMtt]);
  const filterdData = useMemo(() => {
    return dataSource.filter((item) =>
      item.text.toLowerCase().includes(filter.toLowerCase())
    );
  }, [dataSource, filter]);
  const _selectedData = useMemo(() => {
    return dataSource.find((item) => item.id === props.value);
  }, [props.value, dataSource]);
  useEffect(() => {
    if (props.value === "" && props.isAutoSelectIfHasOneItem) {
      if (dataSource.length === 1) {
        props.onValueChanged(dataSource[0].id);
      }
    }
  }, [props.value, dataSource, props.isAutoSelectIfHasOneItem]);

  const placeholderText =
    isLoading && !props.loai_hoa_don_ct_id
      ? "Đang tải..."
      : props.loai_hoa_don_ct_id <= 0
        ? "Chọn loại HĐ trước"
        : "Chọn mẫu số";

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
            disabled={isLoading || props.loai_hoa_don_ct_id <= 0}
            {...anchorProps}
          >
            <p
              style={{
                maxWidth: props.maxWidth,
                overflow: "hidden",
                textOverflow: "ellipsis",
              }}
            >
              {children || placeholderText}
            </p>
          </Button>
        )}
        title={
          <>
            <Box sx={{ display: "flex", alignItems: "center" }}>
              <Box sx={{ flex: 1 }}>Chọn mẫu số</Box>
              {props.isShowClearBtn && props.value !== "" && (
                <Button
                  trailingVisual={XCircleFillIcon}
                  variant="invisible"
                  sx={{
                    color: "danger.emphasis",
                  }}
                  onClick={() => {
                    props.onValueChanged("");
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
        overlayProps={{ width: "auto", height: "medium" }}
      />
    </>
  );
};

export default SelectBoxMauSoPhatHanh;
