import { call, put, takeLatest } from "redux-saga/effects";
import { contactStatusApi } from '../../../api/contact/contactStatusApi';
import { IBaseRespone } from "../../../models/responses/IBaseRespone";
import { eContactStatusActionTypeIds } from "../../action-types/contact/IContactStatusActionType";
import { rootAction } from "../../actions/rootAction";
import { IContactStatusLoadStart } from './../../action-types/contact/IContactStatusActionType';

const mainAction = rootAction.contact.contactStatusAction;

export function* contactStatusSaga(): any {
    yield takeLatest(eContactStatusActionTypeIds.LOAD_START, loadWorker)

}
function* loadWorker(action:IContactStatusLoadStart): any {
    const res: IBaseRespone = yield call(contactStatusApi.getAll)
    if (res.is_success) {
        yield put(mainAction.loadSuccess(res.data))
    } else {
        yield put(mainAction.loadError(res.message ?? ""))
    }
}
