import { call, put, takeEvery, takeLatest } from "redux-saga/effects";
import { rootAction } from "../../actions/rootAction";
import { IHangHoaDeleteStart, IHangHoaLoadStart, IHangHoaSaveStart, eHangHoaActionTypeIds } from "../../action-types/category/IHangHoaActionType";
import { IBaseRespone } from "../../../models/responses/IBaseRespone";
import { hangHoaApi } from "../../../api/category/hangHoaApi";

const mainAction = rootAction.category.hangHoaAction;

export function* hangHoaSaga(): any {
    yield takeLatest(eHangHoaActionTypeIds.LOAD_START, loadWorker)
    yield takeEvery(eHangHoaActionTypeIds.SAVE_START, saveWorker)
    yield takeEvery(eHangHoaActionTypeIds.DELETE_START, deleteWorker)

}
function* loadWorker(action:IHangHoaLoadStart): any {
    const res: IBaseRespone = yield call(hangHoaApi.getByDonViPaging,action.payload)
    if (res.is_success) {
        yield put(mainAction.loadSuccess(res.data))
    } else {
        yield put(mainAction.loadError(res.message ?? ""))
    }
}

function* saveWorker(action: IHangHoaSaveStart): any {
    let res: IBaseRespone;
    if (action.payload.id > 0) {
        res = yield call(hangHoaApi.update, action.payload)
    } else {
        res = yield call(hangHoaApi.insert, action.payload)
    }
    if (res.is_success) {
        yield put(mainAction.saveSuccess(res.data))
    } else {
        yield put(mainAction.saveError(res.message ?? ""))
    }
}
function* deleteWorker(action: IHangHoaDeleteStart): any {
    const res: IBaseRespone = yield call(hangHoaApi.delete, action.payload)
    if (res.is_success) {
        yield put(mainAction.deleteSuccess())
    } else {
        yield put(mainAction.deleteError(res.message ?? ""))
    }
}
