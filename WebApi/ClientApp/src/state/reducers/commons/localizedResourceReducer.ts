import { ILocalizedResourceActionTypes, eLocalizedResourceActionTypeIds } from "../../action-types/commons/ILocalizedResourceActionType";
import { ILocalizedResourceReducer } from "../../reducer-models/commons/ILocalizedResourceReducer";
import { eReducerStatusBase } from "../../reducer-models/eReducerStatusBase";

const iniState: ILocalizedResourceReducer = {
    localized_resources: new Map<string, string>(),
    status: eReducerStatusBase.is_not_initialization,
    lan: localStorage.getItem("lan") === "en" ? "en" : "vi"
}
export const localizedResourceReducer = (state: ILocalizedResourceReducer = iniState, action: ILocalizedResourceActionTypes): ILocalizedResourceReducer => {
    switch (action.type) {
        case eLocalizedResourceActionTypeIds.LOAD_START:
            return {
                ...state,
                status: eReducerStatusBase.is_loading
            }
        case eLocalizedResourceActionTypeIds.LOAD_SUCCESS:

            return {
                ...state,
                status: eReducerStatusBase.is_loaded,
                localized_resources: action.payload
            }
        case eLocalizedResourceActionTypeIds.LOAD_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_load_err
            }
        case eLocalizedResourceActionTypeIds.CHANGE_LANGUAGE:
            return {
                ...state,
                lan: action.payload
            }

        default:
            return { ...state }
    }
}