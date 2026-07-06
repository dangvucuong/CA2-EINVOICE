import { TriangleDownIcon } from "@primer/octicons-react";

import { Button, SelectPanel } from "@primer/react";
import { memo, useEffect, useState } from "react";

interface ISelectBoxMauSoChungTuPhatHanhProps {
  onValueChanged: (value: string) => void;
  value: string;
  maxWidth?: any;
  isShowClearBtn?: boolean;
  loai_chung_tu?: string;
}

const data = [
  {
    id: 1,
    value: "03/TNCN",
    text: "03/TNCN",
  },
  {
    id: 2,
    value: "CTT56",
    text: "CTT56",
  },
];

const SelectBoxMauSoChungTuPhatHanh = (
  props: ISelectBoxMauSoChungTuPhatHanhProps,
) => {
  const { loai_chung_tu, maxWidth, value, onValueChanged = () => {} } = props;
  const [open, setOpen] = useState(false);
  const [selected, setSelected] = useState<any>();

  useEffect(() => {
    const code = loai_chung_tu || value;
    if (!code) {
      setSelected(undefined);
      return;
    }

    const option = data.find((item) => item.value === code);
    if (option) {
      setSelected(option);
      if (!value) {
        onValueChanged(option.value);
      }
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [loai_chung_tu, value]);

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
              {children || "Mẫu số"}
            </p>
          </Button>
        )}
        placeholderText="Search"
        open={open}
        onOpenChange={setOpen}
        items={data}
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

export default memo(SelectBoxMauSoChungTuPhatHanh);
