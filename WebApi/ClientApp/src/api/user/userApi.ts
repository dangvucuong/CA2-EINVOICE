import { IUserLoadRequest } from "../../models/requests/user/IUserLoadRequest";
import { IUserEditModel } from "../../models/responses/user/IUserEditModel";
import { IUserUpadteRemoteSigningSerialRequest } from "../../models/responses/user/IUserUpadteRemoteSigningSerialRequest";
import { apiClient, formatQueryString } from "../apiClient";
export const USER_API_ENDPOINT = "user";
export const userApi = {
  getUsers: (request: IUserLoadRequest) =>
    apiClient.get(`user?${formatQueryString(request)}`),
  // getUsersByDonVi: (request: IUserLoadByDonViRequest) => apiClient.get(`user/don-vi/${request.donvi_ma_dv}?${formatQueryString(request)}`),
  getUser: (id: number) => apiClient.get(`user/${id}`),
  saveUser: (request: IUserEditModel) => apiClient.put(`user`, request),
  deleteUser: (id: number) => apiClient.delete(`user/${id}`),
  updateSerialNumber: (request: any) => apiClient.put(`user/serial`, request),
  updateRemoteSigningSerial: (request: IUserUpadteRemoteSigningSerialRequest) =>
    apiClient.put(`user/remote-siging-serial`, request),
};
