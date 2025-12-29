import { apiClient } from "../apiClient";

export const notifyApi = {
    getSummary: () => apiClient.get(`notify/summary`)
}