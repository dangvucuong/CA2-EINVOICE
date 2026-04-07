import { TriangleDownIcon, XCircleFillIcon } from "@primer/octicons-react";

import { Box, Button, SelectPanel } from "@primer/react";
import { useEffect, useMemo, useState } from "react";
import { useDispatch } from "react-redux";
import { useAppSelector } from "../../hooks/useAppSelector";
import { rootAction } from "../../state/actions/rootAction";
import { eReducerStatusBase } from "../../state/reducer-models/eReducerStatusBase";
import moment from "moment";
interface ISelectBoxLoaiHoaDonCTPhatHanhProps {
  onValueChanged: (id: number) => void;
  value: number;
  maxWidth?: any;
  isShowClearBtn?: boolean;
  // isOnlyShowCurrentYear?: boolean
}

const SelectBoxLoaiHoaDonCTPhatHanh = (
  props: ISelectBoxLoaiHoaDonCTPhatHanhProps
) => {
  const [open, setOpen] = useState(false);
  const { hoaDonDangKyPhatHanhs, status } = useAppSelector(
    (x) => x.hoaDon.hoaDonDangKyPhatHanhReducer
  );
  const [filter, setFilter] = useState("");
  const dispatch = useDispatch();
  const dataSource = useMemo(() => {
    var uniqueData = new Set();
    hoaDonDangKyPhatHanhs
      ///chỉ lấy ra những bản ghi có ngay_su_dung trong năm hiện tại, dùng momentjs để lấy năm hiện tại
      .filter((x) => {
        return moment(x?.ngay_su_dung).year() === moment().year();
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
  }, [hoaDonDangKyPhatHanhs]);
  const filterdData = useMemo(() => {
    return dataSource.filter((item) =>
      item.text.toLowerCase().includes(filter.toLowerCase())
    );
  }, [dataSource, filter]);
  const _selectedData = useMemo(() => {
    return dataSource.find((item) => item.id === props.value);
  }, [props.value, dataSource]);
  useEffect(() => {
    if (status === eReducerStatusBase.is_not_initialization) {
      dispatch(rootAction.hoaDon.hoaDonDangKyPhatHanhAction.loadStart());
    }
  }, [status]);

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
              {children || "Chọn loại hóa đơn"}
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
