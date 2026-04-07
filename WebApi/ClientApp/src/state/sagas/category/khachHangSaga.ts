import { call, put, takeEvery, takeLatest } from "redux-saga/effects";
import { rootAction } from "../../actions/rootAction";
import { IKhachHangDeleteStart, IKhachHangLoadStart, IKhachHangSaveStart, eKhachHangActionTypeIds } from "../../action-types/category/IKhachHangActionType";
import { IBaseRespone } from "../../../models/responses/IBaseRespone";
import { khachHangApi } from "../../../api/category/khachHangApi";

const mainAction = rootAction.category.khachHangAction;

export function* khachHangSaga(): any {
    yield takeLatest(eKhachHangActionTypeIds.LOAD_START, loadWorker)
    yield takeEvery(eKhachHangActionTypeIds.SAVE_START, saveWorker)
    yield takeEvery(eKhachHangActionTypeIds.DELETE_START, deleteWorker)

}
function* loadWorker(action:IKhachHangLoadStart): any {
    const res: IBaseRespone = yield call(khachHangApi.getByDonViPaging,action.payload)
    if (res.is_success) {
        yield put(mainAction.loadSuccess(res.data))
    } else {
        yield put(mainAction.loadError(res.message ?? ""))
    }
}

function* saveWorker(action: IKhachHangSaveStart): any {
    let res: IBaseRespone;
    if (action.payload.id > 0) {
        res = yield call(khachHangApi.update, action.payload)
    } else {
        res = yield call(khachHangApi.insert, action.payload)
    }
    if (res.is_success) {
        yield put(mainAction.saveSuccess(res.data))
    } else {
        yield put(mainAction.saveError(res.message ?? ""))
    }
}
function* deleteWorker(action: IKhachHangDeleteStart): any {
    const res: IBaseRespone = yield call(khachHangApi.delete, action.payload)
    if (res.is_success) {
        yield put(mainAction.deleteSuccess())
    } else {
        yield put(mainAction.deleteError(res.message ?? ""))
    }
}
