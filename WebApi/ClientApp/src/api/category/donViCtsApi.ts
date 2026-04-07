import { apiClient } from "../apiClient";
export const DONVI_CTS_API = "don-vi/cts";
export const donViCtsApi = {
    getAll: () => apiClient.get(`${DONVI_CTS_API}`),
    insert: (rq: any) => apiClient.post(`${DONVI_CTS_API}`, rq),
    update: (rq: any) => apiClient.put(`${DONVI_CTS_API}`, rq),
    
}