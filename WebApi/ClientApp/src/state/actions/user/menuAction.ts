import { NotifyHelper } from "../../../helpers/toast";
import { IMenu } from "../../../models/responses/user/IMenu";
import { eMenuActionTypeIds } from "../../action-types/user/IMenuActionType";

import { IMenuLoadError, IMenuLoadStart, IMenuLoadSuccess } from "../../action-types/user/IMenuActionType";
import { baseAction } from "../baseAction";

export const menuAction = {
    loadStart: (subSystemId: number): IMenuLoadStart =>
        baseAction(eMenuActionTypeIds.LOAD_START, subSystemId),
    loadSuccess: (res: IMenu[]): IMenuLoadSuccess =>
        baseAction(eMenuActionTypeIds.LOAD_SUCCESS, res),
    loadError: (message: string): IMenuLoadError => {
        NotifyHelper.Error(message)
        return baseAction(eMenuActionTypeIds.LOAD_ERROR, message)
    },


}