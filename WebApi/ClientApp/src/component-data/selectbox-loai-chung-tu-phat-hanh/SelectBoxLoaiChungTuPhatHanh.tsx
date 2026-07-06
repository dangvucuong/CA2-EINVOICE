import { TriangleDownIcon } from "@primer/octicons-react";

import { Button, SelectPanel } from "@primer/react";
import { memo, useEffect, useState } from "react";
import { useAuth } from "../../hooks/useAuth";
import { axiosClient } from "../../api/axiosClient";
import { parseSoapResponse } from "../../helpers/common";
import { getChungTuMadonvi } from "../../helpers/chungTuConstants";

interface ISelectBoxLoaiChungTuProps {
  onValueChanged: (value: string) => void;
  value: string;
  maxWidth?: any;
  isShowClearBtn?: boolean;
  loadData?: () => void;
  isFormLap?: boolean;
}

const SelectBoxLoaiChungTuPhatHanh = (props: ISelectBoxLoaiChungTuProps) => {
  const { loadData = () => {}, isFormLap = false, value } = props;
  const [open, setOpen] = useState(false);
  const { user } = useAuth();
  const [options, setOptions] = useState<any[]>([]);
  const [selected, setSelected] = useState<any>();
  const [isLoading, setIsLoading] = useState(false);

  const madonvi = getChungTuMadonvi(user);

  useEffect(() => {
    if (!madonvi) return;

    let cancelled = false;

    const loadLoaiChungTu = async () => {
      setIsLoading(true);
      const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <LayLoaiChungTu xmlns="http://tempuri.org/">
      <maDonVi>${madonvi}</maDonVi>
    </LayLoaiChungTu>
  </soap12:Body>
</soap12:Envelope>`;

      try {
        const res: string = await axiosClient.post(
          process.env.REACT_APP_API_CHUNG_TU as string,
          soap,
          {
            headers: {
              "Content-Type": "text/xml; charset=utf-8",
            },
          },
        );

        if (cancelled) return;

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

          const initial = value
            ? opts.find((item: any) => item.value === value) ?? opts[0]
            : opts[0];

          if (initial) {
            setSelected(initial);
            if (!value) {
              props.onValueChanged(initial.value);
            }
          }
        }
      } finally {
        if (!cancelled) {
          setIsLoading(false);
        }
      }
    };

    loadLoaiChungTu();

    return () => {
      cancelled = true;
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [madonvi, isFormLap]);

  useEffect(() => {
    if (!value || !options.length) return;
    const match = options.find((item) => item.value === value);
    if (match) {
      setSelected(match);
    }
  }, [value, options]);

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
              {children || (isLoading ? "Đang tải..." : "Chọn loại chứng từ")}
            </p>
          </Button>
        )}
        placeholderText="Search"
        open={open}
        onOpenChange={setOpen}
        items={options}
        selected={selected}
        onSelectedChange={(item: any) => {
          if (item) {
            props.onValueChanged(item?.value);
            setSelected(item);
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
