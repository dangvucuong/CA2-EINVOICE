import { IContactStatus } from "../../../models/responses/contact/IContactStatus";
import { eReducerStatusBase } from "../eReducerStatusBase";

export interface IContactStatusReducer {
    status: eReducerStatusBase,
    contactStatuses: IContactStatus[],
}