import React from 'react';
import Modal from '../../component-ui/modal';
import { Box } from '@primer/react';
import ModalActions from '../../component-ui/modal/ModalActions';
import HoaDonTimeLine from '../hoa-don/HoaDonTimeLine';
import Button from '../../component-ui/button';
import { IHoaDonPhatHanhPushNotifyModel } from '../../models/responses/hub/IHoaDonPhatHanhPushNotifyModel';
import HoaDonStatus from '../../component-data/hoa-don-status';
interface IHoaDonPhatHanhResultModalProps {
    id: number,
    data: IHoaDonPhatHanhPushNotifyModel,
    onClose: () => void,

}
const HoaDonPhatHanhResultModal = (props: IHoaDonPhatHanhResultModalProps) => {
    const { onClose } = props;
    return (
        <Modal
            title={"Kết quả phát hành"}
            onClose={() => {
                props.onClose()
            }}
            isOpen={true}
            width='large'
            height={"auto"}

        >

            <Box>
                <Box>
                    {/* {JSON.stringify(props.data)} */}
                    <HoaDonStatus id={props.data.hoa_don_trang_thai_id} />
                </Box>
                <Box>
                    <HoaDonTimeLine hoaDonId={props.id} hoa_don_trang_thai_id={props.data.hoa_don_trang_thai_id}/>
                </Box>
                <ModalActions>
                    <Button onClick={() => {
                        onClose();
                    }} text='Đóng' />

                </ModalActions>
            </Box>
        </Modal>


    );
};

export default HoaDonPhatHanhResultModal;