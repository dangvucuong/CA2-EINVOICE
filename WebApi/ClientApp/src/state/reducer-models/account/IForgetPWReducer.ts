import { ISendOTPRespone } from "../../../models/responses/account/ISendOTPRespone";

export enum eForgetPWReducerStatus {
    is_sending_otp,
    is_send_otp_success,
    is_send_otp_error,
    is_reseting_pw,
    is_reset_pw_success,
    is_reset_pw_error,
}
export interface IForgetPWReducer {
    status?: eForgetPWReducerStatus,
    otpRespone?: ISendOTPRespone,
    message?:string
}