import { call, put, takeEvery, takeLatest } from "redux-saga/effects";
import { hoaDonDangKyPhatHanhApi } from "../../../api/hoa-don/hoaDonDangKyPhatHanhApi";
import { IBaseRespone } from "../../../models/responses/IBaseRespone";
import { IHoaDonDangKyPhatHanhDeleteStart, IHoaDonDangKyPhatHanhLoadStart, IHoaDonDangKyPhatHanhSaveStart, eHoaDonDangKyPhatHanhActionTypeIds } from "../../action-types/hoa-don/IHoaDonDangKyPhatHanhActionType";
import { rootAction } from "../../actions/rootAction";

const mainAction = rootAction.hoaDon.hoaDonDangKyPhatHanhAction;

export function* hoaDonDangKyPhatHanhSaga(): any {
    yield takeLatest(eHoaDonDangKyPhatHanhActionTypeIds.LOAD_START, loadWorker)
    yield takeEvery(eHoaDonDangKyPhatHanhActionTypeIds.SAVE_START, saveWorker)
    yield takeEvery(eHoaDonDangKyPhatHanhActionTypeIds.DELETE_START, deleteWorker)

}
function* loadWorker(action:IHoaDonDangKyPhatHanhLoadStart): any {
    const res: IBaseRespone = yield call(hoaDonDangKyPhatHanhApi.getByDonVi)
    if (res.is_success) {
        yield put(mainAction.loadSuccess(res.data))
    } else {
        yield put(mainAction.loadError(res.message ?? ""))
    }
}

function* saveWorker(action: IHoaDonDangKyPhatHanhSaveStart): any {
    let res: IBaseRespone;
    if (action.payload.id > 0) {
        res = yield call(hoaDonDangKyPhatHanhApi.update, action.payload)
    } else {
        res = yield call(hoaDonDangKyPhatHanhApi.insert, action.payload)
    }
    if (res.is_success) {
        yield put(mainAction.saveSuccess(res.data))
    } else {
        yield put(
            mainAction.saveError(
                res.message?.trim()
                    ? res.message
                    : "Cập nhật đăng ký phát hành không thành công"
            )
        )
    }
}
function* deleteWorker(action: IHoaDonDangKyPhatHanhDeleteStart): any {
    const res: IBaseRespone = yield call(hoaDonDangKyPhatHanhApi.delete, action.payload)
    if (res.is_success) {
        yield put(mainAction.deleteSuccess())
    } else {
        yield put(mainAction.deleteError(res.message ?? ""))
    }
}
