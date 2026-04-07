import { IContactSelectRequest } from "../../models/requests/contact/IContactSelectRequest";
import { IContact } from "../../models/responses/contact/IContact";
import { apiClient, formatQueryString } from "../apiClient";
export const CONTACT_API_ENDPOINT = "contact";
export const contactApi = {
    select: (request: IContactSelectRequest) => apiClient.get(`${CONTACT_API_ENDPOINT}?${formatQueryString(request)}`),
    insert: (rq: IContact) => apiClient.post(`${CONTACT_API_ENDPOINT}`, rq),
    update: (rq: IContact) => apiClient.put(`${CONTACT_API_ENDPOINT}`, rq),
}