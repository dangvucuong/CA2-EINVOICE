import { TasklistIcon } from "@primer/octicons-react";
import { Box } from '@primer/react';
import { useEffect, useState } from 'react';
import Button from '../../component-ui/button';
import Modal from '../../component-ui/modal';
import ModalActions from '../../component-ui/modal/ModalActions';
import Text from '../../component-ui/text';
import { IHoaDon } from '../../models/responses/hoa-don/IHoaDon';
import HoaDonSelect from '../hoa-don-select';
interface ISelectBoxHoaDonProps {
    onValueChanged: (id: number[], hoaDon: IHoaDon[]) => void,
    value: number,
    maxWidth?: any,
    isShowClearBtn?: boolean,
    placeHolder?: string,
    leadingVisual?: any,
    variant?: any
}

const getHoaDonIcon = (hoaDon: IHoaDon) => {
    return (
        <Box sx={{ display: "flex", alignItems: "center" }}>

            <Box>
                {hoaDon.ten_hoa_don}
            </Box>
            <Box>

            </Box>
        </Box>
    );
}
const SelectBoxHoaDon = (props: ISelectBoxHoaDonProps) => {
    const [isShowModal, setIsShowModal] = useState(false);
    const [hoaDonIds, setHoaDonIds] = useState<number[]>([]);
    const [hoaDons, setHoaDons] = useState<IHoaDon[]>([]);

    const [hoaDonSelectedData, setHoaDonSelectedData] = useState<IHoaDon>();

    useEffect(() => {
        const temp = hoaDons.find(x => x.id === props.value)
        if (temp) {
            setHoaDonSelectedData(temp)
        } else {

        }
    }, [props.value])
    // const handleGetHoaDonSelectedData= async () =>{
    //     const res = await hoaDonApi.getViewModel()
    // }
    const handleApply = () => {
        // if (hoaDonIds.length !== 1) {
        //     NotifyHelper.Error("Vui lòng chọn một hóa đơn")
        // } else {
        //     props.onValueChanged(hoaDonIds[0], hoaDons[0])
        //     setIsShowModal(false)
        // }
        props.onValueChanged(hoaDonIds, hoaDons)
        setIsShowModal(false)
    }
    return (
        <Box>
            {/* <Button text='Click here' onClick={() => {
                setIsShowModal(true)
            }} /> */}
            <>
                {hoaDonSelectedData &&
                    // <Button size='medium'>
                    <Box sx={{
                        p: 2,
                        borderRadius: 2,
                        borderWidth: 1,
                        borderStyle: "solid",
                        borderColor: "border.muted",
                        cursor: "pointer",
                    }}
                        onClick={() => {
                            setIsShowModal(true)
                        }}
                    >
                        <Box><b>{hoaDonSelectedData.so_hoa_don}</b> - {hoaDonSelectedData.ten_hoa_don}</Box>
                        <Box><b>{hoaDonSelectedData.nguoi_mua_ten_donvi}</b></Box>
                        <Box>
                            Tổng tiền: &nbsp;
                            <Text text={hoaDonSelectedData.tong_tien_thanh_toan.toLocaleString()} sx={{
                                color: 'fg.muted'
                            }} />
                        </Box>
                    </Box>
                    // </Button>
                }
                {!hoaDonSelectedData && <Button text='Chọn hóa đơn' size='medium' onClick={() => {
                    setIsShowModal(true)
                }}
                    variant={props.variant ?? "default"}
                    leadingVisual={TasklistIcon}
                />}
            </>
            {isShowModal &&
                <Modal
                    title="Tìm kiếm hóa đơn"
                    onClose={() => {
                        setIsShowModal(false)
                    }}
                    width={"90%"}
                    isOpen
                >
                    {/* <Box sx={{ pt: 1 }}> */}
                    <HoaDonSelect onSelected={(ids, hoaDons) => {
                        setHoaDonIds(ids)
                        setHoaDons(hoaDons)
                    }} />
                    {/* </Box> */}
                    <ModalActions>
                        <Button text='Đóng' onClick={() => {
                            setIsShowModal(false)
                        }} />
                        <Button text='Áp dụng' variant='primary' onClick={handleApply} />
                    </ModalActions>
                </Modal>
            }
        </Box>
    );

}
export default SelectBoxHoaDon;