import { IHoaDonDangKyPhatHanh } from "../../../models/responses/hoa-don/IHoaDonDangKyPhatHanh";
import { eReducerStatusBase } from "../eReducerStatusBase";

export interface IHoaDonDangKyPhatHanhReducer {
    status: eReducerStatusBase,
    hoaDonDangKyPhatHanhs: IHoaDonDangKyPhatHanh[],
    hoaDonDangKyPhatHanhEditing?: IHoaDonDangKyPhatHanh,
    isShowEditModal?: boolean,
    isShowDeleteConfirm?: boolean,
    hoaDonDangKyPhatHanhSelectedId?: number,

}