import { IPagingRequest } from "../../../models/requests/IPagingRequest";
import { ILog } from '../../../models/responses/user/ILog';
import { IActionTypeBase } from "../IActionTypeBase";
import { IPagingRespone } from './../../../models/responses/IBasePagingRespone';

export enum eLogActionTypeIds {

    LOAD_START = "LOG_LOAD_START",
    LOAD_SUCCESS = "LOG_LOAD_SUCCESS",
    LOAD_ERROR = "LOG_LOAD_ERROR",
    CHANGE_FILTER = "LOG_CHANGE_FILTER"
}
export interface ILogLoadStart extends IActionTypeBase<eLogActionTypeIds.LOAD_START, IPagingRequest> { }
export interface ILogLoadSuccess extends IActionTypeBase<eLogActionTypeIds.LOAD_SUCCESS, IPagingRespone<ILog[]>> { }
export interface ILogLoadError extends IActionTypeBase<eLogActionTypeIds.LOAD_ERROR, string> { }

export interface ILogChangeFilter extends IActionTypeBase<eLogActionTypeIds.CHANGE_FILTER, IPagingRequest> { }


export type ILogActionType = ILogLoadStart | ILogLoadSuccess | ILogLoadError|ILogChangeFilter