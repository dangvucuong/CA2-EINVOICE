import { apiClient } from "../apiClient";

export const loaiHoaDonCTApi = {
    selectAll: () => apiClient.get(`loai-hoa-don-ct`)
}