import { IForgetPWActionType, eForgetPWActionTypeIds } from '../../action-types/account/IForgetPWActionType';
import { IForgetPWReducer, eForgetPWReducerStatus } from '../../reducer-models/account/IForgetPWReducer';
const iniState: IForgetPWReducer = {

}
export const forgetPWReducer = (state: IForgetPWReducer = iniState, action: IForgetPWActionType): IForgetPWReducer => {
    switch (action.type) {
        case eForgetPWActionTypeIds.SEND_OTP_START:
            return {
                ...state,
                status: eForgetPWReducerStatus.is_sending_otp,
                message:undefined
            }
        case eForgetPWActionTypeIds.SEND_OTP_SUCCESS:
            return {
                ...state,
                status: eForgetPWReducerStatus.is_send_otp_success,
                message: undefined,
                otpRespone: action.payload
            }
        case eForgetPWActionTypeIds.SEND_OTP_ERROR:
            return {
                ...state,
                status: eForgetPWReducerStatus.is_send_otp_error,
                message:action.payload
            }
        case eForgetPWActionTypeIds.RESET_PW_START:
            return {
                ...state,
                status: eForgetPWReducerStatus.is_reseting_pw,
                message:undefined
            }
        case eForgetPWActionTypeIds.RESET_PW_SUCCESS:
            return {
                ...state,
                status: eForgetPWReducerStatus.is_reset_pw_success,
                message: undefined,
            }
        case eForgetPWActionTypeIds.RESET_PW_ERROR:
            return {
                ...state,
                status: eForgetPWReducerStatus.is_reset_pw_error,
                message: action.payload
            }

        default:
            return {
                ...state
            }
    }
}