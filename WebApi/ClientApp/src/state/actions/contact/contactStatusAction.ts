import { NotifyHelper } from "../../../helpers/toast";
import { IContactStatus } from '../../../models/responses/contact/IContactStatus';
import { IContactStatusLoadStart } from "../../action-types/contact/IContactStatusActionType";
import { baseAction } from "../baseAction";
import { IContactStatusLoadError, IContactStatusLoadSuccess, eContactStatusActionTypeIds } from './../../action-types/contact/IContactStatusActionType';


export const contactStatusAction = {
    loadStart: (): IContactStatusLoadStart =>
        baseAction(eContactStatusActionTypeIds.LOAD_START, undefined),
    loadSuccess: (res: IContactStatus[]): IContactStatusLoadSuccess =>
        baseAction(eContactStatusActionTypeIds.LOAD_SUCCESS, res),
    loadError: (message: string): IContactStatusLoadError => {
        NotifyHelper.Error(message)
        return baseAction(eContactStatusActionTypeIds.LOAD_ERROR, message)
    },
    
}