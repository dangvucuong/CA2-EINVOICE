import { IAppConfig } from "../../../models/responses/common/IAppConfig";
import { eReducerStatusBase } from "../eReducerStatusBase";

export interface IAppConfigReducer {
    status: eReducerStatusBase,
    appConfig?: IAppConfig
}