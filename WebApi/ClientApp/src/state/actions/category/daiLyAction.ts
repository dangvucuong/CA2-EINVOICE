import { NotifyHelper } from "../../../helpers/toast";
import { IPagingRequest } from '../../../models/requests/IPagingRequest';
import { IDaiLy } from "../../../models/responses/category/IDaiLy";
import { IDaiLyPaging } from "../../../models/responses/category/IDaiLyPaging";
import { IDaiLyChangeFilter, IDaiLyCloseDeleteConfirm, IDaiLyCloseEditModal, IDaiLyDeleteError, IDaiLyDeleteStart, IDaiLyDeleteSuccess, IDaiLyLoadError, IDaiLyLoadStart, IDaiLyLoadSuccess, IDaiLySaveError, IDaiLySaveStart, IDaiLySaveSuccess, IDaiLyShowDeleteConfirm, IDaiLyShowEditModal, eDaiLyActionTypeIds } from "../../action-types/category/IDaiLyActionType";
import { baseAction } from "../baseAction";
import { IDaiLyChangeSelectedId } from '../../action-types/category/IDaiLyActionType';


export const daiLyAction = {
    loadStart: (request: IPagingRequest): IDaiLyLoadStart =>
        baseAction(eDaiLyActionTypeIds.LOAD_START, request),
    loadSuccess: (data: IDaiLyPaging): IDaiLyLoadSuccess =>
        baseAction(eDaiLyActionTypeIds.LOAD_SUCCESS, data),
    loadError: (message: string): IDaiLyLoadError => {
        NotifyHelper.Error(message)
        return baseAction(eDaiLyActionTypeIds.LOAD_ERROR, message)
    },
    showEditModal: (HangHoa?: IDaiLy): IDaiLyShowEditModal =>
        baseAction(eDaiLyActionTypeIds.SHOW_EDIT_MODAL, HangHoa),

    closeEditModal: (): IDaiLyCloseEditModal =>
        baseAction(eDaiLyActionTypeIds.CLOSE_EDIT_MODAL, undefined),

    saveStart: (rq: IDaiLy): IDaiLySaveStart =>
        baseAction(eDaiLyActionTypeIds.SAVE_START, rq),
    saveSuccess: (HangHoa: IDaiLy): IDaiLySaveSuccess => {
        NotifyHelper.Success("Success")
        return baseAction(eDaiLyActionTypeIds.SAVE_SUCCESS, HangHoa)
    },
    saveError: (message: string): IDaiLySaveError => {
        NotifyHelper.Error(message)
        return baseAction(eDaiLyActionTypeIds.SAVE_ERROR, message)
    },

    deleteStart: (id: number): IDaiLyDeleteStart =>
        baseAction(eDaiLyActionTypeIds.DELETE_START, id),
    deleteSuccess: (): IDaiLyDeleteSuccess =>
        baseAction(eDaiLyActionTypeIds.DELETE_SUCCESS, undefined),
    deleteError: (message: string): IDaiLyDeleteError => {
        NotifyHelper.Error(message)
        return baseAction(eDaiLyActionTypeIds.DELETE_ERROR, message)
    },
    showDeleteConfirm: (HangHoa: IDaiLy): IDaiLyShowDeleteConfirm =>
        baseAction(eDaiLyActionTypeIds.SHOW_DELETE_CONFIRM, HangHoa),

    closeDeleteConfirm: (): IDaiLyCloseDeleteConfirm =>
        baseAction(eDaiLyActionTypeIds.CLOSE_DELETE_CONFIRM, undefined),

    changeSelectedId: (id: number): IDaiLyChangeSelectedId =>
        baseAction(eDaiLyActionTypeIds.CHANGE_SELECTED_ID, id),
    changeFilter: (filter: IPagingRequest): IDaiLyChangeFilter => {
        return baseAction(eDaiLyActionTypeIds.CHANGE_FILTER, filter)
    },
}