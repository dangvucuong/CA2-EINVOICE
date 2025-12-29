import { ICompanySize } from "../../../models/responses/contact/ICompanySize";
import { IActionTypeBase } from "../IActionTypeBase";

export enum eCompanySizeActionTypeIds {
    LOAD_START = "COMPANY_SIZE_LOAD_START",
    LOAD_SUCCESS = "COMPANY_SIZE_LOAD_SUCCESS",
    LOAD_ERROR = "COMPANY_SIZE_LOAD_ERROR",

}

export interface ICompanySizeLoadStart extends IActionTypeBase<eCompanySizeActionTypeIds.LOAD_START, undefined> { }
export interface ICompanySizeLoadSuccess extends IActionTypeBase<eCompanySizeActionTypeIds.LOAD_SUCCESS, ICompanySize[]> { }
export interface ICompanySizeLoadError extends IActionTypeBase<eCompanySizeActionTypeIds.LOAD_ERROR, string> { }



export type ICompanySizeActionType = ICompanySizeLoadStart | ICompanySizeLoadSuccess | ICompanySizeLoadError 
   