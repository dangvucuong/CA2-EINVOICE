
import { IHoaDonTrangThaiSummaryRequest } from "../../../models/requests/dashboard/IHoaDonTrangThaiSummaryRequest";
import { IDonViSoLuongChuKySoSummary } from "../../../models/responses/dashboard/IDonViSoLuongChuKySoSummary";
import { IHoaDonLichSuPhatHanhItem } from "../../../models/responses/dashboard/IHoaDonLichSuPhatHanhItem";
import { IHoaDonTrangThaiSummary } from "../../../models/responses/dashboard/IHoaDonTrangThaiSummary";
import { eReducerStatusBase } from "../eReducerStatusBase";

export interface IDashboardReducer {
    tongSoHoaDonReport: {
        status: eReducerStatusBase,
        data: IDonViSoLuongChuKySoSummary
    }
    trangThaiReport: {
        status: eReducerStatusBase,
        filter: IHoaDonTrangThaiSummaryRequest,
        data: IHoaDonTrangThaiSummary[]
    },
    trangThaiReportAll: {
        status: eReducerStatusBase,
        data: IHoaDonTrangThaiSummary[]
    },
    lichSuTheoNgayReport: {
        status: eReducerStatusBase,
        filter: IHoaDonTrangThaiSummaryRequest,
        data: IHoaDonLichSuPhatHanhItem[]
    }
}