import { useEffect, useMemo } from 'react';
import { Helmet } from "react-helmet";
import { LOG_API_ENDPOINT } from "../../api/user/logApi";
import DataTableRemotePaging from '../../component-ui/data-table';
import Heading from "../../component-ui/heading";
import UnAuthorizedPage from "../../component-ui/un-authorized-page";
import { useCommonContext } from "../../contexts/common";
import { useAppDispatch } from '../../hooks/useAppDispatch';
import { useAppSelector } from '../../hooks/useAppSelector';
import { useAuth } from "../../hooks/useAuth";
import { eSortMode } from "../../models/commons/eSortMode";
import { rootAction } from '../../state/actions/rootAction';
import { eReducerStatusBase } from '../../state/reducer-models/eReducerStatusBase';
import { ILog } from '../../models/responses/user/ILog';
import { RelativeTime } from '@primer/react';
// import {VisuallyHidden} from "@primer/react/drafts"
const LogPage = () => {
    const { status, logs, filter, paging_res } = useAppSelector(x => x.user.logReducer)

    const { user } = useAuth();
    const dispatch = useAppDispatch();
    const { checkAccesiableTo } = useCommonContext();

    const isCanNotView = useMemo(() => {
        return !checkAccesiableTo(LOG_API_ENDPOINT, "GET")
    }, [])

    useEffect(() => {
        dispatch(rootAction.user.logAction.loadStart({
            ...filter
        }))
    }, [filter])

    return (
        <div>
            <Helmet>
                <title>Nhật ký hoạt động</title>
            </Helmet>
            {isCanNotView && <UnAuthorizedPage />}
            {!isCanNotView &&
                <DataTableRemotePaging
                    titleComponent={<Heading text="Lịch sử" />}
                    subTitle={`Tổng số: ${(paging_res?.total_count ?? 0).toLocaleString()}`}
                    data={logs}
                    height={window.innerHeight - 100}
                    isLoading={status == eReducerStatusBase.is_loading}
                    exportEnable
                    searchConfig={{
                        enable: true,
                        onValueChanged: (key: string) => {
                            dispatch(rootAction.user.logAction.changeFilter({
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
                            dispatch(rootAction.user.logAction.changeFilter({
                                ...filter,
                                sort_by: key,
                                sort_mode: sort_mode
                            }))
                        }
                    }}
                    paging={{
                        onPageIndexChanged: (pageIndex) => {
                            dispatch(rootAction.user.logAction.changeFilter({
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
                            header: 'Mã',
                            field: 'donvi_ma_dv',
                            rowHeader: false,
                        },
                        {
                            header: 'Username',
                            field: 'username',
                            rowHeader: true,
                        },
                        {
                            header: 'Nội dung',
                            field: 'content',
                            rowHeader: false,
                        },
                        {
                            header: 'Thời điểm',
                            field: 'created_at',
                            rowHeader: false,
                            renderCell:(data:ILog)=>{
                                return(
                                    <RelativeTime datetime={data.created_at}></RelativeTime>
                                )
                            }
                        },
                        {
                            header: 'IP',
                            field: 'ip',
                            rowHeader: false,
                        },
                    ]}
                />
            }

        </div>
    );
};

export default LogPage;