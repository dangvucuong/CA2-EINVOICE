import { Box } from '@primer/react';
import Button from '../../component-ui/button';
import Modal from "../../component-ui/modal";
import ModalActions from "../../component-ui/modal/ModalActions";
import BangTongHopTimeline from './BangTongHopTimeline';
interface IBangTongHopTimelineModalProps {
    id: number,
    onClose: () => void
}
export const BangTongHopTimelineModal = (props: IBangTongHopTimelineModalProps) => {
   
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
                <BangTongHopTimeline id={props.id}/>



                <ModalActions>
                    <Button onClick={() => {
                        props.onClose();
                    }} text='Đóng' />

                </ModalActions>
            </Box>
        </Modal>
    );
};
