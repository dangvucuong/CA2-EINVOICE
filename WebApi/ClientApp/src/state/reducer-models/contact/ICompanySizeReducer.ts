import { ICompanySize } from "../../../models/responses/contact/ICompanySize";
import { eReducerStatusBase } from "../eReducerStatusBase";

export interface ICompanySizeReducer {
    status: eReducerStatusBase,
    companySizes: ICompanySize[],
}