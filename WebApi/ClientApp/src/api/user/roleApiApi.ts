import { IRoleApiLoadRequest } from "../../models/requests/user/IRoleApiLoadRequest";
import { IRoleApi } from "../../models/responses/user/IRoleApi";
import { apiClient } from "../apiClient";

export const roleApiApi = {
    getByRoles: (rq:IRoleApiLoadRequest) => apiClient.get(`role/${rq.role_id}/sub-system/${rq.sub_system_id}/api`),
    addNewApi: (roleApi: IRoleApi) => apiClient.post(`role/${roleApi.role_id}/api`,roleApi),
    removeApi: (roleApi: IRoleApi) => apiClient.delete(`role/${roleApi.role_id}/api/${roleApi.id}`),

}