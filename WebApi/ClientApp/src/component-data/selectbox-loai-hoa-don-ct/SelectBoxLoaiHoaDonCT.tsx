import { TriangleDownIcon, XCircleFillIcon } from "@primer/octicons-react";

import { Box, Button, SelectPanel } from "@primer/react";
import { useEffect, useMemo, useState } from "react";
import { useDispatch } from "react-redux";
import { useAppSelector } from "../../hooks/useAppSelector";
import { rootAction } from "../../state/actions/rootAction";
import { eReducerStatusBase } from "../../state/reducer-models/eReducerStatusBase";
interface ISelectBoxLoaiHoaDonCTProps {
  onValueChanged: (id: number) => void;
  value: number;
  maxWidth?: any;
  isShowClearBtn?: boolean;
  isOnlyShowDaThietLapMau?: boolean;
}

const SelectBoxLoaiHoaDonCT = (props: ISelectBoxLoaiHoaDonCTProps) => {
  const [open, setOpen] = useState(false);

  const { loaiHoaDonCTs, status } = useAppSelector(
    (x) => x.hoaDon.loaiHoaDonCTReducer
  );
  const { mauHoaDons, status: mauHoaDonStatus } = useAppSelector(
    (x) => x.hoaDon.mauHoaDonReducer
  );
  const [filter, setFilter] = useState("");
  const dispatch = useDispatch();
  const dataSource = useMemo(() => {
    if (props.isOnlyShowDaThietLapMau === true) {
      var loaiHoaDonCTIdsDaThietLapMau = mauHoaDons
        .filter((x) => x.is_active)
        .map((x) => x.loai_hoa_don_ct_id);
      return loaiHoaDonCTs
        .filter((x) => loaiHoaDonCTIdsDaThietLapMau.includes(x.id))
        .map((x) => ({ id: x.id, text: x.name }));
    }
    return loaiHoaDonCTs.map((x) => ({ id: x.id, text: x.name }));
  }, [loaiHoaDonCTs, filter, props.isOnlyShowDaThietLapMau, mauHoaDons]);
  const filterdData = useMemo(() => {
    return dataSource.filter((item) =>
      item.text.toLowerCase().includes(filter.toLowerCase())
    );
  }, [dataSource, filter]);
  const _selectedData = useMemo(() => {
    return dataSource.find((item) => item.id === props.value);
  }, [props.value, dataSource]);
  useEffect(() => {
    if (status == eReducerStatusBase.is_not_initialization) {
      dispatch(rootAction.hoaDon.loaiHoaDonCTAction.loadStart());
    }
  }, [status]);
  useEffect(() => {
    if (
      mauHoaDonStatus == eReducerStatusBase.is_not_initialization &&
      props.isOnlyShowDaThietLapMau === true
    ) {
      dispatch(rootAction.hoaDon.mauHoaDonAction.loadStart());
    }
  }, [mauHoaDons, props.isOnlyShowDaThietLapMau]);

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
        overlayProps={{ width: "medium", height: "medium" }}
      />
    </>
  );
};

export default SelectBoxLoaiHoaDonCT;
