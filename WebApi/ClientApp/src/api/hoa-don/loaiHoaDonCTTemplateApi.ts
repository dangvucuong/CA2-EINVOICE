import { IMauHoaDon } from "../../models/responses/hoa-don/IMauHoaDon";
import { apiClient } from "../apiClient";

export const loaiHoaDonCTTemplateApi = {
    selectAll: () => apiClient.get(`loai-hoa-don-ct-template`),
    createPreviewData: (re: IMauHoaDon) => apiClient.post(`loai-hoa-don-ct-template/preview`, re)
}