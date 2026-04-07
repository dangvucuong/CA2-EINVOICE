import { IAccountActionType, eAccountActionTypeIds } from '../../action-types/account/IAccountActionType';
import { IAccountReducer, eAccountReducerStatus } from './../../reducer-models/account/IAccountReducer';
const iniState: IAccountReducer = {

}
export const accountReducer = (state: IAccountReducer = iniState, action: IAccountActionType): IAccountReducer => {
    switch (action.type) {
        case eAccountActionTypeIds.GET_PROFILE:
            return {
                ...state,
                status: eAccountReducerStatus.is_getting_profile
            }
        case eAccountActionTypeIds.GET_PROFILE_SUCCESS:
            return {
                ...state,
                status: eAccountReducerStatus.is_get_profile_done,
                user: action.payload,
            }
        case eAccountActionTypeIds.GET_PROFILE_ERROR:
            return {
                ...state,
                status: eAccountReducerStatus.is_get_profile_done,
                user: undefined
            }
        case eAccountActionTypeIds.LOGIN_START:
            return {
                ...state,
                status: eAccountReducerStatus.is_logging_in
            }
        case eAccountActionTypeIds.LOGIN_SUCCESS:
            localStorage.setItem("last_active_time", new Date().getTime().toString())
            return {
                ...state,
                status: eAccountReducerStatus.is_log_in_success,
                user: action.payload.profile,
                is_verify_cert: action.payload.is_verify_cert
            }
        case eAccountActionTypeIds.LOGIN_ERROR:
            return {
                ...state,
                status: eAccountReducerStatus.is_log_in_error,
                user: undefined
            }
        case eAccountActionTypeIds.CHANGE_APP_SELECTED:
            return {
                ...state,
                appSelected: action.payload

            }

        default:
            return {
                ...state
            }
    }
}