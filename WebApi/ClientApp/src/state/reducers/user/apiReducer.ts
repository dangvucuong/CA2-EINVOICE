import { IApiActionType, eApiActionTypeIds } from "../../action-types/user/IApiActionType";
import { eReducerStatusBase } from "../../reducer-models/eReducerStatusBase";
import { IApiReducer } from "../../reducer-models/user/IApiReducer";
const iniState: IApiReducer = {
    status: eReducerStatusBase.is_not_initialization,
    apis: []
}
export const apiReducer = (state: IApiReducer = iniState, action: IApiActionType): IApiReducer => {
    switch (action.type) {
        case eApiActionTypeIds.LOAD_START:
            return {
                ...state,
                status: eReducerStatusBase.is_loading
            }
        case eApiActionTypeIds.LOAD_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_loaded,
                apis: action.payload

            }
        case eApiActionTypeIds.LOAD_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_load_err
            }


        default:
            return state;
    }

}