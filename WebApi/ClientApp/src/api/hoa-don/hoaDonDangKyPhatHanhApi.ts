import { IHoaDonDangKyPhatHanh } from "../../models/responses/hoa-don/IHoaDonDangKyPhatHanh";
import { apiClient } from "../apiClient";
export const HOA_DONG_DANG_KY_PHAT_HANH = "dang-ky-phat-hanh-hoa-don";
export const hoaDonDangKyPhatHanhApi = {
    getByDonVi: () => apiClient.get(`${HOA_DONG_DANG_KY_PHAT_HANH}`),
    insert: (rq: IHoaDonDangKyPhatHanh) => apiClient.post(`${HOA_DONG_DANG_KY_PHAT_HANH}`, rq),
    update: (rq: IHoaDonDangKyPhatHanh) => apiClient.put(`${HOA_DONG_DANG_KY_PHAT_HANH}`, rq),
    delete: (id: number) => apiClient.delete(`${HOA_DONG_DANG_KY_PHAT_HANH}/${id}`),
}