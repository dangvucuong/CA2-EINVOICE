import { call, put, takeEvery, takeLatest } from "redux-saga/effects";
import { IBaseRespone } from "../../../models/responses/IBaseRespone";
import { IDonViDeleteStart, IDonViLoadStart, IDonViSaveStart, eDonViActionTypeIds } from "../../action-types/category/IDonViActionType";
import { rootAction } from "../../actions/rootAction";
import { donViApi } from './../../../api/category/donViApi';

const mainAction = rootAction.category.donViActionType;

export function* donViSaga(): any {
    yield takeLatest(eDonViActionTypeIds.LOAD_START, loadWorker)
    yield takeEvery(eDonViActionTypeIds.SAVE_START, saveWorker)
    yield takeEvery(eDonViActionTypeIds.DELETE_START, deleteWorker)

}
function* loadWorker(action:IDonViLoadStart): any {
    const res: IBaseRespone = yield call(donViApi.getAll,action.payload)
    if (res.is_success) {
        yield put(mainAction.loadSuccess(res.data))
    } else {
        yield put(mainAction.loadError(res.message ?? ""))
    }
}

function* saveWorker(action: IDonViSaveStart): any {
    let res: IBaseRespone;
    if (action.payload.id > 0) {
        res = yield call(donViApi.update, action.payload)
    } else {
        res = yield call(donViApi.insert, action.payload)
    }
    if (res.is_success) {
        yield put(mainAction.saveSuccess(res.data))
    } else {
        yield put(mainAction.saveError(res.message ?? ""))
    }
}
function* deleteWorker(action: IDonViDeleteStart): any {
    const res: IBaseRespone = yield call(donViApi.delete, action.payload)
    if (res.is_success) {
        yield put(mainAction.deleteSuccess())
    } else {
        yield put(mainAction.deleteError(res.message ?? ""))
    }
}
