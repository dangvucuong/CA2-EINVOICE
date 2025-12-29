import { ILoaiHoaDonCT } from "../../../models/responses/hoa-don/ILoaiHoaDonCT";
import { eReducerStatusBase } from "../eReducerStatusBase";

export interface ILoaiHoaDonCTReducer {
    status: eReducerStatusBase,
    loaiHoaDonCTs: ILoaiHoaDonCT[],  
}