import { IDaiLy } from "../../../models/responses/category/IDaiLy";
import { IDaiLyPaging } from '../../../models/responses/category/IDaiLyPaging';
import { IActionTypeBase } from "../IActionTypeBase";
import { IPagingRequest } from './../../../models/requests/IPagingRequest';

export enum eDaiLyActionTypeIds {
    LOAD_START = "DAILY_LOAD_START",
    LOAD_SUCCESS = "DAILY_LOAD_SUCCESS",
    LOAD_ERROR = "DAILY_LOAD_ERROR",

    CHANGE_SELECTED_ID = "DAILY_CHANGE_SELECTED_ID",
    CHANGE_FILTER = "DAILY_CHANGE_FILTER",


    SHOW_EDIT_MODAL = "DAILY_SHOW_EDIT_MODAL",
    CLOSE_EDIT_MODAL = "DAILY_CLOSE_EDIT_MODAL",

    SAVE_START = "DAILY_SAVE_START",
    SAVE_SUCCESS = "DAILY_SAVE_SUCCESS",
    SAVE_ERROR = "DAILY_SAVE_ERROR",

    SHOW_DELETE_CONFIRM = "DAILY_SHOW_DELETE_CONFIRM",
    CLOSE_DELETE_CONFIRM = "DAILY_CLOSE_DELETE_CONFIRM",

    DELETE_START = "DAILY_DELETE_START",
    DELETE_SUCCESS = "DAILY_DELETE_SUCCESS",
    DELETE_ERROR = "DAILY_DELETE_ERROR",

}

export interface IDaiLyLoadStart extends IActionTypeBase<eDaiLyActionTypeIds.LOAD_START, IPagingRequest> { }
export interface IDaiLyLoadSuccess extends IActionTypeBase<eDaiLyActionTypeIds.LOAD_SUCCESS, IDaiLyPaging> { }
export interface IDaiLyLoadError extends IActionTypeBase<eDaiLyActionTypeIds.LOAD_ERROR, string> { }

export interface IDaiLyShowEditModal extends IActionTypeBase<eDaiLyActionTypeIds.SHOW_EDIT_MODAL, IDaiLy | undefined> { }
export interface IDaiLyCloseEditModal extends IActionTypeBase<eDaiLyActionTypeIds.CLOSE_EDIT_MODAL, undefined> { }

export interface IDaiLySaveStart extends IActionTypeBase<eDaiLyActionTypeIds.SAVE_START, IDaiLy> { }
export interface IDaiLySaveSuccess extends IActionTypeBase<eDaiLyActionTypeIds.SAVE_SUCCESS, IDaiLy> { }
export interface IDaiLySaveError extends IActionTypeBase<eDaiLyActionTypeIds.SAVE_ERROR, string> { }

export interface IDaiLyShowDeleteConfirm extends IActionTypeBase<eDaiLyActionTypeIds.SHOW_DELETE_CONFIRM, IDaiLy> { }
export interface IDaiLyCloseDeleteConfirm extends IActionTypeBase<eDaiLyActionTypeIds.CLOSE_DELETE_CONFIRM, undefined> { }


export interface IDaiLyDeleteStart extends IActionTypeBase<eDaiLyActionTypeIds.DELETE_START, number> { }
export interface IDaiLyDeleteSuccess extends IActionTypeBase<eDaiLyActionTypeIds.DELETE_SUCCESS, undefined> { }
export interface IDaiLyDeleteError extends IActionTypeBase<eDaiLyActionTypeIds.DELETE_ERROR, string> { }

export interface IDaiLyChangeSelectedId extends IActionTypeBase<eDaiLyActionTypeIds.CHANGE_SELECTED_ID, number> { }
export interface IDaiLyChangeFilter extends IActionTypeBase<eDaiLyActionTypeIds.CHANGE_FILTER, IPagingRequest> { }


export type IDaiLyActionType = IDaiLyLoadStart | IDaiLyLoadSuccess | IDaiLyLoadError |
    IDaiLyShowEditModal | IDaiLyCloseEditModal |
    IDaiLySaveStart | IDaiLySaveSuccess | IDaiLySaveError |
    IDaiLyDeleteStart | IDaiLyDeleteSuccess | IDaiLyDeleteError |
    IDaiLyShowDeleteConfirm | IDaiLyCloseDeleteConfirm |
    IDaiLyChangeSelectedId |IDaiLyChangeFilter