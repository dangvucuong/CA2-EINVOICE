import { Box, FormControl, Radio, RadioGroup } from "@primer/react";
import { useState } from "react";

interface IThongbaoSaiSotCTLoaiTBProps {
  onValueChanged: (id: number, data?: any) => void;
  value: number;
  maxWidth?: any;
  isShowClearBtn?: boolean;
}

const ThongbaoSaiSotCTLoaiTB = (props: IThongbaoSaiSotCTLoaiTBProps) => {
  const { value, onValueChanged = () => {} } = props;

  return (
    <>
      <Box className="">
        <RadioGroup name="viTriLogo">
          <FormControl>
            <Radio
              value="left"
              checked={value === 1}
              onChange={(e) => {
                if (e.target.checked) {
                  onValueChanged(1);
                }
              }}
            />
            <FormControl.Label>NNT thông báo</FormControl.Label>
          </FormControl>
          <FormControl>
            <Radio
              value="right"
              checked={value === 2}
              onChange={(e) => {
                if (e.target.checked) {
                  onValueChanged(2);
                }
              }}
            />
            <FormControl.Label>
              Giải trình của NNT theo thông báo của CQT
            </FormControl.Label>
          </FormControl>
        </RadioGroup>
      </Box>
    </>
  );
};

export default ThongbaoSaiSotCTLoaiTB;
