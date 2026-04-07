import { PaperAirplaneIcon } from "@primer/octicons-react";
import { Box, FormControl } from "@primer/react";
import { Helmet } from "react-helmet";
import { useForm } from "react-hook-form";
import CmbCompanySize from "../../component-data/cmb-company-size";
import Button from "../../component-ui/button";
import Heading from "../../component-ui/heading/Heading";
import Text from "../../component-ui/text";
import TextInput from "../../component-ui/text-input";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { useAppSelector } from "../../hooks/useAppSelector";
import { rootAction } from "../../state/actions/rootAction";
import { useState } from "react";
import { eReducerStatusBase } from "../../state/reducer-models/eReducerStatusBase";
import { Flash } from "@primer/react";
import { eSize } from "../../models/commons/eSize";
import { Link } from "react-router-dom";
import TextArea from "../../component-ui/text-area";
import { useRef } from "react";
import ReCAPTCHA from "react-google-recaptcha";

const RegisterPage = () => {
  const dispatch = useAppDispatch();
  const { appConfig } = useAppSelector((x) => x.common.appConfigReducer);
  const recaptchaRef = useRef<any>();

  const { status } = useAppSelector((x) => x.contact.contactReducer);
  const [companySizeId, setCompanySizeId] = useState<number>(0);

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({
    shouldUseNativeValidation: false,
  });

  const onSubmit = async (data: any) => {
    const token = await recaptchaRef.current.executeAsync();
    recaptchaRef.current.reset();
    dispatch(
      rootAction.contact.contactAction.saveStart({
        ...data,
        company_size_id: companySizeId,
        reCaptchaToken: token,
      })
    );
  };
  return (
    <Box sx={{ mt: 4 }}>
      <Helmet>
        <title>Đăng ký</title>
      </Helmet>
      <Heading text="Đăng ký" sx={{ mb: 3 }} />

      <Box
        sx={{
          mt: 3,
        }}
      >
        {(appConfig?.ReCAPTCHASiteKey ?? "") !== "" && (
          <ReCAPTCHA
            ref={recaptchaRef}
            size="invisible"
            sitekey={appConfig?.ReCAPTCHASiteKey ?? ""}
          />
        )}
        {status === eReducerStatusBase.is_saved && (
          <Box
            sx={{
              mt: 4,
              display: "flex",
              flexDirection: "column",
              alignItems: "center",
            }}
          >
            <Flash variant="success">
              <Heading text="Đăng ký thành công" size={eSize.smalll} />
              <Text text="Chúng tôi sẽ sớm liên hệ lại với Quý công ty" />
            </Flash>
            <Box sx={{ mt: 3 }}>
              <Link to={"../../login"} style={{ textDecoration: "none" }}>
                <Button
                  text="Quay lại đăng nhập"
                  variant="primary"
                  size="large"
                  onClick={() => {}}
                />
              </Link>
            </Box>
          </Box>
        )}
        {status !== eReducerStatusBase.is_saved && (
          <form onSubmit={handleSubmit(onSubmit)}>
            <Box
              display={"grid"}
              sx={{
                gap: 2,
              }}
            >
              <FormControl>
                <FormControl.Label>
                  <Text text="Tên công ty" />
                </FormControl.Label>
                <TextInput
                  block
                  register={register}
                  name="name"
                  required
                  validateMessage="Vui lòng điền Tên công ty"
                  errors={errors}
                />
              </FormControl>
              <FormControl>
                <FormControl.Label>
                  <Text text="Địa chỉ" />
                </FormControl.Label>
                <TextInput
                  register={register}
                  block
                  name="address"
                  required
                  validateMessage="Vui lòng điền Địa chỉ"
                  errors={errors}
                />
              </FormControl>

              <Box display={"grid"} gridTemplateColumns={"1fr 1fr"}>
                <FormControl sx={{ mr: 2 }}>
                  <FormControl.Label>
                    <Text text="Mã số thuế" />
                  </FormControl.Label>
                  <TextInput
                    register={register}
                    name="tax_code"
                    required
                    validateMessage="Vui lòng điền Mã số thuế"
                    errors={errors}
                  />
                </FormControl>
                <FormControl>
                  <FormControl.Label>
                    <Text text="Người liên hệ" />
                  </FormControl.Label>
                  <TextInput
                    register={register}
                    name="serial"
                    block
                    errors={errors}
                  />
                </FormControl>
              </Box>
              <Box display={"grid"} gridTemplateColumns={"1fr 1fr"}>
                <FormControl sx={{ mr: 2 }}>
                  <FormControl.Label>
                    <Text text="Email" />
                  </FormControl.Label>
                  <TextInput
                    register={register}
                    name="email"
                    required
                    validateMessage="Vui lòng điền Email"
                    errors={errors}
                  />
                </FormControl>
                <FormControl>
                  <FormControl.Label>
                    <Text text="Điện thoại" />
                  </FormControl.Label>
                  <TextInput
                    register={register}
                    name="phone"
                    required
                    block
                    validateMessage="Vui lòng điền Điện thoại"
                    errors={errors}
                  />
                </FormControl>
              </Box>
              <Box display={"grid"} gridTemplateColumns={"1fr 1fr"}>
                <FormControl>
                  <FormControl.Label>
                    <Text text="Quy mô công ty" />
                  </FormControl.Label>
                  <CmbCompanySize
                    onValueChanged={(value) => {
                      console.log({
                        value,
                      });

                      setCompanySizeId(value);
                    }}
                    value={companySizeId}
                  />
                </FormControl>
                <FormControl>&nbsp;</FormControl>
              </Box>
              <FormControl>
                <FormControl.Label>
                  <Text text="Nội dung đăng ký" />
                </FormControl.Label>
                <TextArea
                  rows={2}
                  register={register}
                  name="info"
                  block
                  errors={errors}
                />
              </FormControl>
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
                <Button
                  text="Gửi yêu cầu"
                  variant="primary"
                  size="large"
                  type="submit"
                  leadingVisual={PaperAirplaneIcon}
                  isLoading={status === eReducerStatusBase.is_saving}
                />
              </Box>
            </Box>
          </form>
        )}
      </Box>
    </Box>
  );
};

export default RegisterPage;
