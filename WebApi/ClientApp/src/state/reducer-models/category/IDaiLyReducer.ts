import { IPagingRequest } from "../../../models/requests/IPagingRequest";
import { IPagingResultSummary } from "../../../models/responses/IBasePagingRespone";
import { IDaiLy } from "../../../models/responses/category/IDaiLy";
import { eReducerStatusBase } from "../eReducerStatusBase";

export interface IDaiLyReducer {
    status: eReducerStatusBase,
    daiLys: IDaiLy[],
    daiLyEditing?: IDaiLy,
    isShowEditModal?: boolean,
    isShowDeleteConfirm?: boolean,
    daiLySelectedId?: number,
    paging_res?: IPagingResultSummary,
    filter: IPagingRequest,


}