import { IMauHoaDonVM } from "../../../models/responses/hoa-don/IMauHoaDonVM";
import { eReducerStatusBase } from "../eReducerStatusBase";

export interface IMauHoaDonReducer {
    status: eReducerStatusBase,
    mauHoaDons: IMauHoaDonVM[],
    mauHoaDonEditing?: IMauHoaDonVM,
    isShowEditModal?: boolean,
    isShowDeleteConfirm?: boolean,
    mauHoaDonSelectedId?: number,
}