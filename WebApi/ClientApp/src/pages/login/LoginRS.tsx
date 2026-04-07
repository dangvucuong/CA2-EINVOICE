import { HubConnectionState } from "@microsoft/signalr";
import { CloudIcon, CloudOfflineIcon } from "@primer/octicons-react";
import { Box, Flash, FormControl, Octicon } from "@primer/react";
import React, { useEffect, useState } from "react";
import ReCAPTCHA from "react-google-recaptcha";
import { useForm } from "react-hook-form";
import { accountApi } from "../../api/account/accountApi";
import Button from "../../component-ui/button";
import CoutdownTimer from "../../component-ui/coutdown-timer";
import Text from "../../component-ui/text";
import TextInput from "../../component-ui/text-input";
import { useHubPublicContext } from "../../contexts/HubPublicProvider";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { useAppSelector } from "../../hooks/useAppSelector";
import { rootAction } from "../../state/actions/rootAction";
import { NotifyHelper } from "../../helpers/toast";

const LoginRS = () => {
  const recaptchaRef = React.useRef<any>();
  const dispatch = useAppDispatch();
  const { status } = useAppSelector((x) => x.accountReducer);
  const { appConfig } = useAppSelector((x) => x.common.appConfigReducer);
  const { sessionId, _connectionServer } = useHubPublicContext();
  const [isLoading, setIsLoading] = useState(false);
  const [loginCode, setLoginCode] = useState<string>("");

  const [conectionState, setConectionState] = useState();
  const isConnected = conectionState === HubConnectionState.Connected;

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({
    shouldUseNativeValidation: false,
    // defaultValues: roleEditing
  });

  useEffect(() => {
    const intervalId = setInterval(() => {
      // console.log({
      //   isConnected,
      //   sessionId,
      //   state: _connectionServer?.state ?? undefined
      // });
      setConectionState(_connectionServer?.state ?? undefined);
    }, 1000);
    return () => {
      clearInterval(intervalId);
    };
  }, []);
  const handleCheckLoginRS = async () => {
    const res = await accountApi.checkLoginInRS(loginCode);

    if (res.is_success) {
      dispatch(rootAction.accountAction.loginSuccess(res.data));
    } else {
      NotifyHelper.Error(res.message ?? "Dữ liệu không hợp lệ");
    }
  };
  const onSubmit = async (data: any) => {
    const token = await recaptchaRef.current.executeAsync();
    recaptchaRef.current.reset();
    setIsLoading(true);
    const res = await accountApi.logInRS({
      ...data,
      reCaptchaToken: token,
      session_id: sessionId,
    });

    if (res.is_success) {
      setLoginCode(res.data);
      setIsLoading(false);
    } else {
      NotifyHelper.Error(res.message ?? "Dữ liệu không hợp lệ");
      setIsLoading(false);
    }
    // dispatch(
    //   rootAction.accountAction.loginStart({
    //     ...data,
    //     reCaptchaToken: token,
    //     session_id: sessionId
    //   })
    // );
  };

  useEffect(() => {
    if (conectionState === HubConnectionState.Connected) {
      if (
        _connectionServer &&
        _connectionServer.state === HubConnectionState.Connected
      ) {
        // console.log({
        //   state: _connectionServer.state
        // });

        _connectionServer.on("REMOTE_SIGNING_SUCCESS", OnLoginSuccess);
      }
    }
    return () => {
      if (_connectionServer)
        _connectionServer.off("REMOTE_SIGNING_SUCCESS", OnLoginSuccess);
    };
  }, [_connectionServer, conectionState]);
  const OnLoginSuccess = (data: any) => {
    console.log({
      data,
    });

    dispatch(rootAction.accountAction.loginSuccess(data.data));
  };

  return (
    <Box sx={{ p: 4 }}>
      <form onSubmit={handleSubmit(onSubmit)}>
        <Box
          display={"grid"}
          sx={{
            gap: 2,
            px: [2, 0],
          }}
        >
          {(appConfig?.ReCAPTCHASiteKey ?? "") !== "" && (
            <ReCAPTCHA
              ref={recaptchaRef}
              size="invisible"
              sitekey={appConfig?.ReCAPTCHASiteKey ?? ""}
            />
          )}
          {(!loginCode || loginCode === "") && (
            <>
              <FormControl>
                <FormControl.Label>
                  <Text text="Mã bút ký" />
                </FormControl.Label>
                <TextInput
                  register={register}
                  block
                  name="rs_ma_but_ky"
                  required
                  validateMessage="Vui lòng điền Mã bút ký"
                  errors={errors}
                  disabled={!isConnected}
                />
                <FormControl.Caption>
                  <Box
                    sx={{
                      whiteSpace: "break-spaces",
                      textAlign: "left",
                      width: ["100%", "350px"],
                    }}
                  >
                    <Text text="Mã bút ký cần được thiết lập trên Ca2 E-invoice trước." />
                    <br />
                    <Text text="Đối với lần đầu đăng nhập anh/chị vui lòng sử dụng phương thức khác để đăng nhập và thiết lập Mã bút ký." />
                  </Box>
                </FormControl.Caption>
              </FormControl>
            </>
          )}

          {(!loginCode || loginCode === "") && (
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
                isLoading={isLoading}
              />
            </Box>
          )}
          {!isLoading && loginCode !== "" && (
            <Box
              sx={{
                display: "grid",
                gap: 2,
              }}
            >
              <Flash
                variant="default"
                sx={{
                  textAlign: "center",
                  display: "flex",
                  alignItems: "center",
                  justifyContent: "center",
                }}
              >
                <Box sx={{ mr: 2 }}>
                  <CoutdownTimer
                    seconds={300}
                    onTimeout={() => {
                      window.location.reload();
                    }}
                  />
                </Box>
                <Box
                  sx={{
                    display: "grid",
                    textAlign: "left",
                  }}
                >
                  <b> Vui lòng ký số trên ứng dụng CA2 RS để tiếp tục</b>
                  <Box sx={{ color: "fg.muted" }}>
                    Sau khi ký số trên CA2 RS, hệ thống sẽ tự động đăng nhập
                  </Box>
                </Box>
              </Flash>
              <Flash variant="warning">
                <FormControl>
                  <Button
                    text="Kiểm tra đăng nhập"
                    onClick={handleCheckLoginRS}
                  />
                  <Box sx={{ color: "fg.muted", textAlign: "left" }}>
                    Trường hợp Anh/Chị đã ký xác thực trên Ứng dụng CA2 RS
                    <br />
                    Anh/Chị có thể nhấn vào đây để đăng nhập thủ công
                  </Box>
                </FormControl>
              </Flash>

              {/* <Box sx={{ display: "flex", gap: 2, textAlign: "left" }}>
                <Box>
                  <Octicon icon={isConnected ? CloudIcon : CloudOfflineIcon} />
                </Box>
                <Box sx={{ color: "fg.muted", flex: 1 }}>{loginCode}</Box>
              </Box> */}
              {/* <Box sx={{ color: "fg.muted", flex: 1, textAlign: "left" }}>
                {sessionId}
              </Box> */}
            </Box>
          )}
        </Box>
      </form>
    </Box>
  );
};

export default LoginRS;
