import { NotifyHelper } from "../../../helpers/toast";
import { ISubSystem } from "../../../models/responses/user/ISubSystem";

import { ISubSystemChangeEditing, ISubSystemLoadError, ISubSystemLoadStart, ISubSystemLoadSuccess, eSubSystemTypeIds } from "../../action-types/user/ISubSystemType";
import { baseAction } from "../baseAction";

export const subSystemAction = {
    loadStart: (): ISubSystemLoadStart =>
        baseAction(eSubSystemTypeIds.LOAD_START, undefined),
    loadSuccess: (res: ISubSystem[]): ISubSystemLoadSuccess =>
        baseAction(eSubSystemTypeIds.LOAD_SUCCESS, res),
    loadError: (message: string): ISubSystemLoadError => {
        NotifyHelper.Error(message)
        return baseAction(eSubSystemTypeIds.LOAD_ERROR, message)
    },
    changeEditing: (id: number): ISubSystemChangeEditing => {
        return baseAction(eSubSystemTypeIds.CHANGE_EDITING, id)
    },


}