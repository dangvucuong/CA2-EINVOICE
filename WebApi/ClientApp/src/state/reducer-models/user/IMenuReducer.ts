import { IMenu } from "../../../models/responses/user/IMenu";
import { eReducerStatusBase } from "../eReducerStatusBase";

export interface IMenuReducer {
    status: eReducerStatusBase,
    menus: IMenu[]

}