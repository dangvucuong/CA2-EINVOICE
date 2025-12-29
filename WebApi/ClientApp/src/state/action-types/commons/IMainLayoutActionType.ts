import { eNavSubMode } from "../../../models/commons/eNavSubMode";
import { IActionTypeBase } from "../IActionTypeBase";

export enum eMainLayoutActionTypes {
    CHANGE_NAV_SUB_MODE = "MAIN_LAYOUT_CHANGE_NAV_SUB_MODE",
    SHOW_NOTIFY_OVERLAY = "MAIN_LAYOUT_SHOW_NOTIFY_OVERLAY"

}
export interface IMainLayoutChangeNavSubMode
    extends IActionTypeBase<eMainLayoutActionTypes.CHANGE_NAV_SUB_MODE, eNavSubMode> { }

export interface IMainLayoutShowNotifyOverlay
    extends IActionTypeBase<eMainLayoutActionTypes.SHOW_NOTIFY_OVERLAY, boolean> { }
export type IMainLayoutActionTypes = IMainLayoutChangeNavSubMode |IMainLayoutShowNotifyOverlay