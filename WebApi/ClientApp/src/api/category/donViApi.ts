import { IPagingRequest } from "../../models/requests/IPagingRequest";
import { IDonVi } from "../../models/responses/category/IDonVi";
import { apiClient, formatQueryString } from "../apiClient"
export const DONVI_API_ENDPOINT = "don-vi";
export const donViApi = {
    getAll: (request: IPagingRequest) => apiClient.get(`${DONVI_API_ENDPOINT}?${formatQueryString(request)}`),
    insert: (rq: IDonVi) => apiClient.post(`${DONVI_API_ENDPOINT}`, rq),
    update: (rq: IDonVi) => apiClient.put(`${DONVI_API_ENDPOINT}`, rq),
    updateThongTinLienHe: (rq: IDonVi) => apiClient.put(`${DONVI_API_ENDPOINT}/lien-he`, rq),
    getLichSuMuaCKS: () => apiClient.get(`${DONVI_API_ENDPOINT}/lich-su-mua-cks`),
    delete: (id: number) => apiClient.delete(`${DONVI_API_ENDPOINT}/${id}`),
    getGipInfo: (mst:string) => apiClient.get(`${DONVI_API_ENDPOINT}/gip/${mst}`),
}