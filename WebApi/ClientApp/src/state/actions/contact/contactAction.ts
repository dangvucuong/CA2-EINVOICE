import { NotifyHelper } from "../../../helpers/toast";
import { IPagingRequest } from '../../../models/requests/IPagingRequest';
import { IContact } from "../../../models/responses/contact/IContact";
import { IContactPaging } from "../../../models/responses/contact/IContactPaging";
import { IContactChangeFilter, IContactCloseDeleteConfirm, IContactCloseEditModal, IContactDeleteError, IContactDeleteStart, IContactDeleteSuccess, IContactLoadError, IContactLoadStart, IContactLoadSuccess, IContactSaveError, IContactSaveStart, IContactSaveSuccess, IContactShowDeleteConfirm, IContactShowEditModal, eContactActionTypeIds } from "../../action-types/contact/IContactActionType";
import { baseAction } from "../baseAction";
import { IContactChangeSelectedId } from '../../action-types/contact/IContactActionType';
import { IContactSelectRequest } from "../../../models/requests/contact/IContactSelectRequest";


export const contactAction = {
    loadStart: (request: IContactSelectRequest): IContactLoadStart =>
        baseAction(eContactActionTypeIds.LOAD_START, request),
    loadSuccess: (Contacts: IContactPaging): IContactLoadSuccess =>
        baseAction(eContactActionTypeIds.LOAD_SUCCESS, Contacts),
    loadError: (message: string): IContactLoadError => {
        NotifyHelper.Error(message)
        return baseAction(eContactActionTypeIds.LOAD_ERROR, message)
    },
    showEditModal: (Contact?: IContact): IContactShowEditModal =>
        baseAction(eContactActionTypeIds.SHOW_EDIT_MODAL, Contact),

    closeEditModal: (): IContactCloseEditModal =>
        baseAction(eContactActionTypeIds.CLOSE_EDIT_MODAL, undefined),

    saveStart: (rq: IContact): IContactSaveStart =>
        baseAction(eContactActionTypeIds.SAVE_START, rq),
    saveSuccess: (Contact: IContact): IContactSaveSuccess => {
        NotifyHelper.Success("Success")
        return baseAction(eContactActionTypeIds.SAVE_SUCCESS, Contact)
    },
    saveError: (message: string): IContactSaveError => {
        NotifyHelper.Error(message)
        return baseAction(eContactActionTypeIds.SAVE_ERROR, message)
    },

    deleteStart: (id: number): IContactDeleteStart =>
        baseAction(eContactActionTypeIds.DELETE_START, id),
    deleteSuccess: (): IContactDeleteSuccess =>
        baseAction(eContactActionTypeIds.DELETE_SUCCESS, undefined),
    deleteError: (message: string): IContactDeleteError => {
        NotifyHelper.Error(message)
        return baseAction(eContactActionTypeIds.DELETE_ERROR, message)
    },
    showDeleteConfirm: (Contact: IContact): IContactShowDeleteConfirm =>
        baseAction(eContactActionTypeIds.SHOW_DELETE_CONFIRM, Contact),

    closeDeleteConfirm: (): IContactCloseDeleteConfirm =>
        baseAction(eContactActionTypeIds.CLOSE_DELETE_CONFIRM, undefined),

    changeSelectedId: (id: number): IContactChangeSelectedId =>
        baseAction(eContactActionTypeIds.CHANGE_SELECTED_ID, id),
    changeFilter: (filter: IContactSelectRequest): IContactChangeFilter => {
        return baseAction(eContactActionTypeIds.CHANGE_FILTER, filter)
    },
}