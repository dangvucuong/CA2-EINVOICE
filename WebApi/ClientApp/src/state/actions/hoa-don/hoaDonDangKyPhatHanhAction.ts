import { NotifyHelper } from "../../../helpers/toast";
import { IPagingRequest } from '../../../models/requests/IPagingRequest';
import { IHoaDonDangKyPhatHanh } from "../../../models/responses/hoa-don/IHoaDonDangKyPhatHanh";
import { IHoaDonDangKyPhatHanhChangeSelectedId, IHoaDonDangKyPhatHanhCloseDeleteConfirm, IHoaDonDangKyPhatHanhCloseEditModal, IHoaDonDangKyPhatHanhDeleteError, IHoaDonDangKyPhatHanhDeleteStart, IHoaDonDangKyPhatHanhDeleteSuccess, IHoaDonDangKyPhatHanhLoadError, IHoaDonDangKyPhatHanhLoadStart, IHoaDonDangKyPhatHanhLoadSuccess, IHoaDonDangKyPhatHanhSaveError, IHoaDonDangKyPhatHanhSaveStart, IHoaDonDangKyPhatHanhSaveSuccess, IHoaDonDangKyPhatHanhShowDeleteConfirm, IHoaDonDangKyPhatHanhShowEditModal, eHoaDonDangKyPhatHanhActionTypeIds } from "../../action-types/hoa-don/IHoaDonDangKyPhatHanhActionType";
import { baseAction } from "../baseAction";


export const hoaDonDangKyPhatHanhAction = {
    loadStart: (): IHoaDonDangKyPhatHanhLoadStart =>
        baseAction(eHoaDonDangKyPhatHanhActionTypeIds.LOAD_START, undefined),
    loadSuccess: (data: IHoaDonDangKyPhatHanh[]): IHoaDonDangKyPhatHanhLoadSuccess =>
        baseAction(eHoaDonDangKyPhatHanhActionTypeIds.LOAD_SUCCESS, data),
    loadError: (message: string): IHoaDonDangKyPhatHanhLoadError => {
        NotifyHelper.Error(message)
        return baseAction(eHoaDonDangKyPhatHanhActionTypeIds.LOAD_ERROR, message)
    },
    showEditModal: (data?: IHoaDonDangKyPhatHanh): IHoaDonDangKyPhatHanhShowEditModal =>
        baseAction(eHoaDonDangKyPhatHanhActionTypeIds.SHOW_EDIT_MODAL, data),

    closeEditModal: (): IHoaDonDangKyPhatHanhCloseEditModal =>
        baseAction(eHoaDonDangKyPhatHanhActionTypeIds.CLOSE_EDIT_MODAL, undefined),

    saveStart: (rq: IHoaDonDangKyPhatHanh): IHoaDonDangKyPhatHanhSaveStart =>
        baseAction(eHoaDonDangKyPhatHanhActionTypeIds.SAVE_START, rq),
    saveSuccess: (data: IHoaDonDangKyPhatHanh): IHoaDonDangKyPhatHanhSaveSuccess => {
        NotifyHelper.Success("Success")
        return baseAction(eHoaDonDangKyPhatHanhActionTypeIds.SAVE_SUCCESS, data)
    },
    saveError: (message: string): IHoaDonDangKyPhatHanhSaveError => {
        NotifyHelper.Error(message)
        return baseAction(eHoaDonDangKyPhatHanhActionTypeIds.SAVE_ERROR, message)
    },

    deleteStart: (id: number): IHoaDonDangKyPhatHanhDeleteStart =>
        baseAction(eHoaDonDangKyPhatHanhActionTypeIds.DELETE_START, id),
    deleteSuccess: (): IHoaDonDangKyPhatHanhDeleteSuccess =>
        baseAction(eHoaDonDangKyPhatHanhActionTypeIds.DELETE_SUCCESS, undefined),
    deleteError: (message: string): IHoaDonDangKyPhatHanhDeleteError => {
        NotifyHelper.Error(message)
        return baseAction(eHoaDonDangKyPhatHanhActionTypeIds.DELETE_ERROR, message)
    },
    showDeleteConfirm: (data: IHoaDonDangKyPhatHanh): IHoaDonDangKyPhatHanhShowDeleteConfirm =>
        baseAction(eHoaDonDangKyPhatHanhActionTypeIds.SHOW_DELETE_CONFIRM, data),

    closeDeleteConfirm: (): IHoaDonDangKyPhatHanhCloseDeleteConfirm =>
        baseAction(eHoaDonDangKyPhatHanhActionTypeIds.CLOSE_DELETE_CONFIRM, undefined),

    changeSelectedId: (id: number): IHoaDonDangKyPhatHanhChangeSelectedId =>
        baseAction(eHoaDonDangKyPhatHanhActionTypeIds.CHANGE_SELECTED_ID, id),
    
}