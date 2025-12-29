import { eNavSubMode } from "../../../models/commons/eNavSubMode";
import { IMainLayoutActionTypes, eMainLayoutActionTypes } from "../../action-types/commons/IMainLayoutActionType";
import { IMainLayoutReducerModel } from "../../reducer-models/commons/IMainLayoutReducerModel";
import { eReducerStatusBase } from "../../reducer-models/eReducerStatusBase";
const iniState: IMainLayoutReducerModel = {
    navSubMode: window.innerWidth >= 1400 ? eNavSubMode.FULL : eNavSubMode.POPUP,
    status: eReducerStatusBase.is_not_initialization,
    isOpenNotifyOverlay: false
}
export const mainLayoutReducer = (state: IMainLayoutReducerModel = iniState, action: IMainLayoutActionTypes)
    : IMainLayoutReducerModel => {
    switch (action.type) {
        case eMainLayoutActionTypes.CHANGE_NAV_SUB_MODE:
            return {
                ...state,
                navSubMode: action.payload
            }
        case eMainLayoutActionTypes.SHOW_NOTIFY_OVERLAY:
            return {
                ...state,
                isOpenNotifyOverlay: action.payload
            }
        default:
            return {
                ...state
            }
    }
}