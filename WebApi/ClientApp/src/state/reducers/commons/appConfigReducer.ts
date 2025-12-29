import { IAppConfigActionType, eAppConfigActionTypeIds } from "../../action-types/commons/IAppConfigActionType"
import { IAppConfigReducer } from "../../reducer-models/commons/IAppConfigReducer"
import { eReducerStatusBase } from "../../reducer-models/eReducerStatusBase"

const iniState: IAppConfigReducer = {
    status: eReducerStatusBase.is_not_initialization,
}
export const appConfigReducer = (state: IAppConfigReducer = iniState, action: IAppConfigActionType): IAppConfigReducer => {
    switch (action.type) {
        case eAppConfigActionTypeIds.LOAD_START:
            return {
                ...state,
                status: eReducerStatusBase.is_loading
            }
        case eAppConfigActionTypeIds.LOAD_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_loaded,
                appConfig: action.payload
            }
        case eAppConfigActionTypeIds.LOAD_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_load_err
            }
        default:
            return {
                ...state
            }
    }
}