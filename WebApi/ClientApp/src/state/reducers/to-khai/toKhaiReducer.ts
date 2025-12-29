import { IToKhaiActionType, eToKhaiActionTypeIds } from "../../action-types/to-khai/IToKhaiActionType"
import { eReducerStatusBase } from "../../reducer-models/eReducerStatusBase"
import { IToKhaiReducer } from "../../reducer-models/to-khai/IToKhaiReducer"

const iniState: IToKhaiReducer = {
    status: eReducerStatusBase.is_not_initialization,
    toKhais: [],
    isShowLogModal: false,
    isShowDeleteConfirm:false
}
export const toKhaiReducer = (state: IToKhaiReducer = iniState, action: IToKhaiActionType): IToKhaiReducer => {
    switch (action.type) {
        case eToKhaiActionTypeIds.LOAD_START:
            return {
                ...state,
                status: eReducerStatusBase.is_loading
            }
        case eToKhaiActionTypeIds.LOAD_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_loaded,
                toKhais: action.payload,
            }
        case eToKhaiActionTypeIds.LOAD_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_load_err,
            }
        case eToKhaiActionTypeIds.SHOW_LOG_MODAL:
            return {
                ...state,
                isShowLogModal:true,
                toKhaiEditing: action.payload
            }
        case eToKhaiActionTypeIds.CLOSE_LOG_MODAL:
            return {
                ...state,
                isShowLogModal: false
            }
       
        case eToKhaiActionTypeIds.SHOW_DELETE_CONFIRM:
            return {
                ...state,
                isShowDeleteConfirm: true,
                toKhaiEditing: action.payload
            }
        case eToKhaiActionTypeIds.CLOSE_DELETE_CONFIRM:
            return {
                ...state,
                isShowDeleteConfirm: false
            }
        case eToKhaiActionTypeIds.DELETE_START:
            return {
                ...state,
                status: eReducerStatusBase.is_deleting
            }
        case eToKhaiActionTypeIds.DELETE_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_need_reload,
                isShowDeleteConfirm: false
            }
        case eToKhaiActionTypeIds.DELETE_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_delete_err,
            }
        

        default:
            return state;
    }
}