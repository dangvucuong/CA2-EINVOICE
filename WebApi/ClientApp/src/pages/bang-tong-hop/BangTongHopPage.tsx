import { HistoryIcon, PencilIcon, PlusIcon, TrashIcon } from "@primer/octicons-react";
import { Box, Checkbox, useConfirm } from "@primer/react";
import { useEffect, useState } from 'react';

import { IconButton } from '@primer/react';
import { Helmet } from 'react-helmet';
import { useHistory } from 'react-router-dom';
import { BANG_TONG_HOP_API, bangTongHopApi } from "../../api/bang-tong-hop/bangTongHopApi";
import ThongBaoSaiSotStatus from "../../component-data/tbss-status";
import Button from '../../component-ui/button';
import DataTable from '../../component-ui/data-table/DataTable';
import Heading from '../../component-ui/heading';
import { IBangTongHopDuLieu } from "../../models/responses/bang-tong-hop/IBangTongHopDuLieu";
import { NotifyHelper } from "../../helpers/toast";
import { BangTongHopTimelineModal } from "./BangTongHopTimelineModal";

const BangTongHopPage = () => {
    const [bangTongHops, setBangTongHops] = useState<IBangTongHopDuLieu[]>([]);
    const [isShowLogModal, setIsShowLogModal] = useState(false);
    const [editingData, setEditingData] = useState<IBangTongHopDuLieu>();
    const confirm = useConfirm();
    const history = useHistory();
    useEffect(() => {
        handleReloadAsync();
    }, [])
    const handleReloadAsync = async () => {
        const res = await bangTongHopApi.getByDonVi();
        if (res.is_success) {
            setBangTongHops(res.data)
        }
    }
    const handleDeleteAsync = async () => {
        if (editingData) {
            if (await confirm({
                content: "Bạn có chắc chắn muốn xóa Bảng tổng hợp này?",
                title: "Xóa Bảng tổng hợp",
                cancelButtonContent: "Không xóa",
                confirmButtonContent: "Xóa Bảng tổng hợp",
                confirmButtonType: "danger"
            })) {
                const res = await bangTongHopApi.delete(editingData?.id ?? 0);
                if (res.is_success) {
                    handleReloadAsync();
                    NotifyHelper.Success("Success")

                } else {
                    NotifyHelper.Error(res.message ?? "")
                }
            }
        }


    }
    return (
        <Box>
            <Helmet>
                <title>Bảng tổng hợp</title>
            </Helmet>

            <DataTable
                titleComponent={<Heading text='Danh sách bảng tổng hợp' />}
                subTitle={`Tổng số: ${(bangTongHops.length).toLocaleString()}`}
                data={bangTongHops}
                height={window.innerHeight - 100}
                // isLoading={status === eReducerStatusBase.is_loading}
                exportEnable
                actionComponent={<>
                    <Button text='Thêm mới'
                        variant='primary'
                        leadingVisual={PlusIcon}
                        apiAuthorizedMethod='POST'
                        apiAuthorized={BANG_TONG_HOP_API}
                        onClick={() => {
                            history.push("../../bang-tong-hop/0")
                            // dispatch(rootAction.category.khachHangAction.showEditModal())
                        }}
                    />
                </>}
                searchEnable={true}

                columns={[
                    {
                        header: 'Id',
                        field: 'id',
                        rowHeader: false,
                        width: "50px"
                    },
                    {
                        id: "actions",
                        header: "",
                        width: "120px",
                        renderCell: (data: IBangTongHopDuLieu) => {
                            return (
                                <>
                                    <Box sx={{
                                        mt: -2,
                                        mb: -2,
                                        display: "flex",
                                        justifyContent: "center"
                                    }}>
                                        {data.bang_tong_hop_du_lieu_trang_thai_id === 1 &&
                                            <>
                                                <IconButton
                                                    aria-label={`Edit: ${data.id}`}
                                                    title={`Edit: ${data.id}`}
                                                    icon={PencilIcon}
                                                    variant="invisible"
                                                    onClick={() => {
                                                        history.push(`../../bang-tong-hop/${data.id}`)

                                                    }}
                                                />
                                                <IconButton
                                                    aria-label={`Edit: ${data.id}`}
                                                    title={`Edit: ${data.id}`}
                                                    icon={TrashIcon}
                                                    variant="invisible"
                                                    onClick={() => {
                                                        setEditingData(data)
                                                        handleDeleteAsync();

                                                    }}
                                                />
                                            </>
                                        }

                                        <IconButton
                                            aria-label={`Edit: ${data.id}`}
                                            title={`Edit: ${data.id}`}
                                            icon={HistoryIcon}
                                            variant="invisible"
                                            onClick={() => {
                                                setIsShowLogModal(true)
                                                setEditingData(data)

                                            }}
                                        />



                                    </Box>
                                </>
                            )
                        }
                    },
                    {
                        header: 'Loại',
                        field: 'bang_tong_hop_du_lieu_type',
                        rowHeader: false,
                        width: "100px",
                        renderCell: (data: IBangTongHopDuLieu) => {
                            return (
                                <Box>
                                    {data.bang_tong_hop_du_lieu_type === "ngay" && <>Theo ngày</>}
                                    {data.bang_tong_hop_du_lieu_type === "thang" && <>Theo tháng</>}
                                    {data.bang_tong_hop_du_lieu_type === "quy" && <>Theo quý</>}
                                </Box>
                            );
                        }
                    },
                    {
                        header: 'Kỳ dữ liệu',
                        field: 'ky_du_lieu',
                        rowHeader: true,
                        width: "150px"
                    },
                    {
                        header: 'Nộp lần đầu',
                        field: 'is_lan_dau',
                        width: "100px",
                        rowHeader: false,
                        renderCell: (data: IBangTongHopDuLieu) => {
                            return <Checkbox checked={data.is_lan_dau} readOnly />
                        }
                    },
                    {
                        header: 'STT',
                        field: 'so_thu_tu_lan_bo_sung',
                        width: "80px",
                        rowHeader: false,

                    },
                    {
                        header: 'Loại hàng hóa',
                        field: 'is_lan_dau',
                        width: "200px",
                        rowHeader: false,
                        renderCell: (data: IBangTongHopDuLieu) => {
                            return <Box>
                                {data.bang_tong_hop_du_lieu_loai_hang_hoa_id === 1 && <>Hàng hóa, dịch vụ khác</>}
                                {data.bang_tong_hop_du_lieu_loai_hang_hoa_id === 2 && <>Vận tải hàng không</>}
                                {data.bang_tong_hop_du_lieu_loai_hang_hoa_id === 3 && <>Xăng dầu</>}
                            </Box>
                        }
                    },
                    {
                        header: 'SL hóa đơn',
                        field: 'so_luong_hoa_don',
                        width: "120px",
                        rowHeader: false,

                    },
                    {
                        header: 'Trạng thái',
                        field: 'bang_tong_hop_du_lieu_trang_thai_id',
                        // maxWidth: "200px",
                        rowHeader: false,
                        renderCell: (data: IBangTongHopDuLieu) => {
                            return <ThongBaoSaiSotStatus id={data.bang_tong_hop_du_lieu_trang_thai_id} />
                        }
                    },


                    {
                        header: 'Kết quả phản hồi',
                        field: 'ket_qua_phan_hoi',
                        minWidth: "200px",
                        rowHeader: false,
                    }
                ]}
            />
            {isShowLogModal && editingData &&
                <BangTongHopTimelineModal
                    id={editingData.id}
                    onClose={() => {
                        setIsShowLogModal(false)
                    }}
                />
            }
        </Box>
    );
};

export default BangTongHopPage;