import { TriangleDownIcon, XCircleFillIcon } from "@primer/octicons-react";

import { Box, Button, Label, SelectPanel } from "@primer/react";
import { useEffect, useMemo, useState } from "react";
import { useAppSelector } from "../../hooks/useAppSelector";
import { useHoaDonDangKyPhatHanhLoader } from "../../hooks/useHoaDonDangKyPhatHanhLoader";
import { useLocation } from "react-router-dom";
import { isDangKyPhatHanhMtt, isKyHieuMayTinhTien } from "../../utils/hoaDonKyHieu";

interface ISelectBoxKyHieuPhatHanhProps {
  onValueChanged: (id: string) => void;
  value: string;
  maxWidth?: any;
  loai_hoa_don_ct_id: number;
  mau_so: string;
  isAutoSelectIfHasOneItem: boolean;
  isShowClearBtn?: boolean;
  isShowKyHieuTheoNam?: boolean;
  onlyMtt?: boolean;
}
const getLeadingVisual = (isMTT: boolean) => {
  return (
    <>
      {isMTT && (
        <Label size="small" variant="attention">
          Máy tính tiền
        </Label>
      )}
    </>
  );
};

const SelectBoxKyHieuPhatHanh = (props: ISelectBoxKyHieuPhatHanhProps) => {
  const { isShowKyHieuTheoNam = false } = props;
  const [open, setOpen] = useState(false);
  const location = useLocation();

  const { hoaDonDangKyPhatHanhs, isLoading } = useHoaDonDangKyPhatHanhLoader();
  const [filter, setFilter] = useState("");
  const dataSource = useMemo(() => {
    var uniqueData = new Set();
    hoaDonDangKyPhatHanhs
      .sort((a, b) => b.id - a.id)
      .filter((x) => {
        if (
          x.loai_hoa_don_ct_id !== props.loai_hoa_don_ct_id ||
          x.mau_so !== props.mau_so
        ) {
          return false;
        }
        if (props.onlyMtt || location.state?.is_may_tinh_tien === true) {
          return isDangKyPhatHanhMtt(x);
        }
        return true;
      })
      .map((x) => ({ id: x.ky_hieu, text: x.ky_hieu }))
      .forEach((item) => {
        uniqueData.add(JSON.stringify(item));
      });
    var result = Array.from(uniqueData).map((item: any) => JSON.parse(item));

    return result.map((x) => {
      return {
        ...x,
        trailingVisual: getLeadingVisual(isKyHieuMayTinhTien(x.text)),
      };
    });
  }, [
    hoaDonDangKyPhatHanhs,
    props.loai_hoa_don_ct_id,
    props.mau_so,
    props.onlyMtt,
    location.state?.is_may_tinh_tien,
  ]);
  const filterdData = useMemo(() => {
    const data = dataSource.filter((item) =>
      item.text.toLowerCase().includes(filter.toLowerCase()),
    );

    if (!isShowKyHieuTheoNam) return data;

    return data.filter((item) => {
      if (item.text.length >= 4) {
        const year = item.text.substring(1, 3);

        const currentYear = new Date().getFullYear() % 100;
        if (parseInt(year) < currentYear) {
          return false;
        }
      }
      return true;
    });
  }, [dataSource, filter, isShowKyHieuTheoNam]);

  const _selectedData = useMemo(() => {
    return dataSource.find((item) => item.id === props.value);
  }, [props.value, dataSource]);

  useEffect(() => {
    if (!isShowKyHieuTheoNam || !props.value) {
      return;
    }
    const year = props.value.substring(1, 3);
    const currentYear = new Date().getFullYear() % 100;
    if (parseInt(year) !== currentYear && filterdData[0]?.id) {
      props.onValueChanged(filterdData[0].id);
    }
  }, [isShowKyHieuTheoNam, props.value, filterdData]);

  useEffect(() => {
    if (props.value === "" && props.isAutoSelectIfHasOneItem) {
      if (filterdData.length === 1) {
        props.onValueChanged(filterdData[0].id);
      }
    }
  }, [props.value, filterdData, props.isAutoSelectIfHasOneItem]);

  const placeholderText =
    isLoading && !props.mau_so
      ? "Đang tải..."
      : !props.mau_so
        ? "Chọn mẫu số trước"
        : "Chọn Ký hiệu";

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
            disabled={isLoading || !props.mau_so}
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
              <Box sx={{ flex: 1 }}>Chọn ký hiệu</Box>
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
        overlayProps={{ width: "small", height: "medium" }}
      />
    </>
  );
};

export default SelectBoxKyHieuPhatHanh;
