import { eSortMode } from "../../../models/commons/eSortMode"
import { getPagingSummary } from "../../../models/responses/IBasePagingRespone"
import { IHoaDonActionType, eHoaDonActionTypeIds } from "../../action-types/hoa-don/IHoaDonActionType"
import { IHoaDonReducer } from "../../reducer-models/hoa-don/IHoaDonReducer"
import { eReducerStatusBase } from "../../reducer-models/eReducerStatusBase"

const iniState: IHoaDonReducer = {
    status: eReducerStatusBase.is_not_initialization,
    hoaDons: [],
    filter: {
        hoa_don_trang_thai_ids: [],
        loai_hoa_don_ct_id: 0,
        hoa_don_dang_ky_phat_hanh_mau_so: "",
        hoa_don_dang_ky_phat_hanh_ky_hieu: "",
        page_index: 0,
        page_size: 20,
        search_key: undefined,
        sort_by: "",
        sort_mode: eSortMode.DESC,
        
    }
}
export const hoaDonReducer = (state: IHoaDonReducer = iniState, action: IHoaDonActionType): IHoaDonReducer => {
    switch (action.type) {
        case eHoaDonActionTypeIds.LOAD_START:
            return {
                ...state,
                status: eReducerStatusBase.is_loading
            }
        case eHoaDonActionTypeIds.LOAD_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_loaded,
                hoaDons: action.payload.data,
                hoaDonSelectedIds: [],
                paging_res: getPagingSummary(action.payload)
            }
        case eHoaDonActionTypeIds.LOAD_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_load_err,
            }
        case eHoaDonActionTypeIds.SHOW_EDIT_MODAL:
            return {
                ...state,
                isShowEditModal: true,
                hoaDonEditing: action.payload
            }
        case eHoaDonActionTypeIds.CLOSE_EDIT_MODAL:
            return {
                ...state,
                isShowEditModal: false
            }
        case eHoaDonActionTypeIds.SAVE_START:
            return {
                ...state,
                status: eReducerStatusBase.is_saving
            }
        case eHoaDonActionTypeIds.SAVE_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_saved,
                isShowEditModal: false
            }
        case eHoaDonActionTypeIds.SAVE_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_save_err,
            }
        case eHoaDonActionTypeIds.SHOW_DELETE_CONFIRM:
            return {
                ...state,
                isShowDeleteConfirm: true,
                hoaDonEditing: action.payload
            }
        case eHoaDonActionTypeIds.CLOSE_DELETE_CONFIRM:
            return {
                ...state,
                isShowDeleteConfirm: false
            }
        case eHoaDonActionTypeIds.DELETE_START:
            return {
                ...state,
                status: eReducerStatusBase.is_deleting
            }
        case eHoaDonActionTypeIds.DELETE_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_deleted,
                isShowDeleteConfirm: false
            }
        case eHoaDonActionTypeIds.DELETE_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_delete_err,
            }
        case eHoaDonActionTypeIds.CHANGE_SELECTED_ID:
            return {
                ...state,
                hoaDonSelectedIds: action.payload
            }
        case eHoaDonActionTypeIds.CHANGE_FILTER:
            return {
                ...state,
                filter: action.payload
            }
        case eHoaDonActionTypeIds.SHOW_LOG_MODAL:
            return {
                ...state,
                isShowLogModal: true,
                hoaDonEditing: action.payload
            }
        case eHoaDonActionTypeIds.CLOSE_LOG_MODAL:
            return {
                ...state,
                isShowLogModal: false
            }

        default:
            return state;
    }
}