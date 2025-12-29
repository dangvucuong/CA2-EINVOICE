import { ICompanySizeActionType, eCompanySizeActionTypeIds } from "../../action-types/contact/ICompanySizeActionType";
import { ICompanySizeReducer } from "../../reducer-models/contact/ICompanySizeReducer";
import { eReducerStatusBase } from "../../reducer-models/eReducerStatusBase";

const iniState: ICompanySizeReducer = {
    status: eReducerStatusBase.is_not_initialization,
    companySizes: []
}
export const companySizeReducer = (state: ICompanySizeReducer = iniState, action: ICompanySizeActionType): ICompanySizeReducer => {
    switch (action.type) {
        case eCompanySizeActionTypeIds.LOAD_START:
            return {
                ...state,
                status: eReducerStatusBase.is_loading
            }
        case eCompanySizeActionTypeIds.LOAD_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_loaded,
                companySizes: action.payload
            }
        case eCompanySizeActionTypeIds.LOAD_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_load_err,
            }

        default:
            return state;
    }
}