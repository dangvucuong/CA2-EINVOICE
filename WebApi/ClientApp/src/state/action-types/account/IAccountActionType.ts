import { ILoginRequest } from "../../../models/requests/account/ILoginRequest";
import { ILoginRSRequest } from "../../../models/requests/account/ILoginRSRequest";
import { ILoginSerialRequest } from "../../../models/requests/account/ILoginSerialRequest";
import { ILoginRespone } from "../../../models/responses/account/ILoginRespone";
import { IProfileRespone } from "../../../models/responses/account/IProfileRespone";
import { IMenuViewModel } from "../../../models/responses/user/IMenu";
import { IActionTypeBase } from "../IActionTypeBase";

export enum eAccountActionTypeIds {
    GET_PROFILE = "ACCOUNT_GETTING_PROFILE",
    GET_PROFILE_SUCCESS = "ACCOUNT_GET_PROFILE_SUCCESS",
    GET_PROFILE_ERROR = "ACCOUNT_GET_PROFILE_ERROR",
    LOGIN_START = "ACCOUNT_LOGIN_START",
    LOGIN_SUCCESS = "ACCOUNT_LOGIN_SUCCESS",
    LOGIN_ERROR = "ACCOUNT_LOGIN_ERROR",

    CHANGE_APP_SELECTED = "CHANGE_APP_SELECTED"
}
export interface IAccountGetProfileStart extends IActionTypeBase<eAccountActionTypeIds.GET_PROFILE, undefined> { }
export interface IAccountGetProfileSuccess extends IActionTypeBase<eAccountActionTypeIds.GET_PROFILE_SUCCESS, IProfileRespone> { }
export interface IAccountGetProfileError extends IActionTypeBase<eAccountActionTypeIds.GET_PROFILE_ERROR, string> { }

export interface IAccountLoginStart extends IActionTypeBase<eAccountActionTypeIds.LOGIN_START, ILoginRequest | ILoginSerialRequest | ILoginRSRequest> { }
export interface IAccountLoginSuccess extends IActionTypeBase<eAccountActionTypeIds.LOGIN_SUCCESS, ILoginRespone> { }
export interface IAccountLoginError extends IActionTypeBase<eAccountActionTypeIds.LOGIN_ERROR, string> { }


export interface IAccountChangeAppSelected extends IActionTypeBase<eAccountActionTypeIds.CHANGE_APP_SELECTED, IMenuViewModel> { }


export type IAccountActionType = IAccountGetProfileStart | IAccountGetProfileSuccess | IAccountGetProfileError |
    IAccountLoginStart | IAccountLoginSuccess | IAccountLoginError |
    IAccountChangeAppSelected