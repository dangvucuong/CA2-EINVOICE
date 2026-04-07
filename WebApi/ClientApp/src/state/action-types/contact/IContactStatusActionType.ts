import { IContactStatus } from "../../../models/responses/contact/IContactStatus";
import { IActionTypeBase } from "../IActionTypeBase";

export enum eContactStatusActionTypeIds {
    LOAD_START = "CONTACT_STATUS_LOAD_START",
    LOAD_SUCCESS = "CONTACT_STATUS_LOAD_SUCCESS",
    LOAD_ERROR = "CONTACT_STATUS_LOAD_ERROR",

}

export interface IContactStatusLoadStart extends IActionTypeBase<eContactStatusActionTypeIds.LOAD_START, undefined> { }
export interface IContactStatusLoadSuccess extends IActionTypeBase<eContactStatusActionTypeIds.LOAD_SUCCESS, IContactStatus[]> { }
export interface IContactStatusLoadError extends IActionTypeBase<eContactStatusActionTypeIds.LOAD_ERROR, string> { }



export type IContactStatusActionType = IContactStatusLoadStart | IContactStatusLoadSuccess | IContactStatusLoadError 
   