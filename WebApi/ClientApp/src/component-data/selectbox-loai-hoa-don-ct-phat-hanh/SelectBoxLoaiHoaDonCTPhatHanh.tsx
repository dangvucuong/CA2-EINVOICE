import { TriangleDownIcon, XCircleFillIcon } from "@primer/octicons-react";

import { Box, Button, SelectPanel } from "@primer/react";
import { useMemo, useState } from "react";
import { useHoaDonDangKyPhatHanhLoader } from "../../hooks/useHoaDonDangKyPhatHanhLoader";
import { useLoaiHoaDonCTs } from "../../hooks/useLoaiHoaDonCT";
import { isDangKyPhatHanhMtt } from "../../utils/hoaDonKyHieu";
import { isDangKyPhatHanhInCurrentYear } from "../../utils/dangKyPhatHanhFilter";

interface ISelectBoxLoaiHoaDonCTPhatHanhProps {
  onValueChanged: (id: number, tenHoaDon?: string) => void;
  value: number;
  maxWidth?: any;
  isShowClearBtn?: boolean;
  /** Chỉ hiển thị loại HĐ có đăng ký phát hành ký hiệu MTT */
  onlyMtt?: boolean;
}

const SelectBoxLoaiHoaDonCTPhatHanh = (
  props: ISelectBoxLoaiHoaDonCTPhatHanhProps
) => {
  const [open, setOpen] = useState(false);
  const { hoaDonDangKyPhatHanhs, isLoading, isLoadError } =
    useHoaDonDangKyPhatHanhLoader();
  const { loaiHoaDonCTs } = useLoaiHoaDonCTs();
  const [filter, setFilter] = useState("");

  const resolveTenHoaDon = (loaiHoaDonCtId: number, tenHoaDon?: string) => {
    const fromDkph = (tenHoaDon ?? "").trim();
    if (fromDkph) return fromDkph;
    return (
      loaiHoaDonCTs.find((x) => x.id === loaiHoaDonCtId)?.name?.trim() || ""
    );
  };

  const dataSource = useMemo(() => {
    const byId = new Map<number, { id: number; text: string }>();
    hoaDonDangKyPhatHanhs
      .filter((x) => {
        if (!isDangKyPhatHanhInCurrentYear(x?.ngay_su_dung)) {
          return false;
        }
        if (props.onlyMtt) {
          return isDangKyPhatHanhMtt(x);
        }
        return true;
      })
      .forEach((x) => {
        const text = resolveTenHoaDon(x.loai_hoa_don_ct_id, x.ten_hoa_don);
        const existing = byId.get(x.loai_hoa_don_ct_id);
        // Ưu tiên bản ghi đã có tên (tránh giữ item text rỗng)
        if (!existing || (!existing.text && text)) {
          byId.set(x.loai_hoa_don_ct_id, {
            id: x.loai_hoa_don_ct_id,
            text,
          });
        }
      });
    return Array.from(byId.values()).sort((a, b) => a.id - b.id);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [hoaDonDangKyPhatHanhs, props.onlyMtt, loaiHoaDonCTs]);

  const filterdData = useMemo(() => {
    return dataSource.filter((item) =>
      item.text.toLowerCase().includes(filter.toLowerCase())
    );
  }, [dataSource, filter]);

  // Chỉ chọn được loại HĐ có ĐKPH năm hiện tại — không fallback năm cũ.
  const _selectedData = useMemo(() => {
    if (props.value <= 0) return undefined;
    return dataSource.find((item) => item.id === props.value);
  }, [props.value, dataSource]);

  const placeholderText = isLoading
    ? "Đang tải..."
    : isLoadError
      ? "Lỗi tải dữ liệu"
      : "Chọn loại hóa đơn";

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
            disabled={isLoading}
            {...anchorProps}
          >
            <p
              style={{
                maxWidth: props.maxWidth,
                overflow: "hidden",
                textOverflow: "ellipsis",
              }}
            >
              {_selectedData?.text || placeholderText}
            </p>
          </Button>
        )}
        title={
          <>
            <Box sx={{ display: "flex", alignItems: "center" }}>
              <Box sx={{ flex: 1 }}>Chọn loại hóa đơn</Box>
              {props.isShowClearBtn && props.value > 0 && (
                <Button
                  trailingVisual={XCircleFillIcon}
                  variant="invisible"
                  sx={{
                    color: "danger.emphasis",
                  }}
                  onClick={() => {
                    props.onValueChanged(0, "");
                  }}
                >
                  Bỏ chọn
                </Button>
              )}
            </Box>
          </>
        }
        placeholderText="Search"
        open={open}
        onOpenChange={setOpen}
        items={filterdData}
        selected={_selectedData}
        onSelectedChange={(data: any) => {
          if (data) {
            props.onValueChanged(data.id, data.text);
          }
        }}
        onFilterChange={setFilter}
        showItemDividers={true}
        overlayProps={{ width: "small", height: "medium" }}
      />
    </>
  );
};

export default SelectBoxLoaiHoaDonCTPhatHanh;
