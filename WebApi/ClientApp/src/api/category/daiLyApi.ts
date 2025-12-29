import { IPagingRequest } from "../../models/requests/IPagingRequest";
import { IDaiLy } from "../../models/responses/category/IDaiLy";
import { apiClient, formatQueryString } from "../apiClient";
export const DAI_LY_API_ENDPOIT = "dai-ly";
export const daiLyApi = {
    getByDonViPaging: (request: IPagingRequest) => apiClient.get(`${DAI_LY_API_ENDPOIT}?${formatQueryString(request)}`),
    insert: (rq: IDaiLy) => apiClient.post(`${DAI_LY_API_ENDPOIT}`, rq),
    update: (rq: IDaiLy) => apiClient.put(`${DAI_LY_API_ENDPOIT}`, rq),
    delete: (id: number) => apiClient.delete(`${DAI_LY_API_ENDPOIT}/${id}`),
}