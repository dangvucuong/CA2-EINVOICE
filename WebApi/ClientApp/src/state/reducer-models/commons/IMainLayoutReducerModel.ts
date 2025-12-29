import { eNavSubMode } from "../../../models/commons/eNavSubMode";
import { eReducerStatusBase } from "../eReducerStatusBase";

export interface IMainLayoutReducerModel {
    status: eReducerStatusBase,
    navSubMode: eNavSubMode
    isOpenNotifyOverlay: boolean

}