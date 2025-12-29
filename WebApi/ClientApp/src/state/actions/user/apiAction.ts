import { NotifyHelper } from "../../../helpers/toast";
import { IApi } from "../../../models/responses/user/IApi";
import { eApiActionTypeIds } from "../../action-types/user/IApiActionType";

import { IApiLoadError, IApiLoadStart, IApiLoadSuccess } from "../../action-types/user/IApiActionType";
import { baseAction } from "../baseAction";

export const apiAction = {
    loadStart: (subSystemId: number): IApiLoadStart =>
        baseAction(eApiActionTypeIds.LOAD_START, subSystemId),
    loadSuccess: (res: IApi[]): IApiLoadSuccess =>
        baseAction(eApiActionTypeIds.LOAD_SUCCESS, res),
    loadError: (message: string): IApiLoadError => {
        NotifyHelper.Error(message)
        return baseAction(eApiActionTypeIds.LOAD_ERROR, message)
    },


}