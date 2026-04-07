import { IWatermarkTemplate } from "../../../models/responses/category/IWatermarkTemplate";
import { eReducerStatusBase } from "../eReducerStatusBase";

export interface IWatermarkTemplateReducer {
    status: eReducerStatusBase,
    watermarkTemplates: IWatermarkTemplate[],
   
}