import { IRole } from "../../models/responses/user/IRole";
import { apiClient } from "../apiClient";
export const ROLE_API_ENDPOINT="role"
export const ROLE_API_VIEWALL_ENDPOINT="role/all"
export const roleApi = {
    getRoles: () => apiClient.get(`role`),
    insertRole: (role: IRole) => apiClient.post(`role`, role),
    updateRole: (role: IRole) => apiClient.put(`role`, role),
    deleteRole: (id: number) => apiClient.delete(`role/${id}`),
}