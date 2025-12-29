import { Box, IconButton, Truncate } from '@primer/react';
import { PencilIcon, PlusIcon, TrashIcon } from "@primer/octicons-react"
import React, { useEffect, useMemo } from 'react';
import { Helmet } from 'react-helmet';
import { useAppSelector } from '../../hooks/useAppSelector';
import { useAppDispatch } from '../../hooks/useAppDispatch';
import { useCommonContext } from '../../contexts/common';
import { KHACH_HANG_API_ENDPOINT, khachHangApi } from '../../api/category/khachHangApi';
import { rootAction } from '../../state/actions/rootAction';
import UnAuthorizedPage from '../../component-ui/un-authorized-page';
import DataTableRemotePaging from '../../component-ui/data-table';
import { eReducerStatusBase } from '../../state/reducer-models/eReducerStatusBase';
import { eSortMode } from '../../models/commons/eSortMode';
import Heading from '../../component-ui/heading';
import Button from '../../component-ui/button';
import KhachHangEditFormModal from './KhachHangEditFormModal';
import { IKhachHang } from '../../models/responses/category/IKhachHang';
import ConfirmModal from '../../component-ui/confirm-modal';
import ExportToExcelBtn from '../../component-data/export-excel-btn/ExportToExcelBtn';
import { NotifyHelper } from '../../helpers/toast';
import KhachHangImportButton from './KhachHangImportButton';

const KhachHangPage = () => {
    const { status, khachHangs, filter, paging_res, isShowDeleteConfirm,
        khachHangEditing,
        isShowEditModal } = useAppSelector(x => x.category.khachHangReducer)
    const dispatch = useAppDispatch();
    const { checkAccesiableTo } = useCommonContext();
    const isCanNotView = useMemo(() => {
        return !checkAccesiableTo(KHACH_HANG_API_ENDPOINT, "GET")
    }, [])
    const isCanNotEdit = useMemo(() => {
        return !checkAccesiableTo(KHACH_HANG_API_ENDPOINT, "PUT")
    }, [])
    const isCanNotDelete = useMemo(() => {
        return !checkAccesiableTo(KHACH_HANG_API_ENDPOINT + "/{id}", "DELETE")
    }, [])
    useEffect(() => {
        dispatch(rootAction.category.khachHangAction.loadStart({
            ...filter
        }))
    }, [filter])
    useEffect(() => {
        if (status === eReducerStatusBase.is_saved ||
            status === eReducerStatusBase.is_deleted
        ) {
            dispatch(rootAction.category.khachHangAction.loadStart({
                ...filter
            }))
        }
    }, [status, filter])
    return (
        <Box>
            <Helmet>
                <title>Khách hàng</title>
            </Helmet>
            {isCanNotView && <UnAuthorizedPage />}
            {!isCanNotView &&
                <DataTableRemotePaging
                    titleComponent={<Heading text='Danh sách khách hàng' />}
                    subTitle={`Tổng số: ${(paging_res?.total_count ?? 0).toLocaleString()}`}
                    data={khachHangs}
                    height={window.innerHeight - 100}
                    isLoading={status === eReducerStatusBase.is_loading}
                    exportEnable
                    actionComponent={<>
                        <Button text='Thêm khách hàng'
                            variant='primary'
                            leadingVisual={PlusIcon}
                            apiAuthorizedMethod='POST'
                            apiAuthorized={KHACH_HANG_API_ENDPOINT}
                            onClick={() => {
                                dispatch(rootAction.category.khachHangAction.showEditModal())
                            }}
                        />
                        <KhachHangImportButton onSuccess={() => {
                            dispatch(rootAction.category.khachHangAction.loadStart({
                                ...filter
                            }))
                        }} />
                        <ExportToExcelBtn
                            fileName="khach-hang"
                            formatDataFunction={(data) => {
                                return data.map((x: IKhachHang) => {
                                    return {
                                        "Mã số thuế": x.mst,
                                        "Tên đơn vị": x.ten_don_vi,
                                        "Tên người mua hàng": x.ten_khach_hang,
                                        "Địa chỉ": x.dia_chi,
                                        "Email": x.email,
                                        "Số tài khoản": x.stk
                                    }
                                })
                            }}
                            fetchDataPromise={() => {
                                return new Promise((resolve, reject) => {
                                    return khachHangApi.getByDonViPaging({
                                        ...filter,
                                        page_index: 0,
                                        page_size: paging_res?.total_count
                                    }).then(res => {
                                        if (res.is_success) {
                                            resolve(res.data.data);
                                        } else {
                                            NotifyHelper.Error(res.message ?? "Error")
                                            resolve(undefined);
                                        }
                                    })
                                });
                            }}
                        />
                    </>}
                    searchConfig={{
                        enable: true,
                        onValueChanged: (key: string) => {
                            dispatch(rootAction.category.khachHangAction.changeFilter({
                                ...filter,
                                page_index: 0,
                                search_key: key
                            }))
                        }
                    }}
                    sortConfig={{
                        enable: true,
                        field: filter.sort_by,
                        mode: filter.sort_mode ?? eSortMode.ASC,
                        onValueChanged: (key: string, sort_mode: eSortMode) => {
                            dispatch(rootAction.category.khachHangAction.changeFilter({
                                ...filter,
                                sort_by: key,
                                sort_mode: sort_mode
                            }))
                        }
                    }}
                    paging={{
                        onPageIndexChanged: (pageIndex) => {
                            dispatch(rootAction.category.khachHangAction.changeFilter({
                                ...filter,
                                page_index: pageIndex
                            }))
                        },
                        pageCount: paging_res?.page_count ?? 1,
                        pageIndex: paging_res?.page_number ?? 1,
                        pageSize: paging_res?.page_size ?? 1,
                        totalCount: paging_res?.total_count ?? 1
                    }}
                    columns={[
                        {
                            header: 'Mã số thuế',
                            field: 'mst',
                            rowHeader: false,
                            width: "100px"
                            // sortBy: "alphanumeric"
                        },
                        {
                            header: 'Đơn vị mua hàng',
                            field: 'ten_don_vi',
                            rowHeader: true,
                            width: "250px"
                            // sortBy: "alphanumeric"
                        },
                        {
                            header: 'Tên người mua hàng',
                            field: 'ten_khach_hang',
                            rowHeader: false,
                            width: "200px",
                            renderCell: (khachHang: IKhachHang) => {
                                return (
                                    <Box className="limit2Line" sx={{
                                        whiteSpace: "pre-line"
                                    }}>
                                        {khachHang.ten_khach_hang}
                                    </Box>
                                );
                            }
                        },
                        {
                            header: 'Địa chỉ',
                            field: 'dia_chi',
                            rowHeader: false,
                            maxWidth: "350px",
                            renderCell: (khachHang: IKhachHang) => {
                                return (
                                    <Box className="limit2Line" sx={{
                                        whiteSpace: "pre-line"
                                    }}>
                                        {khachHang.dia_chi}
                                    </Box>
                                );
                            }
                        },
                        {
                            header: 'Email',
                            field: 'email',
                            rowHeader: false,
                            width: "150px",
                            renderCell: (row: IKhachHang) => {
                                return <Truncate title={row.email} maxWidth={"150px"} >
                                    {row.email}
                                </Truncate>

                            }
                            // sortBy: "alphanumeric"
                        },
                        {
                            header: 'Số tài khoản',
                            field: 'stk',
                            rowHeader: false,
                            width: "150px",
                            renderCell: (khachHang: IKhachHang) => {
                                return (
                                    <Box className="limit2Line" sx={{
                                        whiteSpace: "pre-line"
                                    }}>
                                        {khachHang.stk}
                                    </Box>
                                );
                            }
                        },
                        {
                            header: 'Mã quan hệ ngân sách',
                            field: 'stk',
                            rowHeader: false,
                            width: "150px",
                            renderCell: (khachHang: IKhachHang) => {
                                return (
                                    <Box className="limit2Line" sx={{
                                        whiteSpace: "pre-line"
                                    }}>
                                        {khachHang.ma_dv_ngan_sach}
                                    </Box>
                                );
                            }
                        },
                        {
                            id: "actions",
                            header: "",
                            renderCell: (row: any) => {
                                return (
                                    <>
                                        <Box sx={{
                                            mt: -2,
                                            mb: -2
                                        }}>
                                            {!isCanNotEdit &&
                                                <IconButton
                                                    aria-label={`Edit: ${row.name}`}
                                                    title={`Edit: ${row.name}`}
                                                    icon={PencilIcon}
                                                    variant="invisible"
                                                    onClick={() => {
                                                        dispatch(rootAction.category.khachHangAction.showEditModal(row))
                                                    }}
                                                />
                                            }
                                            {(!isCanNotDelete) &&
                                                <IconButton
                                                    aria-label={`Edit: ${row.name}`}
                                                    title={`Edit: ${row.name}`}
                                                    icon={TrashIcon}
                                                    variant="invisible"
                                                    onClick={() => {
                                                        dispatch(rootAction.category.khachHangAction.showDeleteConfirm(row))
                                                    }}
                                                />
                                            }

                                        </Box>
                                    </>
                                )
                            }
                        }
                    ]}
                />
            }
            {isShowEditModal &&
                <KhachHangEditFormModal />
            }
            {isShowDeleteConfirm && khachHangEditing &&
                <ConfirmModal
                    onCancel={() => {
                        dispatch(rootAction.category.khachHangAction.closeDeleteConfirm())
                    }}
                    type="danger"
                    title="Xóa khách hàng"
                    text="Bạn có chắc chắn muốn xóa khách hàng này?"
                    isSaving={status == eReducerStatusBase.is_deleting}
                    onConfirm={() => {
                        dispatch(rootAction.category.khachHangAction.deleteStart(khachHangEditing?.id ?? 0))
                    }}
                />
            }
        </Box>
    );
};

export default KhachHangPage;