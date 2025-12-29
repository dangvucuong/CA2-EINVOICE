import { call, put, takeLatest } from "redux-saga/effects";
import { loaiHoaDonApi } from "../../../api/hoa-don/loaiHoaDonApi";
import { IBaseRespone } from "../../../models/responses/IBaseRespone";
import { ILoaiHoaDonLoadStart, eLoaiHoaDonActionTypeIds } from "../../action-types/hoa-don/ILoaiHoaDonActionType";
import { rootAction } from "../../actions/rootAction";

const mainAction = rootAction.hoaDon.loaiHoaDonAction;

export function* loaiHoaDonSaga(): any {
    yield takeLatest(eLoaiHoaDonActionTypeIds.LOAD_START, loadWorker)
   

}
function* loadWorker(action:ILoaiHoaDonLoadStart): any {
    const res: IBaseRespone = yield call(loaiHoaDonApi.selectAll)
    if (res.is_success) {
        yield put(mainAction.loadSuccess(res.data))
    } else {
        yield put(mainAction.loadError(res.message ?? ""))
    }
}

