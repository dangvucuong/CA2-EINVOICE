
import { IRoleApiLoadRequest } from "../../../models/requests/user/IRoleApiLoadRequest";
import { IRoleApi } from "../../../models/responses/user/IRoleApi";
import { IActionTypeBase } from "../IActionTypeBase";

export enum eRoleApiActionTypeIds {
    LOAD_START = "ROLE_API_LOAD_START",
    LOAD_SUCCESS = "ROLE_API_LOAD_SUCCESS",
    LOAD_ERROR = "ROLE_API_LOAD_ERROR",

    ADD_API_START = "ROLE_API_ADD_API_START",
    ADD_API_SUCCESS = "ROLE_API_ADD_API_SUCCESS",
    ADD_API_ERROR = "ROLE_API_ADD_API_ERROR",

    REMOVE_API_START = "ROLE_API_REMOVE_API_START",
    REMOVE_API_SUCCESS = "ROLE_API_REMOVE_API_SUCCESS",
    REMOVE_API_ERROR = "ROLE_API_REMOVE_API_ERROR",
}
export interface IRoleApiLoadStart extends IActionTypeBase<eRoleApiActionTypeIds.LOAD_START, IRoleApiLoadRequest> { }
export interface IRoleApiLoadSuccess extends IActionTypeBase<eRoleApiActionTypeIds.LOAD_SUCCESS, IRoleApi[]> { }
export interface IRoleApiLoadError extends IActionTypeBase<eRoleApiActionTypeIds.LOAD_ERROR, string> { }

export interface IRoleApiAddStart extends IActionTypeBase<eRoleApiActionTypeIds.ADD_API_START, IRoleApi> { }
export interface IRoleApiAddSuccess extends IActionTypeBase<eRoleApiActionTypeIds.ADD_API_SUCCESS, undefined> { }
export interface IRoleApiAddError extends IActionTypeBase<eRoleApiActionTypeIds.ADD_API_ERROR, string> { }

export interface IRoleApiRemoveStart extends IActionTypeBase<eRoleApiActionTypeIds.REMOVE_API_START, IRoleApi> { }
export interface IRoleApiRemoveSuccess extends IActionTypeBase<eRoleApiActionTypeIds.REMOVE_API_SUCCESS, undefined> { }
export interface IRoleApiRemoveError extends IActionTypeBase<eRoleApiActionTypeIds.REMOVE_API_ERROR, string> { }

export type IRoleApiActionType = IRoleApiLoadStart | IRoleApiLoadSuccess | IRoleApiLoadError |
    IRoleApiAddStart | IRoleApiAddSuccess | IRoleApiAddError |
    IRoleApiRemoveStart | IRoleApiRemoveSuccess | IRoleApiRemoveError