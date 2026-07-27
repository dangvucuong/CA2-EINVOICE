import { Box, IconButton } from '@primer/react';
import { EyeIcon, PlusIcon, HistoryIcon } from "@primer/octicons-react";

import moment from 'moment';
import { useEffect } from 'react';
import HoaDonHinhThuc from '../../component-data/hoa-don-hinh-thuc';
import HoaDonStatus from '../../component-data/hoa-don-status';
import DataTableRemotePaging from '../../component-ui/data-table';
import { useAppDispatch } from '../../hooks/useAppDispatch';
import { useAppSelector } from '../../hooks/useAppSelector';
import { eHoaDonTrangThai } from '../../models/commons/eHoaDonTrangThai';
import { eSortMode } from '../../models/commons/eSortMode';
import { IHoaDon } from '../../models/responses/hoa-don/IHoaDon';
import { hoaDonAction } from '../../state/actions/hoa-don/hoaDonAction';
import { eReducerStatusBase } from '../../state/reducer-models/eReducerStatusBase';
import { HOADON_LIST_SEARCH_PLACEHOLDER } from '../../utils/hoaDonListFilter';
import Button from '../../component-ui/button';
import { useHistory } from 'react-router-dom';
import { rootAction } from '../../state/actions/rootAction';
import { HoaDonTimelineModal } from '../hoa-don/HoaDonTimelineModal';
import Heading from '../../component-ui/heading';
const DanhSachHoaDonGanNhat = () => {
    const history = useHistory();

    const { status, hoaDons, filter, paging_res, isShowLogModal, hoaDonEditing,
        hoaDonSelectedIds } = useAppSelector(x => x.hoaDon.hoaDonReducer)
    const dispatch = useAppDispatch();
    useEffect(() => {
        dispatch(hoaDonAction.changeFilter({
            ...filter,
            hoa_don_hinh_thuc_code: undefined,
            hoa_don_trang_thai_ids:
                [eHoaDonTrangThai.NHAP,
                eHoaDonTrangThai.DA_GUI_LEN_CQT_CHUA_PHAN_HOI_KIEM_TRA_DU_LIEU,
                eHoaDonTrangThai.DA_GUI_LEN_CQT_PHAN_HOI_KY_THUAT,
                eHoaDonTrangThai.DA_HUY,
                eHoaDonTrangThai.DA_PHAT_HANH,
                eHoaDonTrangThai.CHUA_CO_KET_QUA_PHAN_HOI,
                eHoaDonTrangThai.KHONG_HOP_LE
                ]
        }))
    }, [])
    useEffect(() => {
        if (filter.hoa_don_trang_thai_ids.length > 0) {

            dispatch(hoaDonAction.loadStart({
                ...filter
            }))
        }
    }, [filter])
    return (
        <Box>
            <DataTableRemotePaging
                // title={`Hóa đơn xuất gần nhất`}
                titleComponent={<Heading text='Hóa đơn xuất gần nhất' />}
                // subTitle={`Đã chọn: ${hoaDonSelectedIds?.length ?? 0}`}
                data={hoaDons}
                height={window.innerHeight - 100}
                isLoading={status === eReducerStatusBase.is_loading}
                exportEnable
                actionComponent={
                    <>
                        <Button text='Lập hóa đơn mới' variant='primary' leadingVisual={PlusIcon} />
                    </>
                }
                searchConfig={{
                    enable: (hoaDonSelectedIds?.length ?? 0) <= 0,
                    placeholder: HOADON_LIST_SEARCH_PLACEHOLDER,
                    onValueChanged: (key: string) => {
                        dispatch(hoaDonAction.changeFilter({
                            ...filter,
                            page_index: 0,
                            search_key: key
                        }))
                    }
                }}
                sortConfig={{
                    enable: false,
                    field: filter.sort_by,
                    mode: filter.sort_mode ?? eSortMode.ASC,
                    onValueChanged: (key: string, sort_mode: eSortMode) => {
                        dispatch(hoaDonAction.changeFilter({
                            ...filter,
                            sort_by: key,
                            sort_mode: sort_mode
                        }))
                    }
                }}
                paging={{
                    onPageIndexChanged: (pageIndex) => {
                        dispatch(hoaDonAction.changeFilter({
                            ...filter,
                            page_index: pageIndex
                        }))
                    },
                    pageCount: paging_res?.page_count ?? 1,
                    pageIndex: paging_res?.page_number ?? 1,
                    pageSize: paging_res?.page_size ?? 1,
                    totalCount: paging_res?.total_count ?? 1
                }}
                // selection={{
                //     mode: "multiple",
                //     keyExpr: "id",
                //     selectedRowKeys: hoaDonSelectedIds,
                //     onSelectionChanged: (keys) => {
                //         dispatch(hoaDonAction.changeSelectedId(keys))
                //     }
                // }}
                columns={[
                    {
                        header: 'Id',
                        field: 'id',
                        rowHeader: false,
                        width: "80px"
                        // sortBy: "alphanumeric"
                    },
                    {
                        id: "actions",
                        header: "",
                        renderCell: (row: any) => {
                            return (
                                <>
                                    <Box sx={{
                                        mt: -2,
                                        mb: -2,
                                        display: "flex"
                                    }}>

                                        <IconButton
                                            aria-label={`Edit`}
                                            title={`Edit`}
                                            icon={EyeIcon}
                                            variant="invisible"
                                            onClick={() => {
                                                history.push(`../../hoa-don/form/${row.id}`)
                                            }}
                                        />
                                        <IconButton
                                            aria-label={`Edit`}
                                            title={`Edit`}
                                            icon={HistoryIcon}
                                            variant="invisible"
                                            onClick={() => {
                                                dispatch(rootAction.hoaDon.hoaDonAction.showLogModal(row))
                                            }}
                                        />

                                    </Box>
                                </>
                            )
                        }
                    },
                    {
                        header: 'Ký hiệu',
                        field: 'hoa_don_dang_ky_phat_hanh_ky_hieu',
                        rowHeader: true,
                        width: "100px",
                        // sortBy: "alphanumeric"
                    },

                    {
                        header: 'Loại HĐ',
                        field: 'ten_hoa_don',
                        rowHeader: false,
                        minWidth: "200px",
                        // sortBy: "alphanumeric"
                    },
                    {
                        header: 'Ngày HĐ',
                        field: 'ngay_hoa_don',
                        rowHeader: false,
                        width: "100px",
                        renderCell: (cell: IHoaDon) => {
                            return (
                                <Box>{moment(cell.ngay_hoa_don).format("DD/MM/YYYY")}</Box>
                            )
                        }
                        // sortBy: "alphanumeric"
                    },
                    {
                        header: 'Số HĐ',
                        field: 'so_hoa_don',
                        rowHeader: false,
                        width: "100px",
                        // sortBy: "alphanumeric"
                    },
                    {
                        header: 'MST',
                        field: 'nguoi_mua_mst',
                        rowHeader: false,
                        width: "140px",
                        // sortBy: "alphanumeric"
                    },
                    {
                        header: 'Người mua',
                        field: 'nguoi_mua_ten_donvi',
                        rowHeader: false,
                        minWidth: "200px",
                        // sortBy: "alphanumeric"
                    },
                    {
                        header: 'Tổng tiền',
                        field: 'tong_tien_thanh_toan',
                        rowHeader: false,
                        width: "100px",
                        // sortBy: "alphanumeric"
                    },
                    {
                        header: 'Mã tra cứu',
                        field: 'ma_tra_cuu',
                        rowHeader: false,
                        width: "150px",
                        // sortBy: "alphanumeric"
                    },
                    {
                        header: 'Hình thức HĐ',
                        field: 'hoa_don_hinh_thuc_id',
                        rowHeader: false,
                        width: "170px",
                        renderCell: (cell: IHoaDon) => {
                            return (
                                <>
                                    <HoaDonHinhThuc id={cell.hoa_don_hinh_thuc_id} />
                                </>
                            )
                        }
                    },
                    {
                        header: 'Trạng thái',
                        field: 'hoa_don_trang_thai_id',
                        rowHeader: false,
                        width: "200px",
                        renderCell: (cell: IHoaDon) => {
                            return (
                                <>
                                    <HoaDonStatus id={cell.hoa_don_trang_thai_id} />
                                </>
                            )
                        }
                    },
                    {
                        header: 'Nội dung phát hành',
                        field: 'ket_qua_phat_hanh',
                        rowHeader: false,
                        // maxWidth: "200px",
                        minWidth: "200px",
                        renderCell: (data: IHoaDon) => {
                            return (
                                <Box className="limit1Line">
                                    {data.ket_qua_phat_hanh}
                                </Box>
                            )
                        }
                    },


                ]}
            />
            {isShowLogModal && hoaDonEditing &&
                <HoaDonTimelineModal
                    hoaDonId={hoaDonEditing?.id}
                    onClose={() => {
                        dispatch(rootAction.hoaDon.hoaDonAction.closeLogModal())
                    }}
                />
            }
        </Box>
    );
};

export default DanhSachHoaDonGanNhat;