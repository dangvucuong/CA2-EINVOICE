import { TriangleDownIcon } from "@primer/octicons-react";

import { Button, SelectPanel } from "@primer/react";
import { useEffect, useState } from "react";
import { useAuth } from "../../hooks/useAuth";
import { axiosClient } from "../../api/axiosClient";
import { parseSoapResponse } from "../../helpers/common";

interface ISelectBoxKyHieuChungTuPhatHanhProps {
  onValueChanged: (value: string) => void;
  value: string;
  maxWidth?: any;
  isShowClearBtn?: boolean;
  mau_so: string;
}

const SelectBoxKyHieuChungTuPhatHanh = (
  props: ISelectBoxKyHieuChungTuPhatHanhProps,
) => {
  const { value, mau_so } = props;
  const [open, setOpen] = useState(false);
  const { maxWidth, onValueChanged = () => {} } = props;
  const { user } = useAuth();
  const [options, setOptions] = useState<any[]>([]);
  const [selected, setSelected] = useState<any>();

  useEffect(() => {
    if (mau_so) {
      LayDanhSachKyHieu();
    }

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [mau_so]);

  const LayDanhSachKyHieu = async () => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
    <soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
      <soap12:Body>
        <LayDanhSachKyHieu  xmlns="http://tempuri.org/">
          <mauso>${mau_so}</mauso>
          <madonvi>${user?.donvi?.ma_dv}</madonvi>
        </LayDanhSachKyHieu>
      </soap12:Body>
    </soap12:Envelope>`;

    const res: string = await axiosClient.post(
      process.env.REACT_APP_API_CHUNG_TU as string,
      soap,
      {
        headers: {
          "Content-Type": "text/xml; charset=utf-8",
        },
      },
    );

    const parseRes = parseSoapResponse(res);
      const currentYear = new Date().getFullYear().toString().slice(-2);
      const defaultValue = `CT/${currentYear}E`;

      // luôn có default option
      const defaultOption = {
          id: 0,
          text: defaultValue,
          value: defaultValue,
      };

    if (parseRes.status === "success") {      

      //const opts = (parseRes?.data ?? [])
      //  .filter((item: any) => item?.ky_hieu?.includes(`/${currentYear}E`))
      //  .map((item: any, index: number) => ({
      //    id: index,
      //      text: `CT/${currentYear}E`,
      //      value: `CT/${currentYear}E`,
      //  }));
      //setOptions(opts);
        //===UPDATE 19.03.2026==
        let opts = (parseRes?.data ?? [])
            .filter((item: any) => item?.ky_hieu?.includes(`/${currentYear}E`))
            .map((item: any, index: number) => ({
                id: index + 1,
                text: item.ky_hieu,
                value: item.ky_hieu,
            }));

        // đảm bảo luôn có default trong list
        opts = [defaultOption, ...opts];

        setOptions(opts);

      //if (value) {
      //  const selectedOption = opts.find((item: any) => item.value === value);

      //  setSelected(selectedOption);
      //} else {
      //  setSelected(null);
        //}
        // 🔥 set selected chuẩn
        if (value) {
            const selectedOption = opts.find((item: any) => item.value === value);
            setSelected(selectedOption || defaultOption);
        } else {
            setSelected(defaultOption);
            onValueChanged(defaultValue); // 🔥 auto bind về cha
        }
    } else {
    }
  };

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
                maxWidth: maxWidth,
                overflow: "hidden",
                textOverflow: "ellipsis",
              }}
            >
              {children || "Ký hiệu"}
            </p>
          </Button>
        )}
        placeholderText="Search"
        open={open}
        onOpenChange={setOpen}
        items={options}
        selected={selected}
        onSelectedChange={(data: any) => {
          if (data) {
            onValueChanged(data?.value);
            setSelected(data);
          }
        }}
        onFilterChange={() => {}}
        showItemDividers={true}
        overlayProps={{ width: "medium", height: "medium" }}
      />
    </>
  );
};

export default SelectBoxKyHieuChungTuPhatHanh;
