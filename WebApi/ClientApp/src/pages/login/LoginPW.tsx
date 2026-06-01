import { Box, FormControl } from "@primer/react";
import { startAuthentication } from "@simplewebauthn/browser";
import axios from "axios";
import React from "react";
import ReCAPTCHA from "react-google-recaptcha";
import { useForm } from "react-hook-form";
import { Link } from "react-router-dom";
import Button from "../../component-ui/button";
import Text from "../../component-ui/text";
import TextInput from "../../component-ui/text-input";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { useAppSelector } from "../../hooks/useAppSelector";
import { rootAction } from "../../state/actions/rootAction";
import { eAccountReducerStatus } from "../../state/reducer-models/account/IAccountReducer";
const LoginPW = ({
  setShowLoginForm = () => {},
}: {
  setShowLoginForm?: (data: boolean) => void;
}) => {
  const recaptchaRef = React.useRef<any>();
  const dispatch = useAppDispatch();
  const { status } = useAppSelector((x) => x.accountReducer);
  const { appConfig } = useAppSelector((x) => x.common.appConfigReducer);
 console.log(appConfig);
console.log(appConfig?.ReCAPTCHASiteKey);
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({
    shouldUseNativeValidation: false,
    // defaultValues: roleEditing
  });

  const onSubmit = async (data: any) => {
    const token = await recaptchaRef.current.executeAsync();
    recaptchaRef.current.reset();

    dispatch(
      rootAction.accountAction.loginStart({
        ...data,
        reCaptchaToken: token,
      })
    );
  };
  const loginByPasskey = async () => {
    try {
      // loading.value = true;
      // Lấy tọa độ của người dùng
      const getCoordinates = () => {
        return new Promise((resolve, reject) => {
          if (!navigator.geolocation) {
            reject(new Error("Trình duyệt không hỗ trợ Geolocation"));
          } else {
            navigator.geolocation.getCurrentPosition(
              (position) => {
                resolve({
                  latitude: position.coords.latitude,
                  longitude: position.coords.longitude,
                });
              },
              (error) => reject(error)
            );
          }
        });
      };

      // Gọi validate form
      // await formRef.value.validate();

      // Gọi API lấy tùy chọn xác thực
      const resp: any = await axios.get(
        "https://api.nacecomm.online/webauthn/authenticate/generate-options",
        {
          withCredentials: true,
        }
      );

      const opts = resp.data;
      const sessionId = opts.sessionId;
      console.log({
        opts,
      });

      // Bắt đầu xác thực bằng FIDO
      const asseResp = await startAuthentication(opts);

      // Lấy tọa độ hiện tại
      const coordinates: any = await getCoordinates();
      console.log({
        asseResp,
        coordinates,
      });

      // Gọi API verify với thêm thông tin tọa độ
      const verificationResp = await axios.post(
        "https://api.nacecomm.online/webauthn/authenticate/verify",
        { ...asseResp, ...coordinates, sessionId }, // Gộp thông tin xác thực và tọa độ
        {
          headers: {
            "Content-Type": "application/json",
          },
          withCredentials: true,
        }
      );

      const verificationJSON: any = verificationResp.data;

      console.log({
        verificationJSON,
      });
      const token = await recaptchaRef.current.executeAsync();
      recaptchaRef.current.reset();
      // const [donvi_ma_dv, donvi_username] = (verificationJSON.username ?? "").split("_", 2);
      dispatch(
        rootAction.accountAction.loginStart({
          donvi_ma_dv: "passkey",
          username: "passkey",
          password: verificationJSON.token,
          reCaptchaToken: token,
        })
      );
    } catch (error) {
      console.error(error);
    } finally {
    }
  };

  return (
    <Box>
      <form onSubmit={handleSubmit(onSubmit)}>
        <Box
          display={"grid"}
          sx={{
            gap: 2,
          }}
        >
          {(appConfig?.ReCAPTCHASiteKey ?? "") !== "" && (
            <ReCAPTCHA
              ref={recaptchaRef}
              size="invisible"
              sitekey={appConfig?.ReCAPTCHASiteKey ?? ""}
            />
          )}
          <FormControl>
            <FormControl.Label>
              <Text text="Mã số thuế" />
            </FormControl.Label>
            <TextInput
              block
              register={register}
              name="donvi_ma_dv"
              required
              validateMessage="Vui lòng điền Mã đơn vị"
              errors={errors}
            />
          </FormControl>
          <FormControl>
            <FormControl.Label>
              <Text text="Email" />
            </FormControl.Label>
            <TextInput
              register={register}
              block
              name="username"
              required
              validateMessage="Vui lòng điền tài khoản đăng nhập"
              errors={errors}
            />
          </FormControl>
          <FormControl>
            <FormControl.Label>
              <Text text="Mật khẩu" />
            </FormControl.Label>
            <TextInput
              register={register}
              block
              type="password"
              name="password"
              required
              validateMessage="Vui lòng điền mật khẩu"
              errors={errors}
            />
          </FormControl>
          <FormControl
            sx={{
              alignItems: "flex-end",
            }}
          >
            <FormControl.Label></FormControl.Label>
            <Link to={"../../forget-pw"}>
              <Button
                text="Quên mật khẩu"
                variant="invisible"
                onClick={() => {}}
              />
            </Link>
          </FormControl>

          {/* <Box>
            <Button
              onClick={() => {
                setShowLoginForm(false);
              }}
              style={{
                textAlign: "left",
                cursor: "pointer",
              }}
            >
              Quay lại
            </Button>
          </Box> */}
          <Box
            sx={{
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
              sx={{ ml: 1, mr: 1 }}
              isLoading={status === eAccountReducerStatus.is_logging_in}
            />
          </Box>
        </Box>
      </form>
      <Box sx={{ mt: 3, mb: 3 }}>
        <Button
          text="Đăng nhập bằng Passkey"
          variant="danger"
          size="large"
          block
          sx={{ ml: 1, mr: 1 }}
          // type='submit'
          isLoading={status === eAccountReducerStatus.is_logging_in}
          onClick={loginByPasskey}
        />
      </Box>
    </Box>
  );
};

export default LoginPW;
