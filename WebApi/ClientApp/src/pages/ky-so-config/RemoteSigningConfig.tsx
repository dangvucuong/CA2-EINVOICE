import { GearIcon, VerifiedIcon, ShieldIcon, TrashIcon } from "@primer/octicons-react";
import { Box, FormControl, useConfirm } from '@primer/react';
import Button from '../../component-ui/button';
import Heading from '../../component-ui/heading';
import { eSize } from '../../models/commons/eSize';
import { useAuth } from "../../hooks/useAuth";
import { useMemo, useState } from "react";
import Modal from "../../component-ui/modal";
import ModalActions from "../../component-ui/modal/ModalActions";
import Text from "../../component-ui/text";
import TextInput from "../../component-ui/text-input/TextInput";
import { useForm } from "react-hook-form";
import { userApi } from "../../api/user/userApi";
import { NotifyHelper } from "../../helpers/toast";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { rootAction } from "../../state/actions/rootAction";
const RemoteSigningConfig = () => {
    const { user } = useAuth();
    const dispatch = useAppDispatch();
    const [isShowSetingModal, setIsShowSetingModal] = useState(false);
    const confirm = useConfirm();
    const { register, handleSubmit, formState: { errors } } = useForm({
        shouldUseNativeValidation: false,
        defaultValues: {

        }
    })
    const isSetup = useMemo(() => {
        // return true;
        if ((user?.serial_remote_signing_numner ?? "") !== "") {
            return true;
        }
        return false;
    }, [user])
    const handleRemoveRemoteSigning = async () => {
        if (
            await confirm({
                title: `Ngừng sử dụng CA2 Remote Signing`,
                content: `Bạn có chắc chắn muốn ngừng sử dụng CA2 Remote Signing?`,
                confirmButtonType: 'danger',
                cancelButtonContent: `Đóng`,
                confirmButtonContent: `Xác nhận`,
            })
        ) {
            const res = await userApi.updateRemoteSigningSerial({
                rs_ma_but_ky: ""
            })
            if (res.is_success) {
                NotifyHelper.Success("Success")
                setIsShowSetingModal(false)
                if (user) {
                    dispatch(rootAction.accountAction.loadProfileSuccess({
                        ...user,
                        serial_remote_signing_numner: undefined,
                        is_serial_remote_signing_verified: undefined
                    }))
                }

            } else {
                NotifyHelper.Error(res.message ?? "Error")

            }
        }
    }
    const onSubmit = async (data: any) => {

        const res = await userApi.updateRemoteSigningSerial({
            rs_ma_but_ky: data.rs_ma_but_ky
        })
        if (res.is_success) {
            NotifyHelper.Success("Success")
            setIsShowSetingModal(false)
            // dispatch(rootAction.accountAction.loadProfileStart())
            if (user) {
                dispatch(rootAction.accountAction.loadProfileSuccess({
                    ...user,
                    serial_remote_signing_numner: data.serial_remote_signing_numner,
                    is_serial_remote_signing_verified: false
                }))
            }
        } else {
            NotifyHelper.Error(res.message ?? "Error")

        }

        // dispatch(rootAction.category.hangHoaAction.saveStart({
        //     ...data,
        // }))

    }
    return (
        <Box sx={{
            display: "flex",
            flexDirection: "column",
            borderRadius: 2,
            border: "1px",
            borderStyle: "solid",
            borderColor: "border.default",
            p: 3,
            // pb: 4,
            // pt: 4,
            width: "500px",
            // height:"200px",
            justifyContent: "center"
        }}>
            <Box sx={{
                display: "flex",
                mt: 2,
                height: "90px"
            }}>
                <Box id="icon">
                    <img alt='USB' src='../../images/remote_signing.svg' />
                </Box>
                <Box id="content" sx={{
                    ml: 2
                }}>
                    <Heading text='CA2 Remote Signing' size={eSize.medium} />
                    <Box sx={{
                        color: "fg.muted"
                    }}>
                        <Box>Dịch vụ chữ ký số từ xa CA2 Remote Signing</Box>
                        <Box>Ký số mọi lúc, mọi nơi ngay trên thiết bị di động mà không cần USB Token</Box>
                    </Box>
                </Box>
            </Box>
            <Box sx={{
                mt: 4,
                display: "flex"

            }}>
                <Box id="left" sx={{
                    flex: 1
                }}>
                    {isSetup &&
                        <>
                            {user?.is_serial_remote_signing_verified !== true &&
                                <Button text="Chờ xác thực" leadingVisual={ShieldIcon} size="medium" variant="invisible"
                                />
                            }
                            {user?.is_serial_remote_signing_verified === true &&
                                <Button text="Đã xác thực" leadingVisual={VerifiedIcon} size="medium" variant="invisible"
                                />
                            }
                        </>
                    }

                </Box>
                <Box id="right" sx={{
                    display: "flex"
                }}>
                    <Button text='Tìm hiểu thêm' variant='invisible'
                        size='medium'
                    />
                    {!isSetup &&
                        <Button text='Sử dụng ngay' variant='primary' leadingVisual={GearIcon}
                            size='medium'
                            onClick={() => {
                                setIsShowSetingModal(true)
                            }}
                            sx={{
                                ml: 2
                            }} />
                    }
                    {isSetup &&
                        <Button text='Ngưng sử dụng'
                            variant='danger'
                            leadingVisual={TrashIcon}
                            size='medium'
                            onClick={() => {
                                handleRemoveRemoteSigning()
                            }}
                            sx={{
                                ml: 2
                            }} />
                    }
                </Box>
            </Box>
            {isShowSetingModal &&
                <Modal onClose={() => {
                    setIsShowSetingModal(false)
                }}
                    isOpen={true}
                    width={"large"}
                    title="Thiết lập"
                >
                    <form onSubmit={handleSubmit(onSubmit)}>
                        <Box>
                            <Box sx={{
                                display: "flex",
                                mt: 2,
                                alignItems: "center",
                                justifyContent: "center"
                            }}>
                                <Box id="icon">
                                    <img alt='USB' src='../../images/remote_signing.svg' />
                                </Box>
                                <Box id="content" sx={{
                                    ml: 2
                                }}>
                                    <Heading text='CA2' size={eSize.large} />
                                    <Box sx={{ mt: -1 }}>
                                        <Text text='Remote Signing' sx={{
                                            fontSize: "15px",
                                            color: "fg.muted",
                                        }} />
                                    </Box>

                                </Box>

                            </Box>
                            <Box sx={{
                                display: "flex",
                                mt: 2,
                                alignItems: "center",
                                justifyContent: "center"
                            }}>
                                <Heading text='Thiết lập kết nối đến CA2 Remote Signing' size={eSize.medium} />

                            </Box>
                            <Box sx={{
                                mt: 5,
                                mb: 3,
                                width: "100%"
                            }}>
                                <FormControl>
                                    <FormControl.Label>
                                        <Text text='Mã bút ký' />
                                    </FormControl.Label>
                                    <TextInput
                                        register={register}
                                        name='rs_ma_but_ky'
                                        block
                                        required
                                        placeholder=""
                                        validateMessage='Vui lòng mã bút ký'
                                        errors={errors}

                                    />
                                </FormControl>
                            </Box>
                        </Box>
                        <ModalActions>
                            <Button text="Đóng" size="medium" type="button" onClick={() => {
                                setIsShowSetingModal(false)
                            }} />
                            <Button text="Xác nhận" variant="primary" size="medium" type="submit" />
                        </ModalActions>
                    </form>
                </Modal>
            }
        </Box>
    );
};

export default RemoteSigningConfig;