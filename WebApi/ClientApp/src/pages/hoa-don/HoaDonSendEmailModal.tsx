import React, { useState } from 'react';
import Modal from '../../component-ui/modal';
import ModalActions from '../../component-ui/modal/ModalActions';
import Button from '../../component-ui/button';
import { Box, FormControl } from '@primer/react';
import { PaperAirplaneIcon } from '@primer/octicons-react';
import TextInput from '../../component-ui/text-input';
import { hoaDonApi } from '../../api/hoa-don/hoaDonApi';
import { NotifyHelper } from '../../helpers/toast';
interface IHoaDonSendEmailModalProps {
    id: number;
    defaultEmail?: string,
    onClose: () => void,
    onSuccess: () => void
}
const HoaDonSendEmailModal = (props: IHoaDonSendEmailModalProps) => {
    console.log({
        defaultEmail: props.defaultEmail
    });

    const [emails, setEmails] = useState(props.defaultEmail);
    const [isSaving, setIsSaving] = useState(false);

    const handleSendEmailAysnc = async () => {
        setIsSaving(true)
        const res = await hoaDonApi.sendEmailCustom({
            id: props.id,
            emails: emails
        });
        setIsSaving(false)
        if (res.is_success) {
            NotifyHelper.Success("Success")
            props.onSuccess();

        } else {
            NotifyHelper.Error(res.message ?? "Error")

        }
    }
    return (
        <Modal
            title={"Gửi email"}
            isOpen={true}
            onClose={props.onClose}
            width={"large"}
        >
            <Box>
                <FormControl>
                    <FormControl.Label>Địa chỉ nhận email</FormControl.Label>
                    <TextInput block
                        value={emails}
                        onChange={(e) => {
                            setEmails(e.target.value)
                        }}
                    />
                    <FormControl.Caption>
                        Anh/chị có thể nhập nhiều email, ngăn cách giữa các email bằng dấu ";"
                    </FormControl.Caption>
                </FormControl>
            </Box>
            <ModalActions>
                <Button text='Đóng' onClick={() => { props.onClose() }} />
                <Button text='Gửi email' variant='primary'
                    leadingVisual={PaperAirplaneIcon}
                    isLoading={isSaving}
                    onClick={() => {
                        handleSendEmailAysnc();
                    }}
                />
            </ModalActions>
        </Modal>
    );
};

export default HoaDonSendEmailModal;