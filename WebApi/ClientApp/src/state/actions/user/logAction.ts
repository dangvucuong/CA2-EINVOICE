import { NotifyHelper } from "../../../helpers/toast";
import { IPagingRequest } from "../../../models/requests/IPagingRequest";
import { IPagingRespone } from "../../../models/responses/IBasePagingRespone";
import { ILog } from "../../../models/responses/user/ILog";
import { ILogChangeFilter, ILogLoadError, ILogLoadStart, ILogLoadSuccess, eLogActionTypeIds } from "../../action-types/user/ILogActionType";
import { baseAction } from "../baseAction";

export const logAction = {
    loadStart: (rq: IPagingRequest): ILogLoadStart => baseAction(eLogActionTypeIds.LOAD_START, rq),
    loadSuccess: (res: IPagingRespone<ILog[]>): ILogLoadSuccess => baseAction(eLogActionTypeIds.LOAD_SUCCESS, res),
    loadError: (m: string): ILogLoadError => {
        NotifyHelper.Error(m);
        return baseAction(eLogActionTypeIds.LOAD_ERROR, m)
    },
    changeFilter: (filter: IPagingRequest): ILogChangeFilter => {
        return baseAction(eLogActionTypeIds.CHANGE_FILTER, filter)
    },
}