import { IToKhai } from "../../../models/responses/to-khai/IToKhai";
import { eReducerStatusBase } from "../eReducerStatusBase";

export interface IToKhaiReducer {
    status: eReducerStatusBase,
    toKhais: IToKhai[],
    toKhaiEditing?: IToKhai,
    isShowLogModal: boolean,
    isShowDeleteConfirm: boolean


}