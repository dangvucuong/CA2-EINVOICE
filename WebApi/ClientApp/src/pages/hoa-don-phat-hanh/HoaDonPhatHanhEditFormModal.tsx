import { Box, Checkbox, FormControl } from "@primer/react";
import moment from "moment";
import { useEffect, useMemo, useState } from "react";
import { useForm } from "react-hook-form";
import SelectBoxLoaiHoaDonCT from "../../component-data/selectbox-loai-hoa-don-ct";
import SelectBoxHinhThucLoaiMaHD from "../../component-data/selectbox-tinh-chaxx";
import Button from "../../component-ui/button";
import Modal from "../../component-ui/modal";
import ModalActions from "../../component-ui/modal/ModalActions";
import Text from "../../component-ui/text";
import TextInput from "../../component-ui/text-input";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { useAppSelector } from "../../hooks/useAppSelector";
import { useAuth } from "../../hooks/useAuth";
import { useHinhThucLoaiMaHoaDons } from "../../hooks/useHinhThucLoaiMaHoaDon";
import { useLoaiHoaDon } from "../../hooks/useLoaiHoaDon";
import { useLoaiHoaDonCT, useLoaiHoaDonCTs } from "../../hooks/useLoaiHoaDonCT";
import { IHoaDonDangKyPhatHanh } from "../../models/responses/hoa-don/IHoaDonDangKyPhatHanh";
import { rootAction } from "../../state/actions/rootAction";
import { eReducerStatusBase } from "../../state/reducer-models/eReducerStatusBase";
import { NotifyHelper } from "../../helpers/toast";
import { isHoaDonThuongMai } from "../../utils/hoaDonKyHieu";

