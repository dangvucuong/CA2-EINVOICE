import { call, put, takeLatest } from "redux-saga/effects";
import { eAppConfigActionTypeIds } from "../../action-types/commons/IAppConfigActionType";
import { IBaseRespone } from "../../../models/responses/IBaseRespone";
import { rootAction } from "../../actions/rootAction";
import { appConfigApi } from "../../../api/common/appConfigApi";


export function* appConfigSaga(): any {
    yield takeLatest(eAppConfigActionTypeIds.LOAD_START, loadWorker)
}
function* loadWorker(): any {
    const res: IBaseRespone = yield call(appConfigApi.getAll)
    
    if (res.is_success) {
        yield put(rootAction.common.appConfigAction.loadSuccess(res.data))
    } else {
        yield put(rootAction.common.appConfigAction.loadError(res.message ?? ""))
    }
}