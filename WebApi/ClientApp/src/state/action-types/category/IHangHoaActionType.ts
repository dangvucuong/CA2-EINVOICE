import { IPagingRequest } from "../../../models/requests/IPagingRequest";
import { IHangHoa } from "../../../models/responses/category/IHangHoa";
import { IHangHoaPaging } from "../../../models/responses/category/IHangHoaPaging";
import { IActionTypeBase } from "../IActionTypeBase";

export enum eHangHoaActionTypeIds {
    LOAD_START = "HANGHOA_LOAD_START",
    LOAD_SUCCESS = "HANGHOA_LOAD_SUCCESS",
    LOAD_ERROR = "HANGHOA_LOAD_ERROR",

    CHANGE_SELECTED_ID = "HANGHOA_CHANGE_SELECTED_ID",
    CHANGE_FILTER = "HANGHOA_CHANGE_FILTER",


    SHOW_EDIT_MODAL = "HANGHOA_SHOW_EDIT_MODAL",
    CLOSE_EDIT_MODAL = "HANGHOA_CLOSE_EDIT_MODAL",

    SAVE_START = "HANGHOA_SAVE_START",
    SAVE_SUCCESS = "HANGHOA_SAVE_SUCCESS",
    SAVE_ERROR = "HANGHOA_SAVE_ERROR",

    SHOW_DELETE_CONFIRM = "HANGHOA_SHOW_DELETE_CONFIRM",
    CLOSE_DELETE_CONFIRM = "HANGHOA_CLOSE_DELETE_CONFIRM",

    DELETE_START = "HANGHOA_DELETE_START",
    DELETE_SUCCESS = "HANGHOA_DELETE_SUCCESS",
    DELETE_ERROR = "HANGHOA_DELETE_ERROR",

}

 export interface IHangHoaLoadStart extends IActionTypeBase<eHangHoaActionTypeIds.LOAD_START, IPagingRequest> { }

export interface IHangHoaLoadSuccess extends IActionTypeBase<eHangHoaActionTypeIds.LOAD_SUCCESS, IHangHoaPaging> { }
export interface IHangHoaLoadError extends IActionTypeBase<eHangHoaActionTypeIds.LOAD_ERROR, string> { }

export interface IHangHoaShowEditModal extends IActionTypeBase<eHangHoaActionTypeIds.SHOW_EDIT_MODAL, IHangHoa | undefined> { }
export interface IHangHoaCloseEditModal extends IActionTypeBase<eHangHoaActionTypeIds.CLOSE_EDIT_MODAL, undefined> { }

export interface IHangHoaSaveStart extends IActionTypeBase<eHangHoaActionTypeIds.SAVE_START, IHangHoa> { }
export interface IHangHoaSaveSuccess extends IActionTypeBase<eHangHoaActionTypeIds.SAVE_SUCCESS, IHangHoa> { }
export interface IHangHoaSaveError extends IActionTypeBase<eHangHoaActionTypeIds.SAVE_ERROR, string> { }

export interface IHangHoaShowDeleteConfirm extends IActionTypeBase<eHangHoaActionTypeIds.SHOW_DELETE_CONFIRM, IHangHoa> { }
export interface IHangHoaCloseDeleteConfirm extends IActionTypeBase<eHangHoaActionTypeIds.CLOSE_DELETE_CONFIRM, undefined> { }


export interface IHangHoaDeleteStart extends IActionTypeBase<eHangHoaActionTypeIds.DELETE_START, number> { }
export interface IHangHoaDeleteSuccess extends IActionTypeBase<eHangHoaActionTypeIds.DELETE_SUCCESS, undefined> { }
export interface IHangHoaDeleteError extends IActionTypeBase<eHangHoaActionTypeIds.DELETE_ERROR, string> { }

export interface IHangHoaChangeSelectedId extends IActionTypeBase<eHangHoaActionTypeIds.CHANGE_SELECTED_ID, number> { }
export interface IHangHoaChangeFilter extends IActionTypeBase<eHangHoaActionTypeIds.CHANGE_FILTER, IPagingRequest> { }


export type IHangHoaActionType = IHangHoaLoadStart | IHangHoaLoadSuccess | IHangHoaLoadError |
    IHangHoaShowEditModal | IHangHoaCloseEditModal |
    IHangHoaSaveStart | IHangHoaSaveSuccess | IHangHoaSaveError |
    IHangHoaDeleteStart | IHangHoaDeleteSuccess | IHangHoaDeleteError |
    IHangHoaShowDeleteConfirm | IHangHoaCloseDeleteConfirm |
    IHangHoaChangeSelectedId |IHangHoaChangeFilter