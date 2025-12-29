import { PencilIcon } from "@primer/octicons-react";
import { Box, IconButton, Truncate } from '@primer/react';
import moment from "moment";
import { useEffect, useMemo } from 'react';
import { Helmet } from 'react-helmet';
import { CONTACT_API_ENDPOINT } from '../../api/contact/contactApi';
import DataTableRemotePaging from '../../component-ui/data-table';
import Heading from '../../component-ui/heading';
import UnAuthorizedPage from '../../component-ui/un-authorized-page';
import { useCommonContext } from '../../contexts/common';
import { useAppDispatch } from '../../hooks/useAppDispatch';
import { useAppSelector } from '../../hooks/useAppSelector';
import { eSortMode } from '../../models/commons/eSortMode';
import { IContact } from '../../models/responses/contact/IContact';
import { rootAction } from '../../state/actions/rootAction';
import { eReducerStatusBase } from '../../state/reducer-models/eReducerStatusBase';
import ContactStatus from "../../component-ui/contact-status";
import SelectBoxContactStatus from "../../component-data/selectbox-contact-status";
import ContactEditFormModal from "./ContactEditFormModal";
import { Table } from "@primer/react/lib-esm/drafts";

const ContactPage = () => {
    const { status, contacts, filter, paging_res, isShowDeleteConfirm,
        contactEditing,
        isShowEditModal } = useAppSelector(x => x.contact.contactReducer)
    const dispatch = useAppDispatch();
    const { checkAccesiableTo, createUUID } = useCommonContext();
    const isCanNotView = useMemo(() => {
        return !checkAccesiableTo(CONTACT_API_ENDPOINT, "GET")
    }, [])
    const isCanNotEdit = useMemo(() => {
        return !checkAccesiableTo(CONTACT_API_ENDPOINT, "PUT")
    }, [])

    useEffect(() => {
        dispatch(rootAction.contact.contactAction.loadStart({
            ...filter
        }))
    }, [filter])
    useEffect(() => {
        if (status === eReducerStatusBase.is_saved) {
            dispatch(rootAction.contact.contactAction.loadStart({
                ...filter
            }))
            dispatch(rootAction.notify.notifyAction.loadSummaryStart())
        }
    }, [status, filter])

    return (
        <Box>
            <Helmet>
                <title>Đăng ký</title>
            </Helmet>
            {isCanNotView && <UnAuthorizedPage />}
            {!isCanNotView &&
                <DataTableRemotePaging
                    titleComponent={<Heading text='Danh sách đăng ký' />}
                    subTitle={`Tổng số: ${(paging_res?.total_count ?? 0).toLocaleString()}`}
                    data={contacts}
                    height={window.innerHeight - 100}
                    isLoading={status === eReducerStatusBase.is_loading}
                    exportEnable
                    actionComponent={<>
                        <SelectBoxContactStatus
                            onValueChanged={(value) => {
                                console.log({
                                    value
                                });

                                dispatch(rootAction.contact.contactAction.changeFilter({
                                    ...filter,
                                    contact_status_id: value
                                }))
                            }}
                            value={filter?.contact_status_id ?? 0}
                        />
                    </>}
                    searchConfig={{
                        enable: true,
                        onValueChanged: (key: string) => {
                            dispatch(rootAction.contact.contactAction.changeFilter({
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
                            dispatch(rootAction.contact.contactAction.changeFilter({
                                ...filter,
                                sort_by: key,
                                sort_mode: sort_mode
                            }))
                        }
                    }}
                    paging={{
                        onPageIndexChanged: (pageIndex) => {
                            dispatch(rootAction.contact.contactAction.changeFilter({
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
                            header: 'Tên',
                            field: 'name',
                            rowHeader: false,
                            minWidth: "250px",
                            renderCell: (khachHang: IContact) => {
                                return (
                                    <Box>
                                        <Box className="limit2Line" sx={{
                                            whiteSpace: "pre-line"
                                        }}>

                                            {khachHang.name}
                                        </Box>
                                    </Box>

                                );
                            }
                        },
                        {
                            header: 'Status',
                            field: 'contact_status_id',
                            rowHeader: true,
                            width: "100px",
                            renderCell: (khachHang: IContact) => {
                                return (
                                    <ContactStatus id={khachHang.contact_status_id} />
                                );
                            }
                        },
                        {
                            header: 'Email',
                            field: 'email',
                            width: "150px",
                            renderCell: (row: IContact) => {
                                return <Truncate title={row.email} maxWidth={"150px"} >
                                    {row.email}
                                </Truncate>

                            }
                        },

                        {
                            header: 'Điện thoại',
                            field: 'phone',
                            rowHeader: false,
                            width: "100px",
                            // sortBy: "alphanumeric"
                        },
                        {
                            header: 'Mã số thuế',
                            field: 'tax_code',
                            rowHeader: false,
                            width: "150px",
                            // sortBy: "alphanumeric"
                        },
                        {
                            header: 'Địa chỉ',
                            field: 'address',
                            rowHeader: false,
                            maxWidth: "350px",
                            renderCell: (khachHang: IContact) => {
                                return (
                                    <Box className="limit2Line" sx={{
                                        whiteSpace: "pre-line"
                                    }}>
                                        {khachHang.address}
                                    </Box>
                                );
                            }
                        },
                        {
                            header: 'Ngày tạo',
                            field: 'register_at',
                            rowHeader: false,
                            width: "100px",
                            renderCell: (contact: IContact) => {
                                return (
                                    <Box>
                                        {moment(contact.register_at).format("DD/MM/YYYY")}
                                    </Box>
                                );
                            }
                            // sortBy: "alphanumeric"
                        },

                        {
                            id: "actions",
                            header: "",
                            width: "50px",
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
                                                        dispatch(rootAction.contact.contactAction.showEditModal(row))
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
                <ContactEditFormModal />
            }

        </Box>
    );
};

export default ContactPage;