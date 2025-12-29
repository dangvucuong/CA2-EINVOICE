import { Box, FormControl } from '@primer/react';
import { useForm } from 'react-hook-form';
import Modal from '../../component-ui/modal';
import Text from '../../component-ui/text';
import { useAppDispatch } from '../../hooks/useAppDispatch';
import { useAppSelector } from '../../hooks/useAppSelector';
import { IKhachHang } from '../../models/responses/category/IKhachHang';
import { rootAction } from '../../state/actions/rootAction';
import TextInput from '../../component-ui/text-input';
import ModalActions from '../../component-ui/modal/ModalActions';
import Button from '../../component-ui/button';
import { eReducerStatusBase } from '../../state/reducer-models/eReducerStatusBase';
import { useAuth } from '../../hooks/useAuth';

const HangHoaEditFormModal = () => {
    const dispatch = useAppDispatch();
    const { user } = useAuth();
    const { hangHoaEditing, status } = useAppSelector(x => x.category.hangHoaReducer)
    const { register, handleSubmit, formState: { errors } } = useForm<IKhachHang>({
        shouldUseNativeValidation: false,
        defaultValues: {
            ...hangHoaEditing,
            donvi_ma_dv: hangHoaEditing?.donvi_ma_dv ?? user?.donvi_ma_dv
        }
    })


    const onSubmit = async (data: any) => {
        
        dispatch(rootAction.category.hangHoaAction.saveStart({
            ...data,
        }))

    }
    return (
        <Modal title={(hangHoaEditing?.id ?? 0) === 0 ? "Thêm mới" : "Cập nhật"}
            onClose={() => {
                dispatch(rootAction.category.hangHoaAction.closeEditModal())
            }}
            isOpen={true}
            width='large'
            height={"auto"}
            key={hangHoaEditing?.id ?? 0}

        >
            <form onSubmit={handleSubmit(onSubmit)}>
                <Box
                    display={"grid"}
                    sx={{
                        gap: 2
                    }}
                >
                    <FormControl>
                        <FormControl.Label>
                            <Text text='Mã đơn vị bán hàng' />
                        </FormControl.Label>
                        <TextInput
                            register={register}
                            name='donvi_ma_dv'
                            disabled
                            errors={errors}

                        />
                    </FormControl>
                    <FormControl >
                        <FormControl.Label>
                            <Text text='Mã hàng hóa' />
                        </FormControl.Label>
                        <TextInput
                            register={register}
                            name='ma_hang_hoa'
                            required
                            width={150}
                            validateMessage='Vui lòng điền mã hàng hóa'
                            errors={errors}

                        />
                    </FormControl>
                    <FormControl >
                        <FormControl.Label>
                            <Text text='Tên hàng hóa' />
                        </FormControl.Label>
                        <TextInput
                            register={register}
                            name='ten_hang_hoa'
                            required
                            block
                            validateMessage='Vui lòng điền tên người mua hàng'
                            errors={errors}

                        />
                    </FormControl>
                    <FormControl >
                        <FormControl.Label>
                            <Text text='Đơn vị tính' />
                        </FormControl.Label>
                        <TextInput
                            register={register}
                            name='dvt'
                            required
                            width={100}
                            validateMessage='Vui lòng điền Đơn vị tính'
                            errors={errors}
                        />
                    </FormControl>
                   
                    <FormControl>
                        <FormControl.Label>
                            <Text text='Mã loại hàng hóa' />
                        </FormControl.Label>
                        <TextInput
                            register={register}
                            name='ma_loai_hoang_hoa'
                            width={150}
                            validateMessage='Vui lòng điền Mã loại hàng hóa'
                            errors={errors}
                        />
                    </FormControl>



                    <ModalActions>
                        <Button onClick={() => {
                            dispatch(rootAction.category.hangHoaAction.closeEditModal())
                        }} text='Đóng' />
                        <Button variant='primary'
                            type='submit'
                            text={(hangHoaEditing?.id ?? 0) === 0 ? "Thêm mới" : "Cập nhật"}
                            isLoading={status === eReducerStatusBase.is_saving}
                        />
                    </ModalActions>
                </Box>
            </form>
        </Modal>
    );
};

export default HangHoaEditFormModal;