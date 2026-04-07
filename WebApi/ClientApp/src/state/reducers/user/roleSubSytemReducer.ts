import { IRoleSubSystemActionType, eRoleSubSystemTypeIds } from "../../action-types/user/IRoleSubSystemType";
import { eReducerStatusBase } from "../../reducer-models/eReducerStatusBase";
import { IRoleSubSystemReducer } from "../../reducer-models/user/IRoleSubSystemReducer";
const iniState: IRoleSubSystemReducer = {
    status: eReducerStatusBase.is_not_initialization,
    roleSubSystems: []
}
export const roleSubSystemReducer = (state: IRoleSubSystemReducer = iniState, action: IRoleSubSystemActionType): IRoleSubSystemReducer => {
    switch (action.type) {
        case eRoleSubSystemTypeIds.LOAD_START:
            return {
                ...state,
                status: eReducerStatusBase.is_loading
            }
        case eRoleSubSystemTypeIds.LOAD_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_loaded,
                roleSubSystems: action.payload

            }
        case eRoleSubSystemTypeIds.LOAD_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_load_err
            }

        case eRoleSubSystemTypeIds.ADD_SUBSYTEM_START:
            return {
                ...state,
                status: eReducerStatusBase.is_saving
            }
        case eRoleSubSystemTypeIds.ADD_SUBSYTEM_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_saved,

            }
        case eRoleSubSystemTypeIds.ADD_SUBSYTEM_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_save_err
            }


        case eRoleSubSystemTypeIds.REMOVE_SUBSYSTEM_START:
            return {
                ...state,
                status: eReducerStatusBase.is_deleting
            }
        case eRoleSubSystemTypeIds.REMOVE_SUBSYSTEM_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_deleted,

            }
        case eRoleSubSystemTypeIds.REMOVE_SUBSYSTEM_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_deleted
            }


        default:
            return state;
    }

}