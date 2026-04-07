import { call, put, takeLatest } from "redux-saga/effects";
import { companySizeApi } from '../../../api/contact/companySizeApi';
import { IBaseRespone } from "../../../models/responses/IBaseRespone";
import { eCompanySizeActionTypeIds } from "../../action-types/contact/ICompanySizeActionType";
import { IContactStatusLoadStart } from '../../action-types/contact/IContactStatusActionType';
import { rootAction } from "../../actions/rootAction";

const mainAction = rootAction.contact.companySizeAction;

export function* companySizeSaga(): any {
    yield takeLatest(eCompanySizeActionTypeIds.LOAD_START, loadWorker)

}
function* loadWorker(action:IContactStatusLoadStart): any {
    const res: IBaseRespone = yield call(companySizeApi.getAll)
    if (res.is_success) {
        yield put(mainAction.loadSuccess(res.data))
    } else {
        yield put(mainAction.loadError(res.message ?? ""))
    }
}
