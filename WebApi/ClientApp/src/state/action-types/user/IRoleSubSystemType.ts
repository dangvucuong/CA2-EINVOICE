import { IRoleSubSystem } from "../../../models/responses/user/IRoleSubSystem";
import { IActionTypeBase } from "../IActionTypeBase";

export enum eRoleSubSystemTypeIds {
    LOAD_START = "ROLE_SUBSYSTEM_LOAD_START",
    LOAD_SUCCESS = "ROLE_SUBSYSTEM_LOAD_SUCCESS",
    LOAD_ERROR = "ROLE_SUBSYSTEM_LOAD_ERROR",

    ADD_SUBSYTEM_START = "ROLE_SUBSYSTEM_ADD_SUBSYTEM_START",
    ADD_SUBSYTEM_SUCCESS = "ROLE_SUBSYSTEM_ADD_SUBSYTEM_SUCCESS",
    ADD_SUBSYTEM_ERROR = "ROLE_SUBSYSTEM_ADD_SUBSYTEM_ERROR",

    REMOVE_SUBSYSTEM_START = "ROLE_SUBSYSTEM_REMOVE_SUBSYSTEM_START",
    REMOVE_SUBSYSTEM_SUCCESS = "ROLE_SUBSYSTEM_REMOVE_SUBSYSTEM_SUCCESS",
    REMOVE_SUBSYSTEM_ERROR = "ROLE_SUBSYSTEM_REMOVE_SUBSYSTEM_ERROR",
}
export interface IRoleSubSystemLoadStart extends IActionTypeBase<eRoleSubSystemTypeIds.LOAD_START, number> { }
export interface IRoleSubSystemLoadSuccess extends IActionTypeBase<eRoleSubSystemTypeIds.LOAD_SUCCESS, IRoleSubSystem[]> { }
export interface IRoleSubSystemLoadError extends IActionTypeBase<eRoleSubSystemTypeIds.LOAD_ERROR, string> { }

export interface IRoleSubSystemAddStart extends IActionTypeBase<eRoleSubSystemTypeIds.ADD_SUBSYTEM_START, IRoleSubSystem> { }
export interface IRoleSubSystemAddSuccess extends IActionTypeBase<eRoleSubSystemTypeIds.ADD_SUBSYTEM_SUCCESS, undefined> { }
export interface IRoleSubSystemAddError extends IActionTypeBase<eRoleSubSystemTypeIds.ADD_SUBSYTEM_ERROR, string> { }

export interface IRoleSubSystemRemoveStart extends IActionTypeBase<eRoleSubSystemTypeIds.REMOVE_SUBSYSTEM_START, IRoleSubSystem> { }
export interface IRoleSubSystemRemoveSuccess extends IActionTypeBase<eRoleSubSystemTypeIds.REMOVE_SUBSYSTEM_SUCCESS, undefined> { }
export interface IRoleSubSystemRemoveError extends IActionTypeBase<eRoleSubSystemTypeIds.REMOVE_SUBSYSTEM_ERROR, string> { }

export type IRoleSubSystemActionType = IRoleSubSystemLoadStart | IRoleSubSystemLoadSuccess | IRoleSubSystemLoadError |
    IRoleSubSystemAddStart | IRoleSubSystemAddSuccess | IRoleSubSystemAddError |
    IRoleSubSystemRemoveStart | IRoleSubSystemRemoveSuccess | IRoleSubSystemRemoveError