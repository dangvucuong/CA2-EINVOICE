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
import TextArea from '../../component-ui/text-area';
import CmbCompanySize from '../../component-data/cmb-company-size';
import SelectBoxContactStatus from '../../component-data/selectbox-contact-status';
import { useEffect, useState } from 'react';
import { IContact } from '../../models/responses/contact/IContact';

const ContactEditFormModal = () => {
    const dispatch = useAppDispatch();
    const { user } = useAuth();
    const { contactEditing, status } = useAppSelector(x => x.contact.contactReducer)
    const [contactStatusId, setContactStatusId] = useState<number>(0);
    useEffect(() => {
        setContactStatusId(contactEditing?.contact_status_id ?? 0)
    }, [contactEditing])
    const { register, handleSubmit, clearErrors, setError, formState: { errors } } = useForm<IContact>({
        shouldUseNativeValidation: false,
        defaultValues: {
            ...contactEditing,
        }
    })


    const onSubmit = async (data: any) => {
        if (contactStatusId <= 0) {
            setError("contact_status_id", {
                type: "manual",
                message: "Vui lòng chọn trạng thái",
            })
        } else {
            dispatch(rootAction.contact.contactAction.saveStart({
                ...data,
                contact_status_id: contactStatusId
            }))
        }

    }
    return (
        <Modal title={(contactEditing?.id ?? 0) === 0 ? "Thêm mới" : "Cập nhật"}
            onClose={() => {
                dispatch(rootAction.contact.contactAction.closeEditModal())
            }}
            isOpen={true}
            width='large'
            height={"auto"}
            key={contactEditing?.id ?? 0}

        >
            <form onSubmit={handleSubmit(onSubmit)}>
                <Box
                    display={"grid"}
                    sx={{
                        gap: 2
                    }}
                >
                    <FormControl >
                        <FormControl.Label>
                            <Text text='Tên công ty' />
                        </FormControl.Label>
                        <TextInput
                            register={register}
                            name='name'
                            required
                            block
                            validateMessage='Vui lòng điền tên đơn vị mua hàng'
                            errors={errors}
                            disabled
                        />
                    </FormControl>

                    <FormControl >
                        <FormControl.Label>
                            <Text text='Địa chỉ' />
                        </FormControl.Label>
                        <TextInput
                            register={register}
                            name='address'
                            required
                            block
                            disabled
                            validateMessage='Vui lòng điền Địa chỉ'
                            errors={errors}
                        />
                    </FormControl>
                    <Box sx={{ display: "grid" }} gridTemplateColumns={"1fr 1fr"}>
                        <FormControl sx={{ pr: 2 }}>
                            <FormControl.Label>
                                <Text text='Mã số thuế' />
                            </FormControl.Label>
                            <TextInput
                                register={register}
                                name='tax_code'
                                required
                                disabled
                                block
                                validateMessage='Vui lòng điền Mã số thuế'
                                errors={errors}
                            />
                        </FormControl>
                        <FormControl >
                            <FormControl.Label>
                                <Text text='Email' />
                            </FormControl.Label>
                            <TextInput
                                register={register}
                                name='email'
                                width={250}
                                required
                                block
                                disabled
                                validateMessage='Vui lòng điền Email'
                                errors={errors}
                            />
                        </FormControl>
                    </Box>
                    <Box sx={{ display: "grid" }} gridTemplateColumns={"1fr 1fr"}>
                        <FormControl sx={{ pr: 2 }}>
                            <FormControl.Label>
                                <Text text='Điện thoại' />
                            </FormControl.Label>
                            <TextInput
                                register={register}
                                name='phone'
                                block
                                disabled
                                validateMessage='Vui lòng điền Điện thoại'
                                errors={errors}
                            />
                        </FormControl>
                        <FormControl >
                            <FormControl.Label>
                                <Text text='Serial' />
                            </FormControl.Label>
                            <TextInput
                                register={register}
                                name='serial'
                                width={250}
                                block
                                disabled
                                validateMessage='Vui lòng điền Email'
                                errors={errors}
                            />
                        </FormControl>
                    </Box>
                    <Box sx={{ display: "grid" }} gridTemplateColumns={"1fr 1fr"}>
                        <FormControl sx={{ pr: 2 }}>
                            <FormControl.Label>
                                <Text text='Số lượng nhân viên' />
                            </FormControl.Label>
                            <CmbCompanySize
                                readonly
                                onValueChanged={() => { }}
                                value={contactEditing?.company_size_id??0}
                            />
                        </FormControl>

                    </Box>
                    <FormControl>
                        <FormControl.Label>
                            <Text text='Info' />
                        </FormControl.Label>
                        <TextArea
                            rows={2}
                            register={register}
                            name='info'
                            disabled
                            block
                            errors={errors}
                        />
                    </FormControl>
                    <FormControl >
                        <FormControl.Label>
                            <Text text='Trạng thái' />
                        </FormControl.Label>
                        <SelectBoxContactStatus
                            onValueChanged={(value) => {
                                setContactStatusId(value)
                                if (value > 0) {
                                    clearErrors("contact_status_id")
                                }
                            }}
                            value={contactStatusId}
                        />
                        {
                            errors && errors["contact_status_id"] &&
                            <FormControl.Validation id={"contact_status_id"} variant="error">
                                <>{errors["contact_status_id"].message ?? ""}</>
                            </FormControl.Validation>
                        }
                    </FormControl>
                    <FormControl>
                        <FormControl.Label>
                            <Text text='Ghi chú' />
                        </FormControl.Label>
                        <TextArea
                            rows={2}
                            register={register}
                            name='note'
                            block
                            errors={errors}
                        />
                    </FormControl>



                    <ModalActions>
                        <Button onClick={() => {
                            dispatch(rootAction.contact.contactAction.closeEditModal())
                        }} text='Đóng' />
                        <Button variant='primary'
                            type='submit'
                            text={(contactEditing?.id ?? 0) === 0 ? "Thêm mới" : "Cập nhật"}
                            isLoading={status === eReducerStatusBase.is_saving}
                        />
                    </ModalActions>
                </Box>
            </form>
        </Modal>
    );
};

export default ContactEditFormModal;