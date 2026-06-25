import { TriangleDownIcon, XCircleFillIcon } from "@primer/octicons-react";

import { Box, Button, SelectPanel } from "@primer/react";
import { useMemo, useState } from "react";
import { useHoaDonDangKyPhatHanhLoader } from "../../hooks/useHoaDonDangKyPhatHanhLoader";
import { isDangKyPhatHanhMtt } from "../../utils/hoaDonKyHieu";
import { isDangKyPhatHanhInCurrentYear } from "../../utils/dangKyPhatHanhFilter";

interface ISelectBoxLoaiHoaDonCTPhatHanhProps {
  onValueChanged: (id: number) => void;
  value: number;
  maxWidth?: any;
  isShowClearBtn?: boolean;
  /** Chỉ hiển thị loại HĐ có đăng ký phát hành ký hiệu MTT */
  onlyMtt?: boolean;
}

const SelectBoxLoaiHoaDonCTPhatHanh = (
  props: ISelectBoxLoaiHoaDonCTPhatHanhProps
) => {
  const [open, setOpen] = useState(false);
  const { hoaDonDangKyPhatHanhs, isLoading, isLoadError } =
    useHoaDonDangKyPhatHanhLoader();
  const [filter, setFilter] = useState("");
  const dataSource = useMemo(() => {
    var uniqueData = new Set();
    hoaDonDangKyPhatHanhs
      .filter((x) => {
        if (!isDangKyPhatHanhInCurrentYear(x?.ngay_su_dung)) {
          return false;
        }
        if (props.onlyMtt) {
          return isDangKyPhatHanhMtt(x);
        }
        return true;
      })
      .map((x) => ({
        id: x.loai_hoa_don_ct_id,
        text: x.ten_hoa_don,
      }))
      .sort((a, b) => a.id - b.id)
      .forEach((item) => {
        uniqueData.add(JSON.stringify(item));
      });
    var result = Array.from(uniqueData).map((item: any) => JSON.parse(item));
    return result;
  }, [hoaDonDangKyPhatHanhs, props.onlyMtt]);
  const filterdData = useMemo(() => {
    return dataSource.filter((item) =>
      item.text.toLowerCase().includes(filter.toLowerCase())
    );
  }, [dataSource, filter]);
  const _selectedData = useMemo(() => {
    return dataSource.find((item) => item.id === props.value);
  }, [props.value, dataSource]);

  const placeholderText = isLoading
    ? "Đang tải..."
    : isLoadError
      ? "Lỗi tải dữ liệu"
      : "Chọn loại hóa đơn";

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
            disabled={isLoading}
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
              <Box sx={{ flex: 1 }}>Chọn loại hóa đơn</Box>
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
        selected={_selectedData}
        onSelectedChange={(data: any) => {
          if (data) {
            props.onValueChanged(data.id);
          }
        }}
        onFilterChange={setFilter}
        showItemDividers={true}
        overlayProps={{ width: "small", height: "medium" }}
      />
    </>
  );
};

export default SelectBoxLoaiHoaDonCTPhatHanh;
