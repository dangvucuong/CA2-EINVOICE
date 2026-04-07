import { ILoaiHoaDonCTActionType, eLoaiHoaDonCTActionTypeIds } from "../../action-types/hoa-don/ILoaiHoaDonCTActionType"
import { eReducerStatusBase } from "../../reducer-models/eReducerStatusBase"
import { ILoaiHoaDonCTReducer } from "../../reducer-models/hoa-don/ILoaiHoaDonCTReducer"

const iniState: ILoaiHoaDonCTReducer = {
    status: eReducerStatusBase.is_not_initialization,
    loaiHoaDonCTs: []
}
export const loaiHoaDonCTReducer = (state: ILoaiHoaDonCTReducer = iniState, action: ILoaiHoaDonCTActionType): ILoaiHoaDonCTReducer => {
    switch (action.type) {
        case eLoaiHoaDonCTActionTypeIds.LOAD_START:
            return {
                ...state,
                status: eReducerStatusBase.is_loading
            }
        case eLoaiHoaDonCTActionTypeIds.LOAD_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_loaded,
                loaiHoaDonCTs: action.payload,
            }
        case eLoaiHoaDonCTActionTypeIds.LOAD_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_load_err,
            }

        default:
            return state;
    }
}