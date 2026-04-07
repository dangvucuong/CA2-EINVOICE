import { IAccountLoginStart } from './../../action-types/account/IAccountActionType';
import { call, put, takeLatest } from "redux-saga/effects";
import { accountApi } from "../../../api/account/accountApi";
import { IBaseRespone } from "../../../models/responses/IBaseRespone";
import { eAccountActionTypeIds } from "../../action-types/account/IAccountActionType";
import { rootAction } from "../../actions/rootAction";


export function* accountSaga(): any {
    yield takeLatest(eAccountActionTypeIds.GET_PROFILE, getProfileWorker)
    yield takeLatest(eAccountActionTypeIds.LOGIN_START, logInWorker)
}
function* getProfileWorker(): any {
    const res: IBaseRespone = yield call(accountApi.getProfile)
    if (res.is_success) {
        yield put(rootAction.accountAction.loadProfileSuccess(res.data))
    } else {
        yield put(rootAction.accountAction.loadProfileError(res.message ?? ""))
    }
}

function* logInWorker(action: IAccountLoginStart): any {
    const payload: any = action.payload
    if (payload.serial) {

        const res: IBaseRespone = yield call(accountApi.logInSerial, payload)
        if (res.is_success) {
            yield put(rootAction.accountAction.loginSuccess(res.data))
        } else {
            yield put(rootAction.accountAction.loginError(res.message ?? ""))
        }
    } else {
        if (payload.rs_ma_but_ky) {
            const res: IBaseRespone = yield call(accountApi.logInRS, payload)
            if (res.is_success) {
                yield put(rootAction.accountAction.loginSuccess(res.data))
            } else {
                yield put(rootAction.accountAction.loginError(res.message ?? ""))
            }
        } else {
            const res: IBaseRespone = yield call(accountApi.logIn, payload)
            if (res.is_success) {
                yield put(rootAction.accountAction.loginSuccess(res.data))
            } else {
                yield put(rootAction.accountAction.loginError(res.message ?? ""))
            }
        }
    }

}