import { IPagingRequest } from "../../../models/requests/IPagingRequest";
import { IUserLoadByDonViRequest } from "../../../models/requests/user/IUserLoadByDonViRequest";
import { IUserLoadRequest } from "../../../models/requests/user/IUserLoadRequest";
import { IUser } from "../../../models/responses/user/IUser";
import { IUserEditModel } from "../../../models/responses/user/IUserEditModel";
import { IUserPaging } from "../../../models/responses/user/IUserPaging";
import { IActionTypeBase } from "../IActionTypeBase";

export enum eUserActionTypeIds {
  LOAD_START = "USER_LOAD_START",
  LOAD_SUCCESS = "USER_LOAD_SUCCESS",
  LOAD_ERROR = "USER_LOAD_ERROR",

  LOAD_BY_DONVI_START = "USER_LOAD_BY_DONVI_START",
  LOAD_BY_DONVI_SUCCESS = "USER_LOAD_BY_DONVI_SUCCESS",
  LOAD_BY_DONVI_ERROR = "USER_LOAD_BY_DONVI_ERROR",

  CHANGE_FILTER = "USER_CHANGE_FILTER",

  // CHANGE_EDITING_USER="USER_CHANGE_EDITING_USER",

  SHOW_EDIT_MODAL = "USER_SHOW_EDIT_MODAL",
  CLOSE_EDIT_MODAL = "USER_CLOSE_EDIT_MODAL",

  LOAD_USER_FORM_START = "USER_LOAD_USER_FORM_START",
  LOAD_USER_FORM_SUCCESS = "USER_LOAD_USER_FORM_SUCCESS",
  LOAD_USER_FORM_ERROR = "USER_LOAD_USER_FORM_ERROR",

  SAVE_USER_START = "USER_SAVE_USER_START",
  SAVE_USER_SUCCESS = "USER_SAVE_USER_SUCCESS",
  SAVE_USER_ERROR = "USER_SAVE_USER_ERROR",

  SHOW_DELETE_CONFIRM = "USER_SHOW_DELETE_CONFIRM",
  CLOSE_DELETE_CONFIRM = "USER_CLOSE_DELETE_CONFIRM",

  DELETE_START = "USER_DELETE_START",
  DELETE_SUCCESS = "USER_DELETE_SUCCESS",
  DELETE_ERROR = "USER_DELETE_ERROR",
}
export interface IUserLoadStart extends IActionTypeBase<
  eUserActionTypeIds.LOAD_START,
  IUserLoadRequest
> {}
export interface IUserLoadSuccess extends IActionTypeBase<
  eUserActionTypeIds.LOAD_SUCCESS,
  IUserPaging
> {}
export interface IUserLoadError extends IActionTypeBase<
  eUserActionTypeIds.LOAD_ERROR,
  string
> {}

export interface IUserLoadByDonViStart extends IActionTypeBase<
  eUserActionTypeIds.LOAD_BY_DONVI_START,
  IUserLoadByDonViRequest
> {}
export interface IUserLoadByDonViSuccess extends IActionTypeBase<
  eUserActionTypeIds.LOAD_BY_DONVI_SUCCESS,
  IUserPaging
> {}
export interface IUserLoadByDonViError extends IActionTypeBase<
  eUserActionTypeIds.LOAD_BY_DONVI_ERROR,
  string
> {}

export interface IUserChangeFilter extends IActionTypeBase<
  eUserActionTypeIds.CHANGE_FILTER,
  IPagingRequest
> {}
// export interface IUserChangeEditing extends IActionTypeBase<eUserActionTypeIds.CHANGE_EDITING_USER, IUser | undefined> { }

export interface IUserShowEditModal extends IActionTypeBase<
  eUserActionTypeIds.SHOW_EDIT_MODAL,
  IUser | undefined
> {}
export interface IUserCloseEditModal extends IActionTypeBase<
  eUserActionTypeIds.CLOSE_EDIT_MODAL,
  undefined
> {}

export interface IUserLoadFormStart extends IActionTypeBase<
  eUserActionTypeIds.LOAD_USER_FORM_START,
  number
> {}
export interface IUserLoadFormSuccess extends IActionTypeBase<
  eUserActionTypeIds.LOAD_USER_FORM_SUCCESS,
  IUserEditModel
> {}
export interface IUserLoadFormError extends IActionTypeBase<
  eUserActionTypeIds.LOAD_USER_FORM_ERROR,
  string
> {}

export interface IUserSaveFormStart extends IActionTypeBase<
  eUserActionTypeIds.SAVE_USER_START,
  IUserEditModel
> {}
export interface IUserSaveFormSuccess extends IActionTypeBase<
  eUserActionTypeIds.SAVE_USER_SUCCESS,
  IUserEditModel
> {}
export interface IUserSaveFormError extends IActionTypeBase<
  eUserActionTypeIds.SAVE_USER_ERROR,
  string
> {}

export interface IUserShowDeleteConfirm extends IActionTypeBase<
  eUserActionTypeIds.SHOW_DELETE_CONFIRM,
  IUser
> {}
export interface IUserCloseDeleteConfirm extends IActionTypeBase<
  eUserActionTypeIds.CLOSE_DELETE_CONFIRM,
  undefined
> {}

export interface IUserDeleteStart extends IActionTypeBase<
  eUserActionTypeIds.DELETE_START,
  number
> {}
export interface IUserDeleteSuccess extends IActionTypeBase<
  eUserActionTypeIds.DELETE_SUCCESS,
  undefined
> {}
export interface IUserDeleteError extends IActionTypeBase<
  eUserActionTypeIds.DELETE_ERROR,
  string
> {}

export type IUserActionType =
  | IUserLoadStart
  | IUserLoadSuccess
  | IUserLoadError
  | IUserLoadByDonViStart
  | IUserLoadByDonViSuccess
  | IUserLoadByDonViError
  | IUserChangeFilter
  | IUserShowEditModal
  | IUserCloseEditModal
  | IUserLoadFormStart
  | IUserLoadFormSuccess
  | IUserLoadFormError
  | IUserSaveFormStart
  | IUserSaveFormSuccess
  | IUserSaveFormError
  | IUserShowDeleteConfirm
  | IUserCloseDeleteConfirm
  | IUserDeleteStart
  | IUserDeleteSuccess
  | IUserDeleteError;
