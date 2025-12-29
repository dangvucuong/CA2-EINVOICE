import { call, put, takeEvery, takeLatest } from "redux-saga/effects";
import { roleApiApi } from "../../../api/user/roleApiApi";
import { IBaseRespone } from "../../../models/responses/IBaseRespone";
import { IRoleApiAddStart, IRoleApiLoadStart, IRoleApiRemoveStart, eRoleApiActionTypeIds } from "../../action-types/user/IRoleApiActionType";
import { rootAction } from "../../actions/rootAction";


export function* roleApiSaga(): any {
    yield takeLatest(eRoleApiActionTypeIds.LOAD_START, loadWorker)
    yield takeEvery(eRoleApiActionTypeIds.ADD_API_START, addWorker)
    yield takeEvery(eRoleApiActionTypeIds.REMOVE_API_START, removeWorker)
    

}

function* loadWorker(action:IRoleApiLoadStart): any {
    const res: IBaseRespone = yield call(roleApiApi.getByRoles, action.payload)
    if (res.is_success) {
        yield put(rootAction.user.roleApiAction.loadSuccess(res.data))
    } else {
        yield put(rootAction.user.roleApiAction.loadError(res.message ?? ""))
    }
}


function* addWorker(action:IRoleApiAddStart): any {
    const res: IBaseRespone = yield call(roleApiApi.addNewApi, action.payload)
    if (res.is_success) {
        yield put(rootAction.user.roleApiAction.addSuccess())
    } else {
        yield put(rootAction.user.roleApiAction.addError(res.message ?? ""))
    }
}


function* removeWorker(action:IRoleApiRemoveStart): any {
    const res: IBaseRespone = yield call(roleApiApi.removeApi, action.payload)
    if (res.is_success) {
        yield put(rootAction.user.roleApiAction.removeSuccess())
    } else {
        yield put(rootAction.user.roleApiAction.removeError(res.message ?? ""))
    }
}
