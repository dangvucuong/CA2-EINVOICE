import { IAppConfig } from "../../../models/responses/common/IAppConfig"
import { IActionTypeBase } from "../IActionTypeBase"


export enum eAppConfigActionTypeIds {
    LOAD_START = "APP_CONFIG_LOAD_START",
    LOAD_SUCCESS = "APP_CONFIG_LOAD_SUCCESS",
    LOAD_ERROR = "APP_CONFIG_LOAD_ERROR",
}

export interface IAppConfigLoadStart
    extends IActionTypeBase<eAppConfigActionTypeIds.LOAD_START, undefined> { }

export interface IAppConfigLoadSuccess extends IActionTypeBase<eAppConfigActionTypeIds.LOAD_SUCCESS, IAppConfig> { }

export interface IAppConfigLoadError extends IActionTypeBase<eAppConfigActionTypeIds.LOAD_ERROR, string> { }
export type IAppConfigActionType = IAppConfigLoadStart | IAppConfigLoadSuccess | IAppConfigLoadError