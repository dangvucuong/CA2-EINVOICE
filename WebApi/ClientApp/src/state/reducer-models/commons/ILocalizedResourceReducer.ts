import { eReducerStatusBase } from "../eReducerStatusBase";

export interface ILocalizedResourceReducer {
    status: eReducerStatusBase,
    lan: "vi" | "en",
    localized_resources: Map<string, string>
}