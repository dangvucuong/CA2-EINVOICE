import { IPagingRequest } from "../../../models/requests/IPagingRequest";
import { IContactSelectRequest } from "../../../models/requests/contact/IContactSelectRequest";
import { IContact } from "../../../models/responses/contact/IContact";
import { IContactPaging } from "../../../models/responses/contact/IContactPaging";
import { IActionTypeBase } from "../IActionTypeBase";

export enum eContactActionTypeIds {
    LOAD_START = "CONTACT_LOAD_START",
    LOAD_SUCCESS = "CONTACT_LOAD_SUCCESS",
    LOAD_ERROR = "CONTACT_LOAD_ERROR",

    CHANGE_SELECTED_ID = "CONTACT_CHANGE_SELECTED_ID",
    CHANGE_FILTER = "CONTACT_CHANGE_FILTER",


    SHOW_EDIT_MODAL = "CONTACT_SHOW_EDIT_MODAL",
    CLOSE_EDIT_MODAL = "CONTACT_CLOSE_EDIT_MODAL",

    SAVE_START = "CONTACT_SAVE_START",
    SAVE_SUCCESS = "CONTACT_SAVE_SUCCESS",
    SAVE_ERROR = "CONTACT_SAVE_ERROR",

    SHOW_DELETE_CONFIRM = "CONTACT_SHOW_DELETE_CONFIRM",
    CLOSE_DELETE_CONFIRM = "CONTACT_CLOSE_DELETE_CONFIRM",

    DELETE_START = "CONTACT_DELETE_START",
    DELETE_SUCCESS = "CONTACT_DELETE_SUCCESS",
    DELETE_ERROR = "CONTACT_DELETE_ERROR",

}

export interface IContactLoadStart extends IActionTypeBase<eContactActionTypeIds.LOAD_START, IContactSelectRequest> { }
export interface IContactLoadSuccess extends IActionTypeBase<eContactActionTypeIds.LOAD_SUCCESS, IContactPaging> { }
export interface IContactLoadError extends IActionTypeBase<eContactActionTypeIds.LOAD_ERROR, string> { }

export interface IContactShowEditModal extends IActionTypeBase<eContactActionTypeIds.SHOW_EDIT_MODAL, IContact | undefined> { }
export interface IContactCloseEditModal extends IActionTypeBase<eContactActionTypeIds.CLOSE_EDIT_MODAL, undefined> { }

export interface IContactSaveStart extends IActionTypeBase<eContactActionTypeIds.SAVE_START, IContact> { }
export interface IContactSaveSuccess extends IActionTypeBase<eContactActionTypeIds.SAVE_SUCCESS, IContact> { }
export interface IContactSaveError extends IActionTypeBase<eContactActionTypeIds.SAVE_ERROR, string> { }

export interface IContactShowDeleteConfirm extends IActionTypeBase<eContactActionTypeIds.SHOW_DELETE_CONFIRM, IContact> { }
export interface IContactCloseDeleteConfirm extends IActionTypeBase<eContactActionTypeIds.CLOSE_DELETE_CONFIRM, undefined> { }


export interface IContactDeleteStart extends IActionTypeBase<eContactActionTypeIds.DELETE_START, number> { }
export interface IContactDeleteSuccess extends IActionTypeBase<eContactActionTypeIds.DELETE_SUCCESS, undefined> { }
export interface IContactDeleteError extends IActionTypeBase<eContactActionTypeIds.DELETE_ERROR, string> { }

export interface IContactChangeSelectedId extends IActionTypeBase<eContactActionTypeIds.CHANGE_SELECTED_ID, number> { }
export interface IContactChangeFilter extends IActionTypeBase<eContactActionTypeIds.CHANGE_FILTER, IContactSelectRequest> { }


export type IContactActionType = IContactLoadStart | IContactLoadSuccess | IContactLoadError |
    IContactShowEditModal | IContactCloseEditModal |
    IContactSaveStart | IContactSaveSuccess | IContactSaveError |
    IContactDeleteStart | IContactDeleteSuccess | IContactDeleteError |
    IContactShowDeleteConfirm | IContactCloseDeleteConfirm |
    IContactChangeSelectedId |IContactChangeFilter