import { Box, Flash, FormControl } from '@primer/react';
import { useForm } from 'react-hook-form';
import Button from '../../component-ui/button';
import Heading from '../../component-ui/heading';
import Text from '../../component-ui/text';
import TextInput from '../../component-ui/text-input';
import { useAppDispatch } from '../../hooks/useAppDispatch';
import { useAppSelector } from '../../hooks/useAppSelector';
import { accountApi } from '../../api/account/accountApi';
import { NotifyHelper } from '../../helpers/toast';
import { useState } from 'react';

const ChangePWPage = () => {
    const dispatch = useAppDispatch();
    const { status } = useAppSelector(x => x.accountReducer)
    const [isChangePWSuccess, setisChangePWSuccess] = useState(false);
    const [isSaving, setIsSaving] = useState(false);
    

    const { register, handleSubmit, setError, formState: { errors } } = useForm({
        shouldUseNativeValidation: false,
        // defaultValues: roleEditing
    })


    const onSubmit = async (data: any) => {
        let isValid = true;
        if (data.new_password != data.new_password_confirm) {
            setError("new_password_confirm", {
                type: "manual",
                message: "Nhập lại mật khẩu không khớp",
            })
            isValid = false;
            return;
        }
        if (isValid) {
            setIsSaving(true)
            const res = await accountApi.changePassword({
                new_password: data.new_password,
                old_password: data.current_password
            })
            setIsSaving(false)
            if (res.is_success) {
                NotifyHelper.Success("Cập nhật thành công")
                setisChangePWSuccess(true)
            } else {
                NotifyHelper.Error("Cập nhật thất bại")
            }
        }
        // dispatch(rootAction.accountAction.loginStart({
        //     ...data
        // }))

    }
    return (
        <Box sx={{
            display: "flex",
            justifyContent: "center",
        }}>
            <Box sx={{
                mt: 5
            }}>

                <Heading text='Đổi mật khẩu' />
                <Box sx={{
                    minWidth: "350px",
                    mt: 3
                }}>
                    {isChangePWSuccess &&
                        <Flash variant='success'>
                            Đổi mật khẩu thành công
                        </Flash>
                    }
                    {!isChangePWSuccess &&
                        <form onSubmit={handleSubmit(onSubmit)}>
                            <Box
                                display={"grid"}
                                sx={{
                                    gap: 2
                                }}
                            >
                                <FormControl>
                                    <FormControl.Label>
                                        <Text text='Mật khẩu hiện tại' />
                                    </FormControl.Label>
                                    <TextInput
                                        block
                                        register={register}
                                        name='current_password'
                                        required
                                        type='password'
                                        validateMessage='Vui lòng điền mật khẩu hiện tại'
                                        errors={errors}

                                    />
                                </FormControl>
                                <FormControl >
                                    <FormControl.Label>
                                        <Text text='Mật khẩu mới' />
                                    </FormControl.Label>
                                    <TextInput
                                        register={register}
                                        block
                                        name='new_password'
                                        type='password'
                                        required
                                        minLength={6}
                                        validateMessage='Vui lòng điền mật khẩu mới (Tối thiểu 6 ký tự)'
                                        errors={errors}

                                    />
                                </FormControl>
                                <FormControl >
                                    <FormControl.Label>
                                        <Text text='Nhập lại mật khẩu mới' />
                                    </FormControl.Label>
                                    <TextInput
                                        register={register}
                                        block
                                        type='password'
                                        name='new_password_confirm'
                                        required
                                        validateMessage='Vui lòng điền lại mật khẩu mới (Tối thiểu 6 ký tự)'
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
                                    <Button text='Đổi mật khẩu'
                                        variant='primary'
                                        size='large'
                                        type='submit'
                                        isLoading={isSaving}
                                    />
                                </Box>

                            </Box>
                        </form>
                    }
                </Box>
            </Box>
        </Box>
    );
};

export default ChangePWPage;