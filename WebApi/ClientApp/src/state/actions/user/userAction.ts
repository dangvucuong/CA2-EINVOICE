import { IUserLoadByDonViRequest } from './../../../models/requests/user/IUserLoadByDonViRequest';
import { NotifyHelper } from "../../../helpers/toast";
import { IPagingRequest } from "../../../models/requests/IPagingRequest";
import { IUserLoadRequest } from "../../../models/requests/user/IUserLoadRequest";
import { IUserEditModel } from "../../../models/responses/user/IUserEditModel";
import { IUserPaging } from "../../../models/responses/user/IUserPaging";
import { IUserChangeFilter, IUserCloseDeleteConfirm, IUserCloseEditModal, IUserDeleteError, IUserDeleteStart, IUserDeleteSuccess, IUserLoadByDonViError, IUserLoadByDonViStart, IUserLoadByDonViSuccess, IUserLoadError, IUserLoadFormError, IUserLoadFormStart, IUserLoadFormSuccess, IUserLoadStart, IUserLoadSuccess, IUserSaveFormError, IUserSaveFormStart, IUserSaveFormSuccess, IUserShowDeleteConfirm, IUserShowEditModal, eUserActionTypeIds } from "../../action-types/user/IUserActionType";
import { baseAction } from "../baseAction";
import { IUser } from './../../../models/responses/user/IUser';

export const userAction = {
    loadStart: (rq: IUserLoadRequest): IUserLoadStart =>
        baseAction(eUserActionTypeIds.LOAD_START, rq),
    loadSuccess: (res: IUserPaging): IUserLoadSuccess =>
        baseAction(eUserActionTypeIds.LOAD_SUCCESS, res),
    loadError: (message: string): IUserLoadError => {
        NotifyHelper.Error(message)
        return baseAction(eUserActionTypeIds.LOAD_ERROR, message)
    },

    loadByDonViStart: (rq: IUserLoadByDonViRequest): IUserLoadByDonViStart =>
        baseAction(eUserActionTypeIds.LOAD_BY_DONVI_START, rq),
    loadByDonViSuccess: (res: IUserPaging): IUserLoadByDonViSuccess =>
        baseAction(eUserActionTypeIds.LOAD_BY_DONVI_SUCCESS, res),
    loadByDonViError: (message: string): IUserLoadByDonViError => {
        NotifyHelper.Error(message)
        return baseAction(eUserActionTypeIds.LOAD_BY_DONVI_ERROR, message)
    },

    showEditModal: (user?: IUser): IUserShowEditModal =>
        baseAction(eUserActionTypeIds.SHOW_EDIT_MODAL, user),

    closeEditModal: (): IUserCloseEditModal =>
        baseAction(eUserActionTypeIds.CLOSE_EDIT_MODAL, undefined),
    changeFilter: (filter: IPagingRequest): IUserChangeFilter => {
        return baseAction(eUserActionTypeIds.CHANGE_FILTER, filter)
    },

    loadFormStart: (rq: number): IUserLoadFormStart =>
        baseAction(eUserActionTypeIds.LOAD_USER_FORM_START, rq),
    loadFormSuccess: (res: IUserEditModel): IUserLoadFormSuccess =>
        baseAction(eUserActionTypeIds.LOAD_USER_FORM_SUCCESS, res),
    loadFormError: (message: string): IUserLoadFormError => {
        NotifyHelper.Error(message)
        return baseAction(eUserActionTypeIds.LOAD_USER_FORM_ERROR, message)
    },

    saveFormStart: (rq: IUserEditModel): IUserSaveFormStart =>
        baseAction(eUserActionTypeIds.SAVE_USER_START, rq),
    saveFormSuccess: (res: IUserEditModel): IUserSaveFormSuccess => {
        NotifyHelper.Success("Saved!")
        return baseAction(eUserActionTypeIds.SAVE_USER_SUCCESS, res)
    },

    saveFormError: (message: string): IUserSaveFormError => {
        NotifyHelper.Error(message)
        return baseAction(eUserActionTypeIds.SAVE_USER_ERROR, message)
    },
    deleteStart: (id: number): IUserDeleteStart =>
        baseAction(eUserActionTypeIds.DELETE_START, id),
    deleteSuccess: (): IUserDeleteSuccess =>
        baseAction(eUserActionTypeIds.DELETE_SUCCESS, undefined),
    deleteError: (message: string): IUserDeleteError => {
        NotifyHelper.Error(message)
        return baseAction(eUserActionTypeIds.DELETE_ERROR, message)
    },
    showDeleteConfirm: (HangHoa: IUser): IUserShowDeleteConfirm =>
        baseAction(eUserActionTypeIds.SHOW_DELETE_CONFIRM, HangHoa),

    closeDeleteConfirm: (): IUserCloseDeleteConfirm =>
        baseAction(eUserActionTypeIds.CLOSE_DELETE_CONFIRM, undefined),
}