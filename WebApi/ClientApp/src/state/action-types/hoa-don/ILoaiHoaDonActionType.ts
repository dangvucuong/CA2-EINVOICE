import { ILoaiHoaDon } from "../../../models/responses/hoa-don/ILoaiHoaDon";
import { IActionTypeBase } from "../IActionTypeBase";

export enum eLoaiHoaDonActionTypeIds {
    LOAD_START = "LOAI_HOADON_LOAD_START",
    LOAD_SUCCESS = "LOAI_HOADON_LOAD_SUCCESS",
    LOAD_ERROR = "LOAI_HOADON_LOAD_ERROR",

}

export interface ILoaiHoaDonLoadStart extends IActionTypeBase<eLoaiHoaDonActionTypeIds.LOAD_START, undefined> { }
export interface ILoaiHoaDonLoadSuccess extends IActionTypeBase<eLoaiHoaDonActionTypeIds.LOAD_SUCCESS, ILoaiHoaDon[]> { }
export interface ILoaiHoaDonLoadError extends IActionTypeBase<eLoaiHoaDonActionTypeIds.LOAD_ERROR, string> { }


export type ILoaiHoaDonActionType = ILoaiHoaDonLoadStart | ILoaiHoaDonLoadSuccess | ILoaiHoaDonLoadError
