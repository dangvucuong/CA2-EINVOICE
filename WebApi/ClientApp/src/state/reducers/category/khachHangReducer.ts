import { eSortMode } from "../../../models/commons/eSortMode"
import { getPagingSummary } from "../../../models/responses/IBasePagingRespone"
import { IKhachHangActionType, eKhachHangActionTypeIds } from "../../action-types/category/IKhachHangActionType"
import { IKhachHangReducer } from "../../reducer-models/category/IKhachHangReducer"
import { eReducerStatusBase } from "../../reducer-models/eReducerStatusBase"

const iniState: IKhachHangReducer = {
    status: eReducerStatusBase.is_not_initialization,
    khachHangs: [],
    filter: {
        page_index: 0,
        page_size: 20,
        search_key: undefined,
        sort_by: "",
        sort_mode: eSortMode.DESC
    }
}
export const khachHangReducer = (state: IKhachHangReducer = iniState, action: IKhachHangActionType): IKhachHangReducer => {
    switch (action.type) {
        case eKhachHangActionTypeIds.LOAD_START:
            return {
                ...state,
                status: eReducerStatusBase.is_loading
            }
        case eKhachHangActionTypeIds.LOAD_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_loaded,
                khachHangs: action.payload.data,
                paging_res: getPagingSummary(action.payload)
            }
        case eKhachHangActionTypeIds.LOAD_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_load_err,
            }
        case eKhachHangActionTypeIds.SHOW_EDIT_MODAL:
            return {
                ...state,
                isShowEditModal: true,
                khachHangEditing: action.payload
            }
        case eKhachHangActionTypeIds.CLOSE_EDIT_MODAL:
            return {
                ...state,
                isShowEditModal: false
            }
        case eKhachHangActionTypeIds.SAVE_START:
            return {
                ...state,
                status: eReducerStatusBase.is_saving
            }
        case eKhachHangActionTypeIds.SAVE_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_saved,
                isShowEditModal: false
            }
        case eKhachHangActionTypeIds.SAVE_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_save_err,
            }
        case eKhachHangActionTypeIds.SHOW_DELETE_CONFIRM:
            return {
                ...state,
                isShowDeleteConfirm: true,
                khachHangEditing: action.payload
            }
        case eKhachHangActionTypeIds.CLOSE_DELETE_CONFIRM:
            return {
                ...state,
                isShowDeleteConfirm: false
            }
        case eKhachHangActionTypeIds.DELETE_START:
            return {
                ...state,
                status: eReducerStatusBase.is_deleting
            }
        case eKhachHangActionTypeIds.DELETE_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_deleted,
                isShowDeleteConfirm: false
            }
        case eKhachHangActionTypeIds.DELETE_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_delete_err,
            }
        case eKhachHangActionTypeIds.CHANGE_SELECTED_ID:
            return {
                ...state,
                khachHangSelectedId: action.payload
            }
        case eKhachHangActionTypeIds.CHANGE_FILTER:
            return {
                ...state,
                filter: action.payload
            }

        default:
            return state;
    }
}