import { call, put, takeEvery, takeLatest } from "redux-saga/effects";
import { toKhaiApi } from "../../../api/to-khai/toKhaiApi";
import { IBaseRespone } from "../../../models/responses/IBaseRespone";
import { IToKhaiDeleteStart, IToKhaiLoadStart, eToKhaiActionTypeIds } from "../../action-types/to-khai/IToKhaiActionType";
import { rootAction } from "../../actions/rootAction";

const mainAction = rootAction.toKhai.toKhaiAction;

export function* toKhaiSaga(): any {
    yield takeLatest(eToKhaiActionTypeIds.LOAD_START, loadWorker)
    yield takeEvery(eToKhaiActionTypeIds.DELETE_START, deleteWorker)

}
function* loadWorker(action:IToKhaiLoadStart): any {
    const res: IBaseRespone = yield call(toKhaiApi.getByDonVi)
    if (res.is_success) {
        yield put(mainAction.loadSuccess(res.data))
    } else {
        yield put(mainAction.loadError(res.message ?? ""))
    }
}

function* deleteWorker(action: IToKhaiDeleteStart): any {
    const res: IBaseRespone = yield call(toKhaiApi.delete, action.payload)
    if (res.is_success) {
        yield put(mainAction.deleteSuccess())
    } else {
        yield put(mainAction.deleteError(res.message ?? ""))
    }
}
