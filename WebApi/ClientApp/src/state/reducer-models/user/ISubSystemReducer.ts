import { ISubSystem } from "../../../models/responses/user/ISubSystem";
import { eReducerStatusBase } from "../eReducerStatusBase";

export interface ISubSystemReducer {
    status: eReducerStatusBase,
    subSystems: ISubSystem[],
    subSystemSelectedId: number

}