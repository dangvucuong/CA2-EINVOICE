import { IPagingRequest } from "../../models/requests/IPagingRequest";
import { IKhachHang } from "../../models/responses/category/IKhachHang";
import { IUploadRespone } from "../../models/responses/upload/IUploadRespone";
import { apiClient, formatQueryString } from "../apiClient"
export const KHACH_HANG_API_ENDPOINT = "khach-hang";
export const khachHangApi = {
    getByDonViPaging: (request: IPagingRequest) => apiClient.get(`${KHACH_HANG_API_ENDPOINT}?${formatQueryString(request)}`),
    insert: (rq: IKhachHang) => apiClient.post(`${KHACH_HANG_API_ENDPOINT}`, rq),
    update: (rq: IKhachHang) => apiClient.put(`${KHACH_HANG_API_ENDPOINT}`, rq),
    delete: (id: number) => apiClient.delete(`${KHACH_HANG_API_ENDPOINT}/${id}`),
    validImport: (request: IUploadRespone) => apiClient.post(`${KHACH_HANG_API_ENDPOINT}/import/valid`, request),
    importData: (request: IUploadRespone) => apiClient.post(`${KHACH_HANG_API_ENDPOINT}/import`, request),
}