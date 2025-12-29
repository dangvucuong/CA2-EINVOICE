import { call, put, takeLatest } from "redux-saga/effects";
import { dashBoardApi } from "../../../api/dashboard/dashBoardApi";
import { IBaseRespone } from "../../../models/responses/IBaseRespone";
import { IDashBoardLichSuTheoNgayReportLoadStart, IDashBoardTongSoHoaDonReportLoadStart, IDashBoardTrangThaiReportLoadStart, eDashBoardActionType } from "../../action-types/dashboard/IDashBoardActionType";
import { rootAction } from "../../actions/rootAction";

const mainAction = rootAction.dashBoard;

export function* dashBoardSaga(): any {
    yield takeLatest(eDashBoardActionType.SELECT_TRANGTHAI_REPORT_START, loadHoaDonTrangThaiSummary)
    yield takeLatest(eDashBoardActionType.SELECT_TRANGTHAI_ALL_REPORT_START, loadHoaDonTrangThaiAllSummary)
    yield takeLatest(eDashBoardActionType.SELECT_TONG_SO_HOA_DON_REPORT_START, loadTongSoHoaDonReport)
    yield takeLatest(eDashBoardActionType.SELECT_LICH_SU_THEO_NGAY_REPORT_START, loadLichSuTheoNgayReport)


}

function* loadHoaDonTrangThaiSummary(action: IDashBoardTrangThaiReportLoadStart): any {
    const res: IBaseRespone = yield call(dashBoardApi.selectHoaDonTrangThaiSummary, action.payload)
    if (res.is_success) {
        yield put(mainAction.trangThaiReportLoadSuccess(res.data))
    } else {
        yield put(mainAction.trangThaiReportLoadError(res.message ?? "Error"))
    }
}

function* loadHoaDonTrangThaiAllSummary(action: IDashBoardTrangThaiReportLoadStart): any {
    // debugger
    const res: IBaseRespone = yield call(dashBoardApi.selectHoaDonTrangThaiSummaryAll)
    if (res.is_success) {
        yield put(mainAction.trangThaiReportLoadAllSuccess(res.data))
    } else {
        yield put(mainAction.trangThaiReportLoadAllError(res.message ?? "Error"))
    }
}


function* loadTongSoHoaDonReport(action: IDashBoardTongSoHoaDonReportLoadStart): any {
    const res: IBaseRespone = yield call(dashBoardApi.selectTongSoHoaDongSummary)
    if (res.is_success) {
        yield put(mainAction.tongSoHoaDonReportLoadSuccess(res.data))
    } else {
        yield put(mainAction.tongSoHoaDonReportLoadError(res.message ?? "Error"))
    }
}


function* loadLichSuTheoNgayReport(action: IDashBoardLichSuTheoNgayReportLoadStart): any {
    const res: IBaseRespone = yield call(dashBoardApi.selectLichSuPhatHanhSummary, action.payload)
    if (res.is_success) {
        yield put(mainAction.lichSuTheoNgayReportLoadSuccess(res.data))
    } else {
        yield put(mainAction.lichSuTheoNgayReportLoadError(res.message ?? "Error"))
    }
}

