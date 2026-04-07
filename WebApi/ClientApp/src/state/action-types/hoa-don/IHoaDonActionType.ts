import { IIHoaDonAddOrEditModel } from "../../../models/requests/hoa-don/IHoaDonAddOrEditModel";
import { IHoaDonSelectPagingRequest } from "../../../models/requests/hoa-don/IHoaDonSelectPagingRequest";
import { IHoaDon } from "../../../models/responses/hoa-don/IHoaDon";
import { IHoaDonPaging } from "../../../models/responses/hoa-don/IHoaDonPaging";
import { IActionTypeBase } from "../IActionTypeBase";

export enum eHoaDonActionTypeIds {
    LOAD_START = "HOADON_LOAD_START",
    LOAD_SUCCESS = "HOADON_LOAD_SUCCESS",
    LOAD_ERROR = "HOADON_LOAD_ERROR",

    CHANGE_SELECTED_ID = "HOADON_CHANGE_SELECTED_ID",
    CHANGE_FILTER = "HOADON_CHANGE_FILTER",


    SHOW_EDIT_MODAL = "HOADON_SHOW_EDIT_MODAL",
    CLOSE_EDIT_MODAL = "HOADON_CLOSE_EDIT_MODAL",

    SAVE_START = "HOADON_SAVE_START",
    SAVE_SUCCESS = "HOADON_SAVE_SUCCESS",
    SAVE_ERROR = "HOADON_SAVE_ERROR",

    SHOW_DELETE_CONFIRM = "HOADON_SHOW_DELETE_CONFIRM",
    CLOSE_DELETE_CONFIRM = "HOADON_CLOSE_DELETE_CONFIRM",

    DELETE_START = "HOADON_DELETE_START",
    DELETE_SUCCESS = "HOADON_DELETE_SUCCESS",
    DELETE_ERROR = "HOADON_DELETE_ERROR",



    SHOW_LOG_MODAL = "TOKHAI_SHOW_LOG_MODAL",
    CLOSE_LOG_MODAL = "TOKHAI_CLOSE_LOG_MODAL",

}

export interface IHoaDonLoadStart extends IActionTypeBase<eHoaDonActionTypeIds.LOAD_START, IHoaDonSelectPagingRequest> { }
export interface IHoaDonLoadSuccess extends IActionTypeBase<eHoaDonActionTypeIds.LOAD_SUCCESS, IHoaDonPaging> { }
export interface IHoaDonLoadError extends IActionTypeBase<eHoaDonActionTypeIds.LOAD_ERROR, string> { }

export interface IHoaDonShowEditModal extends IActionTypeBase<eHoaDonActionTypeIds.SHOW_EDIT_MODAL, IHoaDon | undefined> { }
export interface IHoaDonCloseEditModal extends IActionTypeBase<eHoaDonActionTypeIds.CLOSE_EDIT_MODAL, undefined> { }

export interface IHoaDonSaveStart extends IActionTypeBase<eHoaDonActionTypeIds.SAVE_START, IIHoaDonAddOrEditModel> { }
export interface IHoaDonSaveSuccess extends IActionTypeBase<eHoaDonActionTypeIds.SAVE_SUCCESS, IIHoaDonAddOrEditModel> { }
export interface IHoaDonSaveError extends IActionTypeBase<eHoaDonActionTypeIds.SAVE_ERROR, string> { }

export interface IHoaDonShowDeleteConfirm extends IActionTypeBase<eHoaDonActionTypeIds.SHOW_DELETE_CONFIRM, IHoaDon> { }
export interface IHoaDonCloseDeleteConfirm extends IActionTypeBase<eHoaDonActionTypeIds.CLOSE_DELETE_CONFIRM, undefined> { }


export interface IHoaDonDeleteStart extends IActionTypeBase<eHoaDonActionTypeIds.DELETE_START, number> { }
export interface IHoaDonDeleteSuccess extends IActionTypeBase<eHoaDonActionTypeIds.DELETE_SUCCESS, undefined> { }
export interface IHoaDonDeleteError extends IActionTypeBase<eHoaDonActionTypeIds.DELETE_ERROR, string> { }

export interface IHoaDonChangeSelectedIds extends IActionTypeBase<eHoaDonActionTypeIds.CHANGE_SELECTED_ID, number[]> { }
export interface IHoaDonChangeFilter extends IActionTypeBase<eHoaDonActionTypeIds.CHANGE_FILTER, IHoaDonSelectPagingRequest> { }


export interface IHoaDonShowLogModal extends IActionTypeBase<eHoaDonActionTypeIds.SHOW_LOG_MODAL, IHoaDon> { }
export interface IHoaDonCloseLogModal extends IActionTypeBase<eHoaDonActionTypeIds.CLOSE_LOG_MODAL, undefined> { }

export type IHoaDonActionType = IHoaDonLoadStart | IHoaDonLoadSuccess | IHoaDonLoadError |
    IHoaDonShowEditModal | IHoaDonCloseEditModal |
    IHoaDonSaveStart | IHoaDonSaveSuccess | IHoaDonSaveError |
    IHoaDonDeleteStart | IHoaDonDeleteSuccess | IHoaDonDeleteError |
    IHoaDonShowDeleteConfirm | IHoaDonCloseDeleteConfirm |
    IHoaDonChangeSelectedIds | IHoaDonChangeFilter|
    IHoaDonShowLogModal| IHoaDonCloseLogModal