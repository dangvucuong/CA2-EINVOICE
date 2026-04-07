import { NotifyHelper } from "../../../helpers/toast";
import { IPagingRequest } from '../../../models/requests/IPagingRequest';
import { IHangHoa } from "../../../models/responses/category/IHangHoa";
import { IHangHoaPaging } from "../../../models/responses/category/IHangHoaPaging";
import { IHangHoaChangeFilter, IHangHoaCloseDeleteConfirm, IHangHoaCloseEditModal, IHangHoaDeleteError, IHangHoaDeleteStart, IHangHoaDeleteSuccess, IHangHoaLoadError, IHangHoaLoadStart, IHangHoaLoadSuccess, IHangHoaSaveError, IHangHoaSaveStart, IHangHoaSaveSuccess, IHangHoaShowDeleteConfirm, IHangHoaShowEditModal, eHangHoaActionTypeIds } from "../../action-types/category/IHangHoaActionType";
import { baseAction } from "../baseAction";
import { IHangHoaChangeSelectedId } from '../../action-types/category/IHangHoaActionType';


export const hangHoaAction = {
    loadStart: (request: IPagingRequest): IHangHoaLoadStart =>
        baseAction(eHangHoaActionTypeIds.LOAD_START, request),
    loadSuccess: (HangHoas: IHangHoaPaging): IHangHoaLoadSuccess =>
        baseAction(eHangHoaActionTypeIds.LOAD_SUCCESS, HangHoas),
    loadError: (message: string): IHangHoaLoadError => {
        NotifyHelper.Error(message)
        return baseAction(eHangHoaActionTypeIds.LOAD_ERROR, message)
    },
    showEditModal: (HangHoa?: IHangHoa): IHangHoaShowEditModal =>
        baseAction(eHangHoaActionTypeIds.SHOW_EDIT_MODAL, HangHoa),

    closeEditModal: (): IHangHoaCloseEditModal =>
        baseAction(eHangHoaActionTypeIds.CLOSE_EDIT_MODAL, undefined),

    saveStart: (rq: IHangHoa): IHangHoaSaveStart =>
        baseAction(eHangHoaActionTypeIds.SAVE_START, rq),
    saveSuccess: (HangHoa: IHangHoa): IHangHoaSaveSuccess => {
        NotifyHelper.Success("Success")
        return baseAction(eHangHoaActionTypeIds.SAVE_SUCCESS, HangHoa)
    },
    saveError: (message: string): IHangHoaSaveError => {
        NotifyHelper.Error(message)
        return baseAction(eHangHoaActionTypeIds.SAVE_ERROR, message)
    },

    deleteStart: (id: number): IHangHoaDeleteStart =>
        baseAction(eHangHoaActionTypeIds.DELETE_START, id),
    deleteSuccess: (): IHangHoaDeleteSuccess =>
        baseAction(eHangHoaActionTypeIds.DELETE_SUCCESS, undefined),
    deleteError: (message: string): IHangHoaDeleteError => {
        NotifyHelper.Error(message)
        return baseAction(eHangHoaActionTypeIds.DELETE_ERROR, message)
    },
    showDeleteConfirm: (HangHoa: IHangHoa): IHangHoaShowDeleteConfirm =>
        baseAction(eHangHoaActionTypeIds.SHOW_DELETE_CONFIRM, HangHoa),

    closeDeleteConfirm: (): IHangHoaCloseDeleteConfirm =>
        baseAction(eHangHoaActionTypeIds.CLOSE_DELETE_CONFIRM, undefined),

    changeSelectedId: (id: number): IHangHoaChangeSelectedId =>
        baseAction(eHangHoaActionTypeIds.CHANGE_SELECTED_ID, id),
    changeFilter: (filter: IPagingRequest): IHangHoaChangeFilter => {
        return baseAction(eHangHoaActionTypeIds.CHANGE_FILTER, filter)
    },
}