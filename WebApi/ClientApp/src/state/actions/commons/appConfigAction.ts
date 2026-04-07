import { IAppConfig } from "../../../models/responses/common/IAppConfig";
import { IAppConfigLoadError, IAppConfigLoadStart, IAppConfigLoadSuccess, eAppConfigActionTypeIds } from "../../action-types/commons/IAppConfigActionType";
import { baseAction } from "../baseAction";



export const appConfigAction = {
    loadStart: (): IAppConfigLoadStart =>
        baseAction(eAppConfigActionTypeIds.LOAD_START, undefined),
    loadSuccess: (payload: IAppConfig): IAppConfigLoadSuccess =>
        baseAction(eAppConfigActionTypeIds.LOAD_SUCCESS, payload),
    loadError: (message: string): IAppConfigLoadError =>
        baseAction(eAppConfigActionTypeIds.LOAD_ERROR, message),
}