import { ILocalizedResource } from "../../../models/responses/common/ILocalizedResource";
import { ILocalizedResourceChangeLanguage, ILocalizedResourceLoadError, ILocalizedResourceLoadStart, ILocalizedResourceLoadSuccess, eLocalizedResourceActionTypeIds } from "../../action-types/commons/ILocalizedResourceActionType";
import { baseAction } from "../baseAction";

export const localizedResourceAction = {
    loadStart: (lan: string): ILocalizedResourceLoadStart => baseAction(eLocalizedResourceActionTypeIds.LOAD_START, lan),
    loadSuccess: (data: ILocalizedResource[]): ILocalizedResourceLoadSuccess => {
        let maps = new Map<string, string>();
        data.forEach(e => {
            maps.set(e.code, e.value)
        })
        return baseAction(eLocalizedResourceActionTypeIds.LOAD_SUCCESS, maps)
    },
    loadError: (message: string): ILocalizedResourceLoadError => baseAction(eLocalizedResourceActionTypeIds.LOAD_ERROR, message),
    changeLanguage: (lan: "vi" | "en"): ILocalizedResourceChangeLanguage => baseAction(eLocalizedResourceActionTypeIds.CHANGE_LANGUAGE, lan)
}