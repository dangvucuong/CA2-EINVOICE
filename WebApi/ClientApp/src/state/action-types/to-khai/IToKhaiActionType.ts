import { IToKhai } from "../../../models/responses/to-khai/IToKhai";
import { IActionTypeBase } from "../IActionTypeBase";

export enum eToKhaiActionTypeIds {
    LOAD_START = "TOKHAI_LOAD_START",
    LOAD_SUCCESS = "TOKHAI_LOAD_SUCCESS",
    LOAD_ERROR = "TOKHAI_LOAD_ERROR",

    CHANGE_SELECTED_ID = "TOKHAI_CHANGE_SELECTED_ID",



    SHOW_DELETE_CONFIRM = "TOKHAI_SHOW_DELETE_CONFIRM",
    CLOSE_DELETE_CONFIRM = "TOKHAI_CLOSE_DELETE_CONFIRM",

    DELETE_START = "TOKHAI_DELETE_START",
    DELETE_SUCCESS = "TOKHAI_DELETE_SUCCESS",
    DELETE_ERROR = "TOKHAI_DELETE_ERROR",


    SHOW_LOG_MODAL = "TOKHAI_SHOW_LOG_MODAL",
    CLOSE_LOG_MODAL = "TOKHAI_CLOSE_LOG_MODAL",



}

export interface IToKhaiLoadStart extends IActionTypeBase<eToKhaiActionTypeIds.LOAD_START, undefined> { }
export interface IToKhaiLoadSuccess extends IActionTypeBase<eToKhaiActionTypeIds.LOAD_SUCCESS, IToKhai[]> { }
export interface IToKhaiLoadError extends IActionTypeBase<eToKhaiActionTypeIds.LOAD_ERROR, string> { }



export interface IToKhaiShowDeleteConfirm extends IActionTypeBase<eToKhaiActionTypeIds.SHOW_DELETE_CONFIRM, IToKhai> { }
export interface IToKhaiCloseDeleteConfirm extends IActionTypeBase<eToKhaiActionTypeIds.CLOSE_DELETE_CONFIRM, undefined> { }


export interface IToKhaiShowLogModal extends IActionTypeBase<eToKhaiActionTypeIds.SHOW_LOG_MODAL, IToKhai> { }
export interface IToKhaiCloseLogModal extends IActionTypeBase<eToKhaiActionTypeIds.CLOSE_LOG_MODAL, undefined> { }


export interface IToKhaiDeleteStart extends IActionTypeBase<eToKhaiActionTypeIds.DELETE_START, number> { }
export interface IToKhaiDeleteSuccess extends IActionTypeBase<eToKhaiActionTypeIds.DELETE_SUCCESS, undefined> { }
export interface IToKhaiDeleteError extends IActionTypeBase<eToKhaiActionTypeIds.DELETE_ERROR, string> { }



export type IToKhaiActionType = IToKhaiLoadStart | IToKhaiLoadSuccess | IToKhaiLoadError |
    IToKhaiDeleteStart | IToKhaiDeleteSuccess | IToKhaiDeleteError |
    IToKhaiShowDeleteConfirm | IToKhaiCloseDeleteConfirm |
    IToKhaiShowLogModal | IToKhaiCloseLogModal