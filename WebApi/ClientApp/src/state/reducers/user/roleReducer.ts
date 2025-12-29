import { IRoleActionType, eRoleActionTypeIds } from "../../action-types/user/IRoleActionType";
import { eReducerStatusBase } from "../../reducer-models/eReducerStatusBase";
import { IRoleReducer } from "../../reducer-models/user/IRoleReducer";
const iniState: IRoleReducer = {
    status: eReducerStatusBase.is_not_initialization,
    roles: []
}
export const roleReducer = (state: IRoleReducer = iniState, action: IRoleActionType): IRoleReducer => {
    switch (action.type) {
        case eRoleActionTypeIds.LOAD_START:
            return {
                ...state,
                status: eReducerStatusBase.is_loading
            }
        case eRoleActionTypeIds.LOAD_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_loaded,
                roles: action.payload,

            }
        case eRoleActionTypeIds.LOAD_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_load_err
            }
        case eRoleActionTypeIds.CHANGE_EDITING:
            return {
                ...state,
               roleEditing: action.payload
            }

        case eRoleActionTypeIds.SAVE_START:
            return {
                ...state,
                status: eReducerStatusBase.is_saving
            }
        case eRoleActionTypeIds.SAVE_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_saved,

            }
        case eRoleActionTypeIds.SAVE_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_save_err
            }

        case eRoleActionTypeIds.DELETE_START:
            return {
                ...state,
                status: eReducerStatusBase.is_deleting
            }
        case eRoleActionTypeIds.DELETE_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_deleted,

            }
        case eRoleActionTypeIds.DELETE_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_delete_err
            }

        default:
            return state;
    }

}