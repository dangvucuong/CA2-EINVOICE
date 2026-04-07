import { NotifyHelper } from "../../../helpers/toast";
import { IForgetPasswordSendOTPRequest } from "../../../models/requests/account/IForgetPasswordSendOTPRequest";
import { IResetNewPassWordRequest } from "../../../models/requests/account/IResetNewPassWordRequest";
import { ISendOTPRespone } from "../../../models/responses/account/ISendOTPRespone";
import { IForgetPWResetPWError, IForgetPWResetPWStart, IForgetPWResetPWSuccess, IForgetPWSendOTPError, IForgetPWSendOTPStart, IForgetPWSendOTPSuccess, eForgetPWActionTypeIds } from "../../action-types/account/IForgetPWActionType";
import { baseAction } from "../baseAction";

export const forgetPWAction = {
    sendOTPStart: (rq: IForgetPasswordSendOTPRequest): IForgetPWSendOTPStart => baseAction(eForgetPWActionTypeIds.SEND_OTP_START, rq),
    sendOTPSuccess: (res: ISendOTPRespone): IForgetPWSendOTPSuccess => {
        NotifyHelper.Success("Success")
        return baseAction(eForgetPWActionTypeIds.SEND_OTP_SUCCESS, res)
    },
    sendOTPError: (rq: string): IForgetPWSendOTPError => {
        NotifyHelper.Error(rq)
        return baseAction(eForgetPWActionTypeIds.SEND_OTP_ERROR, rq)
    },

    resetPWStart: (rq: IResetNewPassWordRequest): IForgetPWResetPWStart => baseAction(eForgetPWActionTypeIds.RESET_PW_START, rq),
    resetPWSuccess: (): IForgetPWResetPWSuccess => {
        NotifyHelper.Success("Success")
        return baseAction(eForgetPWActionTypeIds.RESET_PW_SUCCESS, undefined)
    },
    resetPWError: (rq: string): IForgetPWResetPWError => {
        NotifyHelper.Error(rq)
        return baseAction(eForgetPWActionTypeIds.RESET_PW_ERROR, rq)
    },
}