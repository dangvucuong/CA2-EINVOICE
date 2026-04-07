import { IPagingRequest } from "../../../models/requests/IPagingRequest";
import { IPagingResultSummary } from "../../../models/responses/IBasePagingRespone";
import { IHangHoa } from "../../../models/responses/category/IHangHoa";
import { eReducerStatusBase } from "../eReducerStatusBase";

export interface IHangHoaReducer {
    status: eReducerStatusBase,
    hangHoas: IHangHoa[],
    hangHoaEditing?: IHangHoa,
    isShowEditModal?: boolean,
    isShowDeleteConfirm?: boolean,
    hangHoaSelectedId?: number,
    paging_res?: IPagingResultSummary,
    filter: IPagingRequest,


}