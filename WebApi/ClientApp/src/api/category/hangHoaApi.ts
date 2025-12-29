import { IPagingRequest } from "../../models/requests/IPagingRequest";
import { IHangHoa } from "../../models/responses/category/IHangHoa";
import { IUploadRespone } from "../../models/responses/upload/IUploadRespone";
import { apiClient, formatQueryString } from "../apiClient"
export const HANG_HOA_API_ENDPOINT = "hang-hoa";
export const hangHoaApi = {
    getByDonViPaging: (request: IPagingRequest) => apiClient.get(`${HANG_HOA_API_ENDPOINT}?${formatQueryString(request)}`),
    insert: (rq: IHangHoa) => apiClient.post(`${HANG_HOA_API_ENDPOINT}`, rq),
    update: (rq: IHangHoa) => apiClient.put(`${HANG_HOA_API_ENDPOINT}`, rq),
    delete: (id: number) => apiClient.delete(`${HANG_HOA_API_ENDPOINT}/${id}`),
    validImport: (request: IUploadRespone) => apiClient.post(`${HANG_HOA_API_ENDPOINT}/import/valid`, request),
    importData: (request: IUploadRespone) => apiClient.post(`${HANG_HOA_API_ENDPOINT}/import`, request),
}