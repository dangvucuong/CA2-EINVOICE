import { IPagingResultSummary } from "../../../models/responses/IBasePagingRespone";
import { IContact } from "../../../models/responses/contact/IContact";
import { eReducerStatusBase } from "../eReducerStatusBase";
import { IContactSelectRequest } from './../../../models/requests/contact/IContactSelectRequest';

export interface IContactReducer {
    status: eReducerStatusBase,
    contacts: IContact[],
    contactEditing?: IContact,
    isShowEditModal?: boolean,
    isShowDeleteConfirm?: boolean,
    contactSelectedId?: number,
    paging_res?: IPagingResultSummary,
    filter: IContactSelectRequest,


}