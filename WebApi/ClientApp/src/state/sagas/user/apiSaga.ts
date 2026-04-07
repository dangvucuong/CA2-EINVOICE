import { call, put, takeLatest } from "redux-saga/effects";
import { apiApi } from '../../../api/user/apiApi';
import { IBaseRespone } from "../../../models/responses/IBaseRespone";
import { IApiLoadStart, eApiActionTypeIds } from "../../action-types/user/IApiActionType";
import { rootAction } from "../../actions/rootAction";


export function* apiSaga(): any {
    yield takeLatest(eApiActionTypeIds.LOAD_START, loadWorker)


}
function* loadWorker(action:IApiLoadStart): any {
    const res: IBaseRespone = yield call(apiApi.getBySubSystem, action.payload)
    if (res.is_success) {
        yield put(rootAction.user.apiAction.loadSuccess(res.data))
    } else {
        yield put(rootAction.user.apiAction.loadError(res.message ?? ""))
    }
}
