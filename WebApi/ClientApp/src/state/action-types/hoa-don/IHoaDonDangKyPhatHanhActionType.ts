import { IHoaDonDangKyPhatHanh } from "../../../models/responses/hoa-don/IHoaDonDangKyPhatHanh";
import { IActionTypeBase } from "../IActionTypeBase";

export enum eHoaDonDangKyPhatHanhActionTypeIds {
    LOAD_START = "HOADON_DANGKY_PHATHANH_LOAD_START",
    LOAD_SUCCESS = "HOADON_DANGKY_PHATHANH_LOAD_SUCCESS",
    LOAD_ERROR = "HOADON_DANGKY_PHATHANH_LOAD_ERROR",

    CHANGE_SELECTED_ID = "HOADON_DANGKY_PHATHANH_CHANGE_SELECTED_ID",
    CHANGE_FILTER = "HOADON_DANGKY_PHATHANH_CHANGE_FILTER",


    SHOW_EDIT_MODAL = "HOADON_DANGKY_PHATHANH_SHOW_EDIT_MODAL",
    CLOSE_EDIT_MODAL = "HOADON_DANGKY_PHATHANH_CLOSE_EDIT_MODAL",

    SAVE_START = "HOADON_DANGKY_PHATHANH_SAVE_START",
    SAVE_SUCCESS = "HOADON_DANGKY_PHATHANH_SAVE_SUCCESS",
    SAVE_ERROR = "HOADON_DANGKY_PHATHANH_SAVE_ERROR",

    SHOW_DELETE_CONFIRM = "HOADON_DANGKY_PHATHANH_SHOW_DELETE_CONFIRM",
    CLOSE_DELETE_CONFIRM = "HOADON_DANGKY_PHATHANH_CLOSE_DELETE_CONFIRM",

    DELETE_START = "HOADON_DANGKY_PHATHANH_DELETE_START",
    DELETE_SUCCESS = "HOADON_DANGKY_PHATHANH_DELETE_SUCCESS",
    DELETE_ERROR = "HOADON_DANGKY_PHATHANH_DELETE_ERROR",

}

export interface IHoaDonDangKyPhatHanhLoadStart extends IActionTypeBase<eHoaDonDangKyPhatHanhActionTypeIds.LOAD_START, undefined> { }
export interface IHoaDonDangKyPhatHanhLoadSuccess extends IActionTypeBase<eHoaDonDangKyPhatHanhActionTypeIds.LOAD_SUCCESS, IHoaDonDangKyPhatHanh[]> { }
export interface IHoaDonDangKyPhatHanhLoadError extends IActionTypeBase<eHoaDonDangKyPhatHanhActionTypeIds.LOAD_ERROR, string> { }

export interface IHoaDonDangKyPhatHanhShowEditModal extends IActionTypeBase<eHoaDonDangKyPhatHanhActionTypeIds.SHOW_EDIT_MODAL, IHoaDonDangKyPhatHanh | undefined> { }
export interface IHoaDonDangKyPhatHanhCloseEditModal extends IActionTypeBase<eHoaDonDangKyPhatHanhActionTypeIds.CLOSE_EDIT_MODAL, undefined> { }

export interface IHoaDonDangKyPhatHanhSaveStart extends IActionTypeBase<eHoaDonDangKyPhatHanhActionTypeIds.SAVE_START, IHoaDonDangKyPhatHanh> { }
export interface IHoaDonDangKyPhatHanhSaveSuccess extends IActionTypeBase<eHoaDonDangKyPhatHanhActionTypeIds.SAVE_SUCCESS, IHoaDonDangKyPhatHanh> { }
export interface IHoaDonDangKyPhatHanhSaveError extends IActionTypeBase<eHoaDonDangKyPhatHanhActionTypeIds.SAVE_ERROR, string> { }

export interface IHoaDonDangKyPhatHanhShowDeleteConfirm extends IActionTypeBase<eHoaDonDangKyPhatHanhActionTypeIds.SHOW_DELETE_CONFIRM, IHoaDonDangKyPhatHanh> { }
export interface IHoaDonDangKyPhatHanhCloseDeleteConfirm extends IActionTypeBase<eHoaDonDangKyPhatHanhActionTypeIds.CLOSE_DELETE_CONFIRM, undefined> { }


export interface IHoaDonDangKyPhatHanhDeleteStart extends IActionTypeBase<eHoaDonDangKyPhatHanhActionTypeIds.DELETE_START, number> { }
export interface IHoaDonDangKyPhatHanhDeleteSuccess extends IActionTypeBase<eHoaDonDangKyPhatHanhActionTypeIds.DELETE_SUCCESS, undefined> { }
export interface IHoaDonDangKyPhatHanhDeleteError extends IActionTypeBase<eHoaDonDangKyPhatHanhActionTypeIds.DELETE_ERROR, string> { }

export interface IHoaDonDangKyPhatHanhChangeSelectedId extends IActionTypeBase<eHoaDonDangKyPhatHanhActionTypeIds.CHANGE_SELECTED_ID, number> { }


export type IHoaDonDangKyPhatHanhActionType = IHoaDonDangKyPhatHanhLoadStart | IHoaDonDangKyPhatHanhLoadSuccess | IHoaDonDangKyPhatHanhLoadError |
    IHoaDonDangKyPhatHanhShowEditModal | IHoaDonDangKyPhatHanhCloseEditModal |
    IHoaDonDangKyPhatHanhSaveStart | IHoaDonDangKyPhatHanhSaveSuccess | IHoaDonDangKyPhatHanhSaveError |
    IHoaDonDangKyPhatHanhDeleteStart | IHoaDonDangKyPhatHanhDeleteSuccess | IHoaDonDangKyPhatHanhDeleteError |
    IHoaDonDangKyPhatHanhShowDeleteConfirm | IHoaDonDangKyPhatHanhCloseDeleteConfirm |
    IHoaDonDangKyPhatHanhChangeSelectedId 