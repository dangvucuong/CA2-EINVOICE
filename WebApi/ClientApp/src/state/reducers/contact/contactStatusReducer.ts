import { IContactStatusActionType } from "../../action-types/contact/IContactStatusActionType";
import { IContactStatusReducer } from "../../reducer-models/contact/IContactStatusReducer";
import { eReducerStatusBase } from "../../reducer-models/eReducerStatusBase";
import { eContactStatusActionTypeIds } from './../../action-types/contact/IContactStatusActionType';

const iniState: IContactStatusReducer = {
    status: eReducerStatusBase.is_not_initialization,
    contactStatuses: []
}
export const contactStatusReducer = (state: IContactStatusReducer = iniState, action: IContactStatusActionType): IContactStatusReducer => {
    switch (action.type) {
        case eContactStatusActionTypeIds.LOAD_START:
            return {
                ...state,
                status: eReducerStatusBase.is_loading
            }
        case eContactStatusActionTypeIds.LOAD_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_loaded,
                contactStatuses: action.payload
            }
        case eContactStatusActionTypeIds.LOAD_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_load_err,
            }

        default:
            return state;
    }
}