import { IAccountChangeAppSelected } from './../../action-types/account/IAccountActionType';
import { saveAccessToken, saveRefreshToken } from "../../../api/apiClient";
import { ILoginRequest } from '../../../models/requests/account/ILoginRequest';
import { ILoginRespone } from "../../../models/responses/account/ILoginRespone";
import { IProfileRespone } from "../../../models/responses/account/IProfileRespone";
import { IMenuViewModel } from "../../../models/responses/user/IMenu";
import { IAccountGetProfileError, IAccountGetProfileStart, IAccountGetProfileSuccess, IAccountLoginError, IAccountLoginStart, IAccountLoginSuccess, eAccountActionTypeIds } from "../../action-types/account/IAccountActionType";
import { baseAction } from "../baseAction";
import { NotifyHelper } from '../../../helpers/toast';
import { ILoginSerialRequest } from '../../../models/requests/account/ILoginSerialRequest';
import { ILoginRSRequest } from '../../../models/requests/account/ILoginRSRequest';

export const accountAction = {
    loadProfileStart: (): IAccountGetProfileStart =>
        baseAction(eAccountActionTypeIds.GET_PROFILE, undefined),
    loadProfileSuccess: (profile: IProfileRespone): IAccountGetProfileSuccess =>
        baseAction(eAccountActionTypeIds.GET_PROFILE_SUCCESS, profile),
    loadProfileError: (message: string): IAccountGetProfileError => baseAction(eAccountActionTypeIds.GET_PROFILE_ERROR, message),

    loginStart: (request: ILoginRequest | ILoginSerialRequest | ILoginRSRequest): IAccountLoginStart =>
        baseAction(eAccountActionTypeIds.LOGIN_START, request),
    loginSuccess: (response: ILoginRespone): IAccountLoginSuccess => {
        saveAccessToken(response.token_info.access_token)
        saveRefreshToken(response.token_info.refresh_token)
        return baseAction(eAccountActionTypeIds.LOGIN_SUCCESS, response);
    },
    loginError: (message: string): IAccountLoginError => {
        NotifyHelper.Error(message);
        return baseAction(eAccountActionTypeIds.LOGIN_ERROR, message)
    },
    changeAppSelected: (menu: IMenuViewModel): IAccountChangeAppSelected =>
        baseAction(eAccountActionTypeIds.CHANGE_APP_SELECTED, menu),

}