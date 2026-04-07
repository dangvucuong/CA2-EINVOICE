import { ILoaiHoaDonCTTemplateActionType, eLoaiHoaDonCTTemplateActionTypeIds } from "../../action-types/hoa-don/ILoaiHoaDonCTTemplateActionType"
import { eReducerStatusBase } from "../../reducer-models/eReducerStatusBase"
import { ILoaiHoaDonCTTemplateReducer } from "../../reducer-models/hoa-don/ILoaiHoaDonCTTemplateReducer"

const iniState: ILoaiHoaDonCTTemplateReducer = {
    status: eReducerStatusBase.is_not_initialization,
    loaiHoaDonCTTemplates: []
}
export const loaiHoaDonCTTemplateReducer = (state: ILoaiHoaDonCTTemplateReducer = iniState, action: ILoaiHoaDonCTTemplateActionType): ILoaiHoaDonCTTemplateReducer => {
    switch (action.type) {
        case eLoaiHoaDonCTTemplateActionTypeIds.LOAD_START:
            return {
                ...state,
                status: eReducerStatusBase.is_loading
            }
        case eLoaiHoaDonCTTemplateActionTypeIds.LOAD_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_loaded,
                loaiHoaDonCTTemplates: action.payload,
            }
        case eLoaiHoaDonCTTemplateActionTypeIds.LOAD_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_load_err,
            }

        default:
            return state;
    }
}