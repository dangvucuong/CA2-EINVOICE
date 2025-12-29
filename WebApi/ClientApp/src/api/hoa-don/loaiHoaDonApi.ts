import { apiClient } from "../apiClient";

export const loaiHoaDonApi = {
    selectAll: () => apiClient.get(`loai-hoa-don`)
}