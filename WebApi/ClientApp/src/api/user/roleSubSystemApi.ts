import { IRoleSubSystem } from "../../models/responses/user/IRoleSubSystem";
import { apiClient } from "../apiClient";

export const roleSubSystemApi = {
    getByRoles: (role_id: number) => apiClient.get(`role/${role_id}/sub-system`),
    addNewSubSystem: (roleSubSystem: IRoleSubSystem) => apiClient.post(`role/${roleSubSystem.role_id}/sub-system`,roleSubSystem),
    removeSubSystem: (roleSubSystem: IRoleSubSystem) => apiClient.delete(`role/${roleSubSystem.role_id}/sub-system/${roleSubSystem.sub_system_id}`),

}