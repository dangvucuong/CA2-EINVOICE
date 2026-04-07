import { IForgetPasswordSendOTPRequest } from "../../models/requests/account/IForgetPasswordSendOTPRequest";
import { IResetNewPassWordRequest } from "../../models/requests/account/IResetNewPassWordRequest";
import { apiGuestClient } from "../apiGuestClient";

export const forgetPWApi = {
    sendOTP: (rq:IForgetPasswordSendOTPRequest) => apiGuestClient.post('account/forget-pw/send-otp',rq),
    resetPW: (request: IResetNewPassWordRequest) => {
        return apiGuestClient.post(`account/reset-pw`, request)
    }
}