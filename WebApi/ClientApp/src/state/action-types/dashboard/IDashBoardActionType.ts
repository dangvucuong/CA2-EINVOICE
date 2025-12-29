
import { IHoaDonTrangThaiSummaryRequest } from "../../../models/requests/dashboard/IHoaDonTrangThaiSummaryRequest";
import { IDonViSoLuongChuKySoSummary } from "../../../models/responses/dashboard/IDonViSoLuongChuKySoSummary";
import { IHoaDonLichSuPhatHanhItem } from "../../../models/responses/dashboard/IHoaDonLichSuPhatHanhItem";
import { IHoaDonTrangThaiSummary } from "../../../models/responses/dashboard/IHoaDonTrangThaiSummary";
import { IActionTypeBase } from "../IActionTypeBase";

export enum eDashBoardActionType {
    SELECT_TONG_SO_HOA_DON_REPORT_START = "DASHBOARD_SELECT_TONG_SO_HOA_DON_REPORT_START",
    SELECT_TONG_SO_HOA_DON_REPORT_SUCCESS = "DASHBOARD_SELECT_TONG_SO_HOA_DON_REPORT_SUCCESS",
    SELECT_TONG_SO_HOA_DON_REPORT_ERROR = "DASHBOARD_SELECT_TONG_SO_HOA_DON_REPORT_ERROR",

    CHANGE_TRANGTHAI_REPORT_FILTER = "DASHBOARD_CHANGE_TRANGTHAI_REPORT_FILTER",
    SELECT_TRANGTHAI_REPORT_START = "DASHBOARD_SELECT_TRANGTHAI_REPORT_START",
    SELECT_TRANGTHAI_REPORT_SUCCESS = "DASHBOARD_SELECT_TRANGTHAI_REPORT_SUCCESS",
    SELECT_TRANGTHAI_REPORT_ERROR = "DASHBOARD_SELECT_TRANGTHAI_REPORT_ERROR",

    SELECT_TRANGTHAI_ALL_REPORT_START = "DASHBOARD_SELECT_TRANGTHAI_ALL_REPORT_START",
    SELECT_TRANGTHAI_ALL_REPORT_SUCCESS = "DASHBOARD_SELECT_TRANGTHAI_ALL_REPORT_SUCCESS",
    SELECT_TRANGTHAI_ALL_REPORT_ERROR = "DASHBOARD_SELECT_TRANGTHAI_ALL_REPORT_ERROR",

    CHANGE_LICH_SU_THEO_NGAY_REPORT_FILTER = "DASHBOARD_CHANGE_LICH_SU_THEO_NGAY_REPORT_FILTER",
    SELECT_LICH_SU_THEO_NGAY_REPORT_START = "DASHBOARD_SELECT_LICH_SU_THEO_NGAY_REPORT_START",
    SELECT_LICH_SU_THEO_NGAY_REPORT_SUCCESS = "DASHBOARD_SELECT_LICH_SU_THEO_NGAY_REPORT_SUCCESS",
    SELECT_LICH_SU_THEO_NGAY_REPORT_ERROR = "DASHBOARD_SELECT_LICH_SU_THEO_NGAY_REPORT_ERROR",

}


export interface IDashBoardTongSoHoaDonReportLoadStart extends IActionTypeBase<eDashBoardActionType.SELECT_TONG_SO_HOA_DON_REPORT_START, undefined> { }
export interface IDashBoardTongSoHoaDonReportLoadSuccess extends IActionTypeBase<eDashBoardActionType.SELECT_TONG_SO_HOA_DON_REPORT_SUCCESS, IDonViSoLuongChuKySoSummary> { }
export interface IDashBoardTongSoHoaDonReportLoadError extends IActionTypeBase<eDashBoardActionType.SELECT_TONG_SO_HOA_DON_REPORT_ERROR, string> { }

export interface IDashBoardChangeTrangThaiReportFilter extends IActionTypeBase<eDashBoardActionType.CHANGE_TRANGTHAI_REPORT_FILTER, IHoaDonTrangThaiSummaryRequest> { }

export interface IDashBoardTrangThaiReportLoadStart extends IActionTypeBase<eDashBoardActionType.SELECT_TRANGTHAI_REPORT_START, IHoaDonTrangThaiSummaryRequest> { }
export interface IDashBoardTrangThaiReportLoadSuccess extends IActionTypeBase<eDashBoardActionType.SELECT_TRANGTHAI_REPORT_SUCCESS, IHoaDonTrangThaiSummary[]> { }
export interface IDashBoardTrangThaiReportLoadError extends IActionTypeBase<eDashBoardActionType.SELECT_TRANGTHAI_REPORT_ERROR, string> { }

export interface IDashBoardTrangThaiReportLoadAllStart extends IActionTypeBase<eDashBoardActionType.SELECT_TRANGTHAI_ALL_REPORT_START, undefined> { }
export interface IDashBoardTrangThaiReportLoadAllSuccess extends IActionTypeBase<eDashBoardActionType.SELECT_TRANGTHAI_ALL_REPORT_SUCCESS, IHoaDonTrangThaiSummary[]> { }
export interface IDashBoardTrangThaiReportLoadAllError extends IActionTypeBase<eDashBoardActionType.SELECT_TRANGTHAI_ALL_REPORT_ERROR, string> { }


export interface IDashBoardChangeLichSuTheoNgayReportFilter extends IActionTypeBase<eDashBoardActionType.CHANGE_LICH_SU_THEO_NGAY_REPORT_FILTER, IHoaDonTrangThaiSummaryRequest> { }

export interface IDashBoardLichSuTheoNgayReportLoadStart extends IActionTypeBase<eDashBoardActionType.SELECT_LICH_SU_THEO_NGAY_REPORT_START, IHoaDonTrangThaiSummaryRequest> { }
export interface IDashBoardLichSuTheoNgayReportLoadSuccess extends IActionTypeBase<eDashBoardActionType.SELECT_LICH_SU_THEO_NGAY_REPORT_SUCCESS, IHoaDonLichSuPhatHanhItem[]> { }
export interface IDashBoardLichSuTheoNgayReportLoadError extends IActionTypeBase<eDashBoardActionType.SELECT_LICH_SU_THEO_NGAY_REPORT_ERROR, string> { }


export type IDashBoardActionTypes = IDashBoardChangeTrangThaiReportFilter |
    IDashBoardTrangThaiReportLoadStart | IDashBoardTrangThaiReportLoadSuccess | IDashBoardTrangThaiReportLoadError |
    IDashBoardTongSoHoaDonReportLoadStart | IDashBoardTongSoHoaDonReportLoadSuccess | IDashBoardTongSoHoaDonReportLoadError |
    IDashBoardChangeLichSuTheoNgayReportFilter |
    IDashBoardLichSuTheoNgayReportLoadStart | IDashBoardLichSuTheoNgayReportLoadSuccess | IDashBoardLichSuTheoNgayReportLoadError |
    IDashBoardTrangThaiReportLoadAllStart | IDashBoardTrangThaiReportLoadAllSuccess | IDashBoardTrangThaiReportLoadAllError