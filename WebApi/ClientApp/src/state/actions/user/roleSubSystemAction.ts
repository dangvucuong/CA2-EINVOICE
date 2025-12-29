import { NotifyHelper } from "../../../helpers/toast";
import { IRoleSubSystem } from "../../../models/responses/user/IRoleSubSystem";
import {
    IRoleSubSystemRemoveError, IRoleSubSystemAddStart,
    IRoleSubSystemAddSuccess, IRoleSubSystemLoadStart,
    IRoleSubSystemLoadSuccess, eRoleSubSystemTypeIds, IRoleSubSystemRemoveStart, IRoleSubSystemRemoveSuccess,
    IRoleSubSystemAddError
} from "../../action-types/user/IRoleSubSystemType";
import { baseAction } from "../baseAction";

export const roleSubSystemAction = {
    loadStart: (roleId: number): IRoleSubSystemLoadStart =>
        baseAction(eRoleSubSystemTypeIds.LOAD_START, roleId),
    loadSuccess: (roleSubsytems: IRoleSubSystem[]): IRoleSubSystemLoadSuccess =>
        baseAction(eRoleSubSystemTypeIds.LOAD_SUCCESS, roleSubsytems),
    loadError: (message: string) => {
        NotifyHelper.Error(message);
        return baseAction(eRoleSubSystemTypeIds.LOAD_ERROR, message)
    },

    addStart: (payload: IRoleSubSystem): IRoleSubSystemAddStart =>
        baseAction(eRoleSubSystemTypeIds.ADD_SUBSYTEM_START, payload),
    addSuccess: (): IRoleSubSystemAddSuccess => {
        NotifyHelper.Success("Saved!");
        return baseAction(eRoleSubSystemTypeIds.ADD_SUBSYTEM_SUCCESS, undefined)
    },
    addError: (message: string): IRoleSubSystemAddError => {
        NotifyHelper.Error(message);
        return baseAction(eRoleSubSystemTypeIds.ADD_SUBSYTEM_ERROR, message)
    },

    removeStart: (payload: IRoleSubSystem): IRoleSubSystemRemoveStart =>
        baseAction(eRoleSubSystemTypeIds.REMOVE_SUBSYSTEM_START, payload),
    removeSuccess: (): IRoleSubSystemRemoveSuccess =>{
        NotifyHelper.Success("Saved!");
        return baseAction(eRoleSubSystemTypeIds.REMOVE_SUBSYSTEM_SUCCESS, undefined)
    },
    removeError: (message: string): IRoleSubSystemRemoveError => {
        NotifyHelper.Error(message);
        return baseAction(eRoleSubSystemTypeIds.REMOVE_SUBSYSTEM_ERROR, message)
    }
}