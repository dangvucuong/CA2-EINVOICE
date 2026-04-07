import { ActionList, Box, SelectPanel, TextInput } from "@primer/react";
import React, { useEffect, useRef, useState } from "react";
import { useDebounce } from "use-debounce";
import { hangHoaApi } from "../../api/category/hangHoaApi";
import Button from "../../component-ui/button";
import { IMyTextInputProps } from "../../component-ui/text-input/TextInput";
import { eSortMode } from "../../models/commons/eSortMode";
import { IHangHoa } from "../../models/responses/category/IHangHoa";
interface ITextInputMaHangHoaData {
  text: string;
  hang_hoa?: IHangHoa;
}
interface ITextInputMaHangHoaProps extends IMyTextInputProps {
  onValueChanged: (data: ITextInputMaHangHoaData) => void;
}

const TextInputMaHangHoa = (props: ITextInputMaHangHoaProps) => {
  const [selected, setSelected] = React.useState<any>();
  const [filter, setFilter] = React.useState(props.value?.toString() ?? "");

  const [open, setOpen] = useState(false);
  const textRef = useRef<any>(null);
  const [hangHoas, setHangHoas] = useState<IHangHoa[]>([]);
  const [delayFilter] = useDebounce(filter, 500);
  const dataSource = hangHoas.map((x) => ({
    id: x.id,
    text: x.ma_hang_hoa,
    hang_hoa: x,
  }));
  useEffect(() => {
    setFilter(props.value?.toString() ?? "");
  }, [props.value]);
  useEffect(() => {
    if (open || delayFilter === "") {
      handleLoadHangHoaAsync();
    }
  }, [delayFilter]);

  const handleLoadHangHoaAsync = async () => {
    const res = await hangHoaApi.getByDonViPaging({
      search_key: delayFilter,
      page_index: 0,
      page_size: 10,
      sort_mode: eSortMode.DESC,
      sort_by: "",
    });
    if (res.is_success) {
      setHangHoas(res.data.data);
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
          placeholderText="Tìm kiếm theo mã, tên hàng hóa"
          title={
            <>
              <Button
                text="Áp dụng mã hàng hóa đang nhập"
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
                      hang_hoa: data.hang_hoa,
                    });
                    setOpen(false);
                  }}
                >
                  <Box sx={{ ml: 3, mr: 3 }}>
                    <Box>
                      <b>{data.text}</b> - {data.hang_hoa.ten_hang_hoa}
                    </Box>
                    {/* <Box><b>{data.khachHang.ten_khach_hang}</b></Box>
                                        <Box><Text text={data.khachHang.dia_chi} sx={{
                                            color: 'fg.muted'
                                        }} /></Box> */}
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

export default TextInputMaHangHoa;
