import { Box } from '@primer/react';
import { useEffect, useState } from 'react';
import { donViApi } from '../../api/category/donViApi';
import Button from '../../component-ui/button';
import Modal from '../../component-ui/modal';
import ModalActions from '../../component-ui/modal/ModalActions';
import { NotifyHelper } from '../../helpers/toast';
import { DataTable } from '../../component-ui/data-table';
import moment from 'moment';
interface ILichSuMuaChuKySoModalProps {
    onCancel: () => void
}
const LichSuMuaChuKySoModal = (props: ILichSuMuaChuKySoModalProps) => {
    const [isLoading, setIsLoading] = useState(false);
    const [lichSus, setLichSus] = useState<any[]>([]);
    useEffect(() => {
        handleGetLichSuMuaCKS();
    }, [])
    const handleGetLichSuMuaCKS = async () => {
        setIsLoading(true)
        const res = await donViApi.getLichSuMuaCKS();
        setIsLoading(false)
        if (res.is_success) {
            setLichSus(res.data)
        } else {
            NotifyHelper.Error(res.message ?? "Error")
        }
    }

    return (
        <Modal title={"Lịch sử"}
            onClose={() => {
                props.onCancel();
            }}
            isOpen={true}
            width={"80%"}
            height={"auto"}

        >

            <Box>
                <Box sx={{
                    height: window.innerHeight - 200,
                    overflow: "auto"
                }}>
                    <DataTable
                        data={lichSus}
                        columns={[
                            {
                                header: 'Id',
                                field: 'id',
                                rowHeader: false,
                                width: "100px"
                                // sortBy: "alphanumeric"
                            },
                            {
                                header: 'Nguời yêu cầu',
                                field: 'nguoi_yeu_cau',
                                rowHeader: true,
                                // width: "50px"
                                // sortBy: "alphanumeric"
                            },
                            {
                                header: 'Ngày mua',
                                field: 'ngay_mua',
                                renderCell: (data: any) => {
                                    return (
                                        <Box>{moment(data.ngay_mua).format("DD/MM/YYYY")}</Box>
                                    );
                                }
                            },
                            {
                                header: 'Mã gói',
                                field: 'ma_goi_dich_vu',
                            },
                            {
                                header: 'Loại sản phầm',
                                field: 'loai_san_pham',
                            },
                            {
                                header: 'Số lượng',
                                field: 'so_luong',
                                renderCell: (data: any) => {
                                    return (
                                        <Box>{data.so_luong.toLocaleString()}</Box>
                                    );
                                }
                            },
                            {
                                header: 'Khuyến mại',
                                field: 'so_luong_khuyen_mai',
                                renderCell: (data: any) => {
                                    return (
                                        <Box>{data.so_luong_khuyen_mai.toLocaleString()}</Box>
                                    );
                                }
                            },
                            {
                                header: 'Tổng',
                                field: 'tong_so_luong',
                                renderCell: (data: any) => {
                                    return (
                                        <Box>{data.tong_so_luong.toLocaleString()}</Box>
                                    );
                                }
                            }
                        ]}
                    />
                </Box>
                <ModalActions>
                    <Button onClick={() => {
                        props.onCancel();
                    }} text='Đóng' />

                </ModalActions>
            </Box>

        </Modal>
    );
};

export default LichSuMuaChuKySoModal;