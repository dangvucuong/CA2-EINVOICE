import { TriangleDownIcon } from "@primer/octicons-react";

import { Button, SelectPanel } from "@primer/react";
import { memo, useEffect, useState } from "react";
import { useAuth } from "../../hooks/useAuth";
import { parseSoapResponse } from "../../helpers/common";
import { axiosClient } from "../../api/axiosClient";

interface ISelectBoxSoChungTuPhatHanhProps {
  onValueChanged: (value: string) => void;
  value: string;
  maxWidth?: any;
  isShowClearBtn?: boolean;
  mau_so?: string;
  ky_hieu?: string;
}

const SelectBoxSoChungTuPhatHanh = (
  props: ISelectBoxSoChungTuPhatHanhProps
) => {
  const { mau_so, maxWidth, ky_hieu, onValueChanged = () => {}, value } = props;
  const { user } = useAuth();
  const [open, setOpen] = useState(false);
  const [options, setOptions] = useState<any[]>([]);
  const [selected, setSelected] = useState<any>();

  useEffect(() => {
    if (ky_hieu) {
      LayDanhSachSoChungTu();
    }

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [ky_hieu]);

  const LayDanhSachSoChungTu = async () => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <LoadSoChungTu xmlns="http://tempuri.org/">
      <madonvi>${user?.donvi?.ma_dv}</madonvi>
      <mauso>${mau_so}</mauso>
      <kyhieu>${ky_hieu}</kyhieu>
    </LoadSoChungTu>
  </soap12:Body>
</soap12:Envelope>`;

    const res: string = await axiosClient.post(
      process.env.REACT_APP_API_CHUNG_TU as string,
      soap,
      {
        headers: {
          "Content-Type": "text/xml; charset=utf-8",
        },
      }
    );

    const parseRes = parseSoapResponse(res);

    if (parseRes.status === "success") {
      const opts = (parseRes?.data ?? []).map((item: any, index: number) => ({
        id: index,
        text: item.Sochungtu,
        value: item.Sochungtu,
      }));
      console.log(opts);

      setOptions(opts);
      setSelected(opts?.find((x: any) => x.value === value?.toString()));
      onValueChanged(opts?.find((x: any) => x.value === value?.toString()));
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
              {children || "Số chứng từ"}
            </p>
          </Button>
        )}
        placeholderText="Search"
        open={open}
        onOpenChange={setOpen}
        items={options}
        selected={selected}
        onSelectedChange={(data: any) => {
          onValueChanged(data);
          setSelected(data);
        }}
        onFilterChange={() => {}}
        showItemDividers={true}
        overlayProps={{ width: "medium", height: "medium" }}
      />
    </>
  );
};

export default memo(SelectBoxSoChungTuPhatHanh);
