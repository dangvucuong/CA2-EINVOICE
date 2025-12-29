import { call, put, takeLatest } from "redux-saga/effects";
import { loaiHoaDonCTApi } from "../../../api/hoa-don/loaiHoaDonCTApi";
import { IBaseRespone } from "../../../models/responses/IBaseRespone";
import { eLoaiHoaDonCTActionTypeIds } from "../../action-types/hoa-don/ILoaiHoaDonCTActionType";
import { rootAction } from "../../actions/rootAction";

const mainAction = rootAction.hoaDon.loaiHoaDonCTAction;

export function* loaiHoaDonCTSaga(): any {
    yield takeLatest(eLoaiHoaDonCTActionTypeIds.LOAD_START, loadWorker)
   

}
function* loadWorker(): any {
    const res: IBaseRespone = yield call(loaiHoaDonCTApi.selectAll)
    if (res.is_success) {
        yield put(mainAction.loadSuccess(res.data))
    } else {
        yield put(mainAction.loadError(res.message ?? ""))
    }
}

