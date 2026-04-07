import { Select } from "@primer/react";
import { useMemo } from "react";
interface ISelectBoxTinhChatHangHoaProps {
  onValueChanged: (id: string) => void;
  value: string;
  maxWidth?: any;
}

const SelectBoxLoaiTien = (props: ISelectBoxTinhChatHangHoaProps) => {
  // console.log({
  //     loaiTien: props.value
  // });

  const dataSource = useMemo(() => {
    return [
      { id: "VND", text: "VND" },
      { id: "USD", text: "USD" },
      { id: "EUR", text: "EUR" },
      { id: "SGD", text: "SGD" },
      { id: "JPY", text: "JPY" },
      { id: "CHF", text: "CHF" },
      { id: "AUD", text: "AUD" },
      { id: "GBP", text: "GBP" },
      { id: "CAD", text: "CAD" },
      { id: "CNY", text: "CNY" },
    ];
  }, []);
  return (
    <>
      <Select
        sx={{ width: "100%" }}
        onChange={(e) => {
          // const id: number = e.currentTarget.value ? parseInt(e.currentTarget.value) : 0
          props.onValueChanged(e.currentTarget.value);
        }}
      >
        {dataSource.map((x) => {
          return (
            <Select.Option
              value={x.id.toString()}
              selected={x.id === props.value}
              // onSelect={() => {
              //     props.onValueChanged(x.id)
              // }}
            >
              {x.text}
            </Select.Option>
          );
        })}
      </Select>
    </>
  );
};

export default SelectBoxLoaiTien;
