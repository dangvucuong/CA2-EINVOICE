import { call, put, takeEvery, takeLatest } from "redux-saga/effects";
import { roleApi } from "../../../api/user/roleApi";
import { IBaseRespone } from "../../../models/responses/IBaseRespone";
import { IRoleDeleteStart, IRoleLoadStart, IRoleSaveStart, eRoleActionTypeIds } from '../../action-types/user/IRoleActionType';
import { rootAction } from "../../actions/rootAction";


export function* roleSaga(): any {
    yield takeLatest(eRoleActionTypeIds.LOAD_START, loadRoleWorker)
    yield takeEvery(eRoleActionTypeIds.SAVE_START, saveRoleWorker)
    yield takeEvery(eRoleActionTypeIds.DELETE_START, deleteRoleWorker)

}
function* loadRoleWorker(action: IRoleLoadStart): any {
    const res: IBaseRespone = yield call(roleApi.getRoles)
    if (res.is_success) {
        yield put(rootAction.user.roleAction.loadSuccess(res.data))
    } else {
        yield put(rootAction.user.roleAction.loadError(res.message ?? ""))
    }
}

function* saveRoleWorker(action: IRoleSaveStart): any {
    let res: IBaseRespone;
    if (action.payload.id > 0) {
        res = yield call(roleApi.updateRole, action.payload)
    } else {
        res = yield call(roleApi.insertRole, action.payload)
    }
    if (res.is_success) {
        yield put(rootAction.user.roleAction.saveSuccess(res.data))
    } else {
        yield put(rootAction.user.roleAction.saveError(res.message ?? ""))
    }
}
function* deleteRoleWorker(action: IRoleDeleteStart): any {
    const res: IBaseRespone = yield call(roleApi.deleteRole, action.payload)
    if (res.is_success) {
        yield put(rootAction.user.roleAction.deleteSuccess())
    } else {
        yield put(rootAction.user.roleAction.deleteError(res.message ?? ""))
    }
}
