import { IPagingRequest } from "../../../models/requests/IPagingRequest";
import { IPagingResultSummary } from "../../../models/responses/IBasePagingRespone";
import { ILog } from "../../../models/responses/user/ILog";
import { eReducerStatusBase } from "../eReducerStatusBase";

export interface ILogReducer {
    status: eReducerStatusBase,
    logs: ILog[],
    filter: IPagingRequest,
    paging_res?: IPagingResultSummary,
}