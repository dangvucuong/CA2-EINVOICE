import { IForgetPasswordSendOTPRequest } from "../../../models/requests/account/IForgetPasswordSendOTPRequest";
import { IResetNewPassWordRequest } from "../../../models/requests/account/IResetNewPassWordRequest";
import { ISendOTPRespone } from "../../../models/responses/account/ISendOTPRespone";
import { IActionTypeBase } from "../IActionTypeBase";

export enum eForgetPWActionTypeIds {
    SEND_OTP_START = "FORGET_PW_SEND_OTP_START",
    SEND_OTP_SUCCESS = "FORGET_PW_SEND_OTP_SUCCESS",
    SEND_OTP_ERROR = "FORGET_PW_SEND_OTP_ERROR",

    RESET_PW_START = "FORGET_PW_RESET_PW_START",
    RESET_PW_SUCCESS = "FORGET_PW_RESET_PW_SUCCESS",
    RESET_PW_ERROR = "FORGET_PW_RESET_PW_ERROR",

}

export interface IForgetPWSendOTPStart extends IActionTypeBase<eForgetPWActionTypeIds.SEND_OTP_START, IForgetPasswordSendOTPRequest> { }
export interface IForgetPWSendOTPSuccess extends IActionTypeBase<eForgetPWActionTypeIds.SEND_OTP_SUCCESS, ISendOTPRespone> { }
export interface IForgetPWSendOTPError extends IActionTypeBase<eForgetPWActionTypeIds.SEND_OTP_ERROR, string> { }


export interface IForgetPWResetPWStart extends IActionTypeBase<eForgetPWActionTypeIds.RESET_PW_START, IResetNewPassWordRequest> { }
export interface IForgetPWResetPWSuccess extends IActionTypeBase<eForgetPWActionTypeIds.RESET_PW_SUCCESS, undefined> { }
export interface IForgetPWResetPWError extends IActionTypeBase<eForgetPWActionTypeIds.RESET_PW_ERROR, string> { }

export type IForgetPWActionType = IForgetPWSendOTPStart | IForgetPWSendOTPSuccess | IForgetPWSendOTPError |
    IForgetPWResetPWStart | IForgetPWResetPWSuccess | IForgetPWResetPWError