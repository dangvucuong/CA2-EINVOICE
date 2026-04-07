import { ISubSystemActionType, eSubSystemTypeIds } from "../../action-types/user/ISubSystemType";
import { eReducerStatusBase } from "../../reducer-models/eReducerStatusBase";
import { ISubSystemReducer } from "../../reducer-models/user/ISubSystemReducer";
const iniState: ISubSystemReducer = {
    status: eReducerStatusBase.is_not_initialization,
    subSystemSelectedId: 0,
    subSystems: []
}
export const subSystemReducer = (state: ISubSystemReducer = iniState, action: ISubSystemActionType): ISubSystemReducer => {
    switch (action.type) {
        case eSubSystemTypeIds.LOAD_START:
            return {
                ...state,
                status: eReducerStatusBase.is_loading
            }
        case eSubSystemTypeIds.LOAD_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_loaded,
                subSystems: action.payload

            }
        case eSubSystemTypeIds.LOAD_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_load_err
            }

        case eSubSystemTypeIds.CHANGE_EDITING:
            return {
                ...state,
                subSystemSelectedId: action.payload
            }

        default:
            return state;
    }

}