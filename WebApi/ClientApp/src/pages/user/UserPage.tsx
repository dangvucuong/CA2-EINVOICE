import { PencilIcon, PlusIcon } from "@primer/octicons-react";
import { Box, IconButton } from '@primer/react';
import { useEffect, useMemo } from 'react';
import { Helmet } from "react-helmet";
import { USER_API_ENDPOINT } from "../../api/user/userApi";
import Button from "../../component-ui/button";
import DataTableRemotePaging from '../../component-ui/data-table';
import Heading from "../../component-ui/heading";
import UnAuthorizedPage from "../../component-ui/un-authorized-page";
import { useCommonContext } from "../../contexts/common";
import { useAppDispatch } from '../../hooks/useAppDispatch';
import { useAppSelector } from '../../hooks/useAppSelector';
import { eSortMode } from "../../models/commons/eSortMode";
import { rootAction } from '../../state/actions/rootAction';
import { eReducerStatusBase } from '../../state/reducer-models/eReducerStatusBase';
import UserEditFormModal from "./UserEditFormModal";
import { useAuth } from "../../hooks/useAuth";
// import {VisuallyHidden} from "@primer/react/drafts"
const UserPage = () => {
    const { status, users, filter, paging_res, userEditing, userEditingForm,
        isShowEditModal, isShowDeleteConfirm
    } = useAppSelector(x => x.user.userReducer)

    const { user } = useAuth();
    const dispatch = useAppDispatch();
    const userEditingId = useMemo(() => { return userEditing?.id ?? 0 }, [userEditing])
    const { checkAccesiableTo } = useCommonContext();
    const isCanViewAll = useMemo(() => {
        return checkAccesiableTo(USER_API_ENDPOINT, "GET")
    }, [])
    const isCanNotView = useMemo(() => {
        return (!checkAccesiableTo(USER_API_ENDPOINT, "GET") && !checkAccesiableTo(USER_API_ENDPOINT + "/don-vi/{donvi_ma_dv}", "GET"))
    }, [])
    const isCanNotEdit = useMemo(() => {
        return !checkAccesiableTo(USER_API_ENDPOINT, "PUT")
    }, [])
    useEffect(() => {
        dispatch(rootAction.user.userAction.loadStart({
            ...filter
        }))
        // if (isCanViewAll) {
        //     dispatch(rootAction.user.userAction.loadStart({
        //         ...filter
        //     }))
        // } else {
        //     dispatch(rootAction.user.userAction.loadByDonViStart({
        //         donvi_ma_dv: user?.donvi_ma_dv ?? "",
        //         ...filter
        //     }))
        // }
    }, [filter, isCanViewAll])
    useEffect(() => {
        if (status === eReducerStatusBase.is_saved || status === eReducerStatusBase.is_deleted) {
            dispatch(rootAction.user.userAction.loadStart({
                ...filter
            }))
        }
    }, [status, filter])
    useEffect(() => {
        if (userEditingId > 0) {
            dispatch(rootAction.user.userAction.loadFormStart(userEditingId))
        }

    }, [userEditingId])


    return (
        <div>
            <Helmet>
                <title>Users</title>
            </Helmet>
            {isCanNotView && <UnAuthorizedPage />}
            {!isCanNotView &&
                <DataTableRemotePaging
                    titleComponent={<Heading text="Người dùng" />}
                    subTitle={`Tổng số: ${(paging_res?.total_count ?? 0).toLocaleString()}`}
                    data={users}
                    height={window.innerHeight - 100}
                    isLoading={status == eReducerStatusBase.is_loading}
                    exportEnable
                    actionComponent={<Button
                        text="Thêm người dùng"
                        variant="primary"
                        leadingVisual={PlusIcon}
                        apiAuthorized={USER_API_ENDPOINT}
                        apiAuthorizedMethod="POST"
                        onClick={() => {
                            dispatch(rootAction.user.userAction.showEditModal())
                        }}
                    />}
                    searchConfig={{
                        enable: true,
                        onValueChanged: (key: string) => {
                            dispatch(rootAction.user.userAction.changeFilter({
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
                            dispatch(rootAction.user.userAction.changeFilter({
                                ...filter,
                                sort_by: key,
                                sort_mode: sort_mode
                            }))
                        }
                    }}
                    paging={{
                        onPageIndexChanged: (pageIndex) => {
                            dispatch(rootAction.user.userAction.changeFilter({
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
                            header: 'Username',
                            field: 'username',
                            rowHeader: false,
                            // sortBy: "alphanumeric"
                        },
                        {
                            header: 'Mã',
                            field: 'donvi_ma_dv',
                            rowHeader: false,
                            // sortBy: "alphanumeric"
                        },
                        {
                            header: 'Serial',
                            field: 'serial_number',
                            rowHeader: false,
                            // sortBy: "alphanumeric"
                        },
                        {
                            header: 'Full name',
                            field: 'full_name',
                            rowHeader: true,
                            // sortBy: "alphanumeric"
                        },
                        {
                            header: 'Email',
                            field: 'email',
                            rowHeader: false,
                            // sortBy: "alphanumeric"
                        },

                        {
                            id: "actions",
                            header: "",
                            renderCell: (row: any) => {
                                return (
                                    <>
                                        {!isCanNotEdit &&
                                            <Box sx={{
                                                mt: -2,
                                                mb: -2
                                            }}>
                                                <IconButton
                                                    aria-label={`Edit: ${row.name}`}
                                                    title={`Edit: ${row.name}`}
                                                    icon={PencilIcon}
                                                    variant="invisible"
                                                    onClick={() => {
                                                        dispatch(rootAction.user.userAction.showEditModal(row))
                                                    }}
                                                />

                                            </Box>
                                        }
                                    </>
                                )
                            }
                        }
                    ]}
                />
            }
            {isShowEditModal && (userEditingId === 0 || userEditingForm?.id === userEditingId) &&
                < UserEditFormModal />
            }
        </div>
    );
};

export default UserPage;