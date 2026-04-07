import { call, put, takeEvery, takeLatest } from "redux-saga/effects";
import { hoaDonApi } from "../../../api/hoa-don/hoaDonApi";
import { IBaseRespone } from "../../../models/responses/IBaseRespone";
import { IHoaDonDeleteStart, IHoaDonLoadStart, IHoaDonSaveStart, eHoaDonActionTypeIds } from "../../action-types/hoa-don/IHoaDonActionType";
import { rootAction } from "../../actions/rootAction";

const mainAction = rootAction.hoaDon.hoaDonAction;

export function* hoaDonSaga(): any {
    yield takeLatest(eHoaDonActionTypeIds.LOAD_START, loadWorker)
    yield takeEvery(eHoaDonActionTypeIds.SAVE_START, saveWorker)
    yield takeEvery(eHoaDonActionTypeIds.DELETE_START, deleteWorker)

}
function* loadWorker(action: IHoaDonLoadStart): any {
    const res: IBaseRespone = yield call(hoaDonApi.selectByDonViPaging, action.payload)
    if (res.is_success) {
        yield put(mainAction.loadSuccess(res.data))
    } else {
        yield put(mainAction.loadError(res.message ?? ""))
    }
}

function* saveWorker(action: IHoaDonSaveStart): any {
    const res: IBaseRespone = yield call(hoaDonApi.save, action.payload)
    if (res.is_success) {
        yield put(mainAction.saveSuccess(res.data))
    } else {
        yield put(mainAction.saveError(res.message ?? ""))
    }
}
function* deleteWorker(action: IHoaDonDeleteStart): any {
    const res: IBaseRespone = yield call(hoaDonApi.delete, action.payload)
    if (res.is_success) {
        yield put(mainAction.deleteSuccess())
    } else {
        yield put(mainAction.deleteError(res.message ?? ""))
    }
}
