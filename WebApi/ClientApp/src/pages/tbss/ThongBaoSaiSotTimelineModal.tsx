import { Box } from '@primer/react';
import Button from '../../component-ui/button';
import Modal from "../../component-ui/modal";
import ModalActions from "../../component-ui/modal/ModalActions";
import ThongBaoSaiSotTimeline from './ThongBaoSaiSotTimeline';
interface IThongBaoSaiSotTimelineModalProps {
    id: number,
    onClose: () => void
}
export const ThongBaoSaiSotTimelineModal = (props: IThongBaoSaiSotTimelineModalProps) => {
   
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
                <ThongBaoSaiSotTimeline id={props.id}/>



                <ModalActions>
                    <Button onClick={() => {
                        props.onClose();
                    }} text='Đóng' />

                </ModalActions>
            </Box>
        </Modal>
    );
};
