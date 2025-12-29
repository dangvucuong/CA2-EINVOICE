import { call, put, takeLatest } from "redux-saga/effects";
import { notifyApi } from "../../../api/notify/notifyApi";
import { IBaseRespone } from "../../../models/responses/IBaseRespone";
import { INotifySummaryLoadStart, eNotifyActionTypeIds } from "../../action-types/notify/INotifyActionType";
import { rootAction } from "../../actions/rootAction";

const mainAction = rootAction.notify.notifyAction;

export function* notifySaga(): any {
    yield takeLatest(eNotifyActionTypeIds.LOAD_SUMMARY_START, loadSummaryWorker)

}
function* loadSummaryWorker(action:INotifySummaryLoadStart): any {
    const res: IBaseRespone = yield call(notifyApi.getSummary)
    if (res.is_success) {
        yield put(mainAction.loadSummarySuccess(res.data))
    } else {
        yield put(mainAction.loadSummaryError(res.message ?? ""))
    }
}
