import { ILoaiHoaDon } from "../../../models/responses/hoa-don/ILoaiHoaDon";
import { eReducerStatusBase } from "../eReducerStatusBase";

export interface ILoaiHoaDonReducer {
    status: eReducerStatusBase,
    loaiHoaDons: ILoaiHoaDon[],  
}