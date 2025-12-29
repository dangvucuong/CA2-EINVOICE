import { call, put, takeLatest } from "redux-saga/effects";
import { subSystemApi } from '../../../api/user/subSystemApi';
import { IBaseRespone } from "../../../models/responses/IBaseRespone";
import { eSubSystemTypeIds } from "../../action-types/user/ISubSystemType";
import { rootAction } from "../../actions/rootAction";


export function* subSystemSaga(): any {
    yield takeLatest(eSubSystemTypeIds.LOAD_START, loadWorker)
    

}
function* loadWorker(): any {
    const res: IBaseRespone = yield call(subSystemApi.getAll)
    if (res.is_success) {
        yield put(rootAction.user.subSystemAction.loadSuccess(res.data))
    } else {
        yield put(rootAction.user.subSystemAction.loadError(res.message ?? ""))
    }
}
