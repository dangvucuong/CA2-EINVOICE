import { INotifySummaryRespone } from "../../../models/responses/notify/INotifySummary";
import { IActionTypeBase } from "../IActionTypeBase";

export enum eNotifyActionTypeIds {
    LOAD_SUMMARY_START = "NOTIFY_LOAD_SUMMARY_START",
    LOAD_SUMMARY_SUCCESS = "NOTIFY_LOAD_SUMMARY_SUCCESS",
    LOAD_SUMMARY_ERROR = "NOTIFY_LOAD_SUMMARY_ERROR",
}
export interface INotifySummaryLoadStart extends IActionTypeBase<eNotifyActionTypeIds.LOAD_SUMMARY_START, undefined> { }
export interface INotifySummaryLoadSuccess extends IActionTypeBase<eNotifyActionTypeIds.LOAD_SUMMARY_SUCCESS, INotifySummaryRespone> { }
export interface INotifySummaryLoadError extends IActionTypeBase<eNotifyActionTypeIds.LOAD_SUMMARY_ERROR, string> { }

export type INotifyActionType = INotifySummaryLoadStart | INotifySummaryLoadSuccess | INotifySummaryLoadError