import { eSortMode } from "../../../models/commons/eSortMode"
import { getPagingSummary } from "../../../models/responses/IBasePagingRespone"
import { IContactActionType, eContactActionTypeIds } from "../../action-types/contact/IContactActionType"
import { IContactReducer } from "../../reducer-models/contact/IContactReducer"
import { eReducerStatusBase } from "../../reducer-models/eReducerStatusBase"

const iniState: IContactReducer = {
    status: eReducerStatusBase.is_not_initialization,
    contacts: [],
    filter: {
        page_index: 0,
        page_size: 20,
        search_key: undefined,
        sort_by: "",
        sort_mode: eSortMode.DESC,
        contact_status_id:0
    }
}
export const contactReducer = (state: IContactReducer = iniState, action: IContactActionType): IContactReducer => {
    switch (action.type) {
        case eContactActionTypeIds.LOAD_START:
            return {
                ...state,
                status: eReducerStatusBase.is_loading
            }
        case eContactActionTypeIds.LOAD_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_loaded,
                contacts: action.payload.data,
                paging_res: getPagingSummary(action.payload)
            }
        case eContactActionTypeIds.LOAD_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_load_err,
            }
        case eContactActionTypeIds.SHOW_EDIT_MODAL:
            return {
                ...state,
                isShowEditModal: true,
                contactEditing: action.payload
            }
        case eContactActionTypeIds.CLOSE_EDIT_MODAL:
            return {
                ...state,
                isShowEditModal: false
            }
        case eContactActionTypeIds.SAVE_START:
            return {
                ...state,
                status: eReducerStatusBase.is_saving
            }
        case eContactActionTypeIds.SAVE_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_saved,
                isShowEditModal: false
            }
        case eContactActionTypeIds.SAVE_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_save_err,
            }
        case eContactActionTypeIds.SHOW_DELETE_CONFIRM:
            return {
                ...state,
                isShowDeleteConfirm: true,
                contactEditing: action.payload
            }
        case eContactActionTypeIds.CLOSE_DELETE_CONFIRM:
            return {
                ...state,
                isShowDeleteConfirm: false
            }
        case eContactActionTypeIds.DELETE_START:
            return {
                ...state,
                status: eReducerStatusBase.is_deleting
            }
        case eContactActionTypeIds.DELETE_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_deleted,
                isShowDeleteConfirm: false
            }
        case eContactActionTypeIds.DELETE_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_delete_err,
            }
        case eContactActionTypeIds.CHANGE_SELECTED_ID:
            return {
                ...state,
                contactSelectedId: action.payload
            }
        case eContactActionTypeIds.CHANGE_FILTER:
            return {
                ...state,
                filter: action.payload
            }

        default:
            return state;
    }
}