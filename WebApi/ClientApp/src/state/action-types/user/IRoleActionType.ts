import { IRole } from "../../../models/responses/user/IRole";
import { IRoleViewModel } from "../../../models/responses/user/IRoleViewModel";
import { IActionTypeBase } from "../IActionTypeBase";

export enum eRoleActionTypeIds {
    LOAD_START = "ROLE_LOAD_START",
    LOAD_SUCCESS = "ROLE_LOAD_SUCCESS",
    LOAD_ERROR = "ROLE_LOAD_ERROR",

    CHANGE_EDITING = "ROLE_CHANGE_EDITING",

    SAVE_START = "ROLE_SAVE_START",
    SAVE_SUCCESS = "ROLE_SAVE_SUCCESS",
    SAVE_ERROR = "ROLE_SAVE_ERROR",

    DELETE_START = "ROLE_DELETE_START",
    DELETE_SUCCESS = "ROLE_DELETE_SUCCESS",
    DELETE_ERROR = "ROLE_DELETE_ERROR",
}
export interface IRoleLoadStart extends IActionTypeBase<eRoleActionTypeIds.LOAD_START, undefined> { }
export interface IRoleLoadSuccess extends IActionTypeBase<eRoleActionTypeIds.LOAD_SUCCESS, IRoleViewModel[]> { }
export interface IRoleLoadError extends IActionTypeBase<eRoleActionTypeIds.LOAD_ERROR, string> { }

export interface IRoleChangeEditing extends IActionTypeBase<eRoleActionTypeIds.CHANGE_EDITING, IRole | undefined> { }

export interface IRoleSaveStart extends IActionTypeBase<eRoleActionTypeIds.SAVE_START, IRole> { }
export interface IRoleSaveSuccess extends IActionTypeBase<eRoleActionTypeIds.SAVE_SUCCESS, IRole> { }
export interface IRoleSaveError extends IActionTypeBase<eRoleActionTypeIds.SAVE_ERROR, string> { }


export interface IRoleDeleteStart extends IActionTypeBase<eRoleActionTypeIds.DELETE_START, number> { }
export interface IRoleDeleteSuccess extends IActionTypeBase<eRoleActionTypeIds.DELETE_SUCCESS, undefined> { }
export interface IRoleDeleteError extends IActionTypeBase<eRoleActionTypeIds.DELETE_ERROR, string> { }


export type IRoleActionType = IRoleLoadStart | IRoleLoadSuccess | IRoleLoadError |
    IRoleChangeEditing |
    IRoleSaveStart | IRoleSaveSuccess | IRoleSaveError |
    IRoleDeleteStart | IRoleDeleteSuccess | IRoleDeleteError

