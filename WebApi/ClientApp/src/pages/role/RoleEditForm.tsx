import { Box, Checkbox, FormControl } from '@primer/react';
import { useForm } from 'react-hook-form';
import Button from '../../component-ui/button';
import Modal from '../../component-ui/modal';
import ModalActions from '../../component-ui/modal/ModalActions';
import Text from '../../component-ui/text';
import TextArea from '../../component-ui/text-area';
import TextInput from '../../component-ui/text-input';
import { useAppSelector } from '../../hooks/useAppSelector';
import { IRole } from '../../models/responses/user/IRole';
import { useAppDispatch } from '../../hooks/useAppDispatch';
import { rootAction } from '../../state/actions/rootAction';
import { eReducerStatusBase } from '../../state/reducer-models/eReducerStatusBase';
import { useEffect, useState } from 'react';
import { useCommonContext } from '../../contexts/common';
import { ROLE_API_VIEWALL_ENDPOINT } from '../../api/user/roleApi';
interface IRoleEditFormProps {
    isOpen?: boolean,
    onClose: () => void
}
const RoleEditForm = (props: IRoleEditFormProps) => {
    const dispatch = useAppDispatch();
    const [isPublic, setIsPublic] = useState(false);
    const { checkAccesiableTo } = useCommonContext();
    const { roleEditing, status } = useAppSelector(x => x.user.roleReducer)
    const { register, handleSubmit, formState: { errors } } = useForm<IRole>({
        shouldUseNativeValidation: false,
        defaultValues: roleEditing
    })
    useEffect(() => {
        if (roleEditing) {
            setIsPublic(roleEditing.is_public)
        }
    }, [roleEditing])

    const onSubmit = async (data: any) => {
        console.log({
            data,
            isPublic
        });

        dispatch(rootAction.user.roleAction.saveStart({
            ...data,
            id: roleEditing?.id ?? 0,
            sort_idx: roleEditing?.sort_idx ?? "",
            is_public: isPublic
        }))

    }
    return (
        <>
            {props.isOpen &&
                <Modal title={(roleEditing?.id ?? 0) === 0 ? "Thêm mới" : "Cập nhật"}
                    onClose={props.onClose}
                    isOpen={props.isOpen}
                    width='medium'
                    height={"auto"}
                    key={roleEditing?.id ?? 0}

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
                                    <Text text='Tên Role' />
                                </FormControl.Label>
                                <TextInput

                                    register={register}
                                    name='name'
                                    required
                                    validateMessage='Vui lòng điền tên Role'
                                    errors={errors}

                                />
                            </FormControl>
                            <FormControl >
                                <FormControl.Label>
                                    <Text text='Tên Role (En)' />
                                </FormControl.Label>
                                <TextInput
                                    register={register}


                                    name='name_en'
                                    required
                                    validateMessage='Vui lòng điền tên Role bằng Tiếng Anh'
                                    errors={errors}

                                />
                            </FormControl>
                            <FormControl >
                                <FormControl.Label>
                                    <Text text='Mô tả' />
                                </FormControl.Label>
                                <TextArea
                                    register={register}
                                    name='description'

                                    block
                                    rows={2}
                                    required
                                    validateMessage='Vui lòng điền mô tả'
                                    errors={errors}

                                />
                            </FormControl>
                            {checkAccesiableTo(ROLE_API_VIEWALL_ENDPOINT, "GET") &&
                                <FormControl >
                                    <FormControl.Label>
                                        <Text text='Public' />
                                    </FormControl.Label>
                                    <Checkbox name='is_public'
                                        checked={isPublic}
                                        onChange={(e) => {
                                            console.log({ e });

                                            setIsPublic(e.target.checked)
                                        }}
                                    />
                                    <FormControl.Caption>
                                        Với Role public, Đơn vị có thể nhìn thấy (không được phép sửa) để phân quyền cho Người dùng tại đơn vị
                                    </FormControl.Caption>
                                </FormControl>
                            }
                            <ModalActions>
                                <Button onClick={props.onClose} text='Đóng' />
                                <Button variant='primary'
                                    type='submit'
                                    text={(roleEditing?.id ?? 0) === 0 ? "Thêm mới" : "Cập nhật"}
                                    isLoading={status == eReducerStatusBase.is_saving}
                                />
                            </ModalActions>
                        </Box>
                    </form>
                </Modal>
            }
        </>

    );
};

export default RoleEditForm;