const HoaDonPhatHanhEditFormModal = ({
  hoaDonDangKyPhatHanhs,
}: {
  hoaDonDangKyPhatHanhs: IHoaDonDangKyPhatHanh[];
}) => {
  const dispatch = useAppDispatch();
  const { user } = useAuth();
  const { hoaDonDangKyPhatHanhEditing, status } = useAppSelector(
    (x) => x.hoaDon.hoaDonDangKyPhatHanhReducer
  );
  const [loaiHoaDonCTId, setLoaiHoaDonCTId] = useState(
    hoaDonDangKyPhatHanhEditing?.loai_hoa_don_ct_id ?? 0
  );
  const [hinhThucLoaiHoaDonId, setHinhThucLoaiHoaDonId] = useState(
    hoaDonDangKyPhatHanhEditing?.hinh_thuc_code ?? ""
  );
  const { hinhThucLoaiMaHoaDons } = useHinhThucLoaiMaHoaDons();
  const [kyHieuDoanhNghiep, setKyHieuDoanhNghiep] = useState(
    hoaDonDangKyPhatHanhEditing ? hoaDonDangKyPhatHanhEditing?.so_qd : "QD"
  );
  const [isChiuThue, setIsChiuThue] = useState(
    hoaDonDangKyPhatHanhEditing?.is_chiu_thue ?? false
  );
  const [isChange, setIsChange] = useState(false);

  const hinhThucLoaiMaHoaDon = useMemo(() => {
    return hinhThucLoaiMaHoaDons.find((x) => x.id === hinhThucLoaiHoaDonId);
  }, [hinhThucLoaiMaHoaDons, hinhThucLoaiHoaDonId]);
  const { loaiHoaDonCTs } = useLoaiHoaDonCTs();
  const { loaiHoaDonCT } = useLoaiHoaDonCT(loaiHoaDonCTId);
  const loaidHoaDonId = loaiHoaDonCT?.loai_hoa_don_id ?? 0;
  const { loaiHoaDon } = useLoaiHoaDon(loaidHoaDonId);
  const laHoaDonThuongMai = isHoaDonThuongMai(loaiHoaDonCT);
  const {
    register,
    handleSubmit,
    clearErrors,
    reset,
    setError,
    formState: { errors },
  } = useForm<IHoaDonDangKyPhatHanh>({
    shouldUseNativeValidation: false,
    defaultValues: {
      ...hoaDonDangKyPhatHanhEditing,
      ngay_su_dung: hoaDonDangKyPhatHanhEditing?.ngay_su_dung
        ? moment(hoaDonDangKyPhatHanhEditing?.ngay_su_dung).format("YYYY-MM-DD")
        : "",
      donvi_ma_dv:
        hoaDonDangKyPhatHanhEditing?.donvi_ma_dv ?? user?.donvi_ma_dv,
    },
  });
  useEffect(() => {
    setIsChange(false);
    reset({
      ...hoaDonDangKyPhatHanhEditing,
      ngay_su_dung: hoaDonDangKyPhatHanhEditing?.ngay_su_dung
        ? moment(hoaDonDangKyPhatHanhEditing?.ngay_su_dung).format("YYYY-MM-DD")
        : "",
      donvi_ma_dv:
        hoaDonDangKyPhatHanhEditing?.donvi_ma_dv ?? user?.donvi_ma_dv,
    });
  }, [hoaDonDangKyPhatHanhEditing]);
  const kyHieuHoaDon = useMemo(() => {
    let kyHieu = "";
    const mauSo = loaiHoaDon?.code ?? "";
    const CKM = hinhThucLoaiHoaDonId != "M" ? hinhThucLoaiHoaDonId : "C";
    const YY = moment().format("YYYY").substring(2, 4);
    let CTCode = hinhThucLoaiHoaDonId === "M" ? "M" : loaiHoaDonCT?.code ?? "";
    if (CTCode === "G" && !isChiuThue) CTCode = "H";
    if (laHoaDonThuongMai) CTCode = "X";
    return `${CKM}${YY}${CTCode}${kyHieuDoanhNghiep}`;
  }, [
    hoaDonDangKyPhatHanhEditing,
    loaiHoaDon,
    loaiHoaDonCT,
    hinhThucLoaiHoaDonId,
    kyHieuDoanhNghiep,
    isChiuThue,
    laHoaDonThuongMai,
  ]);

  const onSubmit = async (data: any) => {
    if (!loaiHoaDonCTId || loaiHoaDonCTId === 0) {
      setError("loai_hoa_don_ct_id", {
        type: "manual",
        message: "Vui lòng chọn loại hóa đơn",
      });

      return;
    }

    if (!hinhThucLoaiHoaDonId || hinhThucLoaiHoaDonId === "") {
      setError("hinh_thuc_code", {
        type: "manual",
        message: "Vui lòng chọn hình thức",
      });
      return;
    }

    // check ký hiệu hợp lệ
    // check pattern naymwf phải nằm trong các chữ sau A, B, C, D, E, G, H, K, L, M, N, P, Q, R, S, T, U, V, X, Y
    const pattern = /^[ABCDEGHKLMNPQRSTUVXY]*$/i;

    if (!pattern.test(kyHieuHoaDon.substring(4, 6))) {
      NotifyHelper.Error(`Ký hiệu hóa đơn nằm ngoài 20 kí tự cho phép!`);
      return;
    }

    // check exist ky_hieu in list hoaDonDangKyPhatHanhs
    const checkExist = hoaDonDangKyPhatHanhs.find(
      (x) =>
        x.ky_hieu === kyHieuHoaDon && x.id !== hoaDonDangKyPhatHanhEditing?.id
    );

    if (checkExist) {
      NotifyHelper.Error(`Ký hiệu đã tồn tại và đang còn số hóa đơn sử dụng!`);
      return;
    }

    dispatch(
      rootAction.hoaDon.hoaDonDangKyPhatHanhAction.saveStart({
        ...hoaDonDangKyPhatHanhEditing,
        ...data,
        ten_hoa_don: loaiHoaDonCT?.name ?? "",
        loai_hoa_don_ct_id: loaiHoaDonCTId,
        mau_so: loaiHoaDon?.code ?? "",
        so_qd: kyHieuDoanhNghiep,
        ky_hieu: kyHieuHoaDon,
        hinh_thuc_code: hinhThucLoaiHoaDonId,
        is_chiu_thue: isChiuThue,
      })
    );
  };
  return (
    <Modal
      title={
        (hoaDonDangKyPhatHanhEditing?.id ?? 0) === 0 ? "Thêm mới" : "Cập nhật"
      }
      onClose={() => {
        dispatch(rootAction.hoaDon.hoaDonDangKyPhatHanhAction.closeEditModal());
      }}
      isOpen={true}
      width="large"
      height={"auto"}
      key={hoaDonDangKyPhatHanhEditing?.id ?? 0}
    >
      <form onSubmit={handleSubmit(onSubmit)}>
        <Box
          display={"grid"}
          sx={{
            gap: 2,
          }}
        >
          <FormControl>
            <FormControl.Label>
              <Text text="Mã đơn vị bán hàng" />
            </FormControl.Label>
            <TextInput
              register={register}
              name="donvi_ma_dv"
              disabled
              errors={errors}
            />
          </FormControl>
          <FormControl>
            <FormControl.Label>
              <Text text="Loại hóa đơn" />
            </FormControl.Label>
            <SelectBoxLoaiHoaDonCT
              isOnlyShowDaThietLapMau={true}
              onValueChanged={(id) => {
                setLoaiHoaDonCTId(id);
                setIsChange(true);
                clearErrors("loai_hoa_don_ct_id");
                const selectedLoaiHoaDonCT = loaiHoaDonCTs.find(
                  (x) => x.id === id
                );
                if (isHoaDonThuongMai(selectedLoaiHoaDonCT)) {
                  setHinhThucLoaiHoaDonId("K");
                  clearErrors("hinh_thuc_code");
                }
              }}
              value={loaiHoaDonCTId}
            />
            {errors && errors["loai_hoa_don_ct_id"] && (
              <FormControl.Validation variant="error">
                Vui lòng chọn loại hóa đơn
              </FormControl.Validation>
            )}
          </FormControl>
          {loaiHoaDonCT?.code === "G" && (
            <FormControl>
              <FormControl.Label>
                <Text text="Chịu thuế" />
              </FormControl.Label>
              <Checkbox
                checked={isChiuThue}
                onChange={(e) => {
                  setIsChiuThue(e.target.checked);
                  setIsChange(true);
                }}
              />
            </FormControl>
          )}
          <FormControl>
            <FormControl.Label>
              Loại hóa đơn
              <Text text="Hình thức" />
            </FormControl.Label>
            <SelectBoxHinhThucLoaiMaHD
              onValueChanged={(id) => {
                setHinhThucLoaiHoaDonId(id);
                setIsChange(true);
                clearErrors("hinh_thuc_code");
              }}
              value={hinhThucLoaiHoaDonId}
            />
            {errors && errors["hinh_thuc_code"] && (
              <FormControl.Validation variant="error">
                Vui lòng chọn hình thức
              </FormControl.Validation>
            )}
          </FormControl>
          <Box className="row">
            <Box className="col-md-6">
              <FormControl>
                <FormControl.Label>
                  <Text text="Ký hiệu của doanh nghiệp" />
                </FormControl.Label>
                <TextInput
                  register={register}
                  name="so_qd"
                  value={kyHieuDoanhNghiep}
                  onChange={(e) => {
                    if (e.target.value.length > 2) {
                      return;
                    }

                    setKyHieuDoanhNghiep(e.target.value);
                    setIsChange(true);
                  }}
                  required
                  validateMessage="Vui lòng Số quyết định"
                  errors={errors}
                />
              </FormControl>
            </Box>
            <Box className="col-md-6">
              <FormControl>
                <FormControl.Label>
                  <Text text="Ký hiệu hóa đơn" />
                </FormControl.Label>
                <TextInput
                  register={register}
                  // name='ky_hieu'
                  value={
                    isChange
                      ? kyHieuHoaDon
                      : hoaDonDangKyPhatHanhEditing?.ky_hieu
                  }
                  required
                  readOnly
                  validateMessage="Vui lòng Ký hiệu"
                  errors={errors}
                />
              </FormControl>
            </Box>
          </Box>
          <Box className="row">
            <Box className="col-md-6">
              <FormControl>
                <FormControl.Label>
                  <Text text="Số bắt đầu" />
                </FormControl.Label>
                <TextInput
                  register={register}
                  name="so_bat_dau"
                  required
                  type="number"
                  validateMessage="Vui lòng điền Số bắt đầu"
                  errors={errors}
                />
              </FormControl>
            </Box>
            <Box className="col-md-6">
              <FormControl>
                <FormControl.Label>
                  <Text text="Số kết thúc" />
                </FormControl.Label>
                <TextInput
                  register={register}
                  name="so_ket_thuc"
                  type="number"
                  required
                  validateMessage="Vui lòng điền Số kết thúc"
                  errors={errors}
                  onChange={(e) => {
                    if (parseInt(e.target.value) < 0) {
                      e.target.value = "0";
                    }
                  }}
                />
              </FormControl>
            </Box>
          </Box>
          <Box>
            <FormControl>
              <FormControl.Label>
                <Text text="Ngày sử dụng" />
              </FormControl.Label>
              <TextInput
                register={register}
                name="ngay_su_dung"
                required
                type="date"
                validateMessage="Vui lòng điền Ngày sử dụng"
                errors={errors}
              />
            </FormControl>
          </Box>

          <ModalActions>
            <Button
              onClick={() => {
                dispatch(
                  rootAction.hoaDon.hoaDonDangKyPhatHanhAction.closeEditModal()
                );
              }}
              text="Đóng"
            />
            <Button
              variant="primary"
              type="submit"
              text={
                (hoaDonDangKyPhatHanhEditing?.id ?? 0) === 0
                  ? "Thêm mới"
                  : "Cập nhật"
              }
              isLoading={status === eReducerStatusBase.is_saving}
            />
          </ModalActions>
        </Box>
      </form>
    </Modal>
  );
};

export default HoaDonPhatHanhEditFormModal;
