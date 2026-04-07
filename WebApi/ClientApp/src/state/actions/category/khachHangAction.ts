import { NotifyHelper } from "../../../helpers/toast";
import { IPagingRequest } from '../../../models/requests/IPagingRequest';
import { IKhachHang } from "../../../models/responses/category/IKhachHang";
import { IKhachHangPaging } from "../../../models/responses/category/IKhachHangPaging";
import { IKhachHangChangeFilter, IKhachHangCloseDeleteConfirm, IKhachHangCloseEditModal, IKhachHangDeleteError, IKhachHangDeleteStart, IKhachHangDeleteSuccess, IKhachHangLoadError, IKhachHangLoadStart, IKhachHangLoadSuccess, IKhachHangSaveError, IKhachHangSaveStart, IKhachHangSaveSuccess, IKhachHangShowDeleteConfirm, IKhachHangShowEditModal, eKhachHangActionTypeIds } from "../../action-types/category/IKhachHangActionType";
import { baseAction } from "../baseAction";
import { IKhachHangChangeSelectedId } from './../../action-types/category/IKhachHangActionType';


export const khachHangAction = {
    loadStart: (request: IPagingRequest): IKhachHangLoadStart =>
        baseAction(eKhachHangActionTypeIds.LOAD_START, request),
    loadSuccess: (KhachHangs: IKhachHangPaging): IKhachHangLoadSuccess =>
        baseAction(eKhachHangActionTypeIds.LOAD_SUCCESS, KhachHangs),
    loadError: (message: string): IKhachHangLoadError => {
        NotifyHelper.Error(message)
        return baseAction(eKhachHangActionTypeIds.LOAD_ERROR, message)
    },
    showEditModal: (KhachHang?: IKhachHang): IKhachHangShowEditModal =>
        baseAction(eKhachHangActionTypeIds.SHOW_EDIT_MODAL, KhachHang),

    closeEditModal: (): IKhachHangCloseEditModal =>
        baseAction(eKhachHangActionTypeIds.CLOSE_EDIT_MODAL, undefined),

    saveStart: (rq: IKhachHang): IKhachHangSaveStart =>
        baseAction(eKhachHangActionTypeIds.SAVE_START, rq),
    saveSuccess: (KhachHang: IKhachHang): IKhachHangSaveSuccess => {
        NotifyHelper.Success("Success")
        return baseAction(eKhachHangActionTypeIds.SAVE_SUCCESS, KhachHang)
    },
    saveError: (message: string): IKhachHangSaveError => {
        NotifyHelper.Error(message)
        return baseAction(eKhachHangActionTypeIds.SAVE_ERROR, message)
    },

    deleteStart: (id: number): IKhachHangDeleteStart =>
        baseAction(eKhachHangActionTypeIds.DELETE_START, id),
    deleteSuccess: (): IKhachHangDeleteSuccess =>
        baseAction(eKhachHangActionTypeIds.DELETE_SUCCESS, undefined),
    deleteError: (message: string): IKhachHangDeleteError => {
        NotifyHelper.Error(message)
        return baseAction(eKhachHangActionTypeIds.DELETE_ERROR, message)
    },
    showDeleteConfirm: (KhachHang: IKhachHang): IKhachHangShowDeleteConfirm =>
        baseAction(eKhachHangActionTypeIds.SHOW_DELETE_CONFIRM, KhachHang),

    closeDeleteConfirm: (): IKhachHangCloseDeleteConfirm =>
        baseAction(eKhachHangActionTypeIds.CLOSE_DELETE_CONFIRM, undefined),

    changeSelectedId: (id: number): IKhachHangChangeSelectedId =>
        baseAction(eKhachHangActionTypeIds.CHANGE_SELECTED_ID, id),
    changeFilter: (filter: IPagingRequest): IKhachHangChangeFilter => {
        return baseAction(eKhachHangActionTypeIds.CHANGE_FILTER, filter)
    },
}