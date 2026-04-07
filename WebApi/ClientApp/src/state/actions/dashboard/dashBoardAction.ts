import { IDashBoardChangeLichSuTheoNgayReportFilter, IDashBoardLichSuTheoNgayReportLoadStart, IDashBoardTongSoHoaDonReportLoadError, IDashBoardTongSoHoaDonReportLoadStart, IDashBoardLichSuTheoNgayReportLoadSuccess, IDashBoardLichSuTheoNgayReportLoadError, IDashBoardTrangThaiReportLoadAllStart, IDashBoardTrangThaiReportLoadAllSuccess, IDashBoardTrangThaiReportLoadAllError } from './../../action-types/dashboard/IDashBoardActionType';
import { NotifyHelper } from "../../../helpers/toast";
import { IHoaDonTrangThaiSummaryRequest } from "../../../models/requests/dashboard/IHoaDonTrangThaiSummaryRequest";
import { IHoaDonTrangThaiSummary } from "../../../models/responses/dashboard/IHoaDonTrangThaiSummary";
import { IDashBoardChangeTrangThaiReportFilter, IDashBoardTongSoHoaDonReportLoadSuccess, IDashBoardTrangThaiReportLoadError, IDashBoardTrangThaiReportLoadStart, IDashBoardTrangThaiReportLoadSuccess, eDashBoardActionType } from "../../action-types/dashboard/IDashBoardActionType";
import { baseAction } from "../baseAction";
import { IDonViSoLuongChuKySoSummary } from '../../../models/responses/dashboard/IDonViSoLuongChuKySoSummary';
import { IHoaDonLichSuPhatHanhItem } from '../../../models/responses/dashboard/IHoaDonLichSuPhatHanhItem';

export const dashBoardAction = {
    tongSoHoaDonReportLoadStart: (): IDashBoardTongSoHoaDonReportLoadStart =>
        baseAction(eDashBoardActionType.SELECT_TONG_SO_HOA_DON_REPORT_START, undefined),
    tongSoHoaDonReportLoadSuccess: (data: IDonViSoLuongChuKySoSummary): IDashBoardTongSoHoaDonReportLoadSuccess =>
        baseAction(eDashBoardActionType.SELECT_TONG_SO_HOA_DON_REPORT_SUCCESS, data),
    tongSoHoaDonReportLoadError: (message: string): IDashBoardTongSoHoaDonReportLoadError => {
        NotifyHelper.Error(message)
        return baseAction(eDashBoardActionType.SELECT_TONG_SO_HOA_DON_REPORT_ERROR, message);
    },

    trangThaiReportChangeFilter: (filter: IHoaDonTrangThaiSummaryRequest): IDashBoardChangeTrangThaiReportFilter =>
        baseAction(eDashBoardActionType.CHANGE_TRANGTHAI_REPORT_FILTER, filter),
    trangThaiReportLoadStart: (filter: IHoaDonTrangThaiSummaryRequest): IDashBoardTrangThaiReportLoadStart =>
        baseAction(eDashBoardActionType.SELECT_TRANGTHAI_REPORT_START, filter),
    trangThaiReportLoadSuccess: (data: IHoaDonTrangThaiSummary[]): IDashBoardTrangThaiReportLoadSuccess =>
        baseAction(eDashBoardActionType.SELECT_TRANGTHAI_REPORT_SUCCESS, data),
    trangThaiReportLoadError: (message: string): IDashBoardTrangThaiReportLoadError => {
        NotifyHelper.Error(message)
        return baseAction(eDashBoardActionType.SELECT_TRANGTHAI_REPORT_ERROR, message);
    },

    trangThaiReportLoadAllStart: (): IDashBoardTrangThaiReportLoadAllStart =>
        baseAction(eDashBoardActionType.SELECT_TRANGTHAI_ALL_REPORT_START, undefined),
    trangThaiReportLoadAllSuccess: (data: IHoaDonTrangThaiSummary[]): IDashBoardTrangThaiReportLoadAllSuccess =>
        baseAction(eDashBoardActionType.SELECT_TRANGTHAI_ALL_REPORT_SUCCESS, data),
    trangThaiReportLoadAllError: (message: string): IDashBoardTrangThaiReportLoadAllError => {
        NotifyHelper.Error(message)
        return baseAction(eDashBoardActionType.SELECT_TRANGTHAI_ALL_REPORT_ERROR, message);
    },


    lichSuTheoNgayReportChangeFilter: (filter: IHoaDonTrangThaiSummaryRequest): IDashBoardChangeLichSuTheoNgayReportFilter =>
        baseAction(eDashBoardActionType.CHANGE_LICH_SU_THEO_NGAY_REPORT_FILTER, filter),
    lichSuTheoNgayReportLoadStart: (filter: IHoaDonTrangThaiSummaryRequest): IDashBoardLichSuTheoNgayReportLoadStart =>
        baseAction(eDashBoardActionType.SELECT_LICH_SU_THEO_NGAY_REPORT_START, filter),
    lichSuTheoNgayReportLoadSuccess: (data: IHoaDonLichSuPhatHanhItem[]): IDashBoardLichSuTheoNgayReportLoadSuccess =>
        baseAction(eDashBoardActionType.SELECT_LICH_SU_THEO_NGAY_REPORT_SUCCESS, data),
    lichSuTheoNgayReportLoadError: (message: string): IDashBoardLichSuTheoNgayReportLoadError => {
        NotifyHelper.Error(message)
        return baseAction(eDashBoardActionType.SELECT_LICH_SU_THEO_NGAY_REPORT_ERROR, message);
    }

}