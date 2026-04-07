import { FilterIcon } from '@primer/octicons-react';
import { Box, FormControl } from '@primer/react';
import moment from 'moment';
import { useState } from 'react';
import Button from '../button';
import Modal from '../modal';
import ModalActions from '../modal/ModalActions';
import TextInput from '../text-input';
interface ITuNgayDenNgayInputProps {
    tu_ngay?: string,
    den_ngay?: string,
    onValueChanged: (tu_ngay?: string, den_ngay?: string) => void
}
const TuNgayDenNgayInput = (props: ITuNgayDenNgayInputProps) => {
    const [isShowModal, setIsShowModal] = useState(false);
    const [tuNgay, setTuNgay] = useState(props.tu_ngay);
    const [denNgay, setDenNgay] = useState(props.den_ngay);
    const getText = () => {
        if (!props.tu_ngay && !props.den_ngay) {
            return "Chọn ngày";
        }
        const tuNgayText = props.tu_ngay ? moment(props.tu_ngay).format("DD/MM/YY") : ""
        const denNgayText = props.den_ngay ? moment(props.den_ngay).format("DD/MM/YY") : ""
        return `${(tuNgayText ? `Từ ${tuNgayText}` : "")} ${(denNgayText ? ` đến ${denNgayText}` : "")}`
    }

    return (
        <Box>
            <Button text={getText()} size='medium' leadingVisual={FilterIcon} onClick={() => { setIsShowModal(true) }} />
            {isShowModal &&
                <Modal isOpen={true} title="Chọn ngày"
                    onClose={() => {
                        setIsShowModal(false)
                    }}
                    width={"small"}
                >
                    <Box display={"grid"}
                        sx={{
                            gap: 2
                        }}>
                        <FormControl>
                            <FormControl.Label>Từ ngày</FormControl.Label>
                            <TextInput type="date" value={tuNgay}
                                block
                                onChange={(e) => {
                                    setTuNgay(e.target.value)
                                }}
                            />
                        </FormControl>
                        <FormControl>
                            <FormControl.Label>Đến ngày</FormControl.Label>
                            <TextInput type="date" value={denNgay}
                                block
                                onChange={(e) => {
                                    setDenNgay(e.target.value)
                                }}
                            />
                        </FormControl>
                        <FormControl>
                            <Button text='Bỏ chọn'
                                variant='invisible'
                                onClick={() => {
                                    setTuNgay("")
                                    setDenNgay("")
                                }}
                            />
                        </FormControl>
                    </Box>
                    <ModalActions>
                        <Button text='Đóng' onClick={() => {
                            setIsShowModal(false)
                        }} />
                        <Button text='Áp dụng'
                            variant='primary'
                            onClick={() => {
                                setIsShowModal(false)
                                props.onValueChanged(tuNgay, denNgay);
                            }} />
                    </ModalActions>
                </Modal>
            }
        </Box>
    );
};

export default TuNgayDenNgayInput;