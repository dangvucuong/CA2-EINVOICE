import { IHoaDonTrangThaiSummaryRequest } from "../../models/requests/dashboard/IHoaDonTrangThaiSummaryRequest";
import { apiClient } from "../apiClient";

export const dashBoardApi = {
    selectHoaDonTrangThaiSummary: (rq: IHoaDonTrangThaiSummaryRequest) =>
        apiClient.get(`dashboard/hoa-don/trang-thai/from/${rq.from_date}/to/${rq.to_date}`),
    selectHoaDonTrangThaiSummaryAll: () =>
        apiClient.get(`dashboard/hoa-don/trang-thai`),
    selectTongSoHoaDongSummary: () =>
        apiClient.get(`dashboard/hoa-don/tong-so-luong`),
    selectLichSuPhatHanhSummary: (rq: IHoaDonTrangThaiSummaryRequest) =>
        apiClient.get(`dashboard/hoa-don/phat-hanh-date/from/${rq.from_date}/to/${rq.to_date}`),
}