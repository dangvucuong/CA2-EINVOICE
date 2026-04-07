import { Box, FormControl } from "@primer/react";
import { useForm } from "react-hook-form";
import Button from "../../component-ui/button";
import Heading from "../../component-ui/heading/Heading";
import Text from "../../component-ui/text";
import TextInput from "../../component-ui/text-input";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { useAppSelector } from "../../hooks/useAppSelector";
import { rootAction } from "../../state/actions/rootAction";
import { eForgetPWReducerStatus } from "../../state/reducer-models/account/IForgetPWReducer";
import { Link } from "react-router-dom";
import { Helmet } from "react-helmet";
import ReCAPTCHA from "react-google-recaptcha";
import { useRef } from "react";

const ForgetPWPage = () => {
  const recaptchaRef = useRef<any>();
  const { appConfig } = useAppSelector((x) => x.common.appConfigReducer);

  const dispatch = useAppDispatch();
  const { status, message, otpRespone } = useAppSelector(
    (x) => x.forgetPWReducer
  );
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
    if (otpRespone) {
      dispatch(
        rootAction.forgetPWAction.resetPWStart({
          ...data,
          reCaptchaToken: token,
        })
      );
    } else {
      dispatch(
        rootAction.forgetPWAction.sendOTPStart({
          ...data,
          reCaptchaToken: token,
        })
      );
    }
  };
  return (
    <Box sx={{ mt: 4 }}>
      <Helmet>
        <title>Quên mật khẩu</title>
      </Helmet>
      <Heading text="Quên mật khẩu" sx={{ mb: 3 }} />

      <Box
        sx={{
          mt: 3,
        }}
      >
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
                <Text text="Mã đơn vị" />
              </FormControl.Label>
              <TextInput
                block
                register={register}
                name="donvi_ma_dv"
                required
                disabled={otpRespone != undefined}
                validateMessage="Vui lòng điền mã đơn vị"
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
                name="email"
                required
                disabled={otpRespone != undefined}
                validateMessage="Vui lòng điền email"
                errors={errors}
              />
            </FormControl>
            {otpRespone && (
              <FormControl>
                <FormControl.Label>
                  <Text text="Mã xác thực" />
                </FormControl.Label>
                <TextInput
                  register={register}
                  name="otp"
                  required
                  block
                  validateMessage="Vui lòng điền Mã xác thực"
                  errors={errors}
                />
              </FormControl>
            )}

            <Box
              sx={{
                mt: 3,
                mb: 3,
                display: "flex",
                flexDirection: "column",
                alignItems: "center",
                justifyContent: "center",
              }}
            >
              {message && (
                <FormControl.Validation variant="error" sx={{ mb: 3, mt: -2 }}>
                  {message}
                </FormControl.Validation>
              )}
              {otpRespone &&
                status !== eForgetPWReducerStatus.is_reset_pw_success && (
                  <FormControl.Validation
                    variant="success"
                    sx={{ mb: 3, mt: -2 }}
                  >
                    Vui lòng điền mã xác thực được gửi về Email của bạn
                  </FormControl.Validation>
                )}
              {status === eForgetPWReducerStatus.is_reset_pw_success && (
                <FormControl.Validation
                  variant="success"
                  sx={{ mb: 3, mt: -2 }}
                >
                  Reset mật khẩu thành công, vui lòng kiểm tra Email để lấy mật
                  khẩu mới.
                </FormControl.Validation>
              )}
              {status !== eForgetPWReducerStatus.is_reset_pw_success && (
                <Button
                  text="Tiếp tục"
                  variant="primary"
                  size="large"
                  type="submit"
                  isLoading={status === eForgetPWReducerStatus.is_sending_otp}
                />
              )}
              {status === eForgetPWReducerStatus.is_reset_pw_success && (
                <Link
                  to={{
                    pathname: "../../login",
                    state: { showLoginForm: true },
                  }}
                  style={{
                    textDecoration: "none",
                  }}
                >
                  <Button
                    text="Quay lại đăng nhập"
                    variant="primary"
                    size="large"
                    type="button"
                  />
                </Link>
              )}
              {status !== eForgetPWReducerStatus.is_reset_pw_success && (
                <FormControl
                  sx={{
                    alignItems: "flex-end",
                    mt: 2,
                  }}
                >
                  <Link
                    to={{
                      pathname: "../../login",
                      state: { showLoginForm: true },
                    }}
                  >
                    <Button
                      text="Quay lại đăng nhập"
                      variant="invisible"
                      onClick={() => {}}
                    />
                  </Link>
                </FormControl>
              )}
            </Box>
          </Box>
        </form>
      </Box>
    </Box>
  );
};

export default ForgetPWPage;
