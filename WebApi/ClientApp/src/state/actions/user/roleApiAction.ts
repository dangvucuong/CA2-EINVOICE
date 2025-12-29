import { NotifyHelper } from "../../../helpers/toast";
import { IRoleApiLoadRequest } from "../../../models/requests/user/IRoleApiLoadRequest";
import { IRoleApi } from "../../../models/responses/user/IRoleApi";
import { IRoleApiAddError, IRoleApiAddStart, IRoleApiAddSuccess, IRoleApiLoadStart, IRoleApiLoadSuccess, IRoleApiRemoveError, IRoleApiRemoveStart, IRoleApiRemoveSuccess, eRoleApiActionTypeIds } from "../../action-types/user/IRoleApiActionType";
import { baseAction } from "../baseAction";

export const roleApiAction = {
    loadStart: (rq: IRoleApiLoadRequest): IRoleApiLoadStart =>
        baseAction(eRoleApiActionTypeIds.LOAD_START, rq),
    loadSuccess: (roleSubsytems: IRoleApi[]): IRoleApiLoadSuccess =>
        baseAction(eRoleApiActionTypeIds.LOAD_SUCCESS, roleSubsytems),
    loadError: (message: string) => {
        NotifyHelper.Error(message);
        return baseAction(eRoleApiActionTypeIds.LOAD_ERROR, message)
    },

    addStart: (payload: IRoleApi): IRoleApiAddStart =>
        baseAction(eRoleApiActionTypeIds.ADD_API_START, payload),
    addSuccess: (): IRoleApiAddSuccess => {
        NotifyHelper.Success("Saved!");
        return baseAction(eRoleApiActionTypeIds.ADD_API_SUCCESS, undefined)
    },
    addError: (message: string): IRoleApiAddError => {
        NotifyHelper.Error(message);
        return baseAction(eRoleApiActionTypeIds.ADD_API_ERROR, message)
    },

    removeStart: (payload: IRoleApi): IRoleApiRemoveStart =>
        baseAction(eRoleApiActionTypeIds.REMOVE_API_START, payload),
    removeSuccess: (): IRoleApiRemoveSuccess => {
        NotifyHelper.Success("Saved!");
        return baseAction(eRoleApiActionTypeIds.REMOVE_API_SUCCESS, undefined)
    },
    removeError: (message: string): IRoleApiRemoveError => {
        NotifyHelper.Error(message);
        return baseAction(eRoleApiActionTypeIds.REMOVE_API_ERROR, message)
    }
}