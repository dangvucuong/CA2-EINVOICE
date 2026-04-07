import { IChangeWordRequest } from "../../models/requests/account/IChangeWordRequest";
import { ILoginRequest } from "../../models/requests/account/ILoginRequest";
import { ILoginRSRequest } from "../../models/requests/account/ILoginRSRequest";
import { ILoginSerialRequest } from "../../models/requests/account/ILoginSerialRequest";
import { apiClient } from "../apiClient";
import { apiGuestClient } from "../apiGuestClient";

export const accountApi = {
    getProfile: () => apiClient.get('account/info'),
    logIn: (request: ILoginRequest) => {
        // 
        return apiGuestClient.post(`account/login`, request)
    },
    logInSerial: (request: ILoginSerialRequest) => {
        // 
        return apiGuestClient.post(`account/login-mst`, request)
    },
    logInRS: (request: ILoginRSRequest) => {
        // 
        return apiGuestClient.post(`account/login-rs`, request)
    },
    checkLoginInRS: (code: string) => {
        // 
        return apiGuestClient.get(`account/login-rs/${code}`)
    },
    changePassword: (request: IChangeWordRequest) => {
        // 
        return apiClient.post(`account/change-pw`, request)
    },
    deletePasskey: (request: ILoginRequest) => {
        // 
        return apiGuestClient.post(`account/pass-key/delete`, request)
    },
}