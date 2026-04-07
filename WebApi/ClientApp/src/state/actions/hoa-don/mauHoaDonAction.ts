import { NotifyHelper } from "../../../helpers/toast";
import { IMauHoaDon } from "../../../models/responses/hoa-don/IMauHoaDon";
import { IMauHoaDonVM } from "../../../models/responses/hoa-don/IMauHoaDonVM";
import { IMauHoaDonChangeSelectedId, IMauHoaDonCloseDeleteConfirm, IMauHoaDonCloseEditModal, IMauHoaDonDeleteError, IMauHoaDonDeleteStart, IMauHoaDonDeleteSuccess, IMauHoaDonLoadError, IMauHoaDonLoadStart, IMauHoaDonLoadSuccess, IMauHoaDonSaveError, IMauHoaDonSaveStart, IMauHoaDonSaveSuccess, IMauHoaDonShowDeleteConfirm, IMauHoaDonShowEditModal, eMauHoaDonActionTypeIds } from "../../action-types/hoa-don/IMauHoaDonActionType";
import { baseAction } from "../baseAction";


export const mauHoaDonAction = {
    loadStart: (): IMauHoaDonLoadStart =>
        baseAction(eMauHoaDonActionTypeIds.LOAD_START, undefined),
    loadSuccess: (data: IMauHoaDonVM[]): IMauHoaDonLoadSuccess =>
        baseAction(eMauHoaDonActionTypeIds.LOAD_SUCCESS, data),
    loadError: (message: string): IMauHoaDonLoadError => {
        NotifyHelper.Error(message)
        return baseAction(eMauHoaDonActionTypeIds.LOAD_ERROR, message)
    },
    showEditModal: (data?: IMauHoaDonVM): IMauHoaDonShowEditModal =>
        baseAction(eMauHoaDonActionTypeIds.SHOW_EDIT_MODAL, data),

    closeEditModal: (): IMauHoaDonCloseEditModal =>
        baseAction(eMauHoaDonActionTypeIds.CLOSE_EDIT_MODAL, undefined),

    saveStart: (rq: IMauHoaDon): IMauHoaDonSaveStart =>
        baseAction(eMauHoaDonActionTypeIds.SAVE_START, rq),
    saveSuccess: (HangHoa: IMauHoaDon): IMauHoaDonSaveSuccess => {
        NotifyHelper.Success("Success")
        return baseAction(eMauHoaDonActionTypeIds.SAVE_SUCCESS, HangHoa)
    },
    saveError: (message: string): IMauHoaDonSaveError => {
        NotifyHelper.Error(message)
        return baseAction(eMauHoaDonActionTypeIds.SAVE_ERROR, message)
    },

    deleteStart: (id: number): IMauHoaDonDeleteStart =>
        baseAction(eMauHoaDonActionTypeIds.DELETE_START, id),
    deleteSuccess: (): IMauHoaDonDeleteSuccess =>
        baseAction(eMauHoaDonActionTypeIds.DELETE_SUCCESS, undefined),
    deleteError: (message: string): IMauHoaDonDeleteError => {
        NotifyHelper.Error(message)
        return baseAction(eMauHoaDonActionTypeIds.DELETE_ERROR, message)
    },
    showDeleteConfirm: (HangHoa: IMauHoaDonVM): IMauHoaDonShowDeleteConfirm =>
        baseAction(eMauHoaDonActionTypeIds.SHOW_DELETE_CONFIRM, HangHoa),

    closeDeleteConfirm: (): IMauHoaDonCloseDeleteConfirm =>
        baseAction(eMauHoaDonActionTypeIds.CLOSE_DELETE_CONFIRM, undefined),

    changeSelectedId: (id: number): IMauHoaDonChangeSelectedId =>
        baseAction(eMauHoaDonActionTypeIds.CHANGE_SELECTED_ID, id),
    
}