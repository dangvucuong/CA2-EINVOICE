import React, { useEffect, useState } from "react";
import TextInput from "../text-input";
import { IMyTextInputProps } from "../text-input/TextInput";
import clsx from "clsx";
import { SearchIcon } from "@primer/octicons-react";
import { Box, Tooltip } from "@primer/react";
function numberWithCommas(x: any) {
  return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, " ");
}
interface ITextInputNumberProps extends IMyTextInputProps {
  onValueChanged: (value: number) => void;
  isSoAm?: boolean;
}
const TextInputNumber = (props: ITextInputNumberProps) => {
  const [number, setNumber] = useState("");
  useEffect(() => {
    if (props.value) {
      const inputValue = props.value?.toString().replace(/,/g, "");
      const formattedValue = numberWithCommas(inputValue);
      setNumber(formattedValue);
    }
  }, [props.value]);
  useEffect(() => {
    if (props.defaultValue) {
      let inputValue = props.defaultValue?.toString().replace(/ /g, "");

      if (props.isSoAm) {
        inputValue = inputValue.replace("-", "");
      }
      const formattedValue = numberWithCommas(inputValue);
      setNumber(formattedValue);
    }
  }, [props.defaultValue, props.isSoAm]);
  const handleChange = (e: any) => {
    // Lấy giá trị nhập vào từ input
    let inputValue = e.target.value;

    // Xóa dấu phân cách trước khi parse số
    inputValue = inputValue.replace(/ /g, "");

    // Kiểm tra xem giá trị nhập vào có phải là số không
    if (!isNaN(inputValue)) {
      // Format số với dấu phân cách
      const formattedValue = numberWithCommas(inputValue);
      // Cập nhật state với giá trị đã được format
      setNumber(formattedValue);
      if (props.onChange) {
        props.onChange(inputValue);
      }
    }
  };
  return (
    <TextInput
      type="text"
      {...props}
      value={number}
      leadingVisual={
        props.isSoAm ? (
          <Box>
            <Tooltip text="Số âm" direction="e">
              -
            </Tooltip>
          </Box>
        ) : null
      }
      onChange={handleChange}
      className={clsx(props.className, "input-align-right")}
      onBlur={(e: any) => {
        // 
        let inputValue = e.target.value;
        // 
        // Xóa dấu phân cách trước khi parse số
        inputValue = inputValue.replace(/ /g, "");

        // Kiểm tra xem giá trị nhập vào có phải là số không
        if (inputValue && !isNaN(inputValue)) {
          props.onValueChanged(
            parseFloat(inputValue) * (props.isSoAm ? -1 : 1)
          );
        } else {
          props.onValueChanged(0);
        }
      }}
    />
  );
};

export default TextInputNumber;
