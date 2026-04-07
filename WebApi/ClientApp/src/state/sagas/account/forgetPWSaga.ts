import { call, put, takeLatest } from "redux-saga/effects";
import { forgetPWApi } from "../../../api/account/forgetPWApi";
import { IBaseRespone } from "../../../models/responses/IBaseRespone";
import { IForgetPWResetPWStart, IForgetPWSendOTPStart, eForgetPWActionTypeIds } from '../../action-types/account/IForgetPWActionType';
import { rootAction } from "../../actions/rootAction";


export function* forgetPWSaga(): any {
    yield takeLatest(eForgetPWActionTypeIds.SEND_OTP_START, sendOTPWorker)
    yield takeLatest(eForgetPWActionTypeIds.RESET_PW_START, resetPWWorker)
}
function* sendOTPWorker(action: IForgetPWSendOTPStart): any {
    const res: IBaseRespone = yield call(forgetPWApi.sendOTP, action.payload)
    if (res.is_success) {
        yield put(rootAction.forgetPWAction.sendOTPSuccess(res.data))
    } else {
        yield put(rootAction.forgetPWAction.sendOTPError(res.message ?? ""))
    }
}

function* resetPWWorker(action: IForgetPWResetPWStart): any {
    const res: IBaseRespone = yield call(forgetPWApi.resetPW, action.payload)
    if (res.is_success) {
        yield put(rootAction.forgetPWAction.resetPWSuccess())
    } else {
        yield put(rootAction.forgetPWAction.resetPWError(res.message ?? ""))
    }
}