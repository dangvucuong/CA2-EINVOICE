import { call, put, takeEvery, takeLatest } from "redux-saga/effects";
import { roleSubSystemApi } from "../../../api/user/roleSubSystemApi";
import { IBaseRespone } from "../../../models/responses/IBaseRespone";
import { IRoleSubSystemAddStart, IRoleSubSystemLoadStart, IRoleSubSystemRemoveStart, eRoleSubSystemTypeIds } from "../../action-types/user/IRoleSubSystemType";
import { rootAction } from "../../actions/rootAction";


export function* roleSubSystemSaga(): any {
    yield takeLatest(eRoleSubSystemTypeIds.LOAD_START, loadWorker)
    yield takeEvery(eRoleSubSystemTypeIds.ADD_SUBSYTEM_START, addWorker)
    yield takeEvery(eRoleSubSystemTypeIds.REMOVE_SUBSYSTEM_START, removeWorker)
    

}

function* loadWorker(action:IRoleSubSystemLoadStart): any {
    const res: IBaseRespone = yield call(roleSubSystemApi.getByRoles, action.payload)
    if (res.is_success) {
        yield put(rootAction.user.roleSubSystemAction.loadSuccess(res.data))
    } else {
        yield put(rootAction.user.roleSubSystemAction.loadError(res.message ?? ""))
    }
}


function* addWorker(action:IRoleSubSystemAddStart): any {
    const res: IBaseRespone = yield call(roleSubSystemApi.addNewSubSystem, action.payload)
    if (res.is_success) {
        yield put(rootAction.user.roleSubSystemAction.addSuccess())
    } else {
        yield put(rootAction.user.roleSubSystemAction.addError(res.message ?? ""))
    }
}


function* removeWorker(action:IRoleSubSystemRemoveStart): any {
    const res: IBaseRespone = yield call(roleSubSystemApi.removeSubSystem, action.payload)
    if (res.is_success) {
        yield put(rootAction.user.roleSubSystemAction.removeSuccess())
    } else {
        yield put(rootAction.user.roleSubSystemAction.removeError(res.message ?? ""))
    }
}
