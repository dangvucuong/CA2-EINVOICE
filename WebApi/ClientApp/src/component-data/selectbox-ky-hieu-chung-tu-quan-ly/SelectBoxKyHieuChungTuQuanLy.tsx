import { TriangleDownIcon } from "@primer/octicons-react";

import { Button, SelectPanel } from "@primer/react";
import { useEffect, useRef, useState } from "react";
import { useAuth } from "../../hooks/useAuth";
import { axiosClient } from "../../api/axiosClient";
import { parseSoapResponse } from "../../helpers/common";
import {
  getChungTuMadonvi,
  resolveChungTuMauSo,
} from "../../helpers/chungTuConstants";

interface ISelectBoxKyHieuChungTuQuanLyProps {
  onValueChanged: (value: string) => void;
  value: string;
  maxWidth?: any;
  isShowClearBtn?: boolean;
  mau_so?: string;
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
  const [isLoading, setIsLoading] = useState(false);
  const onValueChangedRef = useRef(onValueChanged);
  onValueChangedRef.current = onValueChanged;

  const madonvi = getChungTuMadonvi(user);
  const resolvedMauSo = resolveChungTuMauSo(mau_so);

  useEffect(() => {
    if (!madonvi) {
      setOptions([]);
      setSelected(undefined);
      return;
    }

    let cancelled = false;
    const requestMauSo = resolvedMauSo;
    const requestMadonvi = madonvi;

    const loadKyHieu = async () => {
      setIsLoading(true);
      const soap = `<?xml version="1.0" encoding="utf-8"?>
    <soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
      <soap12:Body>
        <LayDanhSachKyHieu  xmlns="http://tempuri.org/">
          <mauso>${requestMauSo}</mauso>
          <madonvi>${requestMadonvi}</madonvi>
        </LayDanhSachKyHieu>
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

        if (parseRes.status === "success") {
          const seen = new Set<string>();
          const opts = (parseRes?.data ?? [])
            .map((item: any, index: number) => ({
              id: index,
              text: item.ky_hieu,
              value: item.ky_hieu,
            }))
            .filter((item: any) => {
              if (!item.value || seen.has(item.value)) return false;
              seen.add(item.value);
              return true;
            });

          setOptions(opts);

          const selectedOption = value
            ? opts.find((item: any) => item.value === value)
            : opts.length === 1
              ? opts[0]
              : undefined;

          if (selectedOption) {
            setSelected(selectedOption);
            if (!value) {
              onValueChangedRef.current(selectedOption.value);
            }
          } else {
            setSelected(undefined);
          }
        } else {
          setOptions([]);
          setSelected(undefined);
        }
      } finally {
        if (!cancelled) {
          setIsLoading(false);
        }
      }
    };

    loadKyHieu();

    return () => {
      cancelled = true;
    };
  }, [resolvedMauSo, madonvi]);

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
                maxWidth: maxWidth,
                overflow: "hidden",
                textOverflow: "ellipsis",
              }}
            >
              {children || (isLoading ? "Đang tải..." : "Ký hiệu")}
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
            onValueChanged(item?.value);
            setSelected(item);
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
