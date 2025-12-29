import { IProfileRespone } from "../../../models/responses/account/IProfileRespone";
import { IMenuViewModel } from "../../../models/responses/user/IMenu";
export enum eAccountReducerStatus {
    is_getting_profile,
    is_get_profile_done,
    is_logging_in,
    is_log_in_success,
    is_log_in_error,
}
export interface IAccountReducer {
    status?: eAccountReducerStatus,
    user?: IProfileRespone,
    appSelected?: IMenuViewModel,
    is_verify_cert?: boolean
}