import { IRoleSubSystem } from "../../../models/responses/user/IRoleSubSystem";
import { eReducerStatusBase } from "../eReducerStatusBase";

export interface IRoleSubSystemReducer {
    status: eReducerStatusBase,
    roleSubSystems: IRoleSubSystem[]

}