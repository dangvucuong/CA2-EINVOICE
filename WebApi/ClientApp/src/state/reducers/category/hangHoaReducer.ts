import { eSortMode } from "../../../models/commons/eSortMode"
import { getPagingSummary } from "../../../models/responses/IBasePagingRespone"
import { IHangHoaActionType, eHangHoaActionTypeIds } from "../../action-types/category/IHangHoaActionType"
import { IHangHoaReducer } from "../../reducer-models/category/IHangHoaReducer"
import { eReducerStatusBase } from "../../reducer-models/eReducerStatusBase"

const iniState: IHangHoaReducer = {
    status: eReducerStatusBase.is_not_initialization,
    hangHoas: [],
    filter: {
        page_index: 0,
        page_size: 20,
        search_key: undefined,
        sort_by: "",
        sort_mode: eSortMode.DESC
    }
}
export const hangHoaReducer = (state: IHangHoaReducer = iniState, action: IHangHoaActionType): IHangHoaReducer => {
    switch (action.type) {
        case eHangHoaActionTypeIds.LOAD_START:
            return {
                ...state,
                status: eReducerStatusBase.is_loading
            }
        case eHangHoaActionTypeIds.LOAD_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_loaded,
                hangHoas: action.payload.data,
                paging_res: getPagingSummary(action.payload)
            }
        case eHangHoaActionTypeIds.LOAD_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_load_err,
            }
        case eHangHoaActionTypeIds.SHOW_EDIT_MODAL:
            return {
                ...state,
                isShowEditModal: true,
                hangHoaEditing: action.payload
            }
        case eHangHoaActionTypeIds.CLOSE_EDIT_MODAL:
            return {
                ...state,
                isShowEditModal: false
            }
        case eHangHoaActionTypeIds.SAVE_START:
            return {
                ...state,
                status: eReducerStatusBase.is_saving
            }
        case eHangHoaActionTypeIds.SAVE_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_saved,
                isShowEditModal: false
            }
        case eHangHoaActionTypeIds.SAVE_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_save_err,
            }
        case eHangHoaActionTypeIds.SHOW_DELETE_CONFIRM:
            return {
                ...state,
                isShowDeleteConfirm: true,
                hangHoaEditing: action.payload
            }
        case eHangHoaActionTypeIds.CLOSE_DELETE_CONFIRM:
            return {
                ...state,
                isShowDeleteConfirm: false
            }
        case eHangHoaActionTypeIds.DELETE_START:
            return {
                ...state,
                status: eReducerStatusBase.is_deleting
            }
        case eHangHoaActionTypeIds.DELETE_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_deleted,
                isShowDeleteConfirm: false
            }
        case eHangHoaActionTypeIds.DELETE_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_delete_err,
            }
        case eHangHoaActionTypeIds.CHANGE_SELECTED_ID:
            return {
                ...state,
                hangHoaSelectedId: action.payload
            }
        case eHangHoaActionTypeIds.CHANGE_FILTER:
            return {
                ...state,
                filter: action.payload
            }

        default:
            return state;
    }
}