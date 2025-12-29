import { ActionList, Box, SelectPanel, TextInput } from "@primer/react";
import React, { useEffect, useRef, useState } from "react";
import { useDebounce } from "use-debounce";
import { khachHangApi } from "../../api/category/khachHangApi";
import Button from "../../component-ui/button";
import Text from "../../component-ui/text";
import { IMyTextInputProps } from "../../component-ui/text-input/TextInput";
import { eSortMode } from "../../models/commons/eSortMode";
import { IKhachHang } from "../../models/responses/category/IKhachHang";
interface ITextInputMstKhachHangData {
  text: string;
  khach_hang?: IKhachHang;
}
interface ITextInputMstKhachHangProps extends IMyTextInputProps {
  onValueChanged: (data: ITextInputMstKhachHangData) => void;
}
const getInfo = (khachHang: IKhachHang) => {
  return <Box>{khachHang.email}</Box>;
};
const TextInputMstKhachHang = (props: ITextInputMstKhachHangProps) => {
  const [selected, setSelected] = React.useState<any>();
  const [filter, setFilter] = React.useState(props.value?.toString() ?? "");

  const [open, setOpen] = useState(false);
  const textRef = useRef<any>(null);
  const [khachHangs, setKhachHangs] = useState<IKhachHang[]>([]);
  const [delayFilter] = useDebounce(filter, 500);
  const dataSource = khachHangs.map((x) => ({
    id: x.id,
    text: x.mst,
    khachHang: x,
  }));
  useEffect(() => {
    setFilter(props.value?.toString() ?? "");
  }, [props.value]);
  useEffect(() => {
    if (open || delayFilter === "") {
      handleLoadKhachHangAsync();
    }
  }, [delayFilter]);

  const handleLoadKhachHangAsync = async () => {
    const res = await khachHangApi.getByDonViPaging({
      search_key: delayFilter,
      page_index: 0,
      page_size: 30,
      sort_mode: eSortMode.DESC,
      sort_by: "",
    });
    if (res.is_success) {
      setKhachHangs(res.data.data);
    }
  };

  return (
    <Box>
      <TextInput
        ref={textRef}
        value={filter}
        {...props}
        onClick={() => {
          setOpen(true);
        }}
        onKeyUp={() => {
          // setOpen(true)
        }}
        onChange={(e) => {
          setOpen(true);
          setFilter(e.target.value);
        }}
      />
      {true && (
        <SelectPanel
          renderAnchor={null}
          anchorRef={textRef}
          placeholderText="Tìm theo MST, đơn vị, người mua hàng"
          title={
            <>
              <Button
                text="Áp dụng mã số thuế đang nhập"
                onClick={() => {
                  setFilter(filter);
                  props.onValueChanged({ text: filter });
                  setOpen(false);
                }}
              />
            </>
          }
          open={open}
          renderItem={(data: any) => {
            return (
              <Box sx={{ ml: 1, mr: 1 }}>
                <ActionList.Item
                  onSelect={() => {
                    setFilter(data.item?.text ?? "");
                    props.onValueChanged({
                      text: data.item?.text ?? "",
                      khach_hang: data.khachHang,
                    });
                    setOpen(false);
                  }}
                >
                  <Box sx={{ ml: 3, mr: 3 }}>
                    <Box>
                      <b>{data.text}</b> - {data.khachHang.ten_don_vi}
                    </Box>
                    <Box>
                      <b>{data.khachHang.ten_khach_hang}</b>
                    </Box>
                    <Box>
                      <Text
                        text={data.khachHang.dia_chi}
                        sx={{
                          color: "fg.muted",
                          fontSize: "12px",
                        }}
                      />
                    </Box>
                  </Box>
                </ActionList.Item>
              </Box>
            );
          }}
          onOpenChange={setOpen}
          items={dataSource}
          selected={selected}
          filterValue={filter}
          onSelectedChange={setSelected}
          onFilterChange={setFilter}
          showItemDividers={true}
          overlayProps={{
            width: window.innerWidth >= 768 ? "xlarge" : "auto",
            height: "medium",
          }}
        />
      )}
    </Box>
  );
};

export default TextInputMstKhachHang;
