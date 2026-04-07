import { IHoaDonDangKyPhatHanhActionType, eHoaDonDangKyPhatHanhActionTypeIds } from "../../action-types/hoa-don/IHoaDonDangKyPhatHanhActionType"
import { eReducerStatusBase } from "../../reducer-models/eReducerStatusBase"
import { IHoaDonDangKyPhatHanhReducer } from "../../reducer-models/hoa-don/IHoaDonDangKyPhatHanhReducer"

const iniState: IHoaDonDangKyPhatHanhReducer = {
    status: eReducerStatusBase.is_not_initialization,
    hoaDonDangKyPhatHanhs: []
}
export const hoaDonDangKyPhatHanhReducer = (state: IHoaDonDangKyPhatHanhReducer = iniState, action: IHoaDonDangKyPhatHanhActionType): IHoaDonDangKyPhatHanhReducer => {
    switch (action.type) {
        case eHoaDonDangKyPhatHanhActionTypeIds.LOAD_START:
            return {
                ...state,
                status: eReducerStatusBase.is_loading
            }
        case eHoaDonDangKyPhatHanhActionTypeIds.LOAD_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_loaded,
                hoaDonDangKyPhatHanhs: action.payload,
            }
        case eHoaDonDangKyPhatHanhActionTypeIds.LOAD_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_load_err,
            }
        case eHoaDonDangKyPhatHanhActionTypeIds.SHOW_EDIT_MODAL:
            return {
                ...state,
                isShowEditModal: true,
                hoaDonDangKyPhatHanhEditing: action.payload
            }
        case eHoaDonDangKyPhatHanhActionTypeIds.CLOSE_EDIT_MODAL:
            return {
                ...state,
                isShowEditModal: false
            }
        case eHoaDonDangKyPhatHanhActionTypeIds.SAVE_START:
            return {
                ...state,
                status: eReducerStatusBase.is_saving
            }
        case eHoaDonDangKyPhatHanhActionTypeIds.SAVE_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_saved,
                isShowEditModal: false
            }
        case eHoaDonDangKyPhatHanhActionTypeIds.SAVE_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_save_err,
            }
        case eHoaDonDangKyPhatHanhActionTypeIds.SHOW_DELETE_CONFIRM:
            return {
                ...state,
                isShowDeleteConfirm: true,
                hoaDonDangKyPhatHanhEditing: action.payload
            }
        case eHoaDonDangKyPhatHanhActionTypeIds.CLOSE_DELETE_CONFIRM:
            return {
                ...state,
                isShowDeleteConfirm: false
            }
        case eHoaDonDangKyPhatHanhActionTypeIds.DELETE_START:
            return {
                ...state,
                status: eReducerStatusBase.is_deleting
            }
        case eHoaDonDangKyPhatHanhActionTypeIds.DELETE_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_deleted,
                isShowDeleteConfirm: false
            }
        case eHoaDonDangKyPhatHanhActionTypeIds.DELETE_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_delete_err,
            }
        case eHoaDonDangKyPhatHanhActionTypeIds.CHANGE_SELECTED_ID:
            return {
                ...state,
                hoaDonDangKyPhatHanhSelectedId: action.payload
            }

        default:
            return state;
    }
}