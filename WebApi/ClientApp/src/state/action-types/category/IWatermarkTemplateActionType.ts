import { IWatermarkTemplate } from "../../../models/responses/category/IWatermarkTemplate";
import { IActionTypeBase } from "../IActionTypeBase";

export enum eWatermarkTemplateActionTypeIds {
    LOAD_START = "WATERMARK_TEMPLATE_LOAD_START",
    LOAD_SUCCESS = "WATERMARK_TEMPLATE_LOAD_SUCCESS",
    LOAD_ERROR = "WATERMARK_TEMPLATE_LOAD_ERROR",


}

export interface IWatermarkTemplateLoadStart extends IActionTypeBase<eWatermarkTemplateActionTypeIds.LOAD_START, undefined> { }
export interface IWatermarkTemplateLoadSuccess extends IActionTypeBase<eWatermarkTemplateActionTypeIds.LOAD_SUCCESS, IWatermarkTemplate[]> { }
export interface IWatermarkTemplateLoadError extends IActionTypeBase<eWatermarkTemplateActionTypeIds.LOAD_ERROR, string> { }


export type IWatermarkTemplateActionType = IWatermarkTemplateLoadStart | IWatermarkTemplateLoadSuccess | IWatermarkTemplateLoadError