import { IRole } from "../../../models/responses/user/IRole";
import { IRoleViewModel } from "../../../models/responses/user/IRoleViewModel";
import { eReducerStatusBase } from "../eReducerStatusBase";

export interface IRoleReducer {
    status: eReducerStatusBase,
    roles: IRoleViewModel[],
    roleEditing?: IRole

}