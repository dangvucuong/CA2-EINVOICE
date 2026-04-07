import { Box, FormControl } from "@primer/react";
import moment from "moment";
import { memo, useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import Button from "../../component-ui/button";
import Modal from "../../component-ui/modal";
import ModalActions from "../../component-ui/modal/ModalActions";
import Text from "../../component-ui/text";
import TextInput from "../../component-ui/text-input";
import { useAuth } from "../../hooks/useAuth";
import SelectBoxLoaiChungTu from "../../component-data/selectbox-loai-chung-tu";
import { NotifyHelper } from "../../helpers/toast";
import { axiosClient } from "../../api/axiosClient";
import { parseSoapResponse } from "../../helpers/common";

const ChungTuPhatHanhEditFormModal = (props: {
  onClose: () => void;
  detailData: any;
  onSuccess: () => void;
}) => {
  const { user } = useAuth();
  const { detailData, onSuccess = () => {}, onClose = () => {} } = props;

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<any>({
    shouldUseNativeValidation: false,
    defaultValues: {
      donvi_ma_dv: user?.donvi_ma_dv,
    },
  });

  useEffect(() => {
    if (detailData) {
      console.log(detailData);
      reset({
        donvi_ma_dv: detailData.donvi_ma_dv,
        ky_hieu_chung_tu: detailData.ky_hieu,
        ma_so_chung_tu: detailData.mau_so,
        so_bat_dau: detailData.so_bat_dau,
        so_ket_thuc: detailData.so_ket_thuc,
        ngay_su_dung: moment(detailData.ngay_su_dung).format("YYYY-MM-DD"),
      });
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [detailData]);

  const onSubmit = async (data: any) => {
    if (user?.user_id) {
      if (detailData) {
        await Capnhatphathanhchungtu({
          madonvi: detailData.donvi_ma_dv || "",
          kyhieuct: detailData.ky_hieu || "",
          idphathanh: detailData.id,
          sobatdau: data.so_bat_dau?.toString() || "",
          soketthuc: data.so_ket_thuc?.toString() || "",
        });
      } else {
        await ThemMoi({
          madonvi: data.donvi_ma_dv || "",
          kyhieuct: data.ky_hieu_chung_tu || "",
          mausoct: data.ma_so_chung_tu || "",
          sobatdau: data.so_bat_dau?.toString() || "",
          soketthuc: data.so_ket_thuc?.toString() || "",
          ngaysudung: moment(data.ngay_su_dung).format("YYYY-MM-DD"),
          loaict: "Chứng từ khấu trừ thuế thu nhập cá nhân theo ND70",
          userid: user?.user_id?.toString(),
        });
      }
    }
  };

  const ThemMoi = async ({
    madonvi,
    kyhieuct,
    mausoct,
    sobatdau,
    soketthuc,
    ngaysudung,
    loaict,
    userid,
  }: {
    madonvi: string;
    kyhieuct: string;
    mausoct: string;
    sobatdau: string;
    soketthuc: string;
    ngaysudung: string;
    loaict: string;
    userid: string;
  }) => {
    const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <DangKyPhatHanhChungTu xmlns="http://tempuri.org/">
        <madonvi>${madonvi}</madonvi>
        <kyhieuct>${kyhieuct}</kyhieuct>
        <mausoct>${mausoct}</mausoct>
        <sobatdau>${sobatdau}</sobatdau>
        <soketthuc>${soketthuc}</soketthuc>
        <ngaysudung>${ngaysudung}</ngaysudung>
        <loaict>${loaict}</loaict>
        <userid>${userid}</userid>
    </DangKyPhatHanhChungTu>
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
      NotifyHelper.Success(parseRes.message);
      onClose();
      onSuccess();
    } else {
      NotifyHelper.Error(parseRes.message);
    }
  };

  const Capnhatphathanhchungtu = async ({
    madonvi,
    kyhieuct,
    idphathanh,
    sobatdau,
    soketthuc,
  }: {
    madonvi: string;
    kyhieuct: string;
    idphathanh: string;
    sobatdau: string;
    soketthuc: string;
  }) => {
    const soluongmoi = (
      parseInt(soketthuc || "0") -
      parseInt(sobatdau || "0") +
      1
    ).toString();

    const soap = `<?xml version="1.0" encoding="utf-8"?>
    <soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
      <soap12:Body>
        <Capnhatphathanhchungtu xmlns="http://tempuri.org/">
            <soluongmoi>${soluongmoi}</soluongmoi>
            <madonvi>${madonvi}</madonvi>
            <idphathanh>${idphathanh}</idphathanh>
            <kyhieu>${kyhieuct}</kyhieu>
        </Capnhatphathanhchungtu>
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
      NotifyHelper.Success(parseRes.message);
      onClose();
      onSuccess();
    } else {
      NotifyHelper.Error(parseRes.message);
    }
  };

  return (
    <Modal
      title={detailData ? "Cập nhật" : "Thêm mới"}
      onClose={() => {
        reset();
        props.onClose();
      }}
      isOpen={true}
      width="large"
      height={"auto"}
      key={0}
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
              <Text text="Loại chứng từ" />
            </FormControl.Label>
            <SelectBoxLoaiChungTu
              isOnlyShowDaThietLapMau={true}
              onValueChanged={(id) => {}}
              value={1}
            />
          </FormControl>
          <Box className="row">
            <Box className="col-md-6">
              <FormControl>
                <FormControl.Label>
                  <Text text="Mã số chứng từ" />
                </FormControl.Label>
                <TextInput
                  register={register}
                  name="ma_so_chung_tu"
                  value={"03/TNCN"}
                  readOnly
                />
              </FormControl>
            </Box>
            <Box className="col-md-6">
              <FormControl>
                <FormControl.Label>
                  <Text text="Ký hiệu chứng từ" />
                </FormControl.Label>
                <TextInput
                  register={register}
                  name="ky_hieu_chung_tu"
                  value={`CT/${moment().format("YY")}E`}
                  readOnly
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
                reset();
                props.onClose();
              }}
              text="Đóng"
            />
            <Button
              variant="primary"
              type="submit"
              text={
                // (hoaDonDangKyPhatHanhEditing?.id ?? 0) === 0
                //   ? "Thêm mới"
                //   : "Cập nhật"

                "Thêm mới"
              }
              // isLoading={status === eReducerStatusBase.is_saving}
            />
          </ModalActions>
        </Box>
      </form>
    </Modal>
  );
};

export default memo(ChungTuPhatHanhEditFormModal);
