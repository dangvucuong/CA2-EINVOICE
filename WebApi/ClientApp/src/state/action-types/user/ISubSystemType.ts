import { ISubSystem } from "../../../models/responses/user/ISubSystem";
import { IActionTypeBase } from "../IActionTypeBase";

export enum eSubSystemTypeIds {
    LOAD_START = "SUBSYTEM_LOAD_START",
    LOAD_SUCCESS = "SUBSYTEM_LOAD_SUCCESS",
    LOAD_ERROR = "SUBSYTEM_LOAD_ERROR",

    CHANGE_EDITING = "SUBSYTEM_CHANGE_EDITING"
}
export interface ISubSystemLoadStart extends IActionTypeBase<eSubSystemTypeIds.LOAD_START, undefined> { }
export interface ISubSystemLoadSuccess extends IActionTypeBase<eSubSystemTypeIds.LOAD_SUCCESS, ISubSystem[]> { }
export interface ISubSystemLoadError extends IActionTypeBase<eSubSystemTypeIds.LOAD_ERROR, string> { }

export interface ISubSystemChangeEditing extends IActionTypeBase<eSubSystemTypeIds.CHANGE_EDITING, number> { }



export type ISubSystemActionType = ISubSystemLoadStart | ISubSystemLoadSuccess | ISubSystemLoadError |ISubSystemChangeEditing

