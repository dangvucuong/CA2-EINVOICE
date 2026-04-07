import { IPagingRequest } from "../../../models/requests/IPagingRequest";
import { IPagingResultSummary } from "../../../models/responses/IBasePagingRespone";
import { IKhachHang } from "../../../models/responses/category/IKhachHang";
import { eReducerStatusBase } from "../eReducerStatusBase";

export interface IKhachHangReducer {
    status: eReducerStatusBase,
    khachHangs: IKhachHang[],
    khachHangEditing?: IKhachHang,
    isShowEditModal?: boolean,
    isShowDeleteConfirm?: boolean,
    khachHangSelectedId?: number,
    paging_res?: IPagingResultSummary,
    filter: IPagingRequest,


}