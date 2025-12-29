import { call, put, takeLatest } from "redux-saga/effects";
import { logApi } from "../../../api/user/logApi";
import { IBaseRespone } from "../../../models/responses/IBaseRespone";
import { ILogLoadStart, eLogActionTypeIds } from "../../action-types/user/ILogActionType";
import { rootAction } from "../../actions/rootAction";


export function* logSaga(): any {
    yield takeLatest(eLogActionTypeIds.LOAD_START, loadWorker)


}
function* loadWorker(action:ILogLoadStart): any {
    const res: IBaseRespone = yield call(logApi.select, action.payload)
    if (res.is_success) {
        yield put(rootAction.user.logAction.loadSuccess(res.data))
    } else {
        yield put(rootAction.user.logAction.loadError(res.message ?? ""))
    }
}
