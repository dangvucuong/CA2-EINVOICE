
import { call, put, takeLatest } from 'redux-saga/effects';
import { ILocalizedResourceLoadStart, eLocalizedResourceActionTypeIds } from '../../action-types/commons/ILocalizedResourceActionType';
import { IBaseRespone } from '../../../models/responses/IBaseRespone';
import { localizedReourceApi } from '../../../api/common/localizedReourceApi';
import { rootAction } from '../../actions/rootAction';

export function* localizedResourceSaga() {
    // yield takeLatest(eLocalizedResourceActionTypeIds.LOAD_START, loadWorker)
}
function* loadWorker(action: ILocalizedResourceLoadStart) {
    try {
        const res: IBaseRespone = yield call(localizedReourceApi.getAll, action.payload);
        if (res.is_success) {
            yield put(rootAction.common.localizedResourceAction.loadSuccess(res.data))
        } else {
            yield put(rootAction.common.localizedResourceAction.loadError(res.message ?? "Error"))
        }
    } catch (error: any) {
        yield put(rootAction.common.localizedResourceAction.loadError(error?.response?.data?.message ?? "Error"))
    }
}