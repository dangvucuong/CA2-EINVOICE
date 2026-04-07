import { PencilIcon, PlusIcon, TrashIcon } from "@primer/octicons-react";
import { Box, IconButton } from '@primer/react';
import { useEffect, useMemo } from 'react';
import { Helmet } from 'react-helmet';
import { DONVI_API_ENDPOINT } from '../../api/category/donViApi';
import Button from '../../component-ui/button';
import ConfirmModal from '../../component-ui/confirm-modal';
import DataTableRemotePaging from '../../component-ui/data-table';
import Heading from '../../component-ui/heading';
import UnAuthorizedPage from '../../component-ui/un-authorized-page';
import { useCommonContext } from '../../contexts/common';
import { useAppDispatch } from '../../hooks/useAppDispatch';
import { useAppSelector } from '../../hooks/useAppSelector';
import { eSortMode } from '../../models/commons/eSortMode';
import { rootAction } from '../../state/actions/rootAction';
import { eReducerStatusBase } from '../../state/reducer-models/eReducerStatusBase';
import HangHoaEditFormModal from "./DonViEditFormModal";

const HangHoaPage = () => {
    const { status, donVis, filter, paging_res, isShowDeleteConfirm,
        donViEditing,
        isShowEditModal } = useAppSelector(x => x.category.donViReducer)
    const dispatch = useAppDispatch();
    const { checkAccesiableTo } = useCommonContext();
    const isCanNotView = useMemo(() => {
        return !checkAccesiableTo(DONVI_API_ENDPOINT, "GET")
    }, [])
    const isCanNotEdit = useMemo(() => {
        return !checkAccesiableTo(DONVI_API_ENDPOINT, "PUT")
    }, [])
    const isCanNotDelete = useMemo(() => {
        return !checkAccesiableTo(DONVI_API_ENDPOINT + "/{id}", "DELETE")
    }, [])
    // useEffect(() => {
    //     dispatch(rootAction.category.donViActionType.loadStart({
    //         ...filter
    //     }))
    // }, [filter])
    useEffect(() => {
        if (status === eReducerStatusBase.is_not_initialization ||
            status === eReducerStatusBase.is_need_reload
        ) {
            dispatch(rootAction.category.donViActionType.loadStart({
                ...filter
            }))
        }
    }, [status, filter])
    return (
        <Box>
            <Helmet>
                <title>Đơn vị</title>
            </Helmet>
            {isCanNotView && <UnAuthorizedPage />}
            {!isCanNotView &&
                <DataTableRemotePaging
                    titleComponent={<Heading text='Danh sách đơn vị' />}
                    subTitle={`Tổng số: ${(paging_res?.total_count ?? 0).toLocaleString()}`}
                    data={donVis}
                    height={window.innerHeight - 100}
                    isLoading={status === eReducerStatusBase.is_loading}
                    exportEnable
                    actionComponent={<>
                        <Button text='Thêm đơn vị'
                            variant='primary'
                            leadingVisual={PlusIcon}
                            apiAuthorizedMethod='POST'
                            apiAuthorized={DONVI_API_ENDPOINT}
                            onClick={() => {
                                dispatch(rootAction.category.donViActionType.showEditModal())
                            }}
                        />
                    </>}
                    searchConfig={{
                        enable: true,
                        onValueChanged: (key: string) => {
                            dispatch(rootAction.category.donViActionType.changeFilter({
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
                            dispatch(rootAction.category.donViActionType.changeFilter({
                                ...filter,
                                sort_by: key,
                                sort_mode: sort_mode
                            }))
                        }
                    }}
                    paging={{
                        onPageIndexChanged: (pageIndex) => {
                            dispatch(rootAction.category.donViActionType.changeFilter({
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
                            header: 'MST',
                            field: 'ma_dv',
                            rowHeader: false,
                            width: "150px"
                            // sortBy: "alphanumeric"
                        },
                        {
                            header: 'Tên đơn vị',
                            field: 'ten_dv',
                            rowHeader: true,
                            minWidth:"300px"
                            // sortBy: "alphanumeric"
                        },

                        {
                            header: 'Email',
                            field: 'email',
                            rowHeader: false,
                            width: "300px",
                            // sortBy: "alphanumeric"
                        },
                        {
                            header: 'Điện thoại',
                            field: 'dien_thoai',
                            rowHeader: false,
                            width: "150px",
                            // sortBy: "alphanumeric"
                        },

                        {
                            id: "actions",
                            header: "",
                            width: "100px",
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
                                                        dispatch(rootAction.category.donViActionType.showEditModal(row))
                                                    }}
                                                />
                                            }
                                            {(!isCanNotDelete) &&
                                                <IconButton
                                                    aria-label={`Delete: ${row.name}`}
                                                    title={`Delete: ${row.name}`}
                                                    icon={TrashIcon}
                                                    variant="invisible"
                                                    onClick={() => {
                                                        dispatch(rootAction.category.donViActionType.showDeleteConfirm(row))
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
                <HangHoaEditFormModal />
            }
            {isShowDeleteConfirm && donViEditing &&
                <ConfirmModal
                    onCancel={() => {
                        dispatch(rootAction.category.donViActionType.closeDeleteConfirm())
                    }}
                    type="danger"
                    title="Xóa đơn vị"
                    text="Bạn có chắc chắn muốn xóa đơn vị này?"
                    isSaving={status == eReducerStatusBase.is_deleting}
                    onConfirm={() => {
                        dispatch(rootAction.category.donViActionType.deleteStart(donViEditing?.id ?? 0))
                    }}
                />
            }

        </Box>
    );
};

export default HangHoaPage;