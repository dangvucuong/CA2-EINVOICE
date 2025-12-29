import { NotifyHelper } from "../../../helpers/toast";
import { IPagingRequest } from '../../../models/requests/IPagingRequest';
import { IDonVi } from "../../../models/responses/category/IDonVi";
import { IDonViPaging } from "../../../models/responses/category/IDonViPaging";
import { IDonViChangeFilter, IDonViCloseDeleteConfirm, IDonViCloseEditModal, IDonViDeleteError, IDonViDeleteStart, IDonViDeleteSuccess, IDonViLoadError, IDonViLoadStart, IDonViLoadSuccess, IDonViSaveError, IDonViSaveStart, IDonViSaveSuccess, IDonViShowDeleteConfirm, IDonViShowEditModal, eDonViActionTypeIds } from "../../action-types/category/IDonViActionType";
import { baseAction } from "../baseAction";
import { IDonViChangeSelectedId } from '../../action-types/category/IDonViActionType';


export const donViActionType = {
    loadStart: (request: IPagingRequest): IDonViLoadStart =>
        baseAction(eDonViActionTypeIds.LOAD_START, request),
    loadSuccess: (HangHoas: IDonViPaging): IDonViLoadSuccess =>
        baseAction(eDonViActionTypeIds.LOAD_SUCCESS, HangHoas),
    loadError: (message: string): IDonViLoadError => {
        NotifyHelper.Error(message)
        return baseAction(eDonViActionTypeIds.LOAD_ERROR, message)
    },
    showEditModal: (HangHoa?: IDonVi): IDonViShowEditModal =>
        baseAction(eDonViActionTypeIds.SHOW_EDIT_MODAL, HangHoa),

    closeEditModal: (): IDonViCloseEditModal =>
        baseAction(eDonViActionTypeIds.CLOSE_EDIT_MODAL, undefined),

    saveStart: (rq: IDonVi): IDonViSaveStart =>
        baseAction(eDonViActionTypeIds.SAVE_START, rq),
    saveSuccess: (HangHoa: IDonVi): IDonViSaveSuccess => {
        NotifyHelper.Success("Success")
        return baseAction(eDonViActionTypeIds.SAVE_SUCCESS, HangHoa)
    },
    saveError: (message: string): IDonViSaveError => {
        NotifyHelper.Error(message)
        return baseAction(eDonViActionTypeIds.SAVE_ERROR, message)
    },

    deleteStart: (id: number): IDonViDeleteStart =>
        baseAction(eDonViActionTypeIds.DELETE_START, id),
    deleteSuccess: (): IDonViDeleteSuccess =>
        baseAction(eDonViActionTypeIds.DELETE_SUCCESS, undefined),
    deleteError: (message: string): IDonViDeleteError => {
        NotifyHelper.Error(message)
        return baseAction(eDonViActionTypeIds.DELETE_ERROR, message)
    },
    showDeleteConfirm: (data: IDonVi): IDonViShowDeleteConfirm =>
        baseAction(eDonViActionTypeIds.SHOW_DELETE_CONFIRM, data),

    closeDeleteConfirm: (): IDonViCloseDeleteConfirm =>
        baseAction(eDonViActionTypeIds.CLOSE_DELETE_CONFIRM, undefined),

    changeSelectedId: (id: number): IDonViChangeSelectedId =>
        baseAction(eDonViActionTypeIds.CHANGE_SELECTED_ID, id),
    changeFilter: (filter: IPagingRequest): IDonViChangeFilter => {
        return baseAction(eDonViActionTypeIds.CHANGE_FILTER, filter)
    },
}