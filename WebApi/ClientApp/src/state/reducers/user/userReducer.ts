import { eSortMode } from "../../../models/commons/eSortMode";
import { getPagingSummary } from "../../../models/responses/IBasePagingRespone";
import { IUserActionType, eUserActionTypeIds } from "../../action-types/user/IUserActionType";
import { eReducerStatusBase } from "../../reducer-models/eReducerStatusBase";
import { IUserReducer, eUserReducerStatus } from "../../reducer-models/user/IUserReducer";
const iniState: IUserReducer = {
    status: eReducerStatusBase.is_not_initialization,
    users: [],
    filter: {
        page_index: 0,
        page_size: 20,
        search_key: undefined,
        sort_by: "",
        sort_mode: eSortMode.DESC
    }
}
export const userReducer = (state: IUserReducer = iniState, action: IUserActionType): IUserReducer => {
    switch (action.type) {
        case eUserActionTypeIds.LOAD_START:
            return {
                ...state,
                status: eReducerStatusBase.is_loading
            }
        case eUserActionTypeIds.LOAD_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_loaded,
                users: action.payload.data,
                paging_res: getPagingSummary(action.payload)

            }
        case eUserActionTypeIds.LOAD_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_load_err
            }
        case eUserActionTypeIds.LOAD_BY_DONVI_START:
            return {
                ...state,
                status: eReducerStatusBase.is_loading
            }
        case eUserActionTypeIds.LOAD_BY_DONVI_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_loaded,
                users: action.payload.data,
                paging_res: getPagingSummary(action.payload)

            }
        case eUserActionTypeIds.LOAD_BY_DONVI_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_load_err
            }
        case eUserActionTypeIds.CHANGE_FILTER:
            return {
                ...state,
                filter: action.payload
            }
        case eUserActionTypeIds.SHOW_EDIT_MODAL:
            return {
                ...state,
                isShowEditModal: true,
                userEditing: action.payload
            }
        case eUserActionTypeIds.CLOSE_EDIT_MODAL:
            return {
                ...state,
                isShowEditModal: false
            }
        case eUserActionTypeIds.LOAD_USER_FORM_START:
            return {
                ...state,
                status: eUserReducerStatus.is_loading_form
            }
        case eUserActionTypeIds.LOAD_USER_FORM_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_loaded,
                userEditingForm: action.payload
            }
        case eUserActionTypeIds.LOAD_USER_FORM_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_load_err
            }

        case eUserActionTypeIds.SAVE_USER_START:
            return {
                ...state,
                status: eReducerStatusBase.is_saving
            }
        case eUserActionTypeIds.SAVE_USER_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_saved,
                isShowEditModal: false,
                userEditing: undefined
            }
        case eUserActionTypeIds.SAVE_USER_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_save_err
            }
        case eUserActionTypeIds.SHOW_DELETE_CONFIRM:
            return {
                ...state,
                isShowDeleteConfirm: true,
                userEditing: action.payload
            }
        case eUserActionTypeIds.CLOSE_DELETE_CONFIRM:
            return {
                ...state,
                isShowDeleteConfirm: false
            }
        case eUserActionTypeIds.DELETE_START:
            return {
                ...state,
                status: eReducerStatusBase.is_deleting
            }
        case eUserActionTypeIds.DELETE_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_deleted,
                isShowDeleteConfirm: false
            }
        case eUserActionTypeIds.DELETE_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_delete_err,
            }
        default:
            return state;
    }

}