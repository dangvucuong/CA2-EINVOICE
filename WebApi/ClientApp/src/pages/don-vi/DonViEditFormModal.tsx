import { Box, FormControl } from '@primer/react';
import { useForm } from 'react-hook-form';
import SelectBoxCoQuanThue from '../../component-data/selectbox-co-quan-thue';
import Button from '../../component-ui/button';
import Modal from '../../component-ui/modal';
import ModalActions from '../../component-ui/modal/ModalActions';
import Text from '../../component-ui/text';
import TextInput from '../../component-ui/text-input';
import { useAppDispatch } from '../../hooks/useAppDispatch';
import { useAppSelector } from '../../hooks/useAppSelector';
import { useAuth } from '../../hooks/useAuth';
import { IDonVi } from '../../models/responses/category/IDonVi';
import { rootAction } from '../../state/actions/rootAction';
import { eReducerStatusBase } from '../../state/reducer-models/eReducerStatusBase';
import { useState } from 'react';
import { NotifyHelper } from '../../helpers/toast';

const DonViEditFormModal = () => {
    const dispatch = useAppDispatch();
    const { user } = useAuth();


    const { donViEditing, status } = useAppSelector(x => x.category.donViReducer)
    const { register, handleSubmit, clearErrors, setError, formState: { errors } } = useForm<IDonVi>({
        shouldUseNativeValidation: false,
        defaultValues: {
            ...donViEditing
        }
    })

    const [coQuanThueId, setCoQuanThueId] = useState(donViEditing?.co_quan_thu_id_chuquan ?? 0);
    const [tenDonViChuQuan, setTenDonViChuQuan] = useState(donViEditing?.donvi_chuquan ?? "");

    const onSubmit = async (data: any) => {
        let isValid = true;
        if (!tenDonViChuQuan) {
            // setError("donvi_chuquan", {});
            NotifyHelper.Error("Vui lòng chọn đơn vị chủ quản")
            isValid = false;
        }
        if (isValid) {
            dispatch(rootAction.category.donViActionType.saveStart({
                ...data,
                donvi_chuquan: tenDonViChuQuan,
                co_quan_thu_id_chuquan: coQuanThueId,
                ngay_hoa_don_max: data.ngay_hoa_don_max && data.ngay_hoa_don_max !== "" ? data.ngay_hoa_don_max : undefined
                // donvi_chuquan:
            }))
        }

    }
    return (
        <Modal title={(donViEditing?.id ?? 0) === 0 ? "Thêm mới" : "Cập nhật"}
            onClose={() => {
                dispatch(rootAction.category.donViActionType.closeEditModal())
            }}
            isOpen={true}
            width='80%'
            height={"auto"}
            key={donViEditing?.id ?? 0}

        >
            <form onSubmit={handleSubmit(onSubmit)}>
                <Box
                >
                    <Box className='row'>
                        <Box className='col-md-6'
                            sx={{
                                borderRight: "1px",
                                borderRightStyle: "dashed",
                                borderRightColor: "border.default",
                                pr: 4
                            }}
                        >
                            <FormControl>
                                <FormControl.Caption>Thông tin cơ bản</FormControl.Caption>
                            </FormControl>
                            <FormControl sx={{ mt: 2 }}>
                                <FormControl.Label>
                                    <Text text='Mã số thuế' />
                                </FormControl.Label>
                                <TextInput
                                    register={register}
                                    name='ma_dv'
                                    disabled={(donViEditing?.id ?? 0) > 0}
                                    required
                                    readOnly={(donViEditing?.id ?? 0) > 0}
                                    errors={errors}
                                    validateMessage='Vui lòng điền Mã số thuế'

                                />
                            </FormControl>
                            <FormControl sx={{ mt: 2 }}>
                                <FormControl.Label>
                                    <Text text='Tên đơn vị' />
                                </FormControl.Label>
                                <TextInput
                                    register={register}
                                    name='ten_dv'
                                    required
                                    block
                                    // width={150}
                                    validateMessage='Vui lòng điền Tên đơn vị'
                                    errors={errors}

                                />
                            </FormControl>
                            <FormControl sx={{ mt: 2 }}>
                                <FormControl.Label>
                                    <Text text='Địa chỉ' />
                                </FormControl.Label>
                                <TextInput
                                    register={register}
                                    name='dia_chi'
                                    required
                                    block
                                    errors={errors}
                                    validateMessage='Vui lòng điền Địa chỉ'

                                />
                            </FormControl>
                            <FormControl sx={{ mt: 2 }}>
                                <FormControl.Label>
                                    <Text text='Serial' />
                                </FormControl.Label>
                                <TextInput
                                    register={register}
                                    name='serials'
                                    required
                                    block
                                    errors={errors}
                                    validateMessage='Vui lòng điền Serial'

                                />
                                 <FormControl.Caption>
                                    <Text text='Nếu có nhiều số Serial, nhập ngăn cách bằng dấu;' />
                                </FormControl.Caption>
                            </FormControl>
                            <FormControl sx={{ mt: 2 }}>
                                <FormControl.Label>
                                    <Text text='Cơ quan thuế quản lý' />
                                </FormControl.Label>

                                <SelectBoxCoQuanThue
                                    maxWidth={"300px"}
                                    onValueChanged={(id, data) => {
                                        setCoQuanThueId(id)
                                        setTenDonViChuQuan(data?.ten ?? "")
                                    }}
                                    value={coQuanThueId}
                                />
                                {/* {
                                    errors && errors["donvi_chuquan"] &&
                                    <FormControl.Validation id={"donvi_chuquan"} variant="error">
                                        <>Vui lòng chọn đơn vị chủ quản</>
                                    </FormControl.Validation>
                                } */}
                            </FormControl>
                        </Box>
                        <Box className='col-md-6'
                            sx={{
                                borderRight: "1px",
                                borderRightStyle: "dashed",
                                borderRightColor: "border.default",
                                pr: 4
                            }}
                        >
                            <FormControl>
                                <FormControl.Caption>Thông tin liên hệ</FormControl.Caption>
                            </FormControl>
                            <FormControl sx={{ mt: 2 }}>
                                <FormControl.Label>
                                    <Text text='Điện thoại' />
                                </FormControl.Label>
                                <TextInput
                                    register={register}
                                    name='dien_thoai'
                                    // required
                                    width={150}
                                    errors={errors}
                                    validateMessage='Vui lòng điền số điện thoại'

                                />
                            </FormControl>
                            <FormControl sx={{ mt: 2 }}>
                                <FormControl.Label>
                                    <Text text='Fax' />
                                </FormControl.Label>
                                <TextInput
                                    register={register}
                                    name='fax'
                                    // required
                                    width={200}
                                    // block
                                    errors={errors}
                                // validateMessage='Vui lòng điền ngân hàng'

                                />
                            </FormControl>
                            <FormControl sx={{ mt: 2 }}>
                                <FormControl.Label>
                                    <Text text='Website' />
                                </FormControl.Label>
                                <TextInput
                                    register={register}
                                    name='website'
                                    // required
                                    // width={150}
                                    block
                                    errors={errors}
                                    validateMessage='Vui lòng điền website'

                                />
                            </FormControl>
                            <FormControl sx={{ mt: 2 }}>
                                <FormControl.Label>
                                    <Text text='Email' />
                                </FormControl.Label>
                                <TextInput
                                    register={register}
                                    name='email'
                                    // required
                                    // width={150}
                                    block
                                    errors={errors}
                                    validateMessage='Vui lòng điền Email'

                                />
                            </FormControl>

                            <FormControl sx={{ mt: 3 }}>
                                <FormControl.Caption>Thông tin ngân hàng</FormControl.Caption>
                            </FormControl>
                            <FormControl sx={{ mt: 2 }}>
                                <FormControl.Label>
                                    <Text text='Số tài khoản' />
                                </FormControl.Label>
                                <TextInput
                                    register={register}
                                    name='stk'
                                    // required
                                    width={200}
                                    errors={errors}
                                    validateMessage='Vui lòng điền số tài khoản'

                                />
                            </FormControl>
                            <FormControl sx={{ mt: 2 }}>
                                <FormControl.Label>
                                    <Text text='Tại ngân hàng' />
                                </FormControl.Label>
                                <TextInput
                                    register={register}
                                    name='ngan_hang'
                                    // required
                                    // width={200}
                                    block
                                    errors={errors}
                                    validateMessage='Vui lòng điền ngân hàng'

                                />
                            </FormControl>
                        </Box>
                        {/* <Box className='col-md-4' sx={{ mt: 3 }}>
                            <FormControl>
                                <FormControl.Caption>Thông tin khác</FormControl.Caption>
                            </FormControl>
                            <FormControl sx={{ mt: 2 }}>
                                <FormControl.Label>
                                    <Text text='Mã số CQT cấp' />
                                </FormControl.Label>
                                <FormControl.Caption>Là mã số được CQT cấp khi duyệt tờ khai, sử dụng mã số này để tạo hóa đơn máy tính tiền</FormControl.Caption>

                                <TextInput
                                    register={register}
                                    name='ma_dang_ky_cqt'
                                    // required
                                    width={200}
                                    errors={errors}
                                    validateMessage='Vui lòng điền số tài khoản'

                                />
                            </FormControl>
                            <FormControl sx={{ mt: 2 }}>
                                <FormControl.Label>
                                    <Text text='Ngày hóa đơn mới nhất' />
                                </FormControl.Label>
                                <FormControl.Caption>Là ngày phát hành hóa đơn mới nhất, Người dùng chỉ được phép tạo hóa đơn từ ngày này trở đi</FormControl.Caption>
                                <TextInput
                                    register={register}
                                    name='ngay_hoa_don_max'
                                    // required
                                    type='date'
                                    // width={200}
                                    // block
                                    errors={errors}
                                    validateMessage='Vui lòng điền ngân hàng'

                                />
                            </FormControl>
                        </Box> */}
                    </Box>

                    <ModalActions>
                        <Button onClick={() => {
                            dispatch(rootAction.category.donViActionType.closeEditModal())
                        }} text='Đóng' />
                        <Button variant='primary'
                            type='submit'
                            text={(donViEditing?.id ?? 0) === 0 ? "Thêm mới" : "Cập nhật"}
                            isLoading={status === eReducerStatusBase.is_saving}
                        />
                    </ModalActions>
                </Box>
            </form>
        </Modal>
    );
};

export default DonViEditFormModal;