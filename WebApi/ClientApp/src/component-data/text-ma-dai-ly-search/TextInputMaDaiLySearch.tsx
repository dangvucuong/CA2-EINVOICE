import { ActionList, Box, SelectPanel, TextInput } from "@primer/react";
import React, { useEffect, useRef, useState } from "react";
import { useDebounce } from "use-debounce";
import { daiLyApi } from "../../api/category/daiLyApi";
import Button from "../../component-ui/button";
import Text from "../../component-ui/text";
import { IMyTextInputProps } from "../../component-ui/text-input/TextInput";
import { eSortMode } from "../../models/commons/eSortMode";
import { IDaiLy } from "../../models/responses/category/IDaiLy";
interface ITextInputMaDaiLySearchData {
  text: string;
  dai_ly?: IDaiLy;
}
interface ITextInputMaDaiLySearchProps extends IMyTextInputProps {
  onValueChanged: (data: ITextInputMaDaiLySearchData) => void;
}
const TextInputMaDaiLySearch = (props: ITextInputMaDaiLySearchProps) => {
  const [selected, setSelected] = React.useState<any>();
  const [filter, setFilter] = React.useState(props.value?.toString() ?? "");

  const [open, setOpen] = useState(false);
  const textRef = useRef<any>(null);
  const [daiLys, setDaiLys] = useState<IDaiLy[]>([]);
  const [delayFilter] = useDebounce(filter, 500);
  const dataSource = daiLys.map((x) => ({
    id: x.id,
    text: x.ma_dai_ly,
    daiLy: x,
  }));
  useEffect(() => {
    setFilter(props.value?.toString() ?? "");
  }, [props.value]);
  useEffect(() => {
    if (open || delayFilter === "") {
      handleLoadDaiLyAsync();
    }
  }, [delayFilter]);

  const handleLoadDaiLyAsync = async () => {
    const res = await daiLyApi.getByDonViPaging({
      search_key: delayFilter,
      page_index: 0,
      page_size: 10,
      sort_mode: eSortMode.DESC,
      sort_by: "",
    });
    if (res.is_success) {
      setDaiLys(res.data.data);
    }
  };

  return (
    <Box sx={{ width: "100%" }}>
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
          placeholderText="Tìm theo mã, tên đại lý"
          title={
            <>
              <Button
                text="Áp dụng mã đại lý đang nhập"
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
                      dai_ly: data.daiLy,
                    });
                    setOpen(false);
                  }}
                >
                  <Box sx={{ ml: 3, mr: 3 }}>
                    <Box>
                      <b>{data.text}</b> - {data.daiLy.ten_dai_ly}
                    </Box>
                    <Box>
                      <Text
                        text={data.daiLy.email}
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

export default TextInputMaDaiLySearch;
