import { eSortMode } from "../../../models/commons/eSortMode"
import { getPagingSummary } from "../../../models/responses/IBasePagingRespone"
import { IDonViActionType, eDonViActionTypeIds } from "../../action-types/category/IDonViActionType"
import { IDonViReducer } from "../../reducer-models/category/IDonViReducer"
import { eReducerStatusBase } from "../../reducer-models/eReducerStatusBase"

const iniState: IDonViReducer = {
    status: eReducerStatusBase.is_not_initialization,
    donVis: [],
    filter: {
        page_index: 0,
        page_size: 20,
        search_key: undefined,
        sort_by: "",
        sort_mode: eSortMode.DESC
    }
}
export const donViReducer = (state: IDonViReducer = iniState, action: IDonViActionType): IDonViReducer => {
    switch (action.type) {
        case eDonViActionTypeIds.LOAD_START:
            return {
                ...state,
                status: eReducerStatusBase.is_loading
            }
        case eDonViActionTypeIds.LOAD_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_loaded,
                donVis: action.payload.data,
                paging_res: getPagingSummary(action.payload)
            }
        case eDonViActionTypeIds.LOAD_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_load_err,
            }
        case eDonViActionTypeIds.SHOW_EDIT_MODAL:
            return {
                ...state,
                isShowEditModal: true,
                donViEditing: action.payload
            }
        case eDonViActionTypeIds.CLOSE_EDIT_MODAL:
            return {
                ...state,
                isShowEditModal: false
            }
        case eDonViActionTypeIds.SAVE_START:
            return {
                ...state,
                status: eReducerStatusBase.is_saving
            }
        case eDonViActionTypeIds.SAVE_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_need_reload,
                isShowEditModal: false
            }
        case eDonViActionTypeIds.SAVE_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_save_err,
            }
        case eDonViActionTypeIds.SHOW_DELETE_CONFIRM:
            return {
                ...state,
                isShowDeleteConfirm: true,
                donViEditing: action.payload
            }
        case eDonViActionTypeIds.CLOSE_DELETE_CONFIRM:
            return {
                ...state,
                isShowDeleteConfirm: false
            }
        case eDonViActionTypeIds.DELETE_START:
            return {
                ...state,
                status: eReducerStatusBase.is_deleting
            }
        case eDonViActionTypeIds.DELETE_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_need_reload,
                isShowDeleteConfirm: false
            }
        case eDonViActionTypeIds.DELETE_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_delete_err,
            }
        case eDonViActionTypeIds.CHANGE_SELECTED_ID:
            return {
                ...state,
                donViSelectedId: action.payload
            }
        case eDonViActionTypeIds.CHANGE_FILTER:
            return {
                ...state,
                filter: action.payload,
                status: eReducerStatusBase.is_need_reload
            }

        default:
            return state;
    }
}