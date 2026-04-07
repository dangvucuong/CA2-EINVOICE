import { eNavSubMode } from "../../../models/commons/eNavSubMode";
import { IMainLayoutChangeNavSubMode, IMainLayoutShowNotifyOverlay, eMainLayoutActionTypes } from "../../action-types/commons/IMainLayoutActionType";
import { baseAction } from "../baseAction";

export const mainLayoutAction = {
    changeNavSubMode: (payload: eNavSubMode): IMainLayoutChangeNavSubMode =>
        baseAction(eMainLayoutActionTypes.CHANGE_NAV_SUB_MODE, payload),
    showNotifyOverlay: (open: boolean): IMainLayoutShowNotifyOverlay =>
        baseAction(eMainLayoutActionTypes.SHOW_NOTIFY_OVERLAY, open)
}