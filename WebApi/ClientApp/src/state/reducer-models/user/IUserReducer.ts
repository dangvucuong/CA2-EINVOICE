import { IPagingRequest } from "../../../models/requests/IPagingRequest";
import { IPagingResultSummary } from "../../../models/responses/IBasePagingRespone";
import { IUser } from "../../../models/responses/user/IUser";
import { IUserEditModel } from "../../../models/responses/user/IUserEditModel";
import { eReducerStatusBase } from "../eReducerStatusBase";
export enum eUserReducerStatus {
    is_loading_form
}
export interface IUserReducer {
    status: eReducerStatusBase | eUserReducerStatus,
    users: IUser[],
    isShowEditModal?: boolean,
    isShowDeleteConfirm?: boolean,
    filter: IPagingRequest,
    paging_res?: IPagingResultSummary,
    userEditing?: IUser,
    userEditingForm?: IUserEditModel
}