import { IHoaDonPhatHanhRequest } from "../../models/requests/hoa-don/IHoaDonPhatHanhRequest";
import { IToKhaiAddOrEditModel } from "../../models/requests/to-khai/IToKhaiAddOrEditModel";
import { apiClient } from "../apiClient";
export const TO_KHAI_API = "to-khai";
export const TO_KHAI_API_PHAT_HANH = "to-khai/phat-hanh";
export const toKhaiApi = {
    getByDonVi: () => apiClient.get(`${TO_KHAI_API}`),
    getLogs: (toKhaiId: number) => apiClient.get(`${TO_KHAI_API}/${toKhaiId}/log`),
    getViewModel: (toKhaiId: number) => apiClient.get(`${TO_KHAI_API}/${toKhaiId}`),
    getHtmlPrint: (toKhaiId: number) => apiClient.get(`${TO_KHAI_API}/${toKhaiId}/print`),
    getHtmlKetQuaPrint: (toKhaiId: number) => apiClient.get(`${TO_KHAI_API}/${toKhaiId}/print/ket-qua`),
    insert: (rq: IToKhaiAddOrEditModel) => apiClient.post(`${TO_KHAI_API}`, rq),
    update: (rq: IToKhaiAddOrEditModel) => apiClient.put(`${TO_KHAI_API}`, rq),
    delete: (id: number) => apiClient.delete(`${TO_KHAI_API}/${id}`),
    createBase64KySo: (id: number) => apiClient.get(`${TO_KHAI_API}/${id}/ky-so`),
    phatHanh: (rq: IHoaDonPhatHanhRequest) => apiClient.post(`${TO_KHAI_API}/phat-hanh`, rq),
    kySoVaPhatHanhRemoteAsync: (id: number) => apiClient.put(`${TO_KHAI_API}/${id}/ky-so-remote`, undefined),

}