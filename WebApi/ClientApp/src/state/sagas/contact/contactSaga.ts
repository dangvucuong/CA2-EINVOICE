import { call, put, takeEvery, takeLatest } from "redux-saga/effects";
import { IBaseRespone } from "../../../models/responses/IBaseRespone";
import { IContactLoadStart, IContactSaveStart, eContactActionTypeIds } from "../../action-types/contact/IContactActionType";
import { rootAction } from "../../actions/rootAction";
import { contactApi } from './../../../api/contact/contactApi';

const mainAction = rootAction.contact.contactAction;

export function* contactSaga(): any {
    yield takeLatest(eContactActionTypeIds.LOAD_START, loadWorker)
    yield takeEvery(eContactActionTypeIds.SAVE_START, saveWorker)

}
function* loadWorker(action:IContactLoadStart): any {
    const res: IBaseRespone = yield call(contactApi.select,action.payload)
    if (res.is_success) {
        yield put(mainAction.loadSuccess(res.data))
    } else {
        yield put(mainAction.loadError(res.message ?? ""))
    }
}

function* saveWorker(action: IContactSaveStart): any {
    let res: IBaseRespone;
    if (action.payload.id > 0) {
        res = yield call(contactApi.update, action.payload)
    } else {
        res = yield call(contactApi.insert, action.payload)
    }
    if (res.is_success) {
        yield put(mainAction.saveSuccess(res.data))
    } else {
        yield put(mainAction.saveError(res.message ?? ""))
    }
}