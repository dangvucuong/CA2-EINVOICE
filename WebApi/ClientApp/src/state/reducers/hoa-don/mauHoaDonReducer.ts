import { IMauHoaDonActionType, eMauHoaDonActionTypeIds } from "../../action-types/hoa-don/IMauHoaDonActionType"
import { eReducerStatusBase } from "../../reducer-models/eReducerStatusBase"
import { IMauHoaDonReducer } from "../../reducer-models/hoa-don/IMauHoaDonReducer"

const iniState: IMauHoaDonReducer = {
    status: eReducerStatusBase.is_not_initialization,
    mauHoaDons: []
}
export const mauHoaDonReducer = (state: IMauHoaDonReducer = iniState, action: IMauHoaDonActionType): IMauHoaDonReducer => {
    switch (action.type) {
        case eMauHoaDonActionTypeIds.LOAD_START:
            return {
                ...state,
                status: eReducerStatusBase.is_loading
            }
        case eMauHoaDonActionTypeIds.LOAD_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_loaded,
                mauHoaDons: action.payload,
            }
        case eMauHoaDonActionTypeIds.LOAD_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_load_err,
            }
        case eMauHoaDonActionTypeIds.SHOW_EDIT_MODAL:
            return {
                ...state,
                isShowEditModal: true,
                mauHoaDonEditing: action.payload
            }
        case eMauHoaDonActionTypeIds.CLOSE_EDIT_MODAL:
            return {
                ...state,
                isShowEditModal: false
            }
        case eMauHoaDonActionTypeIds.SAVE_START:
            return {
                ...state,
                status: eReducerStatusBase.is_saving
            }
        case eMauHoaDonActionTypeIds.SAVE_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_saved,
                isShowEditModal: false
            }
        case eMauHoaDonActionTypeIds.SAVE_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_save_err,
            }
        case eMauHoaDonActionTypeIds.SHOW_DELETE_CONFIRM:
            return {
                ...state,
                isShowDeleteConfirm: true,
                mauHoaDonEditing: action.payload
            }
        case eMauHoaDonActionTypeIds.CLOSE_DELETE_CONFIRM:
            return {
                ...state,
                isShowDeleteConfirm: false
            }
        case eMauHoaDonActionTypeIds.DELETE_START:
            return {
                ...state,
                status: eReducerStatusBase.is_deleting
            }
        case eMauHoaDonActionTypeIds.DELETE_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_deleted,
                isShowDeleteConfirm: false
            }
        case eMauHoaDonActionTypeIds.DELETE_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_delete_err,
            }
        case eMauHoaDonActionTypeIds.CHANGE_SELECTED_ID:
            return {
                ...state,
                mauHoaDonSelectedId: action.payload
            }
        

        default:
            return state;
    }
}