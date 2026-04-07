import { call, put, takeLatest } from "redux-saga/effects";
import { loaiHoaDonCTTemplateApi } from "../../../api/hoa-don/loaiHoaDonCTTemplateApi";
import { IBaseRespone } from "../../../models/responses/IBaseRespone";
import { eLoaiHoaDonCTTemplateActionTypeIds } from "../../action-types/hoa-don/ILoaiHoaDonCTTemplateActionType";
import { rootAction } from "../../actions/rootAction";

const mainAction = rootAction.hoaDon.loaiHoaDonCTTemplateAction;

export function* loaiHoaDonCTTemplateSaga(): any {
    yield takeLatest(eLoaiHoaDonCTTemplateActionTypeIds.LOAD_START, loadWorker)
   

}
function* loadWorker(): any {
    const res: IBaseRespone = yield call(loaiHoaDonCTTemplateApi.selectAll)
    if (res.is_success) {
        yield put(mainAction.loadSuccess(res.data))
    } else {
        yield put(mainAction.loadError(res.message ?? ""))
    }
}

