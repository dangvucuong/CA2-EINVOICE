import { call, put, takeLatest } from "redux-saga/effects";
import { menuApi } from "../../../api/user/menuApi";
import { IBaseRespone } from "../../../models/responses/IBaseRespone";
import { IMenuLoadStart, eMenuActionTypeIds } from "../../action-types/user/IMenuActionType";
import { rootAction } from "../../actions/rootAction";


export function* menuSaga(): any {
    yield takeLatest(eMenuActionTypeIds.LOAD_START, loadWorker)


}
function* loadWorker(action:IMenuLoadStart): any {
    const res: IBaseRespone = yield call(menuApi.getBySubSystem, action.payload)
    if (res.is_success) {
        yield put(rootAction.user.menuAction.loadSuccess(res.data))
    } else {
        yield put(rootAction.user.menuAction.loadError(res.message ?? ""))
    }
}
