import { NotifyHelper } from "../../../helpers/toast";
import { IWatermarkTemplate } from "../../../models/responses/category/IWatermarkTemplate";
import { IWatermarkTemplateLoadError, IWatermarkTemplateLoadStart, IWatermarkTemplateLoadSuccess, eWatermarkTemplateActionTypeIds } from "../../action-types/category/IWatermarkTemplateActionType";
import { baseAction } from "../baseAction";


export const watermarkTemplateAction = {
    loadStart: (): IWatermarkTemplateLoadStart =>
        baseAction(eWatermarkTemplateActionTypeIds.LOAD_START, undefined),
    loadSuccess: (data: IWatermarkTemplate[]): IWatermarkTemplateLoadSuccess =>
        baseAction(eWatermarkTemplateActionTypeIds.LOAD_SUCCESS, data),
    loadError: (message: string): IWatermarkTemplateLoadError => {
        NotifyHelper.Error(message)
        return baseAction(eWatermarkTemplateActionTypeIds.LOAD_ERROR, message)
    }
}