import { Box } from '@primer/react';
import Button from '../../component-ui/button';
import Modal from "../../component-ui/modal";
import ModalActions from "../../component-ui/modal/ModalActions";
import HoaDonTimeLine from './HoaDonTimeLine';
interface IHoaDonTimelineModalProps {
    hoaDonId: number,
    onClose: () => void
}
export const HoaDonTimelineModal = (props: IHoaDonTimelineModalProps) => {
    
   
    return (
        <Modal title={"Lịch sử"}
            onClose={() => {
                props.onClose();
            }}
            isOpen={true}
            width='xlarge'
            height={"auto"}
        // key={khachHangEditing?.id ?? 0}

        >
            <Box
                display={"grid"}
                sx={{
                    gap: 2
                }}
            >
                <HoaDonTimeLine hoaDonId={props.hoaDonId}/>



                <ModalActions>
                    <Button onClick={() => {
                        props.onClose();
                    }} text='Đóng' />

                </ModalActions>
            </Box>
        </Modal>
    );
};
