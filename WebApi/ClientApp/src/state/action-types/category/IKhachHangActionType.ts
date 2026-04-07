import { IPagingRequest } from "../../../models/requests/IPagingRequest";
import { IKhachHang } from "../../../models/responses/category/IKhachHang";
import { IKhachHangPaging } from "../../../models/responses/category/IKhachHangPaging";
import { IActionTypeBase } from "../IActionTypeBase";

export enum eKhachHangActionTypeIds {
    LOAD_START = "KHACHHANG_LOAD_START",
    LOAD_SUCCESS = "KHACHHANG_LOAD_SUCCESS",
    LOAD_ERROR = "KHACHHANG_LOAD_ERROR",

    CHANGE_SELECTED_ID = "KHACHHANG_CHANGE_SELECTED_ID",
    CHANGE_FILTER = "KHACHHANG_CHANGE_FILTER",


    SHOW_EDIT_MODAL = "KHACHHANG_SHOW_EDIT_MODAL",
    CLOSE_EDIT_MODAL = "KHACHHANG_CLOSE_EDIT_MODAL",

    SAVE_START = "KHACHHANG_SAVE_START",
    SAVE_SUCCESS = "KHACHHANG_SAVE_SUCCESS",
    SAVE_ERROR = "KHACHHANG_SAVE_ERROR",

    SHOW_DELETE_CONFIRM = "KHACHHANG_SHOW_DELETE_CONFIRM",
    CLOSE_DELETE_CONFIRM = "KHACHHANG_CLOSE_DELETE_CONFIRM",

    DELETE_START = "KHACHHANG_DELETE_START",
    DELETE_SUCCESS = "KHACHHANG_DELETE_SUCCESS",
    DELETE_ERROR = "KHACHHANG_DELETE_ERROR",

}

export interface IKhachHangLoadStart extends IActionTypeBase<eKhachHangActionTypeIds.LOAD_START, IPagingRequest> { }
export interface IKhachHangLoadSuccess extends IActionTypeBase<eKhachHangActionTypeIds.LOAD_SUCCESS, IKhachHangPaging> { }
export interface IKhachHangLoadError extends IActionTypeBase<eKhachHangActionTypeIds.LOAD_ERROR, string> { }

export interface IKhachHangShowEditModal extends IActionTypeBase<eKhachHangActionTypeIds.SHOW_EDIT_MODAL, IKhachHang | undefined> { }
export interface IKhachHangCloseEditModal extends IActionTypeBase<eKhachHangActionTypeIds.CLOSE_EDIT_MODAL, undefined> { }

export interface IKhachHangSaveStart extends IActionTypeBase<eKhachHangActionTypeIds.SAVE_START, IKhachHang> { }
export interface IKhachHangSaveSuccess extends IActionTypeBase<eKhachHangActionTypeIds.SAVE_SUCCESS, IKhachHang> { }
export interface IKhachHangSaveError extends IActionTypeBase<eKhachHangActionTypeIds.SAVE_ERROR, string> { }

export interface IKhachHangShowDeleteConfirm extends IActionTypeBase<eKhachHangActionTypeIds.SHOW_DELETE_CONFIRM, IKhachHang> { }
export interface IKhachHangCloseDeleteConfirm extends IActionTypeBase<eKhachHangActionTypeIds.CLOSE_DELETE_CONFIRM, undefined> { }


export interface IKhachHangDeleteStart extends IActionTypeBase<eKhachHangActionTypeIds.DELETE_START, number> { }
export interface IKhachHangDeleteSuccess extends IActionTypeBase<eKhachHangActionTypeIds.DELETE_SUCCESS, undefined> { }
export interface IKhachHangDeleteError extends IActionTypeBase<eKhachHangActionTypeIds.DELETE_ERROR, string> { }

export interface IKhachHangChangeSelectedId extends IActionTypeBase<eKhachHangActionTypeIds.CHANGE_SELECTED_ID, number> { }
export interface IKhachHangChangeFilter extends IActionTypeBase<eKhachHangActionTypeIds.CHANGE_FILTER, IPagingRequest> { }


export type IKhachHangActionType = IKhachHangLoadStart | IKhachHangLoadSuccess | IKhachHangLoadError |
    IKhachHangShowEditModal | IKhachHangCloseEditModal |
    IKhachHangSaveStart | IKhachHangSaveSuccess | IKhachHangSaveError |
    IKhachHangDeleteStart | IKhachHangDeleteSuccess | IKhachHangDeleteError |
    IKhachHangShowDeleteConfirm | IKhachHangCloseDeleteConfirm |
    IKhachHangChangeSelectedId |IKhachHangChangeFilter