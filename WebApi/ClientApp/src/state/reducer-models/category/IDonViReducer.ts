import { IPagingRequest } from "../../../models/requests/IPagingRequest";
import { IPagingResultSummary } from "../../../models/responses/IBasePagingRespone";
import { IDonVi } from "../../../models/responses/category/IDonVi";
import { eReducerStatusBase } from "../eReducerStatusBase";

export interface IDonViReducer {
    status: eReducerStatusBase,
    donVis: IDonVi[],
    donViEditing?: IDonVi,
    isShowEditModal?: boolean,
    isShowDeleteConfirm?: boolean,
    donViSelectedId?: number,
    paging_res?: IPagingResultSummary,
    filter: IPagingRequest,


}