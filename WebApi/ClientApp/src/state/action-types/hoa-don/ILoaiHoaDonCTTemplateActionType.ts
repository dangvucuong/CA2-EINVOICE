import { ILoaiHoaDonCTTemplate } from "../../../models/responses/hoa-don/ILoaiHoaDonCTTemplate";
import { IActionTypeBase } from "../IActionTypeBase";

export enum eLoaiHoaDonCTTemplateActionTypeIds {
    LOAD_START = "LOAI_HOADON_CT_TEMPLATE_LOAD_START",
    LOAD_SUCCESS = "LOAI_HOADON_CT_TEMPLATE_LOAD_SUCCESS",
    LOAD_ERROR = "LOAI_HOADON_CT_TEMPLATE_LOAD_ERROR",


}

export interface ILoaiHoaDonCTTemplateLoadStart extends IActionTypeBase<eLoaiHoaDonCTTemplateActionTypeIds.LOAD_START, undefined> { }
export interface ILoaiHoaDonCTTemplateLoadSuccess extends IActionTypeBase<eLoaiHoaDonCTTemplateActionTypeIds.LOAD_SUCCESS, ILoaiHoaDonCTTemplate[]> { }
export interface ILoaiHoaDonCTTemplateLoadError extends IActionTypeBase<eLoaiHoaDonCTTemplateActionTypeIds.LOAD_ERROR, string> { }


export type ILoaiHoaDonCTTemplateActionType = ILoaiHoaDonCTTemplateLoadStart | ILoaiHoaDonCTTemplateLoadSuccess | ILoaiHoaDonCTTemplateLoadError
