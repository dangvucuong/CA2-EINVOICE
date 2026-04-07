import { Box } from "@primer/react";
import SelectBoxKyHieuPhatHanh from "../../component-data/selectbox-ky-hieu-phat-hanh";
import SelectBoxLoaiHoaDonCTPhatHanh from "../../component-data/selectbox-loai-hoa-don-ct-phat-hanh";
import SelectBoxMauSoPhatHanh from "../../component-data/selectbox-mau-so-phat-hanh";
import TuNgayDenNgayInput from "../../component-ui/tu-ngay-den-ngay-input/TuNgayDenNgayInput";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { useAppSelector } from "../../hooks/useAppSelector";
import { IHoaDonSelectPagingRequest } from "../../models/requests/hoa-don/IHoaDonSelectPagingRequest";

interface IHoaDonDieuChinhFilterProps {
  filter: IHoaDonSelectPagingRequest;
  onChanged: (filter: IHoaDonSelectPagingRequest) => void;
}
const HoaDonDieuChinhFilter = (props: IHoaDonDieuChinhFilterProps) => {
  const { filter } = props;
  //   const { filter } = useAppSelector((x) => x.hoaDon.hoaDonReducer);
  const dispatch = useAppDispatch();

  return (
    <>
      <SelectBoxLoaiHoaDonCTPhatHanh
        isShowClearBtn
        value={filter.loai_hoa_don_ct_id}
        onValueChanged={(id) => {
          props.onChanged({
            ...filter,
            loai_hoa_don_ct_id: id,
            hoa_don_dang_ky_phat_hanh_mau_so: "",
            hoa_don_dang_ky_phat_hanh_ky_hieu: "",
          });
        }}
      />
      <Box>
        <SelectBoxMauSoPhatHanh
          value={filter.hoa_don_dang_ky_phat_hanh_mau_so}
          loai_hoa_don_ct_id={filter.loai_hoa_don_ct_id}
          isAutoSelectIfHasOneItem
          isShowClearBtn
          onValueChanged={(id) => {
            props.onChanged({
              ...filter,
              hoa_don_dang_ky_phat_hanh_mau_so: id,
              hoa_don_dang_ky_phat_hanh_ky_hieu: "",
            });
          }}
        />
      </Box>
      <Box>
        <SelectBoxKyHieuPhatHanh
          value={filter.hoa_don_dang_ky_phat_hanh_ky_hieu}
          isAutoSelectIfHasOneItem
          isShowClearBtn
          loai_hoa_don_ct_id={filter.loai_hoa_don_ct_id}
          mau_so={filter.hoa_don_dang_ky_phat_hanh_mau_so}
          onValueChanged={(id) => {
            props.onChanged({
              ...filter,
              hoa_don_dang_ky_phat_hanh_ky_hieu: id,
            });
          }}
        />
      </Box>
      <TuNgayDenNgayInput
        tu_ngay={filter.tu_ngay}
        den_ngay={filter.den_ngay}
        onValueChanged={(tu_ngay, den_ngay) => {
          props.onChanged({
            ...filter,
            tu_ngay: tu_ngay,
            den_ngay: den_ngay,
          });
        }}
      />
      {/* <FormGroupInline label="Từ ngày">
                <TextInput type="date" value={filter.tu_ngay}
                    onBlur={(e) => {
                        props.onChanged({
                            ...filter,
                            tu_ngay: e.target.value
                        })
                    }}
                />
            </FormGroupInline>
            <FormGroupInline label="Đến ngày">
                <TextInput type="date"
                    value={filter.den_ngay}
                    onBlur={(e) => {
                        props.onChanged({
                            ...filter,
                            den_ngay: e.target.value
                        })
                    }}
                />
            </FormGroupInline> */}
    </>
  );
};

export default HoaDonDieuChinhFilter;
