import { apiClient } from "../apiClient";

export const menuApi = {
    getBySubSystem:(subSystemId: number) => apiClient.get(`sub-system/${subSystemId}/menu`)
}