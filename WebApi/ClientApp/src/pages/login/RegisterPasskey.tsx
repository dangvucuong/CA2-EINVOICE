import { Box, Flash, FormControl } from '@primer/react';
import { startRegistration } from "@simplewebauthn/browser";
import axios from 'axios';
import React, { useState } from 'react';
import ReCAPTCHA from "react-google-recaptcha";
import { useForm } from 'react-hook-form';
import { Link, useLocation } from 'react-router-dom';
import { accountApi } from '../../api/account/accountApi';
import Button from '../../component-ui/button';
import Text from '../../component-ui/text';
import TextInput from '../../component-ui/text-input';
import { useAppDispatch } from '../../hooks/useAppDispatch';
import { useAppSelector } from '../../hooks/useAppSelector';
const RegisterPasskey = () => {
    const [isSuccess, setIsSuccess] = useState(false);
    const [message, setMessage] = useState<string>("");

    const recaptchaRef = React.useRef<any>();
    const location = useLocation();
    const queryParams = new URLSearchParams(location.search);
    const username = queryParams.get("email");
    const dispatch = useAppDispatch();
    const { status } = useAppSelector(x => x.accountReducer)
    const { appConfig } = useAppSelector(x => x.common.appConfigReducer)
    const [isLoading, setIsLoading] = useState(false);
    const [donvi_ma_dv, donvi_username] = (username ?? "").split("_", 2);
    const { register, handleSubmit, formState: { errors } } = useForm({
        shouldUseNativeValidation: false,
        defaultValues: {
            donvi_ma_dv: donvi_ma_dv ?? "",
            username: donvi_username ?? ""
        }
    })


    const onSubmit = async (data: any) => {
        setIsLoading(true);
        const token = await recaptchaRef.current.executeAsync();
        recaptchaRef.current.reset();

        // dispatch(rootAction.accountAction.loginStart({
        //     ...data,
        //     reCaptchaToken: token
        // }))

        const { donvi_ma_dv, username, password } = data;
        const resLogin = await accountApi.logIn(
            {
                donvi_ma_dv: donvi_ma_dv,
                password: password,
                username: username,
                reCaptchaToken: token
            }
        );
        if (resLogin.is_success) {
            handleRegisterPasskeyAsync(`${donvi_ma_dv}_${username}`, `12345`);
        }
    }


    const handleRegisterPasskeyAsync = async (username: string, mabutky: string) => {
        try {
            const res = await axios.post(
                "https://api.nacecomm.online/webauthn/register/generate-options",
                {
                    username: `${username}`,
                    mabutky: `${mabutky}`
                }, { withCredentials: true })
            const opts = res.data;
            console.log({
                opts
            });

            const { sessionId } = opts;
            try {
                const asseResp = await startRegistration(opts);
                console.log({
                    asseResp
                });
                try {
                    const resVerify = await axios.post(
                        "https://api.nacecomm.online/webauthn/register/verify",
                        {
                            ...asseResp,
                            mabutky: `${mabutky}`,
                            deviceName: "WEB",
                            sessionId,
                            username: `${username}`,
                        },
                        {
                            headers: { "Content-Type": "application/json" },
                            withCredentials: true,
                        }
                    );
                    console.log({
                        asseResp,
                        resVerify: resVerify.data
                    });
                    setMessage("Đăng ký thành công")
                    setIsSuccess(true)

                } catch (error: any) {
                    debugger
                    if (error?.response?.status === 409) {
                        setMessage("Đã đăng ký trước đó")
                        setIsSuccess(true)
                    }
                    console.log({
                        error
                    });
                }
            } catch (error: any) {
                debugger
                if (error?.response?.status === 409) {
                    setMessage("Đã đăng ký trước đó")
                    setIsSuccess(true)
                }
                console.log({
                    error
                });
            }
        } catch (error: any) {
            debugger
            if (error?.response?.status === 409) {
                setMessage("Đã đăng ký trước đó")
                setIsSuccess(true)
            }
            console.log({
                error
            });
        }
    }
    return (
        <Box>
            {!isSuccess &&
                <form onSubmit={handleSubmit(onSubmit)}>
                    <Box
                        display={"grid"}
                        sx={{
                            gap: 2
                        }}
                    >
                        <Text text='Xác thực' sx={{
                            fontSize: 24,
                            mt: 3,
                            mb: 0,
                            fontWeight: 600
                        }} />
                        {(appConfig?.ReCAPTCHASiteKey ?? "") !== "" &&
                            <ReCAPTCHA
                                ref={recaptchaRef}
                                size="invisible"
                                sitekey={appConfig?.ReCAPTCHASiteKey ?? ""}
                            />
                        }
                        <FormControl>
                            <FormControl.Label>
                                <Text text='Mã đơn vị' />
                            </FormControl.Label>
                            <TextInput
                                block
                                register={register}
                                name='donvi_ma_dv'
                                required
                                validateMessage='Vui lòng điền Mã đơn vị'
                                errors={errors}
                                readOnly
                                disabled

                            />
                        </FormControl>
                        <FormControl >
                            <FormControl.Label>
                                <Text text='Tên đăng nhập' />
                            </FormControl.Label>
                            <TextInput
                                register={register}
                                block
                                name='username'
                                required
                                validateMessage='Vui lòng điền tài khoản đăng nhập'
                                errors={errors}
                                // readOnly
                                // disabled

                            />
                        </FormControl>
                        <FormControl >
                            <FormControl.Label>
                                <Text text='Mật khẩu' />
                            </FormControl.Label>
                            <TextInput
                                register={register}
                                block
                                type='password'
                                name='password'
                                required
                                validateMessage='Vui lòng điền mật khẩu'
                                errors={errors}

                            />
                        </FormControl>

                        <Box sx={{
                            mt: 3,
                            mb: 3,
                            display: "flex",
                            alignItems: "center",
                            justifyContent: "center"
                        }}>
                            <Button text='Tiếp tục'
                                variant='primary'
                                size='large'
                                type='submit'
                                sx={{ ml: 1, mr: 1 }}
                                isLoading={isLoading}
                            />

                        </Box>

                    </Box>
                </form>
            }
            {isSuccess &&
                <Box sx={{ mt: 3 }}>
                    <Flash>
                        <Box sx={{ display: "flex", flexDirection: "column" }}>
                            <Box sx={{
                                fontSize: 16,
                                fontWeight: 600,
                                mb: 2
                            }}>{message}</Box>
                            <Box>
                                Quay lại trang đăng nhập và sử dụng Passkey để xác thực
                            </Box>
                            <Box sx={{ mt: 3, textAlign: "center", alignItems: "center", justifyContent: "center", display: "flex" }}>
                                <Link to={`../../login`}>
                                    <Button text='Tiếp tục' variant='primary' size='medium' />
                                </Link>
                            </Box>
                        </Box>
                    </Flash>
                </Box>
            }
        </Box>
    );
};

export default RegisterPasskey;