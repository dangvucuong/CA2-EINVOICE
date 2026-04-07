import { IMenuActionType, eMenuActionTypeIds } from "../../action-types/user/IMenuActionType";
import { eReducerStatusBase } from "../../reducer-models/eReducerStatusBase";
import { IMenuReducer } from "../../reducer-models/user/IMenuReducer";
const iniState: IMenuReducer = {
    status: eReducerStatusBase.is_not_initialization,
    menus: []
}
export const menuReducer = (state: IMenuReducer = iniState, action: IMenuActionType): IMenuReducer => {
    switch (action.type) {
        case eMenuActionTypeIds.LOAD_START:
            return {
                ...state,
                status: eReducerStatusBase.is_loading
            }
        case eMenuActionTypeIds.LOAD_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_loaded,
                menus: action.payload

            }
        case eMenuActionTypeIds.LOAD_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_load_err
            }


        default:
            return state;
    }

}