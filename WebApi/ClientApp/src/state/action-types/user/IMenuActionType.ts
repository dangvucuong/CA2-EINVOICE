import { IMenu } from "../../../models/responses/user/IMenu";
import { IActionTypeBase } from "../IActionTypeBase";

export enum eMenuActionTypeIds {
    LOAD_START = "MENU_LOAD_START",
    LOAD_SUCCESS = "MENU_LOAD_SUCCESS",
    LOAD_ERROR = "MENU_LOAD_ERROR",

}
export interface IMenuLoadStart extends IActionTypeBase<eMenuActionTypeIds.LOAD_START, number> { }
export interface IMenuLoadSuccess extends IActionTypeBase<eMenuActionTypeIds.LOAD_SUCCESS, IMenu[]> { }
export interface IMenuLoadError extends IActionTypeBase<eMenuActionTypeIds.LOAD_ERROR, string> { }



export type IMenuActionType = IMenuLoadStart | IMenuLoadSuccess | IMenuLoadError

