import { IHoaDonSelectPagingRequest } from "../../../models/requests/hoa-don/IHoaDonSelectPagingRequest";
import { IPagingResultSummary } from "../../../models/responses/IBasePagingRespone";
import { IHoaDon } from "../../../models/responses/hoa-don/IHoaDon";
import { eReducerStatusBase } from "../eReducerStatusBase";

export interface IHoaDonReducer {
    status: eReducerStatusBase,
    hoaDons: IHoaDon[],
    hoaDonEditing?: IHoaDon,
    isShowEditModal?: boolean,
    isShowDeleteConfirm?: boolean,
    hoaDonSelectedIds?: number[],
    paging_res?: IPagingResultSummary,
    filter: IHoaDonSelectPagingRequest,
    isShowLogModal?: boolean


}