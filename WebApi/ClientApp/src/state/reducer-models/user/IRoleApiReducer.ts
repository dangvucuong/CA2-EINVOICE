import { IRoleApi } from "../../../models/responses/user/IRoleApi";
import { eReducerStatusBase } from "../eReducerStatusBase";

export interface IRoleApiReducer {
    status: eReducerStatusBase,
    roleApis: IRoleApi[]

}