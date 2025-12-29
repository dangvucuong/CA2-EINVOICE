import { Box, Button, SelectPanel } from "@primer/react";
import { useEffect, useMemo, useState } from "react";
import { useDispatch } from "react-redux";
import { useAppSelector } from "../../hooks/useAppSelector";
import { TriangleDownIcon, XCircleFillIcon } from "@primer/octicons-react";

interface ISelectBoxMauSoPhatHanhProps {
  onValueChanged: (id: string) => void;
  value: string;
  maxWidth?: any;
  loai_hoa_don_ct_id: number;
  isAutoSelectIfHasOneItem: boolean;
  isShowClearBtn?: boolean;
}

const SelectBoxMauSoPhatHanh = (props: ISelectBoxMauSoPhatHanhProps) => {
  const [open, setOpen] = useState(false);
  const { hoaDonDangKyPhatHanhs, status } = useAppSelector(
    (x) => x.hoaDon.hoaDonDangKyPhatHanhReducer
  );
  const [filter, setFilter] = useState("");
  const dispatch = useDispatch();
  const dataSource = useMemo(() => {
    var uniqueData = new Set();
    // debugger
    hoaDonDangKyPhatHanhs
      .sort((a, b) => b.id - a.id)
      .filter((x) => x.loai_hoa_don_ct_id === props.loai_hoa_don_ct_id)
      .map((x) => ({ id: x.mau_so, text: x.mau_so }))
      .forEach((item) => {
        uniqueData.add(JSON.stringify(item));
      });
    var result = Array.from(uniqueData).map((item: any) => JSON.parse(item));
    return result;
  }, [hoaDonDangKyPhatHanhs, props.loai_hoa_don_ct_id]);
  const filterdData = useMemo(() => {
    return dataSource.filter((item) =>
      item.text.toLowerCase().includes(filter.toLowerCase())
    );
  }, [dataSource, filter]);
  const _selectedData = useMemo(() => {
    return dataSource.find((item) => item.id === props.value);
  }, [props.value, dataSource]);
  useEffect(() => {
    if (props.value === "") {
      if (dataSource.length === 1) {
        props.onValueChanged(dataSource[0].id);
      }
    }
  }, [props.value, dataSource]);
  // useEffect(() => {
  //     if (status === eReducerStatusBase.is_not_initialization) {
  //         dispatch(rootAction.hoaDon.hoaDonDangKyPhatHanhAction.loadStart())
  //     }
  // }, [status])
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
              {children || "Chọn mẫu số"}
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
