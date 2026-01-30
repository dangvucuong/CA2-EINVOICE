import { call, put, takeEvery, takeLatest } from "redux-saga/effects";
import { userApi } from "../../../api/user/userApi";
import { IBaseRespone } from "../../../models/responses/IBaseRespone";
import {
  IUserDeleteStart,
  IUserLoadFormStart,
  IUserLoadStart,
  IUserSaveFormStart,
  eUserActionTypeIds,
} from "../../action-types/user/IUserActionType";
import { rootAction } from "../../actions/rootAction";

export function* userSaga(): any {
  yield takeLatest(eUserActionTypeIds.LOAD_START, loadUserWorker);
  // yield takeLatest(eUserActionTypeIds.LOAD_BY_DONVI_START, loadUserByDonViWorker)
  yield takeLatest(eUserActionTypeIds.LOAD_USER_FORM_START, loadUserFormWorker);
  yield takeEvery(eUserActionTypeIds.SAVE_USER_START, saveUserFormWorker);
  yield takeEvery(eUserActionTypeIds.DELETE_START, deleteWorker);
}

function* loadUserWorker(action: IUserLoadStart): any {
  const res: IBaseRespone = yield call(userApi.getUsers, action.payload);
  if (res.is_success) {
    yield put(rootAction.user.userAction.loadSuccess(res.data));
  } else {
    yield put(rootAction.user.userAction.loadError(res.message ?? ""));
  }
}

// function* loadUserByDonViWorker(action: IUserLoadByDonViStart): any {
//     const res: IBaseRespone = yield call(userApi.getUsersByDonVi, action.payload)
//     if (res.is_success) {
//         yield put(rootAction.user.userAction.loadByDonViSuccess(res.data))
//     } else {
//         yield put(rootAction.user.userAction.loadByDonViError(res.message ?? ""))
//     }
// }

function* loadUserFormWorker(action: IUserLoadFormStart): any {
  const res: IBaseRespone = yield call(userApi.getUser, action.payload);
  if (res.is_success) {
    yield put(rootAction.user.userAction.loadFormSuccess(res.data));
  } else {
    yield put(rootAction.user.userAction.loadFormError(res.message ?? ""));
  }
}

function* saveUserFormWorker(action: IUserSaveFormStart): any {
  const res: IBaseRespone = yield call(userApi.saveUser, action.payload);
  if (res.is_success) {
    yield put(rootAction.user.userAction.saveFormSuccess(res.data));
  } else {
    yield put(rootAction.user.userAction.saveFormError(res.message ?? ""));
  }
}

function* deleteWorker(action: IUserDeleteStart): any {
  const res: IBaseRespone = yield call(userApi.deleteUser, action.payload);
  if (res.is_success) {
    yield put(rootAction.user.userAction.deleteSuccess());
  } else {
    yield put(rootAction.user.userAction.deleteError(res.message ?? ""));
  }
}
