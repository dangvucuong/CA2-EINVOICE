import { IRoleSubSystem } from "../../models/responses/user/IRoleSubSystem";
import { apiClient } from "../apiClient";

export const roleSubSystemApi = {
    getByRoles: (role_id: number) => apiClient.get(`api/role/${role_id}/sub-system`),
    addNewSubSystem: (roleSubSystem: IRoleSubSystem) => apiClient.post(`api/role/${roleSubSystem.role_id}/sub-system`),
    removeSubSystem: (roleSubSystem: IRoleSubSystem) => apiClient.delete(`api/role/${roleSubSystem.role_id}/sub-system`),

}