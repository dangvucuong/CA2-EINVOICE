import { NotifyHelper } from "../../../helpers/toast";
import { IIHoaDonAddOrEditModel } from "../../../models/requests/hoa-don/IHoaDonAddOrEditModel";
import { IHoaDonSelectPagingRequest } from "../../../models/requests/hoa-don/IHoaDonSelectPagingRequest";
import { IHoaDon } from "../../../models/responses/hoa-don/IHoaDon";
import { IHoaDonPaging } from "../../../models/responses/hoa-don/IHoaDonPaging";
import { IHoaDonChangeFilter, IHoaDonChangeSelectedIds, IHoaDonCloseDeleteConfirm, IHoaDonCloseEditModal, IHoaDonCloseLogModal, IHoaDonDeleteError, IHoaDonDeleteStart, IHoaDonDeleteSuccess, IHoaDonLoadError, IHoaDonLoadStart, IHoaDonLoadSuccess, IHoaDonSaveError, IHoaDonSaveStart, IHoaDonSaveSuccess, IHoaDonShowDeleteConfirm, IHoaDonShowEditModal, IHoaDonShowLogModal, eHoaDonActionTypeIds } from "../../action-types/hoa-don/IHoaDonActionType";
import { baseAction } from "../baseAction";


export const hoaDonAction = {
    loadStart: (request: IHoaDonSelectPagingRequest): IHoaDonLoadStart =>
        baseAction(eHoaDonActionTypeIds.LOAD_START, request),
    loadSuccess: (HangHoas: IHoaDonPaging): IHoaDonLoadSuccess =>
        baseAction(eHoaDonActionTypeIds.LOAD_SUCCESS, HangHoas),
    loadError: (message: string): IHoaDonLoadError => {
        NotifyHelper.Error(message)
        return baseAction(eHoaDonActionTypeIds.LOAD_ERROR, message)
    },
    showEditModal: (HangHoa?: IHoaDon): IHoaDonShowEditModal =>
        baseAction(eHoaDonActionTypeIds.SHOW_EDIT_MODAL, HangHoa),

    closeEditModal: (): IHoaDonCloseEditModal =>
        baseAction(eHoaDonActionTypeIds.CLOSE_EDIT_MODAL, undefined),

    saveStart: (rq: IIHoaDonAddOrEditModel): IHoaDonSaveStart =>
        baseAction(eHoaDonActionTypeIds.SAVE_START, rq),
    saveSuccess: (HangHoa: IIHoaDonAddOrEditModel): IHoaDonSaveSuccess => {
        NotifyHelper.Success("Success")
        return baseAction(eHoaDonActionTypeIds.SAVE_SUCCESS, HangHoa)
    },
    saveError: (message: string): IHoaDonSaveError => {
        NotifyHelper.Error(message)
        return baseAction(eHoaDonActionTypeIds.SAVE_ERROR, message)
    },

    deleteStart: (id: number): IHoaDonDeleteStart =>
        baseAction(eHoaDonActionTypeIds.DELETE_START, id),
    deleteSuccess: (): IHoaDonDeleteSuccess =>
        baseAction(eHoaDonActionTypeIds.DELETE_SUCCESS, undefined),
    deleteError: (message: string): IHoaDonDeleteError => {
        NotifyHelper.Error(message)
        return baseAction(eHoaDonActionTypeIds.DELETE_ERROR, message)
    },
    showDeleteConfirm: (HangHoa: IHoaDon): IHoaDonShowDeleteConfirm =>
        baseAction(eHoaDonActionTypeIds.SHOW_DELETE_CONFIRM, HangHoa),

    closeDeleteConfirm: (): IHoaDonCloseDeleteConfirm =>
        baseAction(eHoaDonActionTypeIds.CLOSE_DELETE_CONFIRM, undefined),

    changeSelectedId: (ids: number[]): IHoaDonChangeSelectedIds =>
        baseAction(eHoaDonActionTypeIds.CHANGE_SELECTED_ID, ids),
    changeFilter: (filter: IHoaDonSelectPagingRequest): IHoaDonChangeFilter => {
        return baseAction(eHoaDonActionTypeIds.CHANGE_FILTER, filter)
    },
    showLogModal: (hoaDon: IHoaDon): IHoaDonShowLogModal =>
        baseAction(eHoaDonActionTypeIds.SHOW_LOG_MODAL, hoaDon),

    closeLogModal: (): IHoaDonCloseLogModal =>
        baseAction(eHoaDonActionTypeIds.CLOSE_LOG_MODAL, undefined),
}