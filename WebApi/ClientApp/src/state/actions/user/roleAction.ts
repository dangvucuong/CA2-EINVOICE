import { NotifyHelper } from "../../../helpers/toast";
import { IRole } from "../../../models/responses/user/IRole";
import { IRoleViewModel } from "../../../models/responses/user/IRoleViewModel";
import {
    IRoleChangeEditing,
    IRoleDeleteError,
    IRoleDeleteStart,
    IRoleDeleteSuccess,
    IRoleLoadError, IRoleLoadStart, IRoleLoadSuccess,
    IRoleSaveError, IRoleSaveStart, IRoleSaveSuccess,
    eRoleActionTypeIds
}
    from "../../action-types/user/IRoleActionType";
import { baseAction } from "../baseAction";

export const roleAction = {
    loadStart: (): IRoleLoadStart =>
        baseAction(eRoleActionTypeIds.LOAD_START, undefined),
    loadSuccess: (res: IRoleViewModel[]): IRoleLoadSuccess =>
        baseAction(eRoleActionTypeIds.LOAD_SUCCESS, res),
    loadError: (message: string): IRoleLoadError => {
        NotifyHelper.Error(message)
        return baseAction(eRoleActionTypeIds.LOAD_ERROR, message)
    },

    changeEditing: (role?: IRole): IRoleChangeEditing => baseAction(eRoleActionTypeIds.CHANGE_EDITING, role),

    saveStart: (rq: IRole): IRoleSaveStart =>
        baseAction(eRoleActionTypeIds.SAVE_START, rq),
    saveSuccess: (res: IRole): IRoleSaveSuccess =>
        baseAction(eRoleActionTypeIds.SAVE_SUCCESS, res),
    saveError: (message: string): IRoleSaveError => {
        NotifyHelper.Error(message)
        return baseAction(eRoleActionTypeIds.SAVE_ERROR, message)
    },

    deleteStart: (id: number): IRoleDeleteStart =>
        baseAction(eRoleActionTypeIds.DELETE_START, id),
    deleteSuccess: (): IRoleDeleteSuccess =>
        baseAction(eRoleActionTypeIds.DELETE_SUCCESS, undefined),
    deleteError: (message: string): IRoleDeleteError => {
        NotifyHelper.Error(message)
        return baseAction(eRoleActionTypeIds.DELETE_ERROR, message)
    },

}