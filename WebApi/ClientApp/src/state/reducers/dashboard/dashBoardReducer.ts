import { getFirstDayOfMonth, getLastDayOfMonth } from "../../../helpers/common";
import { IDashBoardActionTypes, eDashBoardActionType } from "../../action-types/dashboard/IDashBoardActionType";
import { IDashboardReducer } from "../../reducer-models/dashboard/IDashboardReducer";
import { eReducerStatusBase } from "../../reducer-models/eReducerStatusBase";
const iniState: IDashboardReducer = {
    trangThaiReport: {
        data: [],
        filter: {
            from_date: getFirstDayOfMonth(),
            to_date: getLastDayOfMonth()
        },
        status: eReducerStatusBase.is_not_initialization
    },
    trangThaiReportAll: {
        data: [],
        status: eReducerStatusBase.is_not_initialization
    },
    tongSoHoaDonReport: {
        data: {
            tong_so_luong_da_mua: 0,
            tong_so_luong_da_su_dung: 0,
            donvi_ma_dv: ""
        },
        status: eReducerStatusBase.is_not_initialization
    },
    lichSuTheoNgayReport: {
        data: [],
        filter: {
            from_date: getFirstDayOfMonth(),
            to_date: getLastDayOfMonth()
        },
        status: eReducerStatusBase.is_not_initialization
    }
}
export const dashBoardReducer = (state: IDashboardReducer = iniState, action: IDashBoardActionTypes): IDashboardReducer => {
    switch (action.type) {
        case eDashBoardActionType.SELECT_TONG_SO_HOA_DON_REPORT_START:
            return {
                ...state,
                tongSoHoaDonReport: {
                    ...state.tongSoHoaDonReport,
                    status: eReducerStatusBase.is_loading
                }
            }
        case eDashBoardActionType.SELECT_TONG_SO_HOA_DON_REPORT_SUCCESS:
            return {
                ...state,
                tongSoHoaDonReport: {
                    ...state.tongSoHoaDonReport,
                    status: eReducerStatusBase.is_loaded,
                    data: action.payload
                }
            }
        case eDashBoardActionType.SELECT_TONG_SO_HOA_DON_REPORT_ERROR:
            return {
                ...state,
                tongSoHoaDonReport: {
                    ...state.tongSoHoaDonReport,
                    status: eReducerStatusBase.is_load_err,
                }
            }
        case eDashBoardActionType.CHANGE_TRANGTHAI_REPORT_FILTER:
            return {
                ...state,
                trangThaiReport: {
                    ...state.trangThaiReport,
                    filter: action.payload
                }
            }
        case eDashBoardActionType.SELECT_TRANGTHAI_REPORT_START:
            return {
                ...state,
                trangThaiReport: {
                    ...state.trangThaiReport,
                    status: eReducerStatusBase.is_loading
                }
            }
        case eDashBoardActionType.SELECT_TRANGTHAI_REPORT_SUCCESS:
            return {
                ...state,
                trangThaiReport: {
                    ...state.trangThaiReport,
                    status: eReducerStatusBase.is_loaded,
                    data: action.payload
                }
            }
        case eDashBoardActionType.SELECT_TRANGTHAI_REPORT_ERROR:
            return {
                ...state,
                trangThaiReport: {
                    ...state.trangThaiReport,
                    status: eReducerStatusBase.is_load_err,
                }
            }

        case eDashBoardActionType.SELECT_TRANGTHAI_ALL_REPORT_START:
            return {
                ...state,
                trangThaiReportAll: {
                    ...state.trangThaiReportAll,
                    status: eReducerStatusBase.is_loading
                }
            }
        case eDashBoardActionType.SELECT_TRANGTHAI_ALL_REPORT_SUCCESS:
            return {
                ...state,
                trangThaiReportAll: {
                    ...state.trangThaiReportAll,
                    status: eReducerStatusBase.is_loaded,
                    data: action.payload
                }
            }
        case eDashBoardActionType.SELECT_TRANGTHAI_ALL_REPORT_ERROR:
            return {
                ...state,
                trangThaiReportAll: {
                    ...state.trangThaiReportAll,
                    status: eReducerStatusBase.is_load_err,
                }
            }

        case eDashBoardActionType.CHANGE_LICH_SU_THEO_NGAY_REPORT_FILTER:
            return {
                ...state,
                lichSuTheoNgayReport: {
                    ...state.lichSuTheoNgayReport,
                    filter: action.payload
                }
            }
        case eDashBoardActionType.SELECT_LICH_SU_THEO_NGAY_REPORT_START:
            return {
                ...state,
                lichSuTheoNgayReport: {
                    ...state.lichSuTheoNgayReport,
                    status: eReducerStatusBase.is_loading
                }
            }
        case eDashBoardActionType.SELECT_LICH_SU_THEO_NGAY_REPORT_SUCCESS:
            return {
                ...state,
                lichSuTheoNgayReport: {
                    ...state.lichSuTheoNgayReport,
                    status: eReducerStatusBase.is_loaded,
                    data: action.payload
                }
            }
        case eDashBoardActionType.SELECT_LICH_SU_THEO_NGAY_REPORT_ERROR:
            return {
                ...state,
                lichSuTheoNgayReport: {
                    ...state.lichSuTheoNgayReport,
                    status: eReducerStatusBase.is_load_err,
                }
            }

        default:
            return state;
    }
}