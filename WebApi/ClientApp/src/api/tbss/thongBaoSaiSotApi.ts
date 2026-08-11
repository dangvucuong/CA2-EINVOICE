import { IHoaDonPhatHanhRequest } from "../../models/requests/hoa-don/IHoaDonPhatHanhRequest";
import { IThongBaoSaiSotAddOrEditRequest } from "../../models/requests/tbss/IThongBaoSaiSotAddOrEditRequest";
import { IUploadRespone } from "../../models/responses/upload/IUploadRespone";
import { apiClient } from "../apiClient";
export const THONG_BAO_SAI_SOT_API = "tbss";
export const THONG_BAO_SAI_SOT_API_PHAT_HANH = "tbss/phat-hanh";

export const thongBaoSaiSotApi = {
    getByDonVi: () => apiClient.get(`${THONG_BAO_SAI_SOT_API}`),
    getLogs: (id: number) => apiClient.get(`${THONG_BAO_SAI_SOT_API}/${id}/log`),
    getViewModel: (id: number) => apiClient.get(`${THONG_BAO_SAI_SOT_API}/${id}`),
    insert: (rq: IThongBaoSaiSotAddOrEditRequest) => apiClient.post(`${THONG_BAO_SAI_SOT_API}`, rq),
    update: (rq: IThongBaoSaiSotAddOrEditRequest) => apiClient.put(`${THONG_BAO_SAI_SOT_API}`, rq),
    delete: (id: number) => apiClient.delete(`${THONG_BAO_SAI_SOT_API}/${id}`),
    createBase64KySo: (id: number) => apiClient.get(`${THONG_BAO_SAI_SOT_API}/${id}/ky-so`),
    phatHanh: (rq: IHoaDonPhatHanhRequest) => apiClient.post(`${THONG_BAO_SAI_SOT_API}/phat-hanh`, rq),
    kySoVaPhatHanhRemoteAsync: (id: number) => apiClient.put(`${THONG_BAO_SAI_SOT_API}/${id}/ky-so-remote`, undefined),
    getHtmlView: (id: number) => apiClient.get(`${THONG_BAO_SAI_SOT_API}/${id}/html`),
    getHtmlKetQua: (id: number) => apiClient.get(`${THONG_BAO_SAI_SOT_API}/${id}/ket-qua`),
    readFromExcel: (rq: IUploadRespone) => apiClient.post(THONG_BAO_SAI_SOT_API + "/import/valid", rq)
}