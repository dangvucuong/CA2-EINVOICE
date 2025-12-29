import { NotifyHelper } from "../../../helpers/toast";
import { ICompanySize } from '../../../models/responses/contact/ICompanySize';
import { ICompanySizeLoadStart } from "../../action-types/contact/ICompanySizeActionType";
import { baseAction } from "../baseAction";
import { ICompanySizeLoadError, ICompanySizeLoadSuccess, eCompanySizeActionTypeIds } from '../../action-types/contact/ICompanySizeActionType';


export const companySizeAction = {
    loadStart: (): ICompanySizeLoadStart =>
        baseAction(eCompanySizeActionTypeIds.LOAD_START, undefined),
    loadSuccess: (res: ICompanySize[]): ICompanySizeLoadSuccess =>
        baseAction(eCompanySizeActionTypeIds.LOAD_SUCCESS, res),
    loadError: (message: string): ICompanySizeLoadError => {
        NotifyHelper.Error(message)
        return baseAction(eCompanySizeActionTypeIds.LOAD_ERROR, message)
    },
    
}