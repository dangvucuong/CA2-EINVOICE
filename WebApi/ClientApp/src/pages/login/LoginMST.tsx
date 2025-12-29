import { Box, Flash, FormControl, Link } from "@primer/react";
import React, { useEffect, useRef, useState } from "react";
import SignalrConnectionStatus from "../../component-data/signalr-connection-status";
import { useCommonContext } from "../../contexts/common";
import Text from "../../component-ui/text";
import TextInput from "../../component-ui/text-input";
import Button from "../../component-ui/button";
import { eAccountReducerStatus } from "../../state/reducer-models/account/IAccountReducer";
import { useForm } from "react-hook-form";
import { useAppSelector } from "../../hooks/useAppSelector";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { rootAction } from "../../state/actions/rootAction";
import { v4 as uuidv4 } from "uuid";
import { NotifyHelper } from "../../helpers/toast";

const LoginMST = () => {
  const {
    _signalrConnected,
    createUUID,
    _signalrHubProxy,
    _signalrSelectCert,
    _signalrSignLogin,
    getMSTFromCertSubject,
  } = useCommonContext();
  // const [loginCode, setLoginCode] = useState("");
  const loginCodeRef = useRef<string>("");

  const dispatch = useAppDispatch();
  const { status } = useAppSelector((x) => x.accountReducer);
  const { appConfig } = useAppSelector((x) => x.common.appConfigReducer);
  const [formData, setFormData] = useState({
    mst: "",
    serial: "",
    signedtext: "",
  });
  const [_signedText, _setSignedText] = useState<string>("");

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm({
    shouldUseNativeValidation: false,
    defaultValues: formData,
  });

  const onSubmit = async (data: any) => {
    if (formData.mst && formData.serial && _signedText) {
      dispatch(
        rootAction.accountAction.loginStart({
          mst: formData.mst,
          serial: formData.serial,
          signed_text: formData.signedtext,
        })
      );
    }
  };
  useEffect(() => {
    reset(formData);
    if (formData.mst && formData.serial) {
      const code = uuidv4();
      // setLoginCode(code)
      loginCodeRef.current = code;
      _signalrSignLogin(code, formData.serial);
      // ký login
    }
  }, [formData]);
  useEffect(() => {
    console.log({
      formData,
      _signedText,
      code: loginCodeRef.current,
    });

    if (formData.mst && formData.serial && _signedText) {
      dispatch(
        rootAction.accountAction.loginStart({
          mst: formData.mst,
          serial: formData.serial,
          signed_text: _signedText,
        })
      );
    }
  }, [formData, _signedText]);

  useEffect(() => {
    if (_signalrConnected) {
      _signalrSelectCert();
    }
  }, [_signalrConnected]);
  useEffect(() => {
    if (_signalrConnected) {
      _signalrHubProxy.on("addMessage", function (eventName: any, data: any) {
        console.log({
          data,
        });
        if (eventName === "SERVER") {
          const ketquas = data.split("|");
          const [returnCode, code, signedtext] = ketquas;

          if (returnCode === "0") {
            NotifyHelper.Error("Chứng thư số không hợp lệ");
            return;
          }

          if (signedtext === "CertInf") {
            const [nhaCungCap, serial, tuNgay, denNgay, subject] =
              ketquas.slice(3);
            const data: any = {
              returnCode,
              code,
              signedtext,
              nhaCungCap,
              serial,
              tuNgay,
              denNgay,
              subject,
            };

            const mst = getMSTFromCertSubject(data.subject);
            setFormData({
              mst: mst,
              serial: data.serial,
              signedtext: "",
              // signedtext: signedtext
            });
          }
          if (code === loginCodeRef.current) {
            // debugger
            _setSignedText(signedtext);
          }
        }
      });
    }
  }, [_signalrConnected, _signalrHubProxy]);
  return (
    <Box>
      {!_signalrConnected && (
        <Box
          sx={{
            mt: 3,
          }}
        >
          <SignalrConnectionStatus />
          <Flash variant="warning" sx={{ mt: 3 }}>
            <b>Bạn chưa chạy tool ký số Hóa đơn</b>
            <p>
              Vui lòng nhấn vào <b>TẢI TOOL KÝ SỐ </b> bên dưới để Tải và Cài
              Tool ký Hóa đơn rồi Đăng nhập
            </p>
          </Flash>
          <Link
            href="https://hsdt.nacencomm.vn/downloads/setup.msi"
            target="_blank"
            sx={{
              display: "flex",
              justifyContent: "center",
              mt: 3,
            }}
          >
            <Button
              text="Tải tool ký số"
              size="medium"
              variant="invisible"
              sx={{}}
            />
          </Link>
        </Box>
      )}
      {_signalrConnected && (
        <Box>
          <form onSubmit={handleSubmit(onSubmit)}>
            <Box
              display={"grid"}
              sx={{
                gap: 2,
              }}
            >
              <Flash variant="success" sx={{ mt: 3 }}>
                <p>Chọn chứng thư số từ tool chữ ký số</p>
              </Flash>

              <FormControl>
                <FormControl.Label>
                  <Text text="Mã số thuế" />
                </FormControl.Label>
                <TextInput
                  block
                  register={register}
                  name="mst"
                  required
                  validateMessage="Vui lòng điền Mã đơn vị"
                  readOnly
                  errors={errors}
                />
              </FormControl>
              <FormControl>
                <FormControl.Label>
                  <Text text="Serial" />
                </FormControl.Label>
                <TextInput
                  block
                  register={register}
                  name="serial"
                  required
                  validateMessage="Vui lòng điền Serial"
                  readOnly
                  errors={errors}
                />
              </FormControl>

              <Box
                sx={{
                  mt: 3,
                  mb: 3,
                  display: "flex",
                  alignItems: "center",
                  justifyContent: "center",
                }}
              >
                <Button
                  text="Đăng nhập"
                  variant="primary"
                  size="large"
                  type="submit"
                  isLoading={status === eAccountReducerStatus.is_logging_in}
                />
              </Box>
            </Box>
          </form>
        </Box>
      )}
    </Box>
  );
};

export default LoginMST;
