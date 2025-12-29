import { ILoaiHoaDonCTTemplate } from "../../../models/responses/hoa-don/ILoaiHoaDonCTTemplate";
import { eReducerStatusBase } from "../eReducerStatusBase";

export interface ILoaiHoaDonCTTemplateReducer {
    status: eReducerStatusBase,
    loaiHoaDonCTTemplates: ILoaiHoaDonCTTemplate[],  
}