import { IPagingRequest } from "../../../models/requests/IPagingRequest";
import { IDonVi } from "../../../models/responses/category/IDonVi";
import { IDonViPaging } from "../../../models/responses/category/IDonViPaging";
import { IActionTypeBase } from "../IActionTypeBase";

export enum eDonViActionTypeIds {
    LOAD_START = "DONVI_LOAD_START",
    LOAD_SUCCESS = "DONVI_LOAD_SUCCESS",
    LOAD_ERROR = "DONVI_LOAD_ERROR",

    CHANGE_SELECTED_ID = "DONVI_CHANGE_SELECTED_ID",
    CHANGE_FILTER = "DONVI_CHANGE_FILTER",


    SHOW_EDIT_MODAL = "DONVI_SHOW_EDIT_MODAL",
    CLOSE_EDIT_MODAL = "DONVI_CLOSE_EDIT_MODAL",

    SAVE_START = "DONVI_SAVE_START",
    SAVE_SUCCESS = "DONVI_SAVE_SUCCESS",
    SAVE_ERROR = "DONVI_SAVE_ERROR",

    SHOW_DELETE_CONFIRM = "DONVI_SHOW_DELETE_CONFIRM",
    CLOSE_DELETE_CONFIRM = "DONVI_CLOSE_DELETE_CONFIRM",

    DELETE_START = "DONVI_DELETE_START",
    DELETE_SUCCESS = "DONVI_DELETE_SUCCESS",
    DELETE_ERROR = "DONVI_DELETE_ERROR",

}

export interface IDonViLoadStart extends IActionTypeBase<eDonViActionTypeIds.LOAD_START, IPagingRequest> { }
export interface IDonViLoadSuccess extends IActionTypeBase<eDonViActionTypeIds.LOAD_SUCCESS, IDonViPaging> { }
export interface IDonViLoadError extends IActionTypeBase<eDonViActionTypeIds.LOAD_ERROR, string> { }

export interface IDonViShowEditModal extends IActionTypeBase<eDonViActionTypeIds.SHOW_EDIT_MODAL, IDonVi | undefined> { }
export interface IDonViCloseEditModal extends IActionTypeBase<eDonViActionTypeIds.CLOSE_EDIT_MODAL, undefined> { }

export interface IDonViSaveStart extends IActionTypeBase<eDonViActionTypeIds.SAVE_START, IDonVi> { }
export interface IDonViSaveSuccess extends IActionTypeBase<eDonViActionTypeIds.SAVE_SUCCESS, IDonVi> { }
export interface IDonViSaveError extends IActionTypeBase<eDonViActionTypeIds.SAVE_ERROR, string> { }

export interface IDonViShowDeleteConfirm extends IActionTypeBase<eDonViActionTypeIds.SHOW_DELETE_CONFIRM, IDonVi> { }
export interface IDonViCloseDeleteConfirm extends IActionTypeBase<eDonViActionTypeIds.CLOSE_DELETE_CONFIRM, undefined> { }


export interface IDonViDeleteStart extends IActionTypeBase<eDonViActionTypeIds.DELETE_START, number> { }
export interface IDonViDeleteSuccess extends IActionTypeBase<eDonViActionTypeIds.DELETE_SUCCESS, undefined> { }
export interface IDonViDeleteError extends IActionTypeBase<eDonViActionTypeIds.DELETE_ERROR, string> { }

export interface IDonViChangeSelectedId extends IActionTypeBase<eDonViActionTypeIds.CHANGE_SELECTED_ID, number> { }
export interface IDonViChangeFilter extends IActionTypeBase<eDonViActionTypeIds.CHANGE_FILTER, IPagingRequest> { }


export type IDonViActionType = IDonViLoadStart | IDonViLoadSuccess | IDonViLoadError |
    IDonViShowEditModal | IDonViCloseEditModal |
    IDonViSaveStart | IDonViSaveSuccess | IDonViSaveError |
    IDonViDeleteStart | IDonViDeleteSuccess | IDonViDeleteError |
    IDonViShowDeleteConfirm | IDonViCloseDeleteConfirm |
    IDonViChangeSelectedId |IDonViChangeFilter