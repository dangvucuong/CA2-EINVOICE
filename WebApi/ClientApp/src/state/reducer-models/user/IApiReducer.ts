import { IApi } from "../../../models/responses/user/IApi";
import { eReducerStatusBase } from "../eReducerStatusBase";

export interface IApiReducer {
    status: eReducerStatusBase,
    apis: IApi[]

}