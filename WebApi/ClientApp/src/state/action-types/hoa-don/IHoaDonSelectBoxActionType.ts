import { IHoaDonSelectPagingRequest } from "../../../models/requests/hoa-don/IHoaDonSelectPagingRequest";
import { IHoaDonPaging } from "../../../models/responses/hoa-don/IHoaDonPaging";
import { IActionTypeBase } from "../IActionTypeBase";

export enum eHoaDonSelectBoxActionTypeIds {
    LOAD_START = "HOADON_SELECTBOX_LOAD_START",
    LOAD_SUCCESS = "HOADON_SELECTBOX_LOAD_SUCCESS",
    LOAD_ERROR = "HOADON_SELECTBOX_LOAD_ERROR",

    CHANGE_SELECTED_ID = "HOADON_SELECTBOX_CHANGE_SELECTED_ID",
    CHANGE_FILTER = "HOADON_SELECTBOX_CHANGE_FILTER",



}

export interface IHoaDonSelectBoxLoadStart extends IActionTypeBase<eHoaDonSelectBoxActionTypeIds.LOAD_START, IHoaDonSelectPagingRequest> { }
export interface IHoaDonSelectBoxLoadSuccess extends IActionTypeBase<eHoaDonSelectBoxActionTypeIds.LOAD_SUCCESS, IHoaDonPaging> { }
export interface IHoaDonSelectBoxLoadError extends IActionTypeBase<eHoaDonSelectBoxActionTypeIds.LOAD_ERROR, string> { }


export interface IHoaDonSelectBoxChangeSelectedIds extends IActionTypeBase<eHoaDonSelectBoxActionTypeIds.CHANGE_SELECTED_ID, number[]> { }
export interface IHoaDonSelectBoxChangeFilter extends IActionTypeBase<eHoaDonSelectBoxActionTypeIds.CHANGE_FILTER, IHoaDonSelectPagingRequest> { }


export type IHoaDonSelectBoxActionType = IHoaDonSelectBoxLoadStart | IHoaDonSelectBoxLoadSuccess | IHoaDonSelectBoxLoadError |
    IHoaDonSelectBoxChangeSelectedIds | IHoaDonSelectBoxChangeFilter
