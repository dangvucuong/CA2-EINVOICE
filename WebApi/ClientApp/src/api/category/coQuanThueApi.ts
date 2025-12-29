import { IPagingRequest } from "../../models/requests/IPagingRequest";
import { apiClient, formatQueryString } from "../apiClient";
export const KHACH_HANG_API_ENDPOINT = "khach-hang";
export const coQuanThueApi = {
    selectPaging: (request: IPagingRequest) => apiClient.get(`co-quan-thue?${formatQueryString(request)}`),
    selectById: (id: number) => apiClient.get(`co-quan-thue/${id}`),

}