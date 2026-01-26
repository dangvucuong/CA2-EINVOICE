import { Box, Checkbox, FormControl, Radio, RadioGroup } from "@primer/react";
import { PlusCircleIcon, UploadIcon } from "@primer/octicons-react";
import { useEffect, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import Button from "../../component-ui/button";
import Modal from "../../component-ui/modal";
import ModalActions from "../../component-ui/modal/ModalActions";
import { IDonViCts } from "../../models/responses/category/IDonViCts";
import { UploadCer } from "../../component-data/upload";
import { IUploadCerRespone } from "../../models/responses/upload/IUploadCerRespone";
import { useCommonContext } from "../../contexts/common";
import TextInput from "../../component-ui/text-input";
import { donViCtsApi } from "../../api/category/donViCtsApi";
import { NotifyHelper } from "../../helpers/toast";
import moment from "moment";
import { useAuth } from "../../hooks/useAuth";
import Text from "../../component-ui/text";

interface IEditModalProps {
  data?: IDonViCts;
  onClose: () => void;
  onSuccess: () => void;
}
const EditModal = (props: IEditModalProps) => {
  const [isSaving, setIsSaving] = useState(false);
  const { user } = useAuth();
  const [signType, setSignType] = useState<"token" | "remote">("token");

  const {
    _signalrConnected,
    createUUID,
    _signalrConnection,
    _signalrSelectCert,
    _signalrSignLogin,
    getMSTFromCertSubject,
  } = useCommonContext();
  const {
    register,
    handleSubmit,
    reset,
    control,
    setValue,
    getValues,
    formState: { errors },
  } = useForm<IDonViCts>({
    shouldUseNativeValidation: false,
    defaultValues: {
      ...props.data,
      not_before: props.data
        ? moment(props.data?.not_before).format("YYYY-MM-DD")
        : undefined,
      not_after: props.data
        ? moment(props.data?.not_after).format("YYYY-MM-DD")
        : undefined,
    },
  });

  const handler = function (eventName: any, data: any) {
    if (eventName === "SERVER") {
      const ketquas = data.split("|");
      const [returnCode, code, signedtext] = ketquas;

      if (signedtext === "CertInf") {
        const [nhaCungCap, serial, tuNgay, denNgay, subject] = ketquas.slice(3);
        let issuer = nhaCungCap;
        const match = nhaCungCap.match(/CN=([^,]+)/);
        if (match) {
          issuer = match[1];
        } else {
        }
        const data: any = {
          returnCode,
          code,
          signedtext,
          nhaCungCap,
          serial,
          tuNgay,
          denNgay,
          subject,
          issuer,
        };

        const mstCert = getMSTFromCertSubject(subject);

        if (mstCert !== user?.donvi_ma_dv) {
          NotifyHelper.Error(
            "Mã số thuế trên chứng thư số không khớp với mã số thuế người nộp thuế",
          );

          return;
        }

        reset({
          ...getValues(),
          not_after: moment(denNgay).format("YYYY-MM-DD"),
          not_before: moment(tuNgay).format("YYYY-MM-DD"),
          issuer: issuer,
          serial_number: serial,
          signature_algorithm: "",
          subject: subject,
          version: "",
        });

        // console.log({
        //   data,
        // });
      }
    }
  };

  useEffect(() => {
    reset({
      ...props.data,
      not_before: props.data
        ? moment(props.data?.not_before).format("YYYY-MM-DD")
        : undefined,
      not_after: props.data
        ? moment(props.data?.not_after).format("YYYY-MM-DD")
        : undefined,
    });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [props.data]);

  useEffect(() => {
    if (_signalrConnected && _signalrConnection) {
      _signalrConnection.on("addMessage", handler);

      // ✅ cleanup khi unmount hoặc reconnect
      return () => {
        _signalrConnection.off("addMessage", handler);
      };
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [_signalrConnected, _signalrConnection, getValues, reset]);

  const onSubmit = async (data: any) => {
    setIsSaving(true);
    if ((props?.data?.id ?? 0) > 0) {
      const res = await donViCtsApi.update({
        ...data,
        id: props.data?.id ?? 0,
      });
      setIsSaving(false);
      if (res.is_success) {
        NotifyHelper.Success("Thành công");
        props.onSuccess();
      } else {
        NotifyHelper.Error(res.message ?? "Có lỗi");
      }
    } else {
      const res = await donViCtsApi.insert({
        ...data,
        id: props.data?.id ?? 0,
      });
      setIsSaving(false);
      if (res.is_success) {
        NotifyHelper.Success("Thành công");
        props.onSuccess();
      } else {
        NotifyHelper.Error(res.message ?? "Có lỗi");
      }
    }
    setIsSaving(false);
  };

  return (
    <Modal
      title={(props.data?.id ?? 0) === 0 ? "Thêm mới" : "Cập nhật"}
      onClose={() => {
        props.onClose();
      }}
      isOpen={true}
      width="large"
      height={"auto"}
      key={props.data?.id ?? 0}
    >
      <form onSubmit={handleSubmit(onSubmit)}>
        <Box
          display={"grid"}
          sx={{
            gap: 2,
          }}
        >
          <Box
            sx={{
              display: "flex",
              gap: 2,
              pt: 1,
              width: "auto",
            }}
          >
            <Box
              display="flex"
              alignItems="center"
              sx={{
                gap: 2,
              }}
            >
              <Radio
                value="0"
                checked={signType === "token"}
                onChange={(e) => {
                  setSignType("token");
                }}
              />
              <Text
                text="Token"
                sx={{
                  display: "block",
                }}
              ></Text>
            </Box>
            <Box
              display="flex"
              alignItems="center"
              sx={{
                gap: 2,
              }}
            >
              <Radio
                value="1"
                checked={signType === "remote"}
                onChange={(e) => {
                  setSignType("remote");
                }}
              />
              <Text
                text="Remote Signing"
                sx={{
                  display: "block",
                }}
              ></Text>
            </Box>
          </Box>

          <Box sx={{ display: "flex" }}>
            <Button
              text="Chọn chứng thư số đã cài đặt"
              leadingVisual={PlusCircleIcon}
              variant="invisible"
              disabled={!_signalrConnected}
              onClick={() => {
                _signalrSelectCert();
              }}
              tooltip={!_signalrConnected ? "Chưa kết nối tool ký số" : ""}
            ></Button>
          </Box>

          <Box>Hoặc upload từ file chứng thư số</Box>
          <UploadCer
            onUploadSuccess={(data: IUploadCerRespone) => {
              reset({
                ...getValues(),
                ...data.cer_info,
                not_after: moment(data.cer_info.not_after).format("YYYY-MM-DD"),
                not_before: moment(data.cer_info.not_before).format(
                  "YYYY-MM-DD",
                ),
              });
            }}
          />

          {signType === "remote" && (
            <Box>
              <FormControl>
                <FormControl.Label>Mã bút ký</FormControl.Label>
                <TextInput
                  name="rs_ma_but_ky"
                  block
                  required
                  errors={errors}
                  validateMessage="Vui lòng điền Mã bút ký"
                  register={register}
                />
              </FormControl>
            </Box>
          )}

          <Box
            sx={{
              fontWeight: 600,
            }}
          >
            Thông tin serial
          </Box>
          <FormControl>
            <FormControl.Label>Serial</FormControl.Label>
            <TextInput
              name="serial_number"
              block
              required
              errors={errors}
              validateMessage="Vui lòng điền serial number"
              register={register}
            />
          </FormControl>
          <FormControl>
            <FormControl.Label>Người sở hữu</FormControl.Label>
            <TextInput
              name="subject"
              block
              required
              errors={errors}
              validateMessage="Vui lòng điền Người sở hữu"
              register={register}
            />
          </FormControl>
          <FormControl>
            <FormControl.Label>Tổ chức phát hành</FormControl.Label>
            <TextInput
              name="issuer"
              block
              required
              errors={errors}
              validateMessage="Vui lòng điền Tổ chức phát hành"
              register={register}
            />
          </FormControl>
          <Box sx={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: 2 }}>
            <FormControl>
              <FormControl.Label>Hiệu lực từ ngày</FormControl.Label>
              <TextInput
                name="not_before"
                block
                required
                type="date"
                errors={errors}
                validateMessage="Vui lòng điền Hiệu lực từ ngày"
                register={register}
              />
            </FormControl>
            <FormControl>
              <FormControl.Label>Hết hạn vào ngày</FormControl.Label>
              <TextInput
                name="not_after"
                block
                required
                type="date"
                errors={errors}
                validateMessage="Vui lòng điền Hiệu lực đến ngày"
                register={register}
              />
            </FormControl>
          </Box>
          <Controller
            control={control}
            name="is_active"
            render={({ field }) => {
              return (
                <FormControl>
                  <FormControl.Label>Sử dụng</FormControl.Label>
                  <Checkbox
                    checked={field.value}
                    onChange={(e) => {
                      field.onChange(e.target.checked);
                    }}
                  />
                  <FormControl.Caption>
                    Trong trường hợp hết hạn hoặc không muốn sử dụng serial này,
                    Anh/Chị có thể bỏ áp dụng
                  </FormControl.Caption>
                </FormControl>
              );
            }}
          />

          <ModalActions>
            <Button
              onClick={() => {
                props.onClose();
              }}
              text="Đóng"
            />
            <Button
              variant="primary"
              type="submit"
              text={(props?.data?.id ?? 0) === 0 ? "Thêm mới" : "Cập nhật"}
              isLoading={isSaving}
            />
          </ModalActions>
        </Box>
      </form>
    </Modal>
  );
};

export default EditModal;
