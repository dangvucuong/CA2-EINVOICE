import { IPagingRequest } from "../../models/requests/IPagingRequest";
import { apiClient, formatQueryString } from "../apiClient";
export const LOG_API_ENDPOINT = "log"
export const logApi = {
    select: (request: IPagingRequest) => apiClient.get(`${LOG_API_ENDPOINT}?${formatQueryString(request)}`),
   
}