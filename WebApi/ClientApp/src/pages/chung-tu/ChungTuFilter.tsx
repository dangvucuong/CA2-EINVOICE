import { Box } from "@primer/react";
import SelectBoxLoaiChungTuPhatHanh from "../../component-data/selectbox-loai-chung-tu-phat-hanh";
import { memo } from "react";
import SelectBoxMauSoChungTuPhatHanh from "../../component-data/selectbox-mau-so-chung-tu-phat-hanh";
import SelectBoxKyHieuChungTuPhatHanh from "../../component-data/selectbox-ky-hieu-chung-tu-phat-hanh";
import TuNgayDenNgayInput from "../../component-ui/tu-ngay-den-ngay-input/TuNgayDenNgayInput";
import SelectBoxKyHieuChungTuQuanLy from "../../component-data/selectbox-ky-hieu-chung-tu-quan-ly";

const ChungTuFilter = ({
  dataFilter,
  setValueFilter,
  loadData,
}: {
  dataFilter: any;
  setValueFilter: (data: any) => void;
  loadData: (
    changes: Partial<{
      mau_so: string;
      ky_hieu: string;
      tu_ngay: string;
      den_ngay: string;
    }>,
  ) => void;
}) => {
  return (
    <>
      <SelectBoxLoaiChungTuPhatHanh
        isShowClearBtn
        value={dataFilter.loai_chung_tu}
        onValueChanged={(value: string) => {
          setValueFilter({
            ...dataFilter,
            loai_chung_tu: value,
            mau_so: value,
            ky_hieu: "",
          });
          loadData({
            mau_so: value,
            ky_hieu: "",
          });
        }}
      />
      <Box>
        <SelectBoxMauSoChungTuPhatHanh
          value={dataFilter.mau_so}
          onValueChanged={(value: string) => {
            setValueFilter({ ...dataFilter, mau_so: value });
            loadData({
              mau_so: value,
              ky_hieu: "",
            });
          }}
          loai_chung_tu={dataFilter.loai_chung_tu}
        />
      </Box>
      <Box>
        <SelectBoxKyHieuChungTuQuanLy
          value={dataFilter.ky_hieu}
          onValueChanged={(value: string) => {
            setValueFilter({ ...dataFilter, ky_hieu: value });
            loadData({
              ky_hieu: value,
            });
          }}
          mau_so={dataFilter.mau_so}
        />
      </Box>
      <Box>
        <TuNgayDenNgayInput
          tu_ngay={dataFilter.tu_ngay}
          den_ngay={dataFilter.den_ngay}
          onValueChanged={(tu_ngay?: string, den_ngay?: string) => {
            setValueFilter({ ...dataFilter, tu_ngay, den_ngay });
            loadData({
              tu_ngay: tu_ngay,
              den_ngay: den_ngay,
            });
          }}
        />
      </Box>
    </>
  );
};

export default memo(ChungTuFilter);
