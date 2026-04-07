import { call, put, takeEvery, takeLatest } from "redux-saga/effects";
import { mauHoaDonApi } from "../../../api/hoa-don/mauHoaDonApi";
import { IBaseRespone } from "../../../models/responses/IBaseRespone";
import { IMauHoaDonDeleteStart, IMauHoaDonLoadStart, IMauHoaDonSaveStart, eMauHoaDonActionTypeIds } from "../../action-types/hoa-don/IMauHoaDonActionType";
import { rootAction } from "../../actions/rootAction";

const mainAction = rootAction.hoaDon.mauHoaDonAction;

export function* mauHoaDonSaga(): any {
    yield takeLatest(eMauHoaDonActionTypeIds.LOAD_START, loadWorker)
    yield takeEvery(eMauHoaDonActionTypeIds.SAVE_START, saveWorker)
    yield takeEvery(eMauHoaDonActionTypeIds.DELETE_START, deleteWorker)

}
function* loadWorker(action:IMauHoaDonLoadStart): any {
    const res: IBaseRespone = yield call(mauHoaDonApi.getByDonVi)
    if (res.is_success) {
        yield put(mainAction.loadSuccess(res.data))
    } else {
        yield put(mainAction.loadError(res.message ?? ""))
    }
}

function* saveWorker(action: IMauHoaDonSaveStart): any {
    let res: IBaseRespone;
    if (action.payload.id > 0) {
        res = yield call(mauHoaDonApi.update, action.payload)
    } else {
        res = yield call(mauHoaDonApi.insert, action.payload)
    }
    if (res.is_success) {
        yield put(mainAction.saveSuccess(res.data))
    } else {
        yield put(mainAction.saveError(res.message ?? ""))
    }
}
function* deleteWorker(action: IMauHoaDonDeleteStart): any {
    const res: IBaseRespone = yield call(mauHoaDonApi.delete, action.payload)
    if (res.is_success) {
        yield put(mainAction.deleteSuccess())
    } else {
        yield put(mainAction.deleteError(res.message ?? ""))
    }
}
