import { TriangleDownIcon } from "@primer/octicons-react";

import { Button, SelectPanel } from "@primer/react";
import { memo, useEffect, useState } from "react";
import { useAuth } from "../../hooks/useAuth";
import { axiosClient } from "../../api/axiosClient";
import { parseSoapResponse } from "../../helpers/common";

interface ISelectBoxLoaiChungTuProps {
  onValueChanged: (value: string) => void;
  value: string;
  maxWidth?: any;
  isShowClearBtn?: boolean;
  loadData?: () => void;
  isFormLap?: boolean;
}

const SelectBoxLoaiChungTuPhatHanh = (props: ISelectBoxLoaiChungTuProps) => {
  const { loadData = () => {}, isFormLap = false } = props;
  const [open, setOpen] = useState(false);
  const { user } = useAuth();
  const [options, setOptions] = useState<any[]>([]);
  const [selected, setSelected] = useState<any>();

  useEffect(() => {
    LayDanhSachLoaiCT();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const LayDanhSachLoaiCT = async () => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <LayLoaiChungTu xmlns="http://tempuri.org/">
      <maDonVi>${user?.donvi?.ma_dv}</maDonVi>
    </LayLoaiChungTu>
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
    let newRes: any = parseRes?.data;

    if (isFormLap) {
      newRes = newRes?.filter((item: any) => item.MSChungtu === "03/TNCN");
    }

    if (parseRes.status === "success") {
      const opts = newRes?.map((item: any, index: number) => ({
        id: index,
        text: item.Tenchungtu,
        value: item.MSChungtu,
      }));

      setOptions(opts);
      setSelected(opts[0]);
      props.onValueChanged(opts[0]?.value);
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
                maxWidth: props.maxWidth,
                overflow: "hidden",
                textOverflow: "ellipsis",
              }}
            >
              {children || "Chọn loại chứng từ"}
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
            props.onValueChanged(data?.value);
            setSelected(data);
            loadData();
          }
        }}
        onFilterChange={() => {}}
        showItemDividers={true}
        overlayProps={{ width: "medium", height: "medium" }}
      />
    </>
  );
};

export default memo(SelectBoxLoaiChungTuPhatHanh);
