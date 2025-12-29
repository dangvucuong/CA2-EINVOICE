import { apiClient } from "../apiClient";

export const apiApi = {
    getBySubSystem:(subSystemId: number) => apiClient.get(`sub-system/${subSystemId}/api`)
}