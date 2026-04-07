import { IApi } from "../../../models/responses/user/IApi";
import { IActionTypeBase } from "../IActionTypeBase";

export enum eApiActionTypeIds {
    LOAD_START = "Api_LOAD_START",
    LOAD_SUCCESS = "Api_LOAD_SUCCESS",
    LOAD_ERROR = "Api_LOAD_ERROR",

}
export interface IApiLoadStart extends IActionTypeBase<eApiActionTypeIds.LOAD_START, number> { }
export interface IApiLoadSuccess extends IActionTypeBase<eApiActionTypeIds.LOAD_SUCCESS, IApi[]> { }
export interface IApiLoadError extends IActionTypeBase<eApiActionTypeIds.LOAD_ERROR, string> { }



export type IApiActionType = IApiLoadStart | IApiLoadSuccess | IApiLoadError

