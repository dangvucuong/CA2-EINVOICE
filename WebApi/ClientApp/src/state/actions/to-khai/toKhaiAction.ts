import { NotifyHelper } from "../../../helpers/toast";
import { IToKhai } from "../../../models/responses/to-khai/IToKhai";
import { IToKhaiCloseDeleteConfirm, IToKhaiCloseLogModal, IToKhaiDeleteError, IToKhaiDeleteStart, IToKhaiDeleteSuccess, IToKhaiLoadError, IToKhaiLoadStart, IToKhaiLoadSuccess, IToKhaiShowDeleteConfirm, IToKhaiShowLogModal, eToKhaiActionTypeIds } from "../../action-types/to-khai/IToKhaiActionType";
import { baseAction } from "../baseAction";

export const toKhaiAction = {
    loadStart: (): IToKhaiLoadStart => baseAction(eToKhaiActionTypeIds.LOAD_START, undefined),
    loadSuccess: (toKhais: IToKhai[]): IToKhaiLoadSuccess => baseAction(eToKhaiActionTypeIds.LOAD_SUCCESS, toKhais),
    loadError: (m: string): IToKhaiLoadError => {
        NotifyHelper.Error(m)
        return baseAction(eToKhaiActionTypeIds.LOAD_ERROR, m)
    },
    deleteStart: (id: number): IToKhaiDeleteStart =>
        baseAction(eToKhaiActionTypeIds.DELETE_START, id),
    deleteSuccess: (): IToKhaiDeleteSuccess => {
        NotifyHelper.Success("Thành công")
        return baseAction(eToKhaiActionTypeIds.DELETE_SUCCESS, undefined);
    },
    deleteError: (message: string): IToKhaiDeleteError => {
        NotifyHelper.Error(message)
        return baseAction(eToKhaiActionTypeIds.DELETE_ERROR, message)
    },
    showDeleteConfirm: (toKhai: IToKhai): IToKhaiShowDeleteConfirm =>
        baseAction(eToKhaiActionTypeIds.SHOW_DELETE_CONFIRM, toKhai),

    closeDeleteConfirm: (): IToKhaiCloseDeleteConfirm =>
        baseAction(eToKhaiActionTypeIds.CLOSE_DELETE_CONFIRM, undefined),

    showLogModal: (toKhai: IToKhai): IToKhaiShowLogModal =>
        baseAction(eToKhaiActionTypeIds.SHOW_LOG_MODAL, toKhai),

    closeLogModal: (): IToKhaiCloseLogModal =>
        baseAction(eToKhaiActionTypeIds.CLOSE_LOG_MODAL, undefined),
}