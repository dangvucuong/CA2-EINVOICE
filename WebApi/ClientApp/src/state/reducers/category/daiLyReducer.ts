import { eSortMode } from "../../../models/commons/eSortMode"
import { getPagingSummary } from "../../../models/responses/IBasePagingRespone"
import { IDaiLyActionType, eDaiLyActionTypeIds } from "../../action-types/category/IDaiLyActionType"
import { IDaiLyReducer } from "../../reducer-models/category/IDaiLyReducer"
import { eReducerStatusBase } from "../../reducer-models/eReducerStatusBase"

const iniState: IDaiLyReducer = {
    status: eReducerStatusBase.is_not_initialization,
    daiLys: [],
    filter: {
        page_index: 0,
        page_size: 20,
        search_key: undefined,
        sort_by: "",
        sort_mode: eSortMode.DESC
    }
}
export const daiLyReducer = (state: IDaiLyReducer = iniState, action: IDaiLyActionType): IDaiLyReducer => {
    switch (action.type) {
        case eDaiLyActionTypeIds.LOAD_START:
            return {
                ...state,
                status: eReducerStatusBase.is_loading
            }
        case eDaiLyActionTypeIds.LOAD_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_loaded,
                daiLys: action.payload.data,
                paging_res: getPagingSummary(action.payload)
            }
        case eDaiLyActionTypeIds.LOAD_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_load_err,
            }
        case eDaiLyActionTypeIds.SHOW_EDIT_MODAL:
            return {
                ...state,
                isShowEditModal: true,
                daiLyEditing: action.payload
            }
        case eDaiLyActionTypeIds.CLOSE_EDIT_MODAL:
            return {
                ...state,
                isShowEditModal: false
            }
        case eDaiLyActionTypeIds.SAVE_START:
            return {
                ...state,
                status: eReducerStatusBase.is_saving
            }
        case eDaiLyActionTypeIds.SAVE_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_saved,
                isShowEditModal: false
            }
        case eDaiLyActionTypeIds.SAVE_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_save_err,
            }
        case eDaiLyActionTypeIds.SHOW_DELETE_CONFIRM:
            return {
                ...state,
                isShowDeleteConfirm: true,
                daiLyEditing: action.payload
            }
        case eDaiLyActionTypeIds.CLOSE_DELETE_CONFIRM:
            return {
                ...state,
                isShowDeleteConfirm: false
            }
        case eDaiLyActionTypeIds.DELETE_START:
            return {
                ...state,
                status: eReducerStatusBase.is_deleting
            }
        case eDaiLyActionTypeIds.DELETE_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_deleted,
                isShowDeleteConfirm: false
            }
        case eDaiLyActionTypeIds.DELETE_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_delete_err,
            }
        case eDaiLyActionTypeIds.CHANGE_SELECTED_ID:
            return {
                ...state,
                daiLySelectedId: action.payload
            }
        case eDaiLyActionTypeIds.CHANGE_FILTER:
            return {
                ...state,
                filter: action.payload
            }

        default:
            return state;
    }
}