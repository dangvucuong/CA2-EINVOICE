import { IHoaDonSelectPagingRequest } from "../../../models/requests/hoa-don/IHoaDonSelectPagingRequest";
import { IPagingResultSummary } from "../../../models/responses/IBasePagingRespone";
import { IHoaDon } from "../../../models/responses/hoa-don/IHoaDon";
import { eReducerStatusBase } from "../eReducerStatusBase";

export interface IHoaDonSelectBoxReducer {
    status: eReducerStatusBase,
    hoaDons: IHoaDon[],
    hoaDonSelectedIds?: number[],
    paging_res?: IPagingResultSummary,
    filter: IHoaDonSelectPagingRequest,
}