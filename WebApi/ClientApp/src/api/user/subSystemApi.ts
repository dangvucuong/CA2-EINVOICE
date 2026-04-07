import { apiClient } from "../apiClient";

export const subSystemApi = {
    getAll:() => apiClient.get(`sub-system`)
}