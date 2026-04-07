import { IWatermarkTemplateActionType, eWatermarkTemplateActionTypeIds } from "../../action-types/category/IWatermarkTemplateActionType"
import { IWatermarkTemplateReducer } from "../../reducer-models/category/IWatermarkTemplateReducer"
import { eReducerStatusBase } from "../../reducer-models/eReducerStatusBase"

const iniState: IWatermarkTemplateReducer = {
    status: eReducerStatusBase.is_not_initialization,
    watermarkTemplates: [],
}
export const watermarkTemplateReducer = (state: IWatermarkTemplateReducer = iniState, action: IWatermarkTemplateActionType): IWatermarkTemplateReducer => {
    switch (action.type) {
        case eWatermarkTemplateActionTypeIds.LOAD_START:
            return {
                ...state,
                status: eReducerStatusBase.is_loading
            }
        case eWatermarkTemplateActionTypeIds.LOAD_SUCCESS:
            return {
                ...state,
                status: eReducerStatusBase.is_loaded,
                watermarkTemplates: action.payload
            }
        case eWatermarkTemplateActionTypeIds.LOAD_ERROR:
            return {
                ...state,
                status: eReducerStatusBase.is_load_err,
            }


        default:
            return state;
    }
}