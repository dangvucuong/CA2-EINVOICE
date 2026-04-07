import { TriangleDownIcon } from "@primer/octicons-react";

import { Button, SelectPanel } from "@primer/react";
import { useEffect, useState } from "react";
import { useAuth } from "../../hooks/useAuth";
import { axiosClient } from "../../api/axiosClient";
import { parseSoapResponse } from "../../helpers/common";

interface ISelectBoxKyHieuChungTuQuanLyProps {
  onValueChanged: (value: string) => void;
  value: string;
  maxWidth?: any;
  isShowClearBtn?: boolean;
  mau_so: string;
}

const SelectBoxKyHieuChungTuQuanLy = (
  props: ISelectBoxKyHieuChungTuQuanLyProps,
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

    if (parseRes.status === "success") {
      const opts = (parseRes?.data ?? []).map((item: any, index: number) => ({
        id: index,
        text: item.ky_hieu,
        value: item.ky_hieu,
      }));
      setOptions(opts);

      if (value) {
        const selectedOption = opts.find((item: any) => item.value === value);

        setSelected(selectedOption);
      } else {
        if (opts.length === 1) {
          onValueChanged(opts[0].value);
          setSelected(opts[0]);
        } else {
          setSelected(null);
        }
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

export default SelectBoxKyHieuChungTuQuanLy;
