import { IRoleApiActionType, eRoleApiActionTypeIds } from "../../action-types/user/IRoleApiActionType";
import { eReducerStatusBase } from "../../reducer-models/eReducerStatusBase";
import { IRoleApiReducer } from "../../reducer-models/user/IRoleApiReducer";
const iniState: IRoleApiReducer = {
    status: eReducerStatusBase.is_not_initialization,
    roleApis: []
}
export const roleApiReducer = (state: IRoleApiReducer = iniState, action: IRoleApiActionType): IRoleApiReducer => {
    switch (action.type) {
        case eRoleApiActionTypeIds.LOAD_START:
            return {
                ...state,
                status: eReducerStatusBase.is_loading
            }
        case eRoleApiActionTypeIds.LOAD_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_loaded,
                roleApis: action.payload

            }
        case eRoleApiActionTypeIds.LOAD_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_load_err
            }

        case eRoleApiActionTypeIds.ADD_API_START:
            return {
                ...state,
                status: eReducerStatusBase.is_saving
            }
        case eRoleApiActionTypeIds.ADD_API_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_saved,

            }
        case eRoleApiActionTypeIds.ADD_API_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_save_err
            }


        case eRoleApiActionTypeIds.REMOVE_API_START:
            return {
                ...state,
                status: eReducerStatusBase.is_deleting
            }
        case eRoleApiActionTypeIds.REMOVE_API_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_deleted,

            }
        case eRoleApiActionTypeIds.REMOVE_API_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_deleted
            }


        default:
            return state;
    }

}