import { eHoaDonTrangThai } from "../../../models/commons/eHoaDonTrangThai"
import { eSortMode } from "../../../models/commons/eSortMode"
import { getPagingSummary } from "../../../models/responses/IBasePagingRespone"
import { IHoaDonSelectBoxActionType, eHoaDonSelectBoxActionTypeIds } from "../../action-types/hoa-don/IHoaDonSelectBoxActionType"
import { eReducerStatusBase } from "../../reducer-models/eReducerStatusBase"
import { IHoaDonSelectBoxReducer } from "../../reducer-models/hoa-don/IHoaDonSelectBoxReducer"

const iniState: IHoaDonSelectBoxReducer = {
    status: eReducerStatusBase.is_not_initialization,
    hoaDons: [],
    filter: {
        hoa_don_trang_thai_ids: [eHoaDonTrangThai.DA_PHAT_HANH],
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
export const hoaDonSelectBoxReducer = (state: IHoaDonSelectBoxReducer = iniState, action: IHoaDonSelectBoxActionType): IHoaDonSelectBoxReducer => {
    switch (action.type) {
        case eHoaDonSelectBoxActionTypeIds.LOAD_START:
            return {
                ...state,
                status: eReducerStatusBase.is_loading
            }
        case eHoaDonSelectBoxActionTypeIds.LOAD_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_loaded,
                hoaDons: action.payload.data,
                hoaDonSelectedIds: [],
                paging_res: getPagingSummary(action.payload)
            }
        case eHoaDonSelectBoxActionTypeIds.LOAD_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_load_err,
            }
    
        case eHoaDonSelectBoxActionTypeIds.CHANGE_SELECTED_ID:
            return {
                ...state,
                hoaDonSelectedIds: action.payload
            }
        case eHoaDonSelectBoxActionTypeIds.CHANGE_FILTER:
            return {
                ...state,
                filter: action.payload,
                status: eReducerStatusBase.is_need_reload
            }


        default:
            return state;
    }
}