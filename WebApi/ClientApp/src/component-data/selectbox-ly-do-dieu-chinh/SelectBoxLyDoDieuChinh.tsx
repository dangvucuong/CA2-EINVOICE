import { TriangleDownIcon, XCircleFillIcon } from "@primer/octicons-react";
import { Box, Button, FormControl, SelectPanel } from "@primer/react";
import { useMemo, useState } from "react";
import { useLyDoDieuChinhsHook } from "../../hooks/useLyDoDieuChinhHook";
import TextArea from "../../component-ui/text-area";
import Text from "../../component-ui/text";

interface ISelectBoxLyDoDieuChinhProps {
  onValueChanged: (id: number) => void;
  value: number;
  maxWidth?: any;
  isShowClearBtn?: boolean;
  register?: any;
  errors?: any;
}

const SelectBoxLyDoDieuChinh = (props: ISelectBoxLyDoDieuChinhProps) => {
  const { register, errors } = props;
  const [open, setOpen] = useState(false);
  const { lyDoDieuChinhs } = useLyDoDieuChinhsHook();
  const [filter, setFilter] = useState("");

  const dataSource = useMemo(() => {
    return lyDoDieuChinhs.map((x) => ({ id: x.id, text: x.name }));
  }, [lyDoDieuChinhs]);
  const filterdData = useMemo(() => {
    return dataSource.filter((item) =>
      item.text.toLowerCase().includes(filter.toLowerCase())
    );
  }, [dataSource, filter]);
  const _selectedData = useMemo(() => {
    return dataSource.find((item) => item.id === props.value);
  }, [props.value, dataSource]);

  return (
    <Box
      sx={{
        display: "flex",
        flexDirection: ["column", "row"],
        gap: 4,
      }}
    >
      <Box sx={{ mt: 2 }}>
        <Box sx={{ mt: 1 }}>
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
                  {children || "Chọn hình thức"}
                </p>
              </Button>
            )}
            title={
              <>
                <Box sx={{ display: "flex", alignItems: "center" }}>
                  <Box sx={{ flex: 1 }}>Chọn mẫu số</Box>
                  {props.isShowClearBtn && props.value !== 0 && (
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
              props.onValueChanged(data.id);
            }}
            onFilterChange={setFilter}
            showItemDividers={true}
            overlayProps={{ width: "auto", height: "medium" }}
          />
        </Box>
      </Box>
      {/* <FormControl sx={{ mt: 2, flex: 1 }}>
        <FormControl.Label>
          <Text text="Lý do điều chỉnh" />
        </FormControl.Label>
        <TextArea
          sx={{
            width: "100%",
          }}
          register={register}
          name="hoa_don_ly_do_dieu_chinh_text"
          rows={3}
          validateMessage="Vui lòng điền Lý do điều chỉnh"
          errors={errors}
          resize="none"
          required={true}
          // onChange={(e) => {
          // })
        />
      </FormControl> */}
    </Box>
  );
};

export default SelectBoxLyDoDieuChinh;
