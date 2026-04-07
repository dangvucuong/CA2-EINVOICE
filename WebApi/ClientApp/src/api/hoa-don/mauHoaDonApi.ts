import { IMauHoaDonActiveRequest } from "../../models/requests/mau-hoa-don/IMauHoaDonActiveRequest";
import { IMauHoaDon } from "../../models/responses/hoa-don/IMauHoaDon";
import { apiClient } from "../apiClient";
export const MAU_HOA_DON_API_ENDPOIT = "mau-hoa-don";
export const mauHoaDonApi = {
    getByDonVi: () => apiClient.get(`${MAU_HOA_DON_API_ENDPOIT}`),
    getById: (id: number) => apiClient.get(`${MAU_HOA_DON_API_ENDPOIT}/${id}`),
    insert: (rq: IMauHoaDon) => apiClient.post(`${MAU_HOA_DON_API_ENDPOIT}`, rq),
    update: (rq: IMauHoaDon) => apiClient.put(`${MAU_HOA_DON_API_ENDPOIT}`, rq),
    delete: (id: number) => apiClient.delete(`${MAU_HOA_DON_API_ENDPOIT}/${id}`),
    updateActive: (rq: IMauHoaDonActiveRequest) => apiClient.put(`${MAU_HOA_DON_API_ENDPOIT}/active`, rq),
}