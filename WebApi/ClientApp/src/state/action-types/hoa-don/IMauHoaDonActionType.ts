
import { IMauHoaDon } from "../../../models/responses/hoa-don/IMauHoaDon";
import { IMauHoaDonVM } from "../../../models/responses/hoa-don/IMauHoaDonVM";
import { IActionTypeBase } from "../IActionTypeBase";

export enum eMauHoaDonActionTypeIds {
    LOAD_START = "MAU_HOADON_LOAD_START",
    LOAD_SUCCESS = "MAU_HOADON_LOAD_SUCCESS",
    LOAD_ERROR = "MAU_HOADON_LOAD_ERROR",

    CHANGE_SELECTED_ID = "MAU_HOADON_CHANGE_SELECTED_ID",
    CHANGE_FILTER = "MAU_HOADON_CHANGE_FILTER",


    SHOW_EDIT_MODAL = "MAU_HOADON_SHOW_EDIT_MODAL",
    CLOSE_EDIT_MODAL = "MAU_HOADON_CLOSE_EDIT_MODAL",

    SAVE_START = "MAU_HOADON_SAVE_START",
    SAVE_SUCCESS = "MAU_HOADON_SAVE_SUCCESS",
    SAVE_ERROR = "MAU_HOADON_SAVE_ERROR",

    SHOW_DELETE_CONFIRM = "MAU_HOADON_SHOW_DELETE_CONFIRM",
    CLOSE_DELETE_CONFIRM = "MAU_HOADON_CLOSE_DELETE_CONFIRM",

    DELETE_START = "MAU_HOADON_DELETE_START",
    DELETE_SUCCESS = "MAU_HOADON_DELETE_SUCCESS",
    DELETE_ERROR = "MAU_HOADON_DELETE_ERROR",

}

export interface IMauHoaDonLoadStart extends IActionTypeBase<eMauHoaDonActionTypeIds.LOAD_START, undefined> { }
export interface IMauHoaDonLoadSuccess extends IActionTypeBase<eMauHoaDonActionTypeIds.LOAD_SUCCESS, IMauHoaDonVM[]> { }
export interface IMauHoaDonLoadError extends IActionTypeBase<eMauHoaDonActionTypeIds.LOAD_ERROR, string> { }

export interface IMauHoaDonShowEditModal extends IActionTypeBase<eMauHoaDonActionTypeIds.SHOW_EDIT_MODAL, IMauHoaDonVM | undefined> { }
export interface IMauHoaDonCloseEditModal extends IActionTypeBase<eMauHoaDonActionTypeIds.CLOSE_EDIT_MODAL, undefined> { }

export interface IMauHoaDonSaveStart extends IActionTypeBase<eMauHoaDonActionTypeIds.SAVE_START, IMauHoaDon> { }
export interface IMauHoaDonSaveSuccess extends IActionTypeBase<eMauHoaDonActionTypeIds.SAVE_SUCCESS, IMauHoaDon> { }
export interface IMauHoaDonSaveError extends IActionTypeBase<eMauHoaDonActionTypeIds.SAVE_ERROR, string> { }

export interface IMauHoaDonShowDeleteConfirm extends IActionTypeBase<eMauHoaDonActionTypeIds.SHOW_DELETE_CONFIRM, IMauHoaDonVM> { }
export interface IMauHoaDonCloseDeleteConfirm extends IActionTypeBase<eMauHoaDonActionTypeIds.CLOSE_DELETE_CONFIRM, undefined> { }


export interface IMauHoaDonDeleteStart extends IActionTypeBase<eMauHoaDonActionTypeIds.DELETE_START, number> { }
export interface IMauHoaDonDeleteSuccess extends IActionTypeBase<eMauHoaDonActionTypeIds.DELETE_SUCCESS, undefined> { }
export interface IMauHoaDonDeleteError extends IActionTypeBase<eMauHoaDonActionTypeIds.DELETE_ERROR, string> { }

export interface IMauHoaDonChangeSelectedId extends IActionTypeBase<eMauHoaDonActionTypeIds.CHANGE_SELECTED_ID, number> { }


export type IMauHoaDonActionType = IMauHoaDonLoadStart | IMauHoaDonLoadSuccess | IMauHoaDonLoadError |
    IMauHoaDonShowEditModal | IMauHoaDonCloseEditModal |
    IMauHoaDonSaveStart | IMauHoaDonSaveSuccess | IMauHoaDonSaveError |
    IMauHoaDonDeleteStart | IMauHoaDonDeleteSuccess | IMauHoaDonDeleteError |
    IMauHoaDonShowDeleteConfirm | IMauHoaDonCloseDeleteConfirm |
    IMauHoaDonChangeSelectedId 