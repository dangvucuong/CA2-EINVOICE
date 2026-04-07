import { ILoaiHoaDonActionType, eLoaiHoaDonActionTypeIds } from "../../action-types/hoa-don/ILoaiHoaDonActionType"
import { eReducerStatusBase } from "../../reducer-models/eReducerStatusBase"
import { ILoaiHoaDonReducer } from "../../reducer-models/hoa-don/ILoaiHoaDonReducer"

const iniState: ILoaiHoaDonReducer = {
    status: eReducerStatusBase.is_not_initialization,
    loaiHoaDons: []
}
export const loaiHoaDonReducer = (state: ILoaiHoaDonReducer = iniState, action: ILoaiHoaDonActionType): ILoaiHoaDonReducer => {
    switch (action.type) {
        case eLoaiHoaDonActionTypeIds.LOAD_START:
            return {
                ...state,
                status: eReducerStatusBase.is_loading
            }
        case eLoaiHoaDonActionTypeIds.LOAD_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_loaded,
                loaiHoaDons: action.payload,
            }
        case eLoaiHoaDonActionTypeIds.LOAD_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_load_err,
            }

        default:
            return state;
    }
}