import { Box } from '@primer/react';
import ThongBaoSaiSotStatus from '../../component-data/tbss-status';
import Button from '../../component-ui/button';
import Modal from '../../component-ui/modal';
import ModalActions from '../../component-ui/modal/ModalActions';
import { ITBSSPhatHanhPushNotifyModel } from '../../models/responses/hub/TBSSPhatHanhPushNotifyModel';
import HoaDonTimeLine from '../hoa-don/HoaDonTimeLine';
import ThongBaoSaiSotTimeline from './ThongBaoSaiSotTimeline';
interface ITBSSPhatHanhResultModalProps {
    id: number,
    data: ITBSSPhatHanhPushNotifyModel,
    onClose: () => void,

}
const TBSSPhatHanhResultModal = (props: ITBSSPhatHanhResultModalProps) => {
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
                    <ThongBaoSaiSotStatus id={props.data.thong_bao_sai_sot_trang_thai_id} />
                </Box>
                <Box>
                    <ThongBaoSaiSotTimeline id={props.id} />
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

export default TBSSPhatHanhResultModal;