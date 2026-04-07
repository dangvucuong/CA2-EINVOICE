import { call, put, takeEvery, takeLatest } from "redux-saga/effects";
import { daiLyApi } from "../../../api/category/daiLyApi";
import { IBaseRespone } from "../../../models/responses/IBaseRespone";
import { IDaiLyDeleteStart, IDaiLyLoadStart, IDaiLySaveStart, eDaiLyActionTypeIds } from "../../action-types/category/IDaiLyActionType";
import { rootAction } from "../../actions/rootAction";

const mainAction = rootAction.category.daiLyAction;

export function* daiLySaga(): any {
    yield takeLatest(eDaiLyActionTypeIds.LOAD_START, loadWorker)
    yield takeEvery(eDaiLyActionTypeIds.SAVE_START, saveWorker)
    yield takeEvery(eDaiLyActionTypeIds.DELETE_START, deleteWorker)

}
function* loadWorker(action:IDaiLyLoadStart): any {
    const res: IBaseRespone = yield call(daiLyApi.getByDonViPaging,action.payload)
    if (res.is_success) {
        yield put(mainAction.loadSuccess(res.data))
    } else {
        yield put(mainAction.loadError(res.message ?? ""))
    }
}

function* saveWorker(action: IDaiLySaveStart): any {
    let res: IBaseRespone;
    if (action.payload.id > 0) {
        res = yield call(daiLyApi.update, action.payload)
    } else {
        res = yield call(daiLyApi.insert, action.payload)
    }
    if (res.is_success) {
        yield put(mainAction.saveSuccess(res.data))
    } else {
        yield put(mainAction.saveError(res.message ?? ""))
    }
}
function* deleteWorker(action: IDaiLyDeleteStart): any {
    const res: IBaseRespone = yield call(daiLyApi.delete, action.payload)
    if (res.is_success) {
        yield put(mainAction.deleteSuccess())
    } else {
        yield put(mainAction.deleteError(res.message ?? ""))
    }
}
