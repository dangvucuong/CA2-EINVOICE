import { INotifySummaryRespone } from '../../../models/responses/notify/INotifySummary';
import { INotifySummaryLoadError, INotifySummaryLoadSuccess, eNotifyActionTypeIds } from "../../action-types/notify/INotifyActionType";
import { baseAction } from "../baseAction";
import { INotifySummaryLoadStart } from './../../action-types/notify/INotifyActionType';

export const notifyAction={
    loadSummaryStart:():INotifySummaryLoadStart=> baseAction(eNotifyActionTypeIds.LOAD_SUMMARY_START, undefined),
    loadSummarySuccess:(notifySummary:INotifySummaryRespone):INotifySummaryLoadSuccess=> baseAction(eNotifyActionTypeIds.LOAD_SUMMARY_SUCCESS, notifySummary),
    loadSummaryError:(m:string):INotifySummaryLoadError=> baseAction(eNotifyActionTypeIds.LOAD_SUMMARY_ERROR, m),
}