import { ILoaiHoaDonCT } from "../../../models/responses/hoa-don/ILoaiHoaDonCT";
import { IActionTypeBase } from "../IActionTypeBase";

export enum eLoaiHoaDonCTActionTypeIds {
    LOAD_START = "LOAI_HOADON_CT_LOAD_START",
    LOAD_SUCCESS = "LOAI_HOADON_CT_LOAD_SUCCESS",
    LOAD_ERROR = "LOAI_HOADON_CT_LOAD_ERROR",


}

export interface ILoaiHoaDonCTLoadStart extends IActionTypeBase<eLoaiHoaDonCTActionTypeIds.LOAD_START, undefined> { }
export interface ILoaiHoaDonCTLoadSuccess extends IActionTypeBase<eLoaiHoaDonCTActionTypeIds.LOAD_SUCCESS, ILoaiHoaDonCT[]> { }
export interface ILoaiHoaDonCTLoadError extends IActionTypeBase<eLoaiHoaDonCTActionTypeIds.LOAD_ERROR, string> { }


export type ILoaiHoaDonCTActionType = ILoaiHoaDonCTLoadStart | ILoaiHoaDonCTLoadSuccess | ILoaiHoaDonCTLoadError
