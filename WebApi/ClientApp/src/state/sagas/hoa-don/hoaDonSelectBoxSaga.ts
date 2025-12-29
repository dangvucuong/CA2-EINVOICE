import { call, put, takeLatest } from "redux-saga/effects";
import { hoaDonApi } from "../../../api/hoa-don/hoaDonApi";
import { IBaseRespone } from "../../../models/responses/IBaseRespone";
import { IHoaDonLoadStart } from "../../action-types/hoa-don/IHoaDonActionType";
import { eHoaDonSelectBoxActionTypeIds } from "../../action-types/hoa-don/IHoaDonSelectBoxActionType";
import { rootAction } from "../../actions/rootAction";

const mainAction = rootAction.hoaDon.hoaDonSelectBoxAction;

export function* hoaDonSelectBoxSaga(): any {
    yield takeLatest(eHoaDonSelectBoxActionTypeIds.LOAD_START, loadWorker)
}
function* loadWorker(action: IHoaDonLoadStart): any {
    const res: IBaseRespone = yield call(hoaDonApi.selectByDonViPaging, action.payload)
    if (res.is_success) {
        yield put(mainAction.loadSuccess(res.data))
    } else {
        yield put(mainAction.loadError(res.message ?? ""))
    }
}
