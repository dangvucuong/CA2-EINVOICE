import { TriangleDownIcon, XCircleFillIcon } from "@primer/octicons-react";

import { Box, Button, Label, SelectPanel } from "@primer/react";
import { useEffect, useMemo, useState } from "react";
import { useDispatch } from "react-redux";
import { useAppSelector } from "../../hooks/useAppSelector";
import {
  LeadingVisual,
  TrailingVisual,
} from "@primer/react/lib/ActionList/Visuals";
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

  const { hoaDonDangKyPhatHanhs, status } = useAppSelector(
    (x) => x.hoaDon.hoaDonDangKyPhatHanhReducer,
  );
  const [filter, setFilter] = useState("");
  const dispatch = useDispatch();
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

    // ký hiệu có giá trị như C25THH, C25THM, C24THH,... 25 với 24 là năm, nếu là năm hiện tại thì bỏ qua các ký hiệu của năm trước
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
    const result = dataSource.find((item) => item.id === props.value);

    if (isShowKyHieuTheoNam && props?.value) {
      const year = props?.value?.substring(1, 3);
      const currentYear = new Date().getFullYear() % 100;

      if (parseInt(year) !== currentYear) {
        props.onValueChanged(dataSource[0]?.id);
        return dataSource[0];
      }
    }

    return result;
  }, [props.value, dataSource]);

  useEffect(() => {
    if (props.value === "") {
      if (filterdData.length === 1) {
        props.onValueChanged(filterdData[0].id);
      }
    }
  }, [props.value, filterdData]);

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
              {children || "Chọn Ký hiệu"}
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
