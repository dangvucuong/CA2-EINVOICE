import { call, put, takeLatest } from "redux-saga/effects";
import { watermarkTemplateApi } from "../../../api/category/watermarkTemplateApi";
import { IBaseRespone } from "../../../models/responses/IBaseRespone";
import { IKhachHangLoadStart } from "../../action-types/category/IKhachHangActionType";
import { eWatermarkTemplateActionTypeIds } from "../../action-types/category/IWatermarkTemplateActionType";
import { rootAction } from "../../actions/rootAction";

const mainAction = rootAction.category.watermarkTemplateAction;

export function* watermarkTemplateSaga(): any {
    yield takeLatest(eWatermarkTemplateActionTypeIds.LOAD_START, loadWorker)


}
function* loadWorker(action:IKhachHangLoadStart): any {
    const res: IBaseRespone = yield call(watermarkTemplateApi.getAll)
    if (res.is_success) {
        yield put(mainAction.loadSuccess(res.data))
    } else {
        yield put(mainAction.loadError(res.message ?? ""))
    }
}
