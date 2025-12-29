import { IActionTypeBase } from './../IActionTypeBase';
export enum eLocalizedResourceActionTypeIds {
    LOAD_START = "LOCALIZED_RESOURCE_LOAD_START",
    LOAD_SUCCESS = "LOCALIZED_RESOURCE_LOAD_SUCCESS",
    LOAD_ERROR = "LOCALIZED_RESOURCE_LOAD_ERROR",

    CHANGE_LANGUAGE = "LOCALIZED_RESOURCE_CHANGE_LANGUAGE"
}
export interface ILocalizedResourceLoadStart extends IActionTypeBase<eLocalizedResourceActionTypeIds.LOAD_START, string> { }
export interface ILocalizedResourceLoadSuccess extends IActionTypeBase<eLocalizedResourceActionTypeIds.LOAD_SUCCESS, Map<string, string>> { }
export interface ILocalizedResourceLoadError extends IActionTypeBase<eLocalizedResourceActionTypeIds.LOAD_ERROR, string> { }

export interface ILocalizedResourceChangeLanguage extends IActionTypeBase<eLocalizedResourceActionTypeIds.CHANGE_LANGUAGE, "vi" | "en"> { }

export type ILocalizedResourceActionTypes = ILocalizedResourceLoadStart | ILocalizedResourceLoadSuccess | ILocalizedResourceLoadError
    | ILocalizedResourceChangeLanguage